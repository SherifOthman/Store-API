using OnlineStore.Api;
using OnlineStore.Api.Middlewares;
using OnlineStore.Api.Utils;
using OnlineStore.Application;
using OnlineStore.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSerilog();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("v1",
    options=> options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

builder.Services.AddApiApiDependencies();
builder.Services.AddInfrastrcureDependencies(builder.Configuration);
builder.Services.AddApplicationDependencies(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

try
{
    Log.Information("Starting web application");
    var app = builder.Build();
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseCors("AllowFrontend");

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseMiddleware<ErrorHandlingMiddleware>();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
