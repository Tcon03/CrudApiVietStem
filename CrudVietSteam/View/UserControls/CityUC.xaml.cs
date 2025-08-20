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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrudVietSteam.View.UserControls
{
    /// <summary>
    /// Interaction logic for CityUC.xaml
    /// </summary>
    public partial class CityUC : UserControl
    {
        public CityUC()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt == null || bt.ContextMenu == null)
                return;
            bt.ContextMenu.PlacementTarget = bt;
            bt.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
            bt.ContextMenu.IsOpen = true;
            e.Handled = true;
        }
    }
}
