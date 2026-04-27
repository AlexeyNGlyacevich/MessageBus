using MessageBusExample.ViewModels;
using System.Windows;

namespace MessageBusExample.Views
{
    /// <summary>
    /// Логика взаимодействия для AditionalMessageView.xaml
    /// </summary>
    public partial class AditionalMessageView : Window
    {
        public AditionalMessageView(AditionalViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}
