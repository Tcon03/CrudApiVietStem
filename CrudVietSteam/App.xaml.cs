using CrudVietSteam.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CrudVietSteam
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static VietstemSevice vietstemService { get; } = new VietstemSevice();

        //private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        //{
        //    var exception = (Exception)e.ExceptionObject;
        //    string errorMessage = string.Format(
        //        "Lỗi nghiêm trọng:\n\n{0}\n\nChi tiết:\n{1}\n\nStack Trace:\n{2}",
        //        exception.GetType().Name,
        //        exception.Message,
        //        exception.StackTrace);

        //    try
        //    {
        //        // Ghi file log ra Desktop để chắc chắn có quyền ghi
        //        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //        string logFilePath = System.IO.Path.Combine(desktopPath, "crash_log.txt");
        //        System.IO.File.WriteAllText(logFilePath, errorMessage);

        //        MessageBox.Show("Đã xảy ra lỗi nghiêm trọng. Vui lòng kiểm tra file crash_log.txt trên Desktop của bạn.", "Lỗi Ứng Dụng", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch
        //    {
        //        MessageBox.Show(errorMessage, "Lỗi Ứng Dụng", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
    }
}
