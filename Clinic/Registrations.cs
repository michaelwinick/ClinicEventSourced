using Clinic.Application;
using Clinic.Domain;
using Clinic.Infrastructure;
using Clinic.Integration;
using Eventuous.Diagnostics.OpenTelemetry;
using Eventuous.EventStore;
using Eventuous.EventStore.Producers;
using Eventuous.EventStore.Subscriptions;
using Eventuous.Producers;
using Eventuous.Projections.MongoDB;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Clinic;

public static class Registrations
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEventStoreClient(configuration["EventStore:ConnectionString"]);
        services.AddAggregateStore<EsdbEventStore>();
        services.AddApplicationService<CommandService, Payment>();
        services.AddSingleton(Mongo.ConfigureMongo(configuration));
        services.AddCheckpointStore<MongoCheckpointStore>();
        services.AddEventProducer<EventStoreProducer>();

        services
            .AddGateway<AllStreamSubscription, AllStreamSubscriptionOptions, EventStoreProducer>(
                "IntegrationSubscription",
                PaymentsGateway.Transform
            );
    }

    public static void AddOpenTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetryMetrics(
            builder => builder
                .AddAspNetCoreInstrumentation()
                .AddEventuous()
                .AddEventuousSubscriptions()
                .AddPrometheusExporter()
        );
        services.AddOpenTelemetryTracing(
            builder => builder
                .AddAspNetCoreInstrumentation()
                .AddGrpcClientInstrumentation()
                .AddEventuousTracing()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("payments"))
                .SetSampler(new AlwaysOnSampler())
                .AddZipkinExporter()
        );
    }
}