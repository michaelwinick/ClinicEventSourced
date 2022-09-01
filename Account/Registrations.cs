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

        //services.AddSingleton<Services.IsRoomAvailable>((id, period) => new ValueTask<bool>(true));

        //services.AddSingleton<Services.ConvertCurrency>((from, currency) => new Money(from.Amount * 2, currency));

        services.AddSingleton(Mongo.ConfigureMongo(configuration));
        services.AddCheckpointStore<MongoCheckpointStore>();

        services.AddSubscription<AllStreamSubscription, AllStreamSubscriptionOptions>(
            "BookingsProjections",
            builder => builder
                .Configure(cfg => cfg.ConcurrencyLimit = 2)
                .UseCheckpointStore<MongoCheckpointStore>()
                .AddEventHandler<BookingStateProjection>()
                .AddEventHandler<MyBookingsProjection>()
                .WithPartitioningByStream(2)
        );

        //services.AddSubscription<AllStreamSubscription, AllStreamSubscriptionOptions>(
        //    "AccountIntegration",
        //    builder => builder
        //        .UseCheckpointStore<MongoCheckpointStore>()
        //        .AddEventHandler<PersonalAccountCreatedHandler>()
        //);

        services.AddEventProducer<EventStoreProducer>();
        services
            .AddGateway<AllStreamSubscription, AllStreamSubscriptionOptions, EventStoreProducer>(
                "Account-Integration",
                PaymentsGateway.Transform
            );

        //services.AddSubscription<StreamSubscription, StreamSubscriptionOptions>(
        //    "Account-Integration",
        //    builder => builder
        //        .Configure(x => x.StreamName = PersonalAccountCreatedHandler.Stream)
        //        .AddEventHandler<PersonalAccountCreatedHandler>()
        //);
    }

    public static void AddOpenTelemetry(this IServiceCollection services)
    {
        var otelEnabled = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") != null;
        services.AddOpenTelemetryMetrics(
            builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("bookings"))
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
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("bookings"))
                    .SetSampler(new AlwaysOnSampler());

                if (otelEnabled)
                    builder.AddOtlpExporter();
                else
                    builder.AddZipkinExporter();
            }
        );
    }
}