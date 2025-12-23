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

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureRepositories(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();