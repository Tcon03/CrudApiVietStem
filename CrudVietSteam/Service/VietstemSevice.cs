using CrudVietSteam.Model;
using CrudVietSteam.Service.DTO;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Management;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace CrudVietSteam.Service
{
    public class VietstemSevice
    {
        public readonly ApiConfiguration _config;


        public readonly HttpClient _client;
        public TokenManager _tokenManager;
        public VietstemSevice()
        {
            _config = new ApiConfiguration();
            _client = new HttpClient();
            _tokenManager = new TokenManager();
            _client.BaseAddress = new Uri(_config.BaseUrl);
        }



        #region Method Get, Post, Put, Delete
        /// <summary>
        /// Get Data Api
        /// </summary>
        public async Task<T> GetDataAsync<T>(string url)
        {
            try
            {
                var accessToken = _tokenManager.LoadToken();
                var response = await _client.GetAsync($"{url}?access_token={accessToken}");
                Debug.WriteLine("======= GetData ====== \n" + response);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("===== Result Get Data =====\n" + result);

                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    var errorResult = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("====== Error Result Get Data ========\n" + errorResult);
                    return default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiện tại đã xảy ra lỗi get dữ liệu \n" + ex.Message);
                return default;
            }
        }

        /// <summary>
        /// Post Data Api - Create Data
        /// </summary>
        public async Task<T> PostData<T>(string url, object obj)
        {
            try
            {
                //1 cần endpoint của phần nào thì truyền vào url 
                var accessToken = _tokenManager.LoadToken();
                string urlWithToken = $"{url}?access_token={accessToken}";

                //2. cần truyền vào object obj để post dữ liệu lên server và chuyển đổi sang chuỗi json
                var convertObj = JsonConvert.SerializeObject(obj);
                var jsonContent = new StringContent(convertObj, Encoding.UTF8, _config.ContentType);

                //3 . Gửi Request Post lên server
                var response = await _client.PostAsync(urlWithToken, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("===== Result Post Data =====\n" + result);

                    //4. Chuyển đổi kết quả trả về từ server sang kiểu T 
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    var errorResult = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("====== Error Result Post Data ========\n" + errorResult);
                    return default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiện tại đã xảy ra lỗi post dữ liệu \n" + ex.Message);
                return default;
            }
        }
        /// <summary>
        /// Update Data Api
        /// </summary>
        public async Task<T> PutData<T>(string url, object obj)
        {
            try
            {
                //1 cần endpoint của phần nào thì truyền vào url 
                var accessToken = _tokenManager.LoadToken();
                string urlWithToken = $"{url}?access_token={accessToken}";
                //2. cần truyền vào object obj để post dữ liệu lên server và chuyển đổi sang chuỗi json
                var convertObj = JsonConvert.SerializeObject(obj);
                var jsonContent = new StringContent(convertObj, Encoding.UTF8, _config.ContentType);
                //3 . Gửi Request Post lên server
                var response = await _client.PutAsync(urlWithToken, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("===== Result Put Data =====\n" + result);
                    //4. Chuyển đổi kết quả trả về từ server sang kiểu T 
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    var errorResult = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("====== Error Result Put Data ========\n" + errorResult);
                    return default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiện tại đã xảy ra lỗi put dữ liệu \n" + ex.Message);
                return default;
            }
        }

        public async Task<T> DeleteData<T>(string url)
        {
            try
            {
                //1 cần endpoint của phần nào thì truyền vào url 
                var accessToken = _tokenManager.LoadToken();
                string urlWithToken = $"{url}?access_token={accessToken}";
                //2. Gửi Request Delete lên server
                var response = await _client.DeleteAsync(urlWithToken);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("===== Result Delete Data =====\n" + result);
                    MessageBox.Show("Xóa dữ liệu thành công !!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    //3. Chuyển đổi kết quả trả về từ server sang kiểu T 
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    var errorResult = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("====== Error Result Delete Data ========\n" + errorResult);
                    return default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiện tại đã xảy ra lỗi delete dữ liệu \n" + ex.Message);
                return default;
            }
        }

        #endregion

        /// <summary>
        /// Post Login Api 
        /// </summary>
        public async Task<bool> LoginAsync(string user, string password)
        {
            var body = new
            {
                email = user,
                password = password
            };
            var loginResult = await PostData<LoginDTO>(_config.LoginEndpoint, body);
            Debug.WriteLine("======= Login Result ========\n" + loginResult);
            if (loginResult != null)
            {

                _tokenManager.SaveToFile(loginResult.id);
                MessageBox.Show("Đăng nhập thành công ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else
            {
                MessageBox.Show("===== Lỗi đăng nhập ==== \n Vui lòng kiểm tra lại tài khoản và mật khẩu !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            //var content = new StringContent(jsonBody, Encoding.UTF8, _config.ContentType);
            //try
            //{

            //    var response = await _client.PostAsync(_config.LoginEndpoint, content);
            //    var resultLogin = await response.Content.ReadAsStringAsync();
            //    if (response.IsSuccessStatusCode)
            //    {
            //        Debug.WriteLine("======= Result Login ========\n" + resultLogin);

            //        var loginResult = JsonConvert.DeserializeObject<LoginDTO>(resultLogin);
            //        // Save Id accessToken to file
            //        _tokenManager.SaveToFile(loginResult.id);

            //        MessageBox.Show("Đăng nhập thành công ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            //        return true;
            //    }

            //    else
            //    {
            //        var errorrLogin = await response.Content.ReadAsStringAsync();
            //        MessageBox.Show("===== Lỗi đăng nhập ==== \n" + errorrLogin);
            //        return false;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Đã xảy ra lỗi đăng nhập" + ex.Message);
            //    return false;
            //}
        }
        public async Task<ObservableCollection<ContestsDTO>> GetDataContest(int pageSize, int currentPage)
        {
            Debug.WriteLine("Đang truy cập vào hàm GetContest Pagging");
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
                var accessToken = _tokenManager.LoadToken();

                var fiterObject = new
                {
                    limit = pageSize,
                    offset = (currentPage - 1) * pageSize // lấy số page hiện tại trừ đi 1 và nhân với số phần tử 
                };
                var convertFilter = JsonConvert.SerializeObject(fiterObject);
                string urlWithFilter = $"{_config.GetContestEndpoint}?filter={convertFilter}&access_token={accessToken}";
                var resultPagging = await GetDataAsync<ObservableCollection<ContestsDTO>>(urlWithFilter);
                return resultPagging;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Hiện tại đã xảy ra lỗi get dữ liệu \n" + ex.Message);
                return null;
            }
            //var convertFilter = JsonConvert.SerializeObject(fiterObject);

            //string urlWithFilter = $"{_config.GetContestEndpoint}?filter={convertFilter}";

            //var response = await _client.GetAsync(urlWithFilter);
            //if (response.IsSuccessStatusCode)
            //{
            //    var result = await response.Content.ReadAsStringAsync();
            //    Debug.WriteLine("Result Get Count Pagging \n" + result);
            //    var convertResult = JsonConvert.DeserializeObject<ObservableCollection<ContestsDTO>>(result);
            //    return convertResult;

            //}
            //else
            //{
            //    var errorrResult = await response.Content.ReadAsStringAsync();
            //    Debug.WriteLine("====== Errorr Result Get Data========\n" + errorrResult);
            //    return null;
            //}
        }

        /// <summary>
        /// Get Count Data Api  
        /// </summary>
        public async Task<int> GetCountContestAsync()
        {
            try
            {
                var urlGetCount = $"{_config.GetContestEndpoint}/count";
                var resultConunt = await GetDataAsync<Item>(urlGetCount);
                Debug.WriteLine("===== Result Get Count =====\n" + resultConunt.count);
                return resultConunt.count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiện tại đã xảy ra lỗi get dữ liệu \n" + ex.Message);
                return 0;
            }

            //string urlGetContest = "http://localhost:3000/api/Contests/count";
            //var response = await _client.GetAsync(urlGetContest);
            //if (response.IsSuccessStatusCode)
            //{
            //    var result = await response.Content.ReadAsStringAsync();
            //    Debug.WriteLine("=====Result Get Count =====\n" + result);
            //    // Chuyển đổi kết quả về kiểu int
            //    // C1 . int counts = int.Parse(result);
            //    var convertResult = JsonConvert.DeserializeObject<Item>(result);
            //    Debug.WriteLine("Lấy dữ liệu thành công " + convertResult.count);
            //    return convertResult.count; // Trả về số lượng bản ghi
            //}
            //else
            //{
            //    var errorrResult = await response.Content.ReadAsStringAsync();
            //    Debug.WriteLine("====== Errorr Result Get Count Data========\n" + errorrResult);
            //    return 0;
            //}
        }

        /// <summary>
        /// Post Data Api reate contest
        /// </summary>
        public async Task<ContestsDTO> CreateContestAsync(ContestsDTO contest)
        {
            try
            {
                var resultContest = await PostData<ContestsDTO>(_config.GetContestEndpoint, contest);
                Debug.WriteLine("===== Result Create Contest =====\n" + resultContest);
                return resultContest;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiện tại đã xảy ra lỗi tạo dữ liệu \n" + ex.Message);
                return null;
            }
            //var convertConterst = JsonConvert.SerializeObject(contest);
            //Debug.WriteLine("Request gửi lên có định dạng như sau nhé :\n" + convertConterst);
            //var content = new StringContent(convertConterst, Encoding.UTF8, _config.ContentType);
            //var response = await _client.PostAsync(pushContest, content);
            //if (response.IsSuccessStatusCode)
            //{
            //    var result = await response.Content.ReadAsStringAsync();
            //    var convertResult = JsonConvert.DeserializeObject<ContestsDTO>(result);
            //    MessageBox.Show("Thêm dữ liệu thành công !!");
            //    return convertResult;
            //}
            //else
            //{
            //    var errorrResult = await response.Content.ReadAsStringAsync();
            //    Debug.WriteLine("====== Errorr Result Create Data========\n" + errorrResult);
            //    return null;
            //}
        }

        /// <summary>
        /// Update Data Api Update contest
        /// </summary>
        public async Task<ContestsDTO> UpdateContestAsync(ContestsDTO contestUpdate)
        {
            string urlUpdate = $"{_config.GetContestEndpoint}/{contestUpdate.id}"; // Thêm ID vào URL để cập nhật

            var response = await PutData<ContestsDTO>(urlUpdate, contestUpdate);
            Debug.WriteLine("===== Result Update Contest =====\n" + response.name);
            return response;
        }
        public async Task<ContestsDTO> DeleteContestAsync(ContestsDTO contest)
        {
            try
            {
                string urlDelete = $"{_config.GetContestEndpoint}/{contest.id}";
                var response = await DeleteData<ContestsDTO>(urlDelete);
                Debug.WriteLine("===== Result Delete Contest =====\n" + response.id);
                return response;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiện tại đã xảy ra lỗi delete dữ liệu \n" + ex.Message);
                return null;
            }
        }



        public async Task<ObservableCollection<CityDTO>> GetCityAsync()
        {
            try
            {
                string url = $"{_config.GetCityEndpoint}"; // Endpoint để lấy danh sách thành phố  
                var response = await GetDataAsync<ObservableCollection<CityDTO>>(url);
                Debug.WriteLine("===== Result Get City =====\n" + response);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiện tại đã xảy ra lỗi get dữ liệu thành phố \n" + ex.Message);
                return null;
            }
        }


    }


}
