using CrudVietSteam.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudVietSteam.Service.DTO
{
    public class CityDTO : ViewModelBase
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                RaisePropertyChange(nameof(IsChecked));
            }
        }
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
    public class CitySearch
    {
        public string Key { get; set; }
        public DateTime? CreatedTo { get; set; }
        public DateTime? CreatedForm { get; set; }
    }
}
