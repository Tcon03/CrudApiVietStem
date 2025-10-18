using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudVietSteam.Model
{
    public class ApiConfiguration
    {
        //ip local

        public string BaseUrl { get; set; } = "http://localhost:3000/explorer/";

        //ip vfast 
        //public string BaseUrl { get; set; } = "http://192.168.10.78:3000/explorer/";

        public string LoginEndpoint { get; set; } = "/api/Accounts/login";
        public string GetContestEndpoint { get; set; } = "/api/Contests";
        public string GetCityEndpoint { get; set; } = "/api/Cities";
        public string ContentType { get; set; } = "application/json";

    }
}
