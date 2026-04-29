using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public partial class RegisterPageVM: ObservableObject
    {
        #region Fields
        private readonly User user = new();
        private bool isBusy;
        #endregion
        #region Commands
        public ICommand ToggleIsPasswordCommand { get; }
        public ICommand RegisterCommand { get; }
        #endregion
        #region Properties
        public bool IsPassword { get; set; } = true;
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }
        public string Age
        {
            get => user.Age;
            set
            {
                user.Age = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }
        #endregion
        #region Constructor
        public RegisterPageVM()
        {
            RegisterCommand = new Command(Register, CanRegister);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            user.OnAuthCompleted += OnAuthComplete;
            user.ShowToastAlert += ShowToastAlert;
        }
        #endregion
        #region Public Methods
        public bool CanRegister()
        {
            return !IsBusy && user.CanRegister();
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
        private void OnAuthComplete(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new HomePage();
            });
        }
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
        private void Register()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                user.Register();
            }               
        }
        #endregion
    }
}
