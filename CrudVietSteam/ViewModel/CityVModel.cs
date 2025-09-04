using CrudVietSteam.Command;
using CrudVietSteam.Service.DTO;
using CrudVietSteam.View.Windows;
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
    public class CityVModel : PaggingVM
    {

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    Debug.WriteLine($"==== Name changed to ======= : {_name}");
                    RaisePropertyChange(nameof(Name));
                }
            }
        }
        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    Debug.WriteLine($"==== Type changed to ======= : {_type}");
                    RaisePropertyChange(nameof(Type));
                }
            }
        }
        private string _mtp;
        public string Mtp
        {
            get => _mtp;
            set
            {
                if (_mtp != value)
                {
                    _mtp = value;
                    Debug.WriteLine($"==== Mtp changed to ======= : {_mtp}");
                    RaisePropertyChange(nameof(Mtp));
                }
            }
        }

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

        public bool IsAnyItemSelected
        {
            get => Citys != null && Citys.Any(c => c.IsChecked);

        }
        public ObservableCollection<CityDTO> Citys { get; set; }
        public ICommand DeleteCityCommand { get; set; }
        public ICommand EditCityCommand { get; set; }
        public ICommand SeletedAllCommand { get; set; }
        public ICommand SeletedItemCommand { get; set; }
        public ICommand AddCityCommand { get; set; }
        public ICommand DeleteAllItem { get; set; }
        public CityVModel()
        {
            Citys = new ObservableCollection<CityDTO>();
            LoadData();
            DeleteCityCommand = new VfxCommand(OnDelete, () => true);
            EditCityCommand = new VfxCommand(OnEdit, () => true);
            SeletedAllCommand = new VfxCommand(OnSelectedAll, () => true);
            SeletedItemCommand = new VfxCommand(OnSelectedItem, () => true);
            IsAllSelected = false; // Khởi tạo IsAllSelected là false 
            AddCityCommand = new VfxCommand(OnAddCity, () => true);
            DeleteAllItem = new VfxCommand(OnDeleteAll, () => true);
        }

        private void OnDeleteAll(object obj)
        {
                var result = MessageBox.Show("Bạn có muốn xóa toàn bộ dữ liệu không ", "Thông báo ", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    var allItem = Citys.Where(x => x.IsChecked).ToList(); 

                }

        }

        private async void OnAddCity(object obj)
        {

            var cityAdd = new CityDTO
            {
                name = Name,
                type = Type,
                mtp = Mtp,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };
            var result = await App.vietstemService.CreateCityAsync(cityAdd);


            if (result != null)
            {
                MessageBox.Show("Thêm thành phố thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Citys.Add(result);
                await LoadData();
                if (obj is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
                Debug.WriteLine($"City added successfully: {result.name}");
            }
            else
            {
                Debug.WriteLine("Failed to add city.");
                MessageBox.Show("Thêm thành phố thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnSelectedItem(object obj)
        {
            if (Citys.All(c => c.IsChecked))
            {
                IsAllSelected = true;
            }
            else if (Citys.All(c => !c.IsChecked))
            {
                IsAllSelected = false;
            }
            else
            {
                IsAllSelected = null;
            }
            RaisePropertyChange(nameof(IsAnyItemSelected));
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
            RaisePropertyChange(nameof(IsAnyItemSelected));

        }

        private async void OnEdit(object obj)
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

            CityEditVM ctVm = new CityEditVM(cityItem);
            CityEditView ct = new CityEditView
            {
                DataContext = ctVm
            };
            ct.ShowDialog();
            await LoadData();

        }

        private async void OnDelete(object obj)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Hiện tại đang lỗi !!" + ex, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
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
