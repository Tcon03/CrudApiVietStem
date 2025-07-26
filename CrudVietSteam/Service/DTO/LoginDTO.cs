using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudVietSteam.Service.DTO
{
    public class LoginDTO
    {
        public string id { get; set; }
        public int ttl { get; set; }
        public DateTime created { get; set; }
        public string userId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
  
}
