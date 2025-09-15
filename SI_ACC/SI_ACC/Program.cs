using Microsoft.EntityFrameworkCore;
using SI_ACC.Entities;
using SI_ACC.Repository;
using SI_ACC.Services;
using Quartz;
using Serilog;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.File(
        path: "Logs/app-log.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .MinimumLevel.Information() // เพิ่มการตั้งค่าระดับ Log ขั้นต่ำ
);

// Add services to the container.
builder.Services.AddDbContext<SI_ACCDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register NSwag (Swagger 2.0)
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
builder.Services.AddScoped<ICallAPIService, CallAPIService>();
builder.Services.AddHttpClient<CallAPIService>();

// Add Quartz.NET services
builder.Services.AddQuartz(q =>
{
    q.AddJob<ScheduledJobPuller>(j => j.WithIdentity("ScheduledJobPuller").StoreDurably());
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// Register your IHostedService to manage jobs
builder.Services.AddHostedService<JobSchedulerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
