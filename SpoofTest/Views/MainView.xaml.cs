using SpoofTest.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpoofTest.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(Key.Text) && !string.IsNullOrWhiteSpace(Name.Text) && !string.IsNullOrWhiteSpace(LastName.Text) && !string.IsNullOrWhiteSpace(Patronymic.Text) && !string.IsNullOrWhiteSpace(Group.Text))
        {
            TestViewModel vm = new();
            TestView test = new()
            {
                DataContext = vm
            };
            await vm.Start(Key.Text, Name.Text, LastName.Text, Patronymic.Text, Group.Text);
            test.ShowDialog();
        }
    }
}
