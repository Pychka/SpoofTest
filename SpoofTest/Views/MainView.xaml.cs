using SpoofTest.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpoofTest.Views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Key.Text) && !string.IsNullOrWhiteSpace(Name.Text) && !string.IsNullOrWhiteSpace(LastName.Text) && !string.IsNullOrWhiteSpace(Patronymic.Text) && !string.IsNullOrWhiteSpace(Group.Text))
            {
                TestViewModel vm = new();
                TestView test = new()
                {
                    DataContext = vm
                };
                vm.Start(Key.Text, Name.Text, LastName.Text, Patronymic.Text, Group.Text);
                test.ShowDialog();
            }
        }
    }
}
