using System.Windows;

namespace MessageBusExample.Abstractions.Windows
{
    public interface IWindowsFactory
    {
        // Переписать Factory как нормальный DialogService (уравляет жизненным циклом окон)
        T Create<T>() where T : Window;
    }
}
