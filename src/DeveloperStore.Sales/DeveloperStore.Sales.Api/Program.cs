using System.Text.Json;
using System.Text.Json.Serialization;
using DeveloperStore.Sales.Infra.IoC.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddDbConnection(builder.Configuration);
builder.Services.AddDependencyInjection(builder.Configuration);
builder.Services.ConfigMediatR();
builder.Services.AddMapperConfig();

builder.Services.AddLogging(loggingbuilder =>
{
    loggingbuilder.ClearProviders();
    loggingbuilder.AddConsole();
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
