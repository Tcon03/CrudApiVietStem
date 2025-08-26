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
        private static string filePath = "curdential.json";


        /// <summary>
        /// Save Account
        /// </summary>
        public static void SaveCurdential(string username, string password)
        {
            //1 . check if  File Path Exist then don't save it   
            if (File.Exists(filePath))
            {
                var readFile = File.ReadAllText(filePath);
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
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Read File Save Account 
        /// </summary>
        public static User Load()
        {
            // 1. if filePath not exist then return null 
            if (!File.Exists(filePath))
            {
                //MessageBox.Show("File hiện tại đang rỗng !! Vui lòng đăng nhập tk,mk" , "Thông Báo" , MessageBoxButton.OK , MessageBoxImage.Information );
                return null;
            }

            var data = File.ReadAllText(filePath);
        
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
            File.Delete(filePath);
        }
    }
}
