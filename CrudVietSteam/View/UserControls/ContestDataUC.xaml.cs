using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ContestDataUC.xaml
    /// </summary>
    public partial class ContestDataUC : UserControl
    {
        public ContestDataUC()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Button bt = sender as Button;
            //// đặt vị trí phù hợp với button 
            //bt.ContextMenu.PlacementTarget = bt;
            //// có thể chỉnh vị trí phù hợp với Button 
            //bt.ContextMenu.Placement = PlacementMode.Left;
            //// Mở ContextMenu bằng chuột phải 
            //bt.ContextMenu.IsOpen = true;
            //e.Handled = true;

            Button bt = sender as Button;
            if (bt == null || bt.ContextMenu == null) return;

            // Dòng này cực kỳ quan trọng để "bắc cầu" DataContext
            bt.ContextMenu.PlacementTarget = bt;
            bt.ContextMenu.Placement = PlacementMode.Left;
            bt.ContextMenu.IsOpen = true;
            e.Handled = true;
        }
    }
}
