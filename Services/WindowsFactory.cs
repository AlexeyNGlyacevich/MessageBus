using MessageBusExample.Abstractions.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MessageBusExample.Services
{
    public class WindowsFactory : IWindowsFactory
    {
        private readonly IServiceProvider _provider;
        public WindowsFactory(IServiceProvider provider) 
        {
            _provider = provider;
        }
        public T Create<T>() where T : Window
        {
           return _provider.GetRequiredService<T>();
        }
    }
}
