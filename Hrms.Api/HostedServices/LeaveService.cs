using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Utilities;
using Org.BouncyCastle.Security;

namespace Hrms.Api.HostedServices
{
    public class LeaveService : BackgroundHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<LeaveService> _logger;
        private Timer _timer;
        public LeaveService(IServiceScopeFactory serviceScopeFactory, ILogger<LeaveService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("IHosted service excecuted.");
            if (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Cancellation requested for IHosted service.");
                return;
            }

            var currentDate = Utility.GetDateTime();
            var firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            if (currentDate.Day > firstDateOfMonth.Day)
            {
                _logger.LogInformation("Current date's day is greater than the first date of the month. Skipping timer setup.");
                return;
            }
            _timer = new Timer(ExecuteMethod, null, TimeSpan.FromSeconds(0), Timeout.InfiniteTimeSpan);
            await Task.CompletedTask;
        }

        private async void ExecuteMethod(object state)
        {
            try
            {
                _logger.LogInformation("Background task execution started.");
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var leaveService = scope.ServiceProvider.GetRequiredService<IMiscellaneousManager>();
                    try
                    {
                        await leaveService.UpdateLeavesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "ExecuteAsync");
                    }
                }
                _logger.LogInformation("Background task execution completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the background task.");
            }

            // Reschedule the method execution for the last day of the next month after midnight
            var currentTime = DateTime.Now;

            var startOfNextMonth = new DateTime(currentTime.Year, currentTime.Month, 1).AddMonths(1);
            var timeUntilExecution = startOfNextMonth - currentTime;
            _timer.Change(timeUntilExecution, Timeout.InfiniteTimeSpan);
        }
    }
}