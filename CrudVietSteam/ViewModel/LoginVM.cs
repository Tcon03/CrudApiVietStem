using CrudVietSteam.Command;
using CrudVietSteam.Service;
using CrudVietSteam.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    public class LoginVM : ViewModelBase
    {
        public readonly VietstemSevice service;
        private string _email;
        public string EmailVM
        {
            get { return _email; }
            set
            {
                _email = value;
                Debug.WriteLine("Event được kích hoạt " + value);
                RaisePropertyChange(nameof(EmailVM));
            }
        }
        private string _password;
        public string PasswordVM
        {
            get { return _password; }
            set
            {
                _password = value;
                Debug.WriteLine("Event được kích hoạt " + value);
                RaisePropertyChange(nameof(PasswordVM));
            }
        }

        private bool _remember;
        public bool RememberMe
        {
            get { return _remember; }
            set
            {
                _remember = value;
                Debug.WriteLine("Event được kích hoạt " + value);
                RaisePropertyChange(nameof(_remember));
            }
        }
        public ICommand LoginCommand { get; set; }
        public EventHandler Authenticated;
        public LoginVM()
        {
            LoginCommand = new VfxCommand(OnLogin, CanExcutedLogin);
            service = new VietstemSevice();

        }



        private bool CanExcutedLogin(object obj)
        {
            if (string.IsNullOrEmpty(EmailVM) || string.IsNullOrEmpty(PasswordVM))
            {
                return false;
            }
            return true;
        }

        private async void OnLogin(object obj)
        {
            bool login = await service.LoginAsync(EmailVM, PasswordVM);
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

                VietsteamDataView viet = new VietsteamDataView();
                viet.Show();
                Authenticated?.Invoke(this, new EventArgs());
                await service.GetContestAsync();
            }
        }
    }
}
