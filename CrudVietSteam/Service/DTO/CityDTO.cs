using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudVietSteam.Service.DTO
{
    public class CityDTO
    {
        public string mtp { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
    public class ItemCount
    {
        public int count { get; set; }
    }
}
