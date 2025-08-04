using CrudVietSteam.Service.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CrudVietSteam.ViewModel
{
    public class CityVM : ViewModelBase
    {
        private ObservableCollection<CityDTO> Citys;
        public ObservableCollection<CityDTO> CitysList
        {
            get => Citys;
            set
            {
                Citys = value;
                RaisePropertyChange(nameof(CitysList));
            }
        }
        public CityVM()
        {
            CitysList = new ObservableCollection<CityDTO>();
            // Giả sử bạn đã có một phương thức để lấy danh sách thành phố từ dịch vụ
            LoadCitys();
        }

        private async void LoadCitys()
        {
            var citys = await App.vietstemService.GetCityAsync();
            if (citys != null)
            {
                Citys.Clear();
                foreach (var city in citys)
                {
                    Citys.Add(city);
                } 

            }
        }
    }
}
