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

        public readonly string urlLogin = "http://localhost:3000/api/Accounts/login";
        public readonly string urlGetContest = "http://localhost:3000/api/Contests";


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

                var response = await _client.PostAsync(urlLogin, content);
                var resultLogin = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("======= Result Login ========\n" + resultLogin);

                    var loginResult = JsonConvert.DeserializeObject<LoginDTO>(resultLogin);
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
        /// Get Data Api Page Size and Current Page
        /// </summary>
        /// <returns></returns>
        public async Task<List<ContestsDTO>> GetContestAsync(int pageSize, int currentPage)
        {

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

                    var convertResult = JsonConvert.DeserializeObject<List<ContestsDTO>>(result);
                    Debug.WriteLine("Lấy dữ liệu thành công " + convertResult.Count);
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

        /// <summary>
        /// Get Count Data Api  
        /// </summary>
        public async Task<int> GetCountAsync()
        {
            string urlGetContest = "http://localhost:3000/api/Contests/count";
            var response = await _client.GetAsync(urlGetContest);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("=====Result Get Count =====\n" + result);
                // Chuyển đổi kết quả về kiểu int
                // C1 . int counts = int.Parse(result);
                var convertResult = JsonConvert.DeserializeObject<Item>(result);
                Debug.WriteLine("Lấy dữ liệu thành công " + convertResult.count);
                return convertResult.count; // Trả về số lượng bản ghi
            }
            else
            {
                var errorrResult = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("====== Errorr Result Get Count Data========\n" + errorrResult);
                return 0;
            }
        }

        /// <summary>
        /// Post Data Api reate contest
        /// </summary>
        public async Task<ContestsDTO> CreateContestAsync(ContestsDTO contest)
        {
            var pushContest = "http://localhost:3000/api/Contests?access_token=tnI0JmrNKKQuxIWuea3w6J4QCYCMFqOElnfISPiI6v6drlkWZhk2KdljNgH9fAdJ";
            var serializerSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

            var convertConterst = JsonConvert.SerializeObject(contest,serializerSettings);
            var content = new StringContent(convertConterst, Encoding.UTF8, contentType);
            var response = await _client.PostAsync(pushContest, content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("===== Result Create =====\n" + result);
                var convertResult = JsonConvert.DeserializeObject<ContestsDTO>(result);
                return convertResult;
            }
            else
            {
                var errorrResult = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("====== Errorr Result Create Data========\n" + errorrResult);
                return null;
            }
        }

        /// <summary>
        /// Update Data Api Update contest
        /// </summary>
        public async Task<ContestsDTO> UpdateContestAsync(ContestsDTO contestUpdate)
        {
            var convertConterst = JsonConvert.SerializeObject(contestUpdate);
            var content = new StringContent(convertConterst, Encoding.UTF8, contentType);
            string urlUpdate = $"{urlGetContest}/{contestUpdate.id}"; // Thêm ID vào URL để cập nhật
            var response = await _client.PutAsync(urlUpdate, content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("===== Result Update =====\n" + result);
                var convertResult = JsonConvert.DeserializeObject<ContestsDTO>(result);
                return convertResult;
            }
            else
            {
                var errorrResult = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("====== Errorr Result Update Data========\n" + errorrResult);
                return null;
            }
        }
    }


}
