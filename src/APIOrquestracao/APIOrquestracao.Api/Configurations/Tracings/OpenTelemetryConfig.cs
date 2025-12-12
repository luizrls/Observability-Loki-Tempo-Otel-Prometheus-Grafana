using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace APIOrquestracao.Api.Configurations.Tracings
{
    internal static class OpenTelemetryConfig
    {
        internal static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenTelemetry()
            .WithTracing((traceBuilder) =>
            {
                traceBuilder
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(serviceName: OpenTelemetryExtensions.ServiceName,
                                serviceVersion: OpenTelemetryExtensions.ServiceVersion))
                    .AddAspNetCoreInstrumentation()
                    .AddNpgsql()
                    .AddOtlpExporter()
                    .AddConsoleExporter();
            });

            return services;
        }
    }
}
