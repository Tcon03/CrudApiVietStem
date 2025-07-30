//using CrudVietSteam.Command;
//using CrudVietSteam.Service.DTO;
//using CrudVietSteam.View.Windows;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using System.Windows;
//using System.Windows.Input;

//public class ContestsVM : INotifyPropertyChanged
//{
//    #region Private Fields
//    private ContestsDTO _selectedContest;
//    private ContestsDTO _editingContest; // Thêm property riêng cho editing
//    private bool _isLoading;
//    private bool _isEditMode;
//    #endregion

//    #region Public Properties
//    public ContestsDTO SelectedContest
//    {
//        get { return _selectedContest; }
//        set
//        {
//            _selectedContest = value;
//            RaisePropertyChange(nameof(SelectedContest));
//            // Refresh command states
//            ((VfxCommand)EditContestCommand).RaiseCanExecuteChanged();
//        }
//    }

//    // Property riêng cho việc editing - tách biệt khỏi SelectedContest
//    public ContestsDTO EditingContest
//    {
//        get { return _editingContest; }
//        set
//        {
//            _editingContest = value;
//            RaisePropertyChange(nameof(EditingContest));
//        }
//    }

//    public bool IsLoading
//    {
//        get { return _isLoading; }
//        set
//        {
//            _isLoading = value;
//            RaisePropertyChange(nameof(IsLoading));
//        }
//    }

//    public bool IsEditMode
//    {
//        get { return _isEditMode; }
//        set
//        {
//            _isEditMode = value;
//            RaisePropertyChange(nameof(IsEditMode));
//        }
//    }

//    public ObservableCollection<ContestsDTO> Contests { get; set; }
//    #endregion

//    #region Commands
//    public ICommand NextPageCommand { get; set; }
//    public ICommand PrevPageCommand { get; set; }
//    public ICommand AddContestCommand { get; set; }
//    public ICommand EditContestCommand { get; set; }
//    public ICommand SaveCommand { get; set; }
//    public ICommand CancelEditCommand { get; set; }
//    public ICommand DeleteContestCommand { get; set; }
//    #endregion

//    #region Events
//    public EventHandler AddSuccess;
//    public EventHandler EditSuccess;
//    #endregion

//    #region Constructor
//    public ContestsVM()
//    {
//        InitializeProperties();
//        InitializeCommands();
//        OnLoad();
//    }

//    private void InitializeProperties()
//    {
//        Contests = new ObservableCollection<ContestsDTO>();
//        IsLoading = false;
//        IsEditMode = false;
//    }

//    private void InitializeCommands()
//    {
//        NextPageCommand = new VfxCommand(OnNextPage, CanNextPage);
//        PrevPageCommand = new VfxCommand(OnPrevPage, CanPrevPage);
//        AddContestCommand = new VfxCommand(o => AddContest(), o => !IsLoading);
//        EditContestCommand = new VfxCommand(OnEdit, o => SelectedContest != null && !IsLoading);
//        SaveCommand = new VfxCommand(o => SaveContest(), o => EditingContest != null && !IsLoading);
//        CancelEditCommand = new VfxCommand(o => CancelEdit(), o => true);
//        DeleteContestCommand = new VfxCommand(OnDelete, o => SelectedContest != null && !IsLoading);
//    }
//    #endregion

//    #region Edit Operations
//    private void OnEdit(object obj)
//    {
//        try
//        {
//            var contest = obj as ContestsDTO ?? SelectedContest;
//            if (contest == null)
//            {
//                MessageBox.Show("Vui lòng chọn cuộc thi cần chỉnh sửa!", "Thông báo",
//                               MessageBoxButton.OK, MessageBoxImage.Information);
//                return;
//            }

//            // Tạo bản copy cho editing - KHÔNG thay đổi SelectedContest
//            EditingContest = CreateContestCopy(contest);
//            IsEditMode = true;

//            // Mở form edit
//            var editWindow = new EditContest();
//            editWindow.DataContext = this;

//            var result = editWindow.ShowDialog();

//            // Xử lý kết quả sau khi đóng dialog
//            if (result == true)
//            {
//                // Dialog đã được đóng bằng OK/Save
//                Debug.WriteLine("Edit dialog closed with success");
//            }
//            else
//            {
//                // Dialog bị hủy
//                CancelEdit();
//            }
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine($"Lỗi khi mở form chỉnh sửa: {ex.Message}");
//            MessageBox.Show($"Lỗi khi mở form chỉnh sửa: {ex.Message}", "Lỗi",
//                           MessageBoxButton.OK, MessageBoxImage.Error);
//        }
//    }

//    public async void SaveContest()
//    {
//        if (EditingContest == null)
//        {
//            MessageBox.Show("Không có dữ liệu để lưu!", "Thông báo",
//                           MessageBoxButton.OK, MessageBoxImage.Information);
//            return;
//        }

//        try
//        {
//            IsLoading = true;

//            // Validate trước khi save
//            if (!ValidateContest(EditingContest))
//            {
//                return; // Validation đã hiển thị thông báo lỗi
//            }

//            // Gọi API update
//            var result = await App.vietstemService.UpdateContestAsync(EditingContest);

//            if (result != null && result.IsSuccess) // Giả sử API trả về object có property IsSuccess
//            {
//                // Cập nhật lại item trong collection
//                UpdateContestInCollection(EditingContest);

//                // Reset editing state
            

//                // Thông báo thành công
//                MessageBox.Show("Cập nhật cuộc thi thành công!", "Thông báo",
//                               MessageBoxButton.OK, MessageBoxImage.Information);

//                // Trigger success event
//                EditSuccess?.Invoke(this, EventArgs.Empty);
//            }
//            else
//            {
//                string errorMessage = result?.ErrorMessage ?? "Có lỗi xảy ra khi cập nhật cuộc thi";
//                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine($"Lỗi khi lưu cuộc thi: {ex.Message}");
//            MessageBox.Show($"Lỗi khi lưu cuộc thi: {ex.Message}", "Lỗi",
//                           MessageBoxButton.OK, MessageBoxImage.Error);
//        }
//        finally
//        {
//            IsLoading = false;
//        }
//    }

//    private void CancelEdit()
//    {
//        IsEditMode = false;
//        EditingContest = null;
//        Debug.WriteLine("Edit operation cancelled");
//    }
//    #endregion

//    #region Helper Methods
//    private ContestsDTO CreateContestCopy(ContestsDTO original)
//    {
//        return new ContestsDTO
//        {
//            id = original.id,
//            name = original.name,
//            introduce = original.introduce,
//            rule = original.rule,
//            guide = original.guide,
//            fromGrade = original.fromGrade,
//            toGrade = original.toGrade,
//            status = original.status,
//            description = original.description,
//            title = original.title,
//            keywords = original.keywords,
//            accountId = original.accountId,
//            cityId = original.cityId,
//            createdAt = original.createdAt,
//            updatedAt = DateTime.Now
//        };
//    }

//    private void UpdateContestInCollection(ContestsDTO updatedContest)
//    {
//        var existingContest = Contests.FirstOrDefault(c => c.id == updatedContest.id);
//        if (existingContest != null)
//        {
//            // Cập nhật từng property để giữ reference trong collection
//            existingContest.name = updatedContest.name;
//            existingContest.introduce = updatedContest.introduce;
//            existingContest.rule = updatedContest.rule;
//            existingContest.guide = updatedContest.guide;
//            existingContest.fromGrade = updatedContest.fromGrade;
//            existingContest.toGrade = updatedContest.toGrade;
//            existingContest.status = updatedContest.status;
//            existingContest.description = updatedContest.description;
//            existingContest.title = updatedContest.title;
//            existingContest.keywords = updatedContest.keywords;
//            existingContest.updatedAt = updatedContest.updatedAt;

//            Debug.WriteLine($"Updated contest {updatedContest.id} in collection");
//        }
//    }
//    #endregion

//    #region Validation
//    public bool ValidateContest(ContestsDTO contest)
//    {
//        if (contest == null)
//        {
//            Debug.WriteLine("Contest is null, cannot validate.");
//            MessageBox.Show("Dữ liệu cuộc thi không hợp lệ!", "Lỗi",
//                           MessageBoxButton.OK, MessageBoxImage.Error);
//            return false;
//        }

//        return true;
//    }
//    #endregion

//    #region Delete Operation
//    private async void OnDelete(object obj)
//    {
//        var contest = obj as ContestsDTO ?? SelectedContest;
//        if (contest == null) return;

//        var result = MessageBox.Show(
//            $"Bạn có chắc chắn muốn xóa cuộc thi '{contest.name}'?",
//            "Xác nhận xóa",
//            MessageBoxButton.YesNo,
//            MessageBoxImage.Question);

//        if (result == MessageBoxResult.Yes)
//        {
//            try
//            {
//                IsLoading = true;
//                var deleteResult = await App.vietstemService.DeleteContestAsync(contest.id.Value);

//                if (deleteResult != null && deleteResult.IsSuccess)
//                {
//                    Contests.Remove(contest);
//                    if (SelectedContest?.id == contest.id)
//                    {
//                        SelectedContest = null;
//                    }

//                    MessageBox.Show("Xóa cuộc thi thành công!", "Thông báo",
//                                   MessageBoxButton.OK, MessageBoxImage.Information);
//                }
//                else
//                {
//                    MessageBox.Show("Có lỗi xảy ra khi xóa cuộc thi!", "Lỗi",
//                                   MessageBoxButton.OK, MessageBoxImage.Error);
//                }
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"Lỗi khi xóa cuộc thi: {ex.Message}");
//                MessageBox.Show($"Lỗi khi xóa cuộc thi: {ex.Message}", "Lỗi",
//                               MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//            finally
//            {
//                IsLoading = false;
//            }
//        }
//    }
//    #endregion

//    // Các method khác của bạn (OnLoad, OnNextPage, OnPrevPage, etc.)
//    #region
//    private async void OnLoad()
//    {
//        // Implementation của OnLoad
//    }

//    private void OnNextPage(object obj)
//    {
//        // Implementation
//    }

//    private bool CanNextPage(object obj)
//    {
//        // Implementation
//        return !IsLoading;
//    }

//    private void OnPrevPage(object obj)
//    {
//        // Implementation  
//    }

//    private bool CanPrevPage(object obj)
//    {
//        // Implementation
//        return !IsLoading;
//    }

//    private void AddContest()
//    {
//        // Implementation
//    }
//    #endregion

//    #region INotifyPropertyChanged Implementation
//    public event PropertyChangedEventHandler PropertyChanged;

//    protected virtual void RaisePropertyChange([CallerMemberName] string propertyName = null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }
//    #endregion
//}