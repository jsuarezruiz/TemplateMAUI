﻿namespace TemplateMAUI.Controls
{
    public class SnackBarTimer
    {
        readonly TimeSpan _timespan;
        readonly Action _callback;
        CancellationTokenSource _cancellation;

        public SnackBarTimer(TimeSpan timespan, Action callback)
        {
            _timespan = timespan;
            _callback = callback;
            _cancellation = new CancellationTokenSource();
        }

        public void Start()
        {
            CancellationTokenSource cts = _cancellation;
            Application.Current.Dispatcher.StartTimer(_timespan,
            () =>
            {
                if (cts.IsCancellationRequested) return false;
                _callback.Invoke();
                return false;
            });
        }

        public void Stop()
        {
            Interlocked.Exchange(ref _cancellation, new CancellationTokenSource()).Cancel();
        }
    }
}