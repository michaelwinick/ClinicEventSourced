using Eventuous.Diagnostics.OpenTelemetry;
using Eventuous.EventStore;
using Eventuous.EventStore.Producers;
using Eventuous.EventStore.Subscriptions;
using Eventuous.Producers;
using Eventuous.Projections.MongoDB;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Pumper.Application;
using Pumper.Infrastructure;
using Pumper.Integration;

namespace Pumper;

public static class Registrations
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEventStoreClient(configuration["EventStore:ConnectionString"]);
        services.AddAggregateStore<EsdbEventStore>();
        services.AddApplicationService<CommandService, Domain.Pumper>();
        services.AddSingleton(Mongo.ConfigureMongo(configuration));
        services.AddCheckpointStore<MongoCheckpointStore>();
        services.AddEventProducer<EventStoreProducer>();

        // PersonalAccountCreatedIntegration event Subscriber
        services.AddSubscription<StreamSubscription, StreamSubscriptionOptions>(
            "Account-Integration",
            builder => builder
                .Configure(x => x.StreamName = IntegrationHandler.Stream)
                .UseCheckpointStore<MongoCheckpointStore>()
                .AddEventHandler<IntegrationHandler>()
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
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("pumper"))
                .SetSampler(new AlwaysOnSampler())
                .AddZipkinExporter()
        );
    }
}