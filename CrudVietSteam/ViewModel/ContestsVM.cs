using CrudVietSteam.Service.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudVietSteam.ViewModel
{
    /// <summary>
    /// Get data Api display contest data
    /// </summary>
    public class ContestsVM 
    {
        public ObservableCollection<ContestsDTO> Contests { get; set; }

        public ContestsVM()
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
