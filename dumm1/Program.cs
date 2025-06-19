using Azure.Core;
using dumm1.auth;
using dumm1.entity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using static dumm1.entity.AppSettings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Add configuration binding
builder.Services.Configure<AppSettings>(builder.Configuration);

// Register TokenCredential (your custom implementation)
builder.Services.AddSingleton<TokenCredential>(provider =>
{
    var config = provider.GetRequiredService<IOptions<AppSettings>>().Value;
    return new MyTokenCredential(
        config.AzureAd.ClientId,
        config.AzureAd.TenantId,
        config.AzureAd.Scopes.Split(',', StringSplitOptions.RemoveEmptyEntries));
});


// Register GraphServiceClient
builder.Services.AddScoped(provider =>
{
    var config = provider.GetRequiredService<IOptions<AppSettings>>().Value;
    return new GraphServiceClient(
        provider.GetRequiredService<TokenCredential>(),
        new[] { config.AzureAd.Scopes });
});

// Register your GraphService
builder.Services.AddScoped<GraphService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
