using APIContagem.Api.Configurations.Tracings;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.Grafana.Loki;

namespace APIContagem.Api.Configurations.Serilogs
{
    internal static class SerilogConfig
    {
        internal static IServiceCollection ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSerilog(new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.GrafanaLoki(
                configuration["Loki:Uri"]!,
                new List<LokiLabel>()
                {
                        new()
                        {
                            Key = "service_name",
                            Value = OpenTelemetryExtensions.ServiceName
                        },
                        new()
                        {
                            Key = "using_database",
                            Value = "true"
                        }
                })
            .Enrich.WithSpan(new SpanOptions() { IncludeOperationName = true, IncludeTags = true })
            .CreateLogger());

            return services;
        }
    }
}
