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

namespace Launcher.Windows
{
    /// <summary>
    /// Interaction logic for fakeLogin.xaml
    /// </summary>
    public partial class fakeLogin : Window
    {
        public fakeLogin()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Regex r = new Regex(@"^[a-zA-Z@]+$");
            //if (!r.IsMatch(e.Text))
            //e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
