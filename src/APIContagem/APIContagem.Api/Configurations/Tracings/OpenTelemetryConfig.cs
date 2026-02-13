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
                        options.Filter = ctx =>
                        {
                            var path = ctx.Request.Path.ToString().ToLower();
                            return !path.Contains("/health") &&
                                   !path.Contains("/swagger") &&
                                   !path.Contains("/favicon.ico") &&
                                   !path.Contains("/metrics");
                        };
                        options.EnrichWithHttpRequest = (activity, request) =>
                        {
                            activity.SetTag("http.client_ip", request.HttpContext.Connection.RemoteIpAddress?.ToString());
                        };
                    })
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.FilterHttpRequestMessage = (httpRequestMessage) =>
                        {
                            var uri = httpRequestMessage.RequestUri?.ToString().ToLower() ?? "";
                            return !uri.Contains("/health") &&
                                   !uri.Contains("/swagger") &&
                                   !uri.Contains("/getScriptTag") && // Elastic APM Server
                                   !uri.Contains("10.0.0.145:8200") && // Elastic APM Server
                                   !uri.Contains("/intake/v2/events"); // Elastic APM intake endpoint
                        };
                    })
                    .AddSqlClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddEntityFrameworkCoreInstrumentation()
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
