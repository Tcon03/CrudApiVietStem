using CrudVietSteam.Model;
using CrudVietSteam.Service.DTO;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;
using System.Windows;
using System.Windows.Documents;

namespace CrudVietSteam.Service
{
    public class VietstemSevice
    {

        public readonly string Url = "http://localhost:3000/api/Accounts/login";
        public string contentType = "application/json";
        public readonly HttpClient _client;
        public TokenManager _tokenManager;
        public VietstemSevice()
        {
            _client = new HttpClient();
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// Login Api
        /// </summary>
        public async Task<bool> LoginAsync(string user, string password)
        {
            var body = new
            {
                email = user,
                password = password
            };
            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, contentType);
            Debug.WriteLine(content.ToString());
            try
            {

                var response = await _client.PostAsync(Url, content);
                var resultLogin = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("======= Result Login ========\n" + resultLogin);

                    var loginResult = JsonConvert.DeserializeObject<VietStemDTO>(resultLogin);
                    // Save Id accessToken to file
                    _tokenManager.SaveToFile(loginResult.id);

                    MessageBox.Show("Đăng nhập thành công ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    return true;
                }

                else
                {
                    var errorrLogin = await response.Content.ReadAsStringAsync();
                    MessageBox.Show("===== Lỗi đăng nhập ==== \n" + errorrLogin);
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi đăng nhập" + ex.Message);
                return false;
            }
        }




        /// <summary>
        /// Get Data Api
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContestsDTO>> GetContestAsync(int pageSize, int currentPage)
        {
            string urlGetContest = "http://localhost:3000/api/Contests";

            /* set mặc định cho pageSize là 10 phần tử
             set mặc định cho currentPage là 1 
             offset =  bỏ qua bảo nhiêu phần tử 
             limit = PageSize lấy bao nhiêu phần tử muốn trả về 
                      {
                          "where": { "status": "open" },
                          "order": "name ASC",
                          "limit": 10,
                          "offset": 10
                       }
            */
            try
            {
                var fiterObject = new
                {
                    limit = pageSize,
                    offset = (currentPage - 1) * pageSize // lấy số page hiện tại trừ đi 1 và nhân với số phần tử 
                };

                var convertFilter = JsonConvert.SerializeObject(fiterObject);
                Debug.WriteLine("=====Filter Object =====\n" + convertFilter);

                string urlWithFilter = $"{urlGetContest}?filter={convertFilter}";

                var response = await _client.GetAsync(urlWithFilter);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("=====Result Get =====\n" + result);

                    var convertResult = JsonConvert.DeserializeObject<List<ContestsDTO>>(result);
                    Debug.WriteLine("Lấy dữ liệu thành công " + convertResult);
                    return convertResult;

                }
                else
                {
                    var errorrResult = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("====== Errorr Result Get Data========\n" + errorrResult);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiện tại đã xảy ra lỗi \n" + ex.Message);
                return null;
            }
        }


    }


}
