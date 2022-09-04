using System.Text.Json;
using Account.Application;
using Account.Application.Queries;
using Account.Domain;
using Account.Infrastructure;
using Account.Integration;
using Eventuous;
using Eventuous.Diagnostics.OpenTelemetry;
using Eventuous.EventStore;
using Eventuous.EventStore.Producers;
using Eventuous.EventStore.Subscriptions;
using Eventuous.Producers;
using Eventuous.Projections.MongoDB;
using Eventuous.Subscriptions.Registrations;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Account;

public static class Registrations
{
    public static void AddEventuous(this IServiceCollection services, IConfiguration configuration)
    {
        DefaultEventSerializer.SetDefaultSerializer(
            new DefaultEventSerializer(
                new JsonSerializerOptions(JsonSerializerDefaults.Web).ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)
            )
        );

        services.AddEventStoreClient(configuration["EventStore:ConnectionString"]);
        services.AddAggregateStore<EsdbEventStore>();
        services.AddApplicationService<AccountCommandService, Domain.Account>();

        services.AddSingleton(Mongo.ConfigureMongo(configuration));
        services.AddCheckpointStore<MongoCheckpointStore>();

        // Read Model based on PersonalAccountCreationStarted, PersonalAccountInformationAdded, PersonalAccountCreated
        services.AddSubscription<AllStreamSubscription, AllStreamSubscriptionOptions>(
            "AccountProjections",
            builder => builder
                .UseCheckpointStore<MongoCheckpointStore>()
                .AddEventHandler<AccountStateProjection>()
        );

        // Gateway from PersonalAccountCreated event to PersonalAccountCreatedIntegration event
        services.AddEventProducer<EventStoreProducer>();
        services
            .AddGateway<AllStreamSubscription, AllStreamSubscriptionOptions, EventStoreProducer>(
                "Account-Integration",
                AccountGateway.Transform
            );

        // PersonalAccountCreatedIntegration event Subscriber
        //services.AddSubscription<StreamSubscription, StreamSubscriptionOptions>(
        //    "PaymentIntegration5",
        //    builder => builder
        //        .Configure(x => x.StreamName = PersonalAccountCreatedIntegrationHandler.Stream)
        //        .UseCheckpointStore<MongoCheckpointStore>()
        //        .AddEventHandler<PersonalAccountCreatedIntegrationHandler>()
        // );
    }

    public static void AddOpenTelemetry(this IServiceCollection services)
    {
        var otelEnabled = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") != null;
        services.AddOpenTelemetryMetrics(
            builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("accounts"))
                    .AddAspNetCoreInstrumentation()
                    .AddEventuous()
                    .AddEventuousSubscriptions()
                    .AddPrometheusExporter();
                if (otelEnabled) builder.AddOtlpExporter();
            }
        );

        services.AddOpenTelemetryTracing(
            builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddEventuousTracing()
                    .AddMongoDBInstrumentation()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("accounts"))
                    .SetSampler(new AlwaysOnSampler());

                if (otelEnabled)
                    builder.AddOtlpExporter();
                else
                    builder.AddZipkinExporter();
            }
        );
    }
}