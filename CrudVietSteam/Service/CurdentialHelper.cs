using CrudVietSteam.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
           
            var curdential = new User
            {
                email = username,
                password = password
            };
            // chuyển đổi đối tượng sang định dạng Json
            var json = JsonConvert.SerializeObject(curdential, Formatting.Indented);
            Debug.WriteLine("==== Save Account ==== \n" + json);

            // ghi nội dung vào filePath 
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Read File Save Account 
        /// </summary>
        public static (string _email, string _password)? Load()
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File hiện tại không có trong thư mục ");
                return null;
            }
            // Read File Path 
            var readFileCurdential = File.ReadAllText(filePath);
            // convert data to object User with two filed data email and password . 
            var data = JsonConvert.DeserializeObject<User>(readFileCurdential);

            string userName = data.email;
            string password = data.password;

            Debug.WriteLine("==== Password ==== : " + password);
            Debug.WriteLine("==== Email ==== : " + userName);

            return (userName, password);
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
