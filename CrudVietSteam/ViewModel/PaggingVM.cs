using CrudVietSteam.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    public abstract class PaggingVM : ViewModelBase
    {
        #region
        private int _totalRecords = 0;
        public int TotalRecords
        {
            get => _totalRecords;
            set
            {
                int oldValue = _totalRecords;
                _totalRecords = value;
                Debug.WriteLine($" --------- [Debug] Total Records Changed ---------:\n {oldValue} => {_totalRecords} ");
                RaisePropertyChange(nameof(TotalRecords));
            }
        }

        private int _totalPage = 0;
        public int TotalPage
        {
            get => _totalPage;
            set
            {
                int oldValue = _totalPage;
                _totalPage = value;
                Debug.WriteLine($" ++++++++++++++ [Debug] Total Page Records Changed +++++++++  \n: {oldValue} => {_totalPage}");
                RaisePropertyChange(nameof(TotalPage));
                RefreshPageCommand();
            }
        }

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    int oldValue = _currentPage;
                    _currentPage = value;
                    Debug.WriteLine($"======== [Debug] Current Page Records Changed ========:\n {oldValue} => {_currentPage}");
                    LoadData();
                    RaisePropertyChange(nameof(CurrentPage));
                }

            }
        }

        private int _pageSize = 15;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                int oldValue = _pageSize;
                _pageSize = value;
                Debug.WriteLine($"********** [Debug] Page Size Changed **********:\n {oldValue} => {_pageSize}");
                RaisePropertyChange(nameof(PageSize));
            }
        }
        #endregion 
        public ICommand NextPage { get; set; }
        public ICommand PreviousPage { get; set; }
        public PaggingVM()
        {
            NextPage = new VfxCommand(OnNext, CanNextPage);
            PreviousPage = new VfxCommand(OnPreviousPage, CanPrevi);
        }

        public void OnPreviousPage(object obj)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                Debug.WriteLine($"[Debug] Previous Page: {CurrentPage}");
            }
        }

        public bool CanPrevi()
        {
            return CurrentPage > 1;
        }

        public void OnNext(object obj)
        {
            if (CurrentPage < TotalPage)
            {
                CurrentPage++;
                Debug.WriteLine($"[Debug] Next Page: {CurrentPage}");
            }

        }

        public bool CanNextPage()
        {
            return CurrentPage < TotalPage;
        }

        public void RefreshPageCommand()
        {
            (NextPage as VfxCommand)?.RaiseCanExecuteChanged();
            (PreviousPage as VfxCommand)?.RaiseCanExecuteChanged();
        }
        public abstract void LoadData();
    }
}
