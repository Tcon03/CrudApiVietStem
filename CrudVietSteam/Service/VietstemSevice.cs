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
                    _tokenManager.SaveToFile(loginResult.id);

                    MessageBox.Show("Đăng nhập thành công ");

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
        public async Task<List<ContestsDTO>> GetContestAsync()
        {
            string urlGetContest = "http://localhost:3000/api/Contests";
            try
            {

                var response = await _client.GetAsync(urlGetContest);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("=====Result Get =====\n" + result);

                    var convertResult = JsonConvert.DeserializeObject<List<ContestsDTO>>(result);

                    MessageBox.Show("Lấy dữ liệu thành công ");
                    //var convertResult = JsonConvert.DeserializeObject<List<ContestsDTO>>(result);
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
