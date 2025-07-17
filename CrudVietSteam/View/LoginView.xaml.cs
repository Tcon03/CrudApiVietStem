using CrudVietSteam.Service;
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
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            LoginVM vm = this.DataContext as LoginVM;
            if (vm != null)
            {

                var remembered = CurdentialHelper.Load();
                // nếu file tồn tại 
                if (remembered != null)
                {
                    // gán các trường dữ liệu cho email và password bên VMD 
                    vm.EmailVM = remembered.Value._email;
                    vm.PasswordVM = remembered.Value._password;
                    vm.RememberMe = true;

                    // Gán tên của thuộc tính pass cho bên passVM
                    tbxPassword.Password = vm.PasswordVM; 
                }
                vm.Authenticated += Authenticated;
            }

        }

        private void Authenticated(object sender, EventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void WindowMiniMine_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Minimized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginVM vm)
            {
                vm.PasswordVM = tbxPassword.Password;
            }

        }
    }
}
