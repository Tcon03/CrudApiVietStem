using CrudVietSteam.Command;
using CrudVietSteam.Service.DTO;
using CrudVietSteam.View;
using CrudVietSteam.View.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    public class EditContestVM : ViewModelBase
    {
        //1. Thuộc tính để giữ đối tượng đang được sửa để cho View có thể binding 
        private ContestsDTO _contest;
        public ContestsDTO ContestEdit
        {
            get => _contest;
            set
            {
                _contest = value;
                RaisePropertyChange(nameof(ContestEdit));
            }
        }

        public ICommand UpdateContestCommand { get; }
        //public EventHandler UpdateSuccess;


        //2. Khởi tạo đối tượng EditContestVM với ContestEdit là đối tượng cần sửa

        public EditContestVM(ContestsDTO contest)
        {
            // contest là đối tượng được truyền vào từ View khi mở cửa sổ sửa
            ContestEdit = contest;
            UpdateContestCommand = new VfxCommand(OnUpdateContest, CanUpdate);
        }

        private bool CanUpdate(object arg)
        {
            return !string.IsNullOrEmpty(ContestEdit.name) || !string.IsNullOrEmpty(ContestEdit.introduce) ||
                    !string.IsNullOrEmpty(ContestEdit.status) || !string.IsNullOrEmpty(ContestEdit.description) ||
                    !string.IsNullOrEmpty(ContestEdit.title) || !string.IsNullOrEmpty(ContestEdit.keywords);
        }


        private async void OnUpdateContest(object obj)
        {
            if (!CanUpdate(obj))
            {
                MessageBox.Show("Vui lòng nhập thông tin không được bỏ trống !!", "Thông báo ", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {

                await App.vietstemService.UpdateContestAsync(ContestEdit);

                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                if (obj is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cập nhật thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
    }
}
