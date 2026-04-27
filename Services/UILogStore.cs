using MessageBusExample.Models.LogModels;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace MessageBusExample.Services
{
    public class UILogStore
    {
        private readonly Object _lock = new();
        public ObservableCollection<UILogEntry> Logs { get; } = [];

        public UILogStore() 
        {
            BindingOperations.EnableCollectionSynchronization(Logs, _lock);
        }
    }
}
