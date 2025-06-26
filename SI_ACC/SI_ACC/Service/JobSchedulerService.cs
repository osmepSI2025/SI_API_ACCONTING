using Microsoft.EntityFrameworkCore;
using SI_ACC.Entities;
using SI_ACC.Models;

public class JobSchedulerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public JobSchedulerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
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
                    _ = RunJobAsync(job.JobName, scope.ServiceProvider);
                }
            }

            // Check every minute
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
          
                await serviceProvider.GetRequiredService<MBudgetentireService>().BatchEndOfDay_BudgetEntire();
                break;

            case "BudgetMappingV4":
       
                await serviceProvider.GetRequiredService<TBudgetMappingService>().BatchEndOfDay_BudgetMapping();
                break;


            default:
                // Optionally log unknown job
                break;
        }
    }
}