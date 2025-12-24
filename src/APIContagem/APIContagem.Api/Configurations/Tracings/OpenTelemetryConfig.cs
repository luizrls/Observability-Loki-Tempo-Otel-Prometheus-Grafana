using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using OpenTelemetry.Metrics;

namespace APIContagem.Api.Configurations.Tracings
{
    internal static class OpenTelemetryConfig
    {
        internal static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenTelemetry()
            .WithTracing(traceBuilder =>
            {
                traceBuilder
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(
                                OpenTelemetryExtensions.ServiceName,
                                serviceVersion: OpenTelemetryExtensions.ServiceVersion))
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = false;
                    })
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddNpgsql()
                    .SetSampler(new AlwaysOnSampler())
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(configuration["Tempo:Uri"]!);

                        options.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
                        {
                            MaxQueueSize = 2048,
                            ScheduledDelayMilliseconds = 1000,
                            ExporterTimeoutMilliseconds = 30000,
                            MaxExportBatchSize = 512
                        };
                    })
                    .AddConsoleExporter(); // <--- só para debug
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddPrometheusExporter()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService(
                                OpenTelemetryExtensions.ServiceName,
                                serviceVersion: OpenTelemetryExtensions.ServiceVersion))
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(configuration["Otel:Uri"]!);
                    });
            });

            return services;
        }
    }
}
