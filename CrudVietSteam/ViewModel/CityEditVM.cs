using CrudVietSteam.Command;
using CrudVietSteam.Service.DTO;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    /// <summary>
    /// Class for editing Button Update City in the application.
    /// </summary>
    public class CityEditVM : ViewModelBase
    {
        private CityDTO cityEdit;
        public CityDTO CityEdit
        {
            get => cityEdit;
            set
            {
                cityEdit = value;
                Debug.WriteLine($"==== CityEdit changed to ======= : {cityEdit}");
                RaisePropertyChange(nameof(CityEdit)); // Uncomment if you have a RaisePropertyChange method to notify the view of changes
            }
        }

        public ICommand UpdateCommand { get; set; }
        public CityEditVM(CityDTO cityEdit)
        {
            CityEdit = cityEdit;
            UpdateCommand = new VfxCommand(OnUpdate, CanUpdate);
        }

        private bool CanUpdate()
        {
            if (!string.IsNullOrWhiteSpace(CityEdit.name) &&
               !string.IsNullOrWhiteSpace(CityEdit.type) &&
               !string.IsNullOrWhiteSpace(CityEdit.mtp))
                return true;
            MessageBox.Show("Vui lòng nhập đầy đủ thông tin không được bỏ trống !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        /// <summary>
        /// Updatate City Edit Command
        /// </summary>
        /// <param name="obj"></param>
        private async void OnUpdate(object obj)
        {
            await App.vietstemService.UpdateCityAsync(CityEdit);
            MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            if (obj is Window wn)
            {
                wn.DialogResult = true;
                wn.Close(); // Đóng cửa sổ sau khi cập nhật thành công
            }
        }
    }
}