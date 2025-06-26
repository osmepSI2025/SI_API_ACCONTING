using Microsoft.EntityFrameworkCore;
using SI_ACC.Entities;
using SI_ACC.Repository;
using SI_ACC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<SI_ACCDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
//Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// ✅ Register NSwag (Swagger 2.0)
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "ACCOUNT_V1";
    config.Title = "API SME ACCOUNT";
    config.Version = "v1";
    config.Description = "API documentation using Swagger 2.0";
    config.SchemaType = NJsonSchema.SchemaType.Swagger2; // This makes it Swagger 2.0
});

builder.Services.AddScoped<MBudgetavailableRepository>();
builder.Services.AddScoped<MBudgetavailableService>();
builder.Services.AddScoped<MBudgetentireRepository>();
builder.Services.AddScoped<MBudgetentireService>();
builder.Services.AddScoped<TBudgetMappingRepository>();
builder.Services.AddScoped<TBudgetMappingService>();





builder.Services.AddScoped<IApiInformationRepository, ApiInformationRepository>();
builder.Services.AddScoped<ICallAPIService, CallAPIService>(); // Register ICallAPIService with CallAPIService
builder.Services.AddHttpClient<CallAPIService>();
 builder.Services.AddHostedService<JobSchedulerService>(); // Register the background service

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseOpenApi();     // Serve the Swagger JSON
    app.UseSwaggerUi3();  // Use Swagger UI v3
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

