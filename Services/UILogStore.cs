using MessageBusExample.Models.LogModels;
using System.Collections.ObjectModel;

namespace MessageBusExample.Services
{
    public class UILogStore
    {
        public ObservableCollection<UILogEntry> Logs { get; } = [];
    }
}
