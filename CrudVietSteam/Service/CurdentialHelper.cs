using CrudVietSteam.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;
using System.Windows;
using System.Windows.Media.Media3D;

namespace CrudVietSteam.Service
{
    public class CurdentialHelper
    {

        public static string accountPath = GetAppFolderPath();
        public static string folderPath = Path.Combine(accountPath, "curdential.json");
        public static string GetAppFolderPath()
        {
            //1. lấy đường dẫn thư mục AppData
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Debug.WriteLine("AppData Path : " + appData);
            string appFolder = Path.Combine(appData, "CrudVietSteam");
            Debug.WriteLine("App Folder Path : " + appFolder);
            Directory.CreateDirectory(appFolder); // Tạo thư mục nếu chưa tồn tại
            return appFolder;
        }

        /// <summary>
        /// Save Account
        /// </summary>
        public static void SaveCurdential(string username, string password)
        {

            //1 . check if  File Path Exist then don't save it    
            if (File.Exists(folderPath))
            {
                var readFile = File.ReadAllText(folderPath);
                var data = JsonConvert.DeserializeObject<User>(readFile);
                if (data != null && data.email == username && data.password == password)
                    Debug.WriteLine("File đã tồn tại và bỏ qua bước save ");
                return;
            }

            //2. If File Path Null then create a new file and save it 
            var curdential = new User
            {
                email = username,
                password = password
            };
            // chuyển đổi Obj sang định dạng Json
            var json = JsonConvert.SerializeObject(curdential, Formatting.Indented);
            Debug.WriteLine("==== Save Account ==== \n" + json);

            // ghi nội dung vào filePath 
            File.WriteAllText(folderPath, json);
            Debug.WriteLine("Lưu tài khoản thành công vào file ");
        }

        /// <summary>
        /// Read File Save Account 
        /// </summary>
        public static User Load()
        {
            // 1. if filePath not exist then return null 
            if (!File.Exists(folderPath))
            {
                //MessageBox.Show("File hiện tại đang rỗng !! Vui lòng đăng nhập tk,mk" , "Thông Báo" , MessageBoxButton.OK , MessageBoxImage.Information );
                return null;
            }

            var data = File.ReadAllText(folderPath);
            Debug.WriteLine("==== Load Account ==== \n" + data);
            return JsonConvert.DeserializeObject<User>(data);

            // C2. lưu dữ liệu vào biến data và chuyển đổi sang đối tượng User
            // var convertData = JsonConvert.DeserializeObject<User>(data);
            //return convertData;

            //if (!File.Exists(filePath))
            //{
            //    MessageBox.Show("File hiện tại không có trong thư mục ");
            //    return null;
            //}
            //// Read File Path 
            //var readFileCurdential = File.ReadAllText(filePath);
            //// convert data to object User with two filed data email and password . 
            //var data = JsonConvert.DeserializeObject<User>(readFileCurdential);

            //string userName = data.email;
            //string password = data.password;

            //Debug.WriteLine("==== Password ==== : " + password);
            //Debug.WriteLine("==== Email ==== : " + userName);

            //return (userName, password);
        }
        /// <summary>
        /// Delete File Path Account
        /// </summary>
        public static void Clear()
        {
            File.Delete(folderPath);
        }
    }
}
