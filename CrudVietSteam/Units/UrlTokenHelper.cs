using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudVietSteam.Units
{
    public class UrlTokenHelper
    {
        public string AddAccessToken(string url, string token)
        {
            //1. check url null or empty
            if (string.IsNullOrEmpty(token))
            {
                Debug.WriteLine("Token is null or empty, returning original URL");
                return url;
            }

            //2. Check if URL already contains a query string
            if (url.Contains("access_token="))
            {
                return url;
            }

            string urlWithToken = $"{url}?access_token={token}";
            Debug.WriteLine($"URL with token: {urlWithToken}");
            return urlWithToken;
        }

    }
}
