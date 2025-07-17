using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudVietSteam.Service.DTO
{
    public class VietStemDTO
    {
        public string id { get; set; }
        public int ttl { get; set; }
        public DateTime created { get; set; }
        public string userId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
    public class DisplayResponse
    {
        public string _id { get; set; }
        public int _ttl { get; set; }
        public DateTime _created { get; set; }
        public string _userId { get; set; }
        public DateTime  _createdAt { get; set; }
        public DateTime _updatedAt { get; set; }
    }
}
