using SpoofTest.ModelView;
using SpoofTest.Views;
using System.Windows;

namespace SpoofTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TestView test = new()
            {
                DataContext = new TestViewModel()
                {
                    
                }
            };
            test.Show();

            MessageBox.Show((test.DataContext as TestViewModel)!.Test.Questions.Count.ToString());
        }
    }
}