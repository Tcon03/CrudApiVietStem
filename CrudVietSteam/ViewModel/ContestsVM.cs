using CrudVietSteam.Command;
using CrudVietSteam.Service.DTO;
using CrudVietSteam.View;
using CrudVietSteam.View.Windows;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    /// <summary>
    /// Get data Api display contest data
    /// </summary>
    public class ContestsVM : PaggingVM
    {

        #region Properties
        //private int _pageSize = 15; // Số lượng bản ghi trên mỗi trang 
        //public int PageSize
        //{
        //    get { return _pageSize; }
        //    set
        //    {
        //        if (_pageSize != value)
        //        {
        //            _pageSize = value;
        //            //OnLoad(); // Tải lại dữ liệu khi thay đổi PageSize 
        //            Debug.WriteLine("PageSize changed to: " + _pageSize);
        //            RaisePropertyChange(nameof(PageSize)); // Thông báo thay đổi thuộc tính PageSize
        //        }
        //    }
        //}
        //private int _currentPage = 1; // Trang hiện tại 
        //public int CurrentPage
        //{
        //    get { return _currentPage; }
        //    set
        //    {
        //        if (_currentPage != value)
        //        {
        //            _currentPage = value;
        //            Debug.WriteLine("CurrentPage changed to: " + _currentPage);
        //            OnLoad(); // Tải lại dữ liệu khi thay đổi CurrentPage  
        //            RaisePropertyChange(nameof(CurrentPage));
        //        }
        //    }
        //}
        //private int _totalPage; // Tổng số trang, mặc định là 1 
        //public int TotalPage
        //{
        //    get { return _totalPage; }
        //    set
        //    {
        //        if (_totalPage != value)
        //        {
        //            _totalPage = value;
        //            Debug.WriteLine("TotalPage changed to: " + _totalPage);
        //            RaisePropertyChange(nameof(TotalPage));
        //        }
        //    }
        //}
        //private int _totalRecords = 0; // Tổng số bản ghi, mặc định là 0 vì chưa có gửi api lên  
        //public int TotalRecords
        //{
        //    get
        //    {
        //        return _totalRecords;
        //    }
        //    set
        //    {
        //        if (_totalRecords != value)
        //        {
        //            _totalRecords = value;
        //            Debug.WriteLine("TotalRecords changed to: " + _totalRecords);
        //            RaisePropertyChange(nameof(TotalRecords)); // Thông báo thay đổi thuộc tính TotalRecords
        //        }
        //    }
        //}

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    Debug.WriteLine("Change Name :" + value);
                    RaisePropertyChange(nameof(Name));
                }
            }
        }


        private string _introduce;
        public string Introduce
        {
            get { return _introduce; }
            set
            {
                if (_introduce != value)
                {
                    _introduce = value;
                    RaisePropertyChange(nameof(Introduce));
                }
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChange(nameof(Status));
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                Debug.WriteLine("Value thay đổi nè " + value);
                RaisePropertyChange(nameof(Description));
            }
        }
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                Debug.WriteLine("Value thay đổi nè " + value);
                RaisePropertyChange(nameof(Title));
            }
        }
        private string _keyword;
        public string Keywords
        {
            get
            {
                return _keyword;
            }
            set
            {
                _keyword = value;
                Debug.WriteLine("Value thay đổi nè " + value);
                RaisePropertyChange(nameof(Keywords));
            }
        }


        #endregion

        private ObservableCollection<ContestsDTO> _contests;
        public ObservableCollection<ContestsDTO> Contests
        {
            get { return _contests; }
            set
            {
                if (_contests != value)
                {
                    _contests = value;
                    RaisePropertyChange(nameof(Contests));
                }
            }
        }

        #region Commands
        public ICommand AddContestCommand { get; set; }
        public ICommand EditContestCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        #endregion

        public EventHandler AddSuccess;


        public ContestsVM()
        {
            Contests = new ObservableCollection<ContestsDTO>();
            InitializeCommands();
            LoadData(); // Gọi hàm LoadData để tải dữ liệu khi khởi tạo ViewModel
        }

        private void InitializeCommands()
        {
            AddContestCommand = new VfxCommand(AddContest, () => true);
            EditContestCommand = new VfxCommand(OnEdit, () => true);
            DeleteItemCommand = new VfxCommand(OnDelete, () => true);
        }



        public async void SearchContest(ContestSearch filter)
        {
            var dataSearch = await App.vietstemService.SeachContestAsync(filter);
            if (dataSearch == null)
            {
                return;
            }
            TotalRecords = dataSearch.Count;
            TotalPage = (int)Math.Ceiling((double)TotalRecords / PageSize);
            Contests.Clear();
            foreach (var item in dataSearch)
            {
                Contests.Add(item);
            }
            Debug.WriteLine("Count sau Add: " + Contests.Count); // = dataSearch.Count
        }


        private async void OnDelete(object obj)
        {
            var data = obj as ContestsDTO;
            if (data != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa cuộc thi này không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Contests.Remove(data);
                    await App.vietstemService.DeleteContestAsync(data); // Gọi API để xóa cuộc thi
                    LoadData();
                }
            }
        }

        private void OnEdit(object obj)
        {
            var contestObj = obj as ContestsDTO;
            if (contestObj != null)
            {
                var contestEdit = new ContestsDTO
                {
                    // gán đối tượng vừa được select cho các trường dữ liệu của ContestDTO
                    id = contestObj.id,
                    name = contestObj.name,
                    introduce = contestObj.introduce,
                    rule = contestObj.rule,
                    guide = contestObj.guide,
                    fromGrade = contestObj.fromGrade,
                    toGrade = contestObj.toGrade,
                    status = contestObj.status,
                    description = contestObj.description,
                    title = contestObj.title,
                    keywords = contestObj.keywords,
                    accountId = contestObj.accountId,
                    cityId = contestObj.cityId,
                    createdAt = contestObj.createdAt,
                    updatedAt = DateTime.Now
                };

                // gọi và truyền các đối tượng này cho class EditContestVM
                EditContestVM editVM = new EditContestVM(contestEdit);
                // show cửa sổ và truyền dataContext cho cửa sổ này 
                EditContest edit = new EditContest
                {
                    DataContext = editVM // Gán DataContext cho cửa sổ EditContest Để sử dụng các thuộc tính chỉnh sửa ở đấy 
                };
                edit.ShowDialog(); // Hiển thị cửa sổ EditContest 

                LoadData();

            }
        }

        public async void AddContest(object obj)
        {

            if ((string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Introduce) ||
                  string.IsNullOrEmpty(Status) || string.IsNullOrEmpty(Description) ||
                    string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Keywords)))
            {
                MessageBox.Show("Vui lòng nhập thông tin đầy đủ không được để trống ");
                return;
            }

            var addContestInfor = new ContestsDTO
            {
                name = Name,
                introduce = Introduce,
                status = Status,
                description = Description,
                title = Title,
                keywords = Keywords,
                rule = string.Empty,
                guide = string.Empty,
                fromGrade = 0,
                toGrade = 0,
                accountId = string.Empty,
                cityId = 0,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };


            var result = await App.vietstemService.CreateContestAsync(addContestInfor);
            MessageBox.Show("Thêm dữ liệu thành công !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result != null)
            {
                Contests.Clear();
                LoadData();

                if (obj is Window win)
                {
                    win.Close();
                }
            }
            else
            {
                MessageBox.Show("Thêm cuộc thi thất bại.");
            }


        }


        public override async void LoadData()
        {
            try
            {
                //1. Get Total Records from Api
                TotalRecords = await App.vietstemService.GetCountContestAsync();

                if (TotalRecords > 0)
                {
                    //  Amount TotalRecord / Amount PageSize (30/1)
                    TotalPage = (int)Math.Ceiling((double)TotalRecords / PageSize); // Tính tổng số trang dựa trên tổng số bản ghi và PageSize
                    //2. Get dữ liệu từ API với PageSize và CurrentPage
                    var dataApi = await App.vietstemService.GetDataContest(PageSize, CurrentPage);
                    if (dataApi == null)
                    {
                        Debug.WriteLine("Không có dữ liệu để hiển thị.");
                        return;
                    }

                    Contests.Clear();
                    foreach (var item in dataApi)
                    {
                        Contests.Add(item);
                    }
                }
                else
                {
                    Debug.WriteLine("Không có bản ghi nào để hiển thị.");
                    TotalPage = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi khi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                RefreshPageCommand();
            }
        }


    }
}
