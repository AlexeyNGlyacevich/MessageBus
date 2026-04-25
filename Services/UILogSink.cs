using MessageBusExample.Models.LogModels;
using Serilog.Core;
using Serilog.Events;
using System.Windows;

namespace MessageBusExample.Services
{
    public class UILogSink : ILogEventSink
    {
        private readonly UILogStore _store;

        public UILogSink(UILogStore store)
        {
            _store = store;
        }

        public void Emit(LogEvent logEvent)
        {
            var entry = new UILogEntry
            {
                Timestamp = logEvent.Timestamp.UtcDateTime,
                Level = logEvent.Level.ToString(),
                Message = logEvent.RenderMessage(),
                Source = logEvent.Properties.ContainsKey("SourceContext")
                    ? logEvent.Properties["SourceContext"].ToString()
                    : ""
            };

            Application.Current.Dispatcher.Invoke(() =>
            {
                _store.Logs.Add(entry);

                if (_store.Logs.Count > 1000)
                    _store.Logs.RemoveAt(0);
            });
        }
    }
}
