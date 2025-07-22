using CrudVietSteam.Command;
using CrudVietSteam.Service.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    /// <summary>
    /// Get data Api display contest data
    /// </summary>
    public class ContestsVM : ViewModelBase
    {
        private int _pageSize = 10; // Số lượng bản ghi trên mỗi trang 
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    //OnLoad(); // Tải lại dữ liệu khi thay đổi PageSize 
                    Debug.WriteLine("PageSize changed to: " + _pageSize);
                    RaisePropertyChange(nameof(PageSize)); // Thông báo thay đổi thuộc tính PageSize
                }
            }
        }
        private int _currentPage = 1; // Trang hiện tại 
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    Debug.WriteLine("CurrentPage changed to: " + _currentPage);
                    //OnLoad(); // Tải lại dữ liệu khi thay đổi CurrentPage n 
                    RaisePropertyChange(nameof(CurrentPage)); // Thông báo thay đổi thuộc tính CurrentPage
                }
            }
        }
        public ObservableCollection<ContestsDTO> Contests { get; set; }

        public ICommand NextPageCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ContestsVM()
        {
            Contests = new ObservableCollection<ContestsDTO>();
            NextPageCommand = new VfxCommand(OnNextPage, CanNextPage);
            PrevPageCommand = new VfxCommand(OnPrevPage, CanPrevPage);
            OnLoad();
        }

        private bool CanPrevPage(object arg)
        {
            // Chỉ có thể lùi khi không phải ở trang đầu tiên
            return CurrentPage > 1;
        }

        private void OnPrevPage(object obj)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--; // Giảm trang hiện tại nếu không phải trang đầu tiên
                OnLoad(); // Tải lại dữ liệu khi chuyển về trang trước
            }
            else
            {
                Debug.WriteLine("Đã ở trang đầu tiên, không thể chuyển về trang trước.");
            }
        }

        private bool CanNextPage(object arg)
        {
            if (Contests == null || Contests.Count == 0)
            {
                return false; // Không có dữ liệu để chuyển trang
            }
            // Kiểm tra nếu có dữ liệu để chuyển sang trang tiếp theo
            return Contests.Count >= PageSize; // Nếu số lượng bản ghi hiện tại >= PageSize thì có thể chuyển trang
        }

        private void OnNextPage(object obj)
        {
            CurrentPage++;
            OnLoad(); // Tải lại dữ liệu khi chuyển sang trang tiếp theo
        }

        private async void OnLoad()
        {
            var data = await App.vietstemService.GetContestAsync(PageSize, CurrentPage);
            if (data == null)
            {
                // Nếu không có dữ liệu, có thể hiển thị thông báo hoặc xử lý khác
                return;
            }
            Contests.Clear();
            foreach (var item in data)
            {
                Contests.Add(item);
            }
        }
    }
}
