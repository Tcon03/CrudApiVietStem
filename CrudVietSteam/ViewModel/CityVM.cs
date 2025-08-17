using CrudVietSteam.Command;
using CrudVietSteam.Service.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace CrudVietSteam.ViewModel
{
    public class CityVM : PaggingVM
    {
        private bool? _isSelectedAll;
        public bool? IsAllSelected
        {
            get => _isSelectedAll;
            set
            {
                if (_isSelectedAll != value)
                {
                    _isSelectedAll = value;
                    Debug.WriteLine($"==== IsSelectAll changed to ======= : {_isSelectedAll}");
                    RaisePropertyChange(nameof(IsAllSelected));
                }
            }
        }
        public ObservableCollection<CityDTO> Citys { get; set; }
        public ICommand DeleteCityCommand { get; set; }
        public ICommand EditCityCommand { get; set; }
        public ICommand SeletedAllCommand { get; set; }
        public ICommand SeletedItemCommand { get; set; }
        public CityVM()
        {
            Citys = new ObservableCollection<CityDTO>();
            LoadData();
            DeleteCityCommand = new VfxCommand(OnDelete, () => true);
            EditCityCommand = new VfxCommand(OnEdit, () => true);
            SeletedAllCommand = new VfxCommand(OnSelectedAll, () => true);
            SeletedItemCommand = new VfxCommand(OnSelectedItem, () => true);
            IsAllSelected = false; // Khởi tạo IsAllSelected là false
        }

        private void OnSelectedItem(object obj)
        {

           
        }

        private void OnSelectedAll(object obj)
        {
            if (IsAllSelected == true)
            {
                foreach (var item in Citys)
                {
                    item.IsChecked = true;
                }
            }
            else
            {
                foreach (var item in Citys)
                {
                    item.IsChecked = false;
                }
            }

        }

        private void OnEdit(object obj)
        {
            var cityItem = obj as CityDTO;
            var cityEdit = new CityDTO
            {
                id = cityItem.id,
                name = cityItem.name,
                mtp = cityItem.mtp,
                createdAt = cityItem.createdAt,
                type = cityItem.type,
                updatedAt = cityItem.updatedAt
            };

        }

        private async void OnDelete(object obj)
        {
            var cityItem = obj as CityDTO;
            if (cityItem != null)
            {
                var reuslt = MessageBox.Show("Bạn có muốn xóa đối tượng này không ", "Thông báo ", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (reuslt == MessageBoxResult.Yes)
                {
                    Citys.Remove(cityItem);
                    await App.vietstemService.DeleteCityAsync(cityItem);
                    await LoadData();
                }
            }
        }

        public async void Searchity(CitySearch city)
        {
            var result = await App.vietstemService.SearchCityAsync(city);
            TotalRecords = result.Count;
            TotalPage = (int)Math.Ceiling((double)TotalRecords / PageSize);
            if (TotalPage == 0)
            {
                Debug.WriteLine("TotalPage is zero, no data to display.");
                return;
            }
            Citys.Clear();
            foreach (var item in result)
            {
                Citys.Add(item);
            }
            Debug.WriteLine($"Search results: {result.Count} records found.");
            RefreshPageCommand();
        }
        public override async Task LoadData()
        {
            try
            {
                TotalRecords = await App.vietstemService.GetCityCountAsync();
                if (TotalRecords == 0)
                {
                    Debug.WriteLine("No records found in CityVM");
                    return;
                }
                TotalPage = (int)Math.Ceiling((double)TotalRecords / PageSize);
                if (TotalPage == 0)
                {
                    Debug.WriteLine("TotalPage is zero, no data to display.");
                    return;
                }
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data in CityVM: {ex.Message}");
            }
            finally
            {
                RefreshPageCommand();
            }
        }


    }
}
