using CrudVietSteam.Command;
using CrudVietSteam.Service.DTO;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    public class CityEditVM : ViewModelBase
    {
        private CityDTO cityEdit;
        public CityDTO CityEdit
        {
            get => cityEdit;
            set
            {
                cityEdit = value;
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
            throw new NotImplementedException();
        }

        private async Task OnUpdate(object obj)
        {
            await App.vietstemService.UpdateCityAsync(CityEdit);
        }
    }
}