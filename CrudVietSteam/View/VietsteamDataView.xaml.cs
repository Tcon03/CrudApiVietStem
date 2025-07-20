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

namespace CrudVietSteam.View
{
    /// <summary>
    /// Interaction logic for VietsteamDataView.xaml
    /// </summary>
    public partial class VietsteamDataView : Window
    {
        public VietsteamDataView()
        {
            InitializeComponent();
        }

        private void visiInputClick(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbxVisiSearch.Text))
            {
                tblSearch.Visibility = Visibility.Visible;

            }
            else
            {
                tblSearch.Visibility = Visibility.Collapsed;

            }
        }
    }
}
