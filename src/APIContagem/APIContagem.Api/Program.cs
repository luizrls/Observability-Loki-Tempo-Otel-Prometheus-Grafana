using APIContagem.Api.Configurations.Database;
using APIContagem.Api.Configurations.Serilogs;
using APIContagem.Api.Configurations.Tracings;
using Serilog;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

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

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureRepositories(builder.Configuration);

// Add health checks (including a DbContext check)
builder.Services.AddHealthChecks()
    .AddDbContextCheck<APIContagem.Api.Data.DatabaseContext>(name: "database");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseSerilogRequestLogging();

// Map health check endpoint
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();