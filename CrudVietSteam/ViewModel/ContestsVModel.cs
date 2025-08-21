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
    public class ContestsVModel : PaggingVM
    {

        #region Properties


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


        public ContestsVModel()
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
                    await LoadData();
                }
            }
        }
        /// <summary>
        /// Edit Contest Command
        /// </summary>
        private async void OnEdit(object obj)
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
                Debug.WriteLine("=====Contest edit Change ===== :\n" + contestEdit.ToString());
                Debug.WriteLine("=====ContestObj Change ===== :\n" + contestObj.ToString());

                // gọi và truyền các đối tượng này cho class EditContestVM
                EditContestVM editVM = new EditContestVM(contestEdit);
                // show cửa sổ và truyền dataContext cho cửa sổ này 
                EditContest viewEdit = new EditContest();
                viewEdit.DataContext = editVM; // Gán DataContext cho cửa sổ EditContest
                viewEdit.ShowDialog(); // Hiển thị cửa sổ EditContest 

                await LoadData();

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
                await LoadData();

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


        public override async Task LoadData()
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
