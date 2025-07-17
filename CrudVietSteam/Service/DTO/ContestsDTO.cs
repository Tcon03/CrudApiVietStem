using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudVietSteam.Service.DTO
{
    public class ContestsDTO
    {
        public string name { get; set; }
        public string introduce { get; set; }
        public string rule { get; set; }
        public string guide { get; set; }
        public int fromGrade { get; set; }
        public int toGrade { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public string keywords { get; set; }
        public int id { get; set; }
        public string accountId { get; set; }
        public int cityId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
