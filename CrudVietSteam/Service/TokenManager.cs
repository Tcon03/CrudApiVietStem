using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudVietSteam.Service
{
    public class TokenManager
    {

        public const string TkFile = "accessToken.txt";

        /// <summary>
        /// save Id accessToken
        /// </summary>
        public  void SaveToFile(string accessToken)
        {
            File.WriteAllText(TkFile,accessToken);
            Debug.WriteLine("===== Lưu File AccessToken thành công =====");
        }


        /// <summary>
        /// Read Token File
        /// </summary>
        /// <returns></returns>
        public string LoadToken()
        {
            if (!File.Exists(TkFile))
            {
                Debug.WriteLine($"{TkFile} does not exist");
                return null;
            }

            var readFile = File.ReadAllText(TkFile);
            return readFile;
        }


        /// <summary>
        /// Delete File and Token
        /// </summary>
        public void ClearToken()
        {
            if (File.Exists(TkFile))
            {
                File.Delete(TkFile);
                Debug.WriteLine("Xóa File thành công !!");
            }
            Debug.WriteLine("Hiện tại không có File Path nào ở trong thư mục để thực hiện xóa !!");
        }

        //private async Task SaveToFile(VietStemDTO display)
        //{
        //    // chuyển đổi tượng về file json
        //    var json = JsonConvert.SerializeObject(display, Formatting.Indented);
        //    File.WriteAllText(FilePath, json);
        //}
        //private async Task<VietStemDTO> ReadFromFile()
        //{
        //    if (!File.Exists(FilePath))
        //    {
        //        MessageBox.Show("Hiện tại không có đường dẫn nào trong thư mục !!");
        //        return null;
        //    }
        //    var readFile = File.ReadAllText(FilePath);
        //    Debug.WriteLine($"File VietStem DTO : {readFile}");
        //    return JsonConvert.DeserializeObject<VietStemDTO>(readFile);

        //}
    }
}
