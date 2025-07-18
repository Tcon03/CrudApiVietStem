using CrudVietSteam.Command;
using CrudVietSteam.Service;
using CrudVietSteam.Service.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    public class ContestVM : ViewModelBase
    {
        public ObservableCollection<ContestsDTO> Contests { get; set; }

        public ICommand LoadDataContest { get; set; }
        public ContestVM()
        {
            Contests = new ObservableCollection<ContestsDTO>();
            OnLoad();
        }

        private async void OnLoad()
        {
            var data = await App.vietstemService.GetContestAsync();
            Contests.Clear();
            foreach (var item in data)
            {
                Contests.Add(item);
            }
        }
    }
}
