using Microsoft.EntityFrameworkCore;
using SI_ACC.Entities;
using SI_ACC.Models;
using Microsoft.Extensions.Logging;
using Quartz;

public class JobSchedulerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<JobSchedulerService> _logger;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly SI_ACCDBContext _dbContext;

    public JobSchedulerService(IServiceProvider serviceProvider, ILogger<JobSchedulerService> logger, ISchedulerFactory schedulerFactory, SI_ACCDBContext dbContext)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _schedulerFactory = schedulerFactory;
        _dbContext = dbContext;
        _logger.LogInformation("JobSchedulerService started.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<SI_ACCDBContext>();
                    var now = DateTime.Now;
                    var jobs = await db.MScheduledJobs
                        .Where(j => j.IsActive == true && j.RunHour == now.Hour && j.RunMinute == now.Minute)
                        .ToListAsync(stoppingToken);

                    foreach (var job in jobs)
                    {
                        try
                        {
                            _logger.LogInformation($"Running job: {job.JobName} at {now}");
                            await RunJobAsync(job.JobName, scope.ServiceProvider);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error running job: {job.JobName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in JobSchedulerService loop.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
    private async Task RunJobAsync(string jobName, IServiceProvider serviceProvider)
    {
        switch (jobName)
        {
            case "BudgetAvailableV4":
                await serviceProvider.GetRequiredService<MBudgetavailableService>().BatchEndOfDay_Budgetavailable();
                break;
            case "BudgetEntriesV4":

                //  await serviceProvider.GetRequiredService<MBudgetentireService>().BatchEndOfDay_BudgetEntire();

                var budgetEntireService = serviceProvider.GetRequiredService<MBudgetentireService>();


                int currentYearThai = DateTime.Now.Year + 543;
                for (int yy = currentYearThai; yy <= currentYearThai + 1; yy++)
                {
                    string yearth = (yy % 100).ToString();
                    for (int mm = 1; mm <= 12; mm++)
                    {
                        string bdy = "budgetyear eq '" + yearth + "/" + mm.ToString() + "'";
                        await budgetEntireService.BatchEndOfDay_BudgetEntireBySearch(bdy);
                    }
                }
                break;

            case "BudgetMappingV4":
                await serviceProvider.GetRequiredService<TBudgetMappingService>().BatchEndOfDay_BudgetMapping();
                break;

            default:
                // Optionally log unknown job
                break;
        }
    }

    public async Task RegisterJobsAsync()
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        var jobs = await _dbContext.MScheduledJobs.Where(j => j.IsActive == true).ToListAsync();

        foreach (var job in jobs)
        {
            var jobDetail = JobBuilder.Create<ScheduledJobPuller>()
                .WithIdentity(job.JobName, "dynamic")
                .Build();

            if (job.RunMinute == null || job.RunHour == null)
            {
                _logger.LogError($"Job {job.JobName} has invalid RunMinute or RunHour.");
                continue;
            }
            string cron = $"{job.RunMinute} {job.RunHour} * * *";

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{job.JobName}-trigger", "dynamic")
                .WithCronSchedule(cron)
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}