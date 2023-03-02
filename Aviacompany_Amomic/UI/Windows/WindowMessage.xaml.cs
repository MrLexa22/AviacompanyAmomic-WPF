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

namespace Aviacompany_Amomic.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowMessage.xaml
    /// </summary>
    public partial class WindowMessage : Window
    {
        public WindowMessage(string title, string message)
        {
            InitializeComponent();
            this.Title = title;
            tb_text_error.Text = message;
        }

        private void close_window_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
