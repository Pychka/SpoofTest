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
    /// Логика взаимодействия для ShowResult.xaml
    /// </summary>
    public partial class ShowResult : Window
    {
        int result;
        string password = "PasswordSecurity";
        public ShowResult(int result)
        {
            InitializeComponent();
            this.result = result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Pass.Password == password)
            {
                MessageBox.Show(result.ToString());
                Close();
            }
            Pass.Password = "";
        }
    }
}
