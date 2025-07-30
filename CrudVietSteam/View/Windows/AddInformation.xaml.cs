using CrudVietSteam.ViewModel;
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
    /// Interaction logic for AddInformation.xaml
    /// </summary>
    public partial class AddInformation : Window
    {
        public AddInformation()
        {
            InitializeComponent();
            ContestsVM ct = this.DataContext as ContestsVM;
            if (ct != null)
            {
                ct.AddSuccess += AddSuccessHandler;
            }
        }

        private void AddSuccessHandler(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            VietstemMain vietstemMain = new VietstemMain();
            vietstemMain.Show();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
