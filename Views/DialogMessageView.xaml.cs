using MessageBusExample.ViewModels;
using System.Windows;

namespace MessageBusExample.Views
{
    /// <summary>
    /// Логика взаимодействия для DialogMessageView.xaml
    /// </summary>
    public partial class DialogMessageView : Window
    {
        public DialogMessageView(MessageViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}
