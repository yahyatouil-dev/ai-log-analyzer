using AiLogAnalyzer.Application.Interfaces;
using AiLogAnalyzer.Infrastructure.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// TY Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AI Log Analyzer API",
        Version = "v1"
    });
});

// TY Dependency Injection
builder.Services.AddScoped<ILogAnalyzerService, OpenAiLogAnalyzerService>();

var app = builder.Build();

// TY Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AI Log Analyzer API v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();