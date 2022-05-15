using Microsoft.Extensions.Options;
using NotificationBot.Telegram.Configuration;
using NotificationBot.Telegram.Services.Interfaces;

namespace NotificationBot.Telegram.Services
{
    public class TimerWrapper : ITimerWrapper
    {
        public event EventHandler? OnPeriodicTimerTickEventHandler;

        private readonly TimerWrapperSettings _timerProviderSettings;

        private PeriodicTimer? _periodicTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerWrapper"/> class.
        /// </summary>
        /// <param name="timerProviderSettings">The timer provider settings.</param>
        public TimerWrapper(IOptions<TimerWrapperSettings> timerProviderSettings)
        {
            ArgumentNullException.ThrowIfNull(timerProviderSettings);

            _timerProviderSettings = timerProviderSettings.Value;
        }

        public bool SetupPeriodicTimer(CancellationToken cancellationToken)
        {
            if (_periodicTimer == null)
            {
                _ = Task.Run(
                    async () => await SetupPeriodicTimerInternalAsync(cancellationToken),
                    cancellationToken);

                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task SetupPeriodicTimerInternalAsync(CancellationToken cancellationToken)
        {
            _periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(_timerProviderSettings.PeriodicTimerIntervalInMinutes));
            _nextTimerTick = DateTime.UtcNow.AddMinutes(_timerProviderSettings.PeriodicTimerIntervalInMinutes);

            while (await _periodicTimer.WaitForNextTickAsync(cancellationToken))
            {
                _previousTimerTick = _nextTimerTick;
                _nextTimerTick = _nextTimerTick.AddMinutes(_timerProviderSettings.PeriodicTimerIntervalInMinutes);
                _timerExecutionCount++;

                OnPeriodicTimerTickEventHandler?.Invoke(this, EventArgs.Empty);
            }
        }

        #region IDiagnosticService Implementation

        private DateTime _previousTimerTick;
        private DateTime _nextTimerTick;
        private long _timerExecutionCount;

        public Dictionary<string, string> GetDiagnosticsInfo()
        {
            TimeZoneInfo mskTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Belarus Standard Time");

            return new Dictionary<string, string>
            {
                { "Periodic Timer Initialized", (_periodicTimer is not null).ToString() },
                { "Timer Inverval Minutes", _timerProviderSettings.PeriodicTimerIntervalInMinutes.ToString() },
                { "Previous timer tick UTC", _previousTimerTick.ToString() },
                { "Previous timer tick MSK", TimeZoneInfo.ConvertTimeFromUtc(_previousTimerTick, mskTimeZoneInfo).ToString() },
                { "Next timer tick UTC", _nextTimerTick.ToString() },
                { "Next timer tick MSK", TimeZoneInfo.ConvertTimeFromUtc(_nextTimerTick, mskTimeZoneInfo).ToString() },
                { "Timer execution count", _timerExecutionCount.ToString() }
            };
        }

        #endregion

        #region IDisposable Implementation

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _periodicTimer?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
