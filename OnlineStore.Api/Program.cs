using Microsoft.Extensions.FileProviders;
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

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
             Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
        RequestPath = "/Resources"
    });

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
