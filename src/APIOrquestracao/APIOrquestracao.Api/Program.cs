using APIOrquestracao.Api.Clients;
using APIOrquestracao.Api.Configurations.Serilogs;
using APIOrquestracao.Api.Configurations.Tracings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//----------------- Configure Observability ----------------//
builder.Services.ConfigureSerilog(builder.Configuration);
builder.Services.ConfigureOpenTelemetry(builder.Configuration);
//---------------------------------------------------------//

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ContagemClient>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();