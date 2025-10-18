using AutoUpdaterDotNET;
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
            try
            {

                InitializeComponent();
                LoginVModel vm = this.DataContext as LoginVModel;
                if (vm != null)
                {
                    var curdential = CurdentialHelper.Load();
                    // nếu file tồn tại luôn điền mk và tích luôn remember me vào ô check box
                    if (curdential != null)
                    {
                        // gán các trường dữ liệu cho email và password bên VMD 
                        vm.EmailVM = curdential.email;
                        vm.PasswordVM = curdential.password;
                        vm.RememberMe = true;
                        // Gán tên của thuộc tính pass cho bên passVM
                        tbxPassword.Password = vm.PasswordVM;
                    }
                    vm.Authenticated += Authenticated;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động chương trình, vui lòng thử lại.\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
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
            if (DataContext is LoginVModel vm)
            {
                vm.PasswordVM = tbxPassword.Password;
            }
        }



    }

}
