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

namespace CrudVietSteam.View.Windows
{
    /// <summary>
    /// Interaction logic for CityInfor.xaml
    /// </summary>
    public partial class CityInfor : Window
    {
        public CityInfor()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
