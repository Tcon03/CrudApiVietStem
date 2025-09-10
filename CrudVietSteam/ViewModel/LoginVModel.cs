using CrudVietSteam.Command;
using CrudVietSteam.Service;
using CrudVietSteam.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    public class LoginVModel : ViewModelBase
    {
        #region Properties
        private string _email;
        public string EmailVM
        {
            get { return _email; }
            set
            {
                _email = value;
                Debug.WriteLine("Event Email được kích hoạt  :\r" + value);
                RaisePropertyChange(nameof(EmailVM));
                (LoginCommand as VfxCommand)?.RaiseCanExecuteChanged();
            }
        }
        private string _password;
        public string PasswordVM
        {
            get { return _password; }
            set
            {
                _password = value;
                Debug.WriteLine("Event Pass được kích hoạt : \r " + value);
                RaisePropertyChange(nameof(PasswordVM));
                (LoginCommand as VfxCommand)?.RaiseCanExecuteChanged();

            }
        }

        private bool _remember;
        public bool RememberMe
        {
            get { return _remember; }
            set
            {
                _remember = value;
                Debug.WriteLine("Event Remember được kích hoạt : \r" + value);
                RaisePropertyChange(nameof(_remember));
            }
        }

        #endregion

        public ICommand LoginCommand { get; set; }
        public EventHandler Authenticated;
        public LoginVModel()
        {
            LoginCommand = new VfxCommand(OnLogin, CanExcutedLogin);
        }



        private bool CanExcutedLogin()
        {
            if (string.IsNullOrEmpty(EmailVM) || string.IsNullOrEmpty(PasswordVM))
            {
                return false;
            }
            return true;
        }

        private async void OnLogin(object obj)
        {
            // 1. Check if login is valid 
            try
            {
                bool login = await App.vietstemService.LoginAsync(EmailVM, PasswordVM);
                if (login)
                {
                    if (RememberMe)
                    {
                        CurdentialHelper.SaveCurdential(EmailVM, PasswordVM);
                    }
                    else
                    {
                        CurdentialHelper.Clear();
                    }

                    VietstemMain viet = new VietstemMain();
                    viet.Show();
                    Authenticated?.Invoke(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi đăng nhập: " + ex.Message);
                MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại email và mật khẩu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
