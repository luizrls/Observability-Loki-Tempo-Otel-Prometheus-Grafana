using APIOrquestracao.Api.Clients;
using APIOrquestracao.Api.Configurations.Serilogs;
using APIOrquestracao.Api.Configurations.Tracings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//----------------- Configure Observability ----------------//
builder.Services.ConfigureSerilog(builder.Configuration);
builder.Services.ConfigureOpenTelemetry(builder.Configuration);
//---------------------------------------------------------//
//----------------- Reduce logs verbosity ----------------//
builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeFormattedMessage = false;
    options.IncludeScopes = false;
});
builder.Logging.AddFilter("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.AspNetCore.Routing", LogLevel.Warning);
//---------------------------------------------------------//

builder.Services.AddHealthChecks();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ContagemClient>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseSerilogRequestLogging();

// Map health check endpoint
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();