using CrudVietSteam.Command;
using CrudVietSteam.Service.DTO;
using CrudVietSteam.View;
using CrudVietSteam.View.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public ICommand UpdateContestCommand { get; set; }
        //public EventHandler UpdateSuccess;


        //2. Khởi tạo đối tượng EditContestVM với ContestEdit là đối tượng cần sửa

        public EditContestVM(ContestsDTO contest)
        {
            // contest là đối tượng được truyền vào từ View khi mở cửa sổ sửa
            ContestEdit = contest;
            UpdateContestCommand = new VfxCommand(OnUpdateContest, CanUpdate);
        }

        private bool CanUpdate()
        {
            if (!string.IsNullOrWhiteSpace(ContestEdit.name) &&
                      !string.IsNullOrWhiteSpace(ContestEdit.introduce) &&
                      !string.IsNullOrWhiteSpace(ContestEdit.status) &&
                      !string.IsNullOrWhiteSpace(ContestEdit.description) &&
                      !string.IsNullOrWhiteSpace(ContestEdit.title) &&
                      !string.IsNullOrWhiteSpace(ContestEdit.keywords))
                return true;
            MessageBox.Show("Vui lòng nhập đầy đủ thông tin không được bỏ trống !!", "Errorr", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }


        private async void OnUpdateContest(object obj)
        {
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
