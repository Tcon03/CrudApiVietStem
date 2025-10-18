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
            ConfigureAutoUpdater();

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

        private string AppCastUrl = "https://raw.githubusercontent.com/Tcon03/AutoUpdate-Version/refs/heads/main/VietStemUpdate.xml";


        private void ConfigureAutoUpdater()
        {
            AutoUpdater.ReportErrors = true;
            AutoUpdater.RunUpdateAsAdmin = false;        
            AutoUpdater.Synchronous = false;
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.ClearAppDirectory = true;
            AutoUpdater.Start(AppCastUrl);
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.Error != null)
            {
                // Sử dụng Dispatcher để đảm bảo MessageBox được hiển thị trên luồng UI chính.
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        $"Không thể kiểm tra cập nhật. Vui lòng kiểm tra lại kết nối mạng hoặc URL cấu hình.\nLỗi: {args.Error.Message}",
                        "Lỗi Cập Nhật",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
                return;
            }
            if (args == null)
            {
                Dispatcher.Invoke(() =>
                    MessageBox.Show("Không thể kiểm tra cập nhật. Kiểm tra lại mạng hoặc URL XML.",
                        "Cập nhật", MessageBoxButton.OK, MessageBoxImage.Warning));
                return;
            }

            if (!args.IsUpdateAvailable)
            {
                // Để yên cho mượt; khi debug có thể mở thông báo:
                Dispatcher.Invoke(() => MessageBox.Show("Bạn đang dùng phiên bản mới nhất :" +args.InstalledVersion, "Thông Báo" ,MessageBoxButton.OK ,MessageBoxImage.Information));
                return;
            }
            Dispatcher.Invoke(() =>
            {
                var result = MessageBox.Show(
                    $"Phát hiện phiên bản mới!\n\n" +
                    $"Phiên bản hiện tại: v{args.InstalledVersion}\n" +
                    $"Phiên bản mới: v{args.CurrentVersion}\n\n" +
                    $"Bạn có muốn cập nhật ngay không?\n" +
                    $"(Ứng dụng sẽ tự động khởi động lại sau khi cập nhật)",
                    "Cập nhật có sẵn",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (AutoUpdater.DownloadUpdate(args))
                        {
                            Application.Current.Shutdown();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Lỗi trong quá trình cập nhật: {ex.Message}",
                            "Lỗi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            });
        }
    }

}
