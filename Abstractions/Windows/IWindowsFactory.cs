using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessageBusExample.Abstractions.Windows
{
    public interface IWindowsFactory
    {
        T Create<T>() where T : Window;
    }
}
