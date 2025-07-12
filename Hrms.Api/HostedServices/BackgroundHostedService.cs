namespace Hrms.Api.HostedServices
{
    public abstract class BackgroundHostedService : IHostedService, IDisposable
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);
        public void Dispose()
        {
            _tokenSource.Cancel();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_tokenSource.Token);

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                _tokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }
    }
}
