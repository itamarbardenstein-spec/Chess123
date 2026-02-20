using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;
namespace Chess.ViewModel
{
    public partial class LoginPagVM : ObservableObject
    {
        #region Fields
        private readonly User user = new();
        private bool isBusy;
        #endregion
        #region Commands
        public ICommand GoogleLoginCommand { get; }
        public ICommand ForgotPaswordCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public ICommand LoginCommand { get; }
        #endregion
        #region Properties
        public bool IsPassword { get; set; } = true;
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        #endregion
        #region Constructor
        public LoginPagVM()
        {
            LoginCommand = new Command(Login, CanLogin);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            ForgotPaswordCommand = new Command(ForgotPassword);
            user.OnAuthCompleted += OnAuthComplete;
            user.ShowToastAlert += ShowToastAlert;
            GoogleLoginCommand = new Command(GoogleLogin);
        }
        #endregion
        #region Public Methods
        public bool CanLogin()
        {
            return !IsBusy && user.CanLogin();
        }
        #endregion
        #region Private Methods
        private void ShowToastAlert(object? sender, string msg)
        {
            isBusy = false;
            OnPropertyChanged(nameof(isBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long).Show();
            });
        }
        private void ForgotPassword(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new ResetPasswodPage();
            });
        }
        private void GoogleLogin()
        {          
            user.GoogleLogin();
        }
        private void OnAuthComplete(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new HomePage();
            });
        }
        private void Login()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                user.Login();
            }                         
        }
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
        #endregion       
    }
}
