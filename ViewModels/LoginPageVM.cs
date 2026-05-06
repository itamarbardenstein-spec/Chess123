using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace Chess.ViewModels
{
    /// ViewModel for the Login page, handling traditional and social authentication
    public partial class LoginPageVM : ObservableObject
    {
        #region Fields
        /// Internal user logic instance for authentication operations
        private readonly User user = new();
        private bool isBusy;
        #endregion
        #region Commands
        /// Command to initiate the native Google sign-in flow
        public ICommand GoogleLoginCommand { get; }
        /// Command to navigate to the password recovery screen
        public ICommand ForgotPaswordCommand { get; }
        /// Command to switch the visibility of the password entry field
        public ICommand ToggleIsPasswordCommand { get; }
        /// Command to perform a standard email/password login
        public ICommand LoginCommand { get; }
        #endregion
        #region Properties
        /// Determines if the password text should be masked or visible
        public bool IsPassword { get; set; } = true;
        /// Gets or sets the username, triggering re-evaluation of the login command's availability
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        /// Indicates if an authentication background task is currently running
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
        /// Gets or sets the user password and updates command executable status
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        /// Gets or sets the email address and updates command executable status
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
        /// Initializes authentication commands and hooks into the user logic events
        public LoginPageVM()
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
        /// Validates if the login action can be triggered based on current state and field population
        public bool CanLogin()
        {
            return !IsBusy && user.CanLogin();
        }
        #endregion
        #region Private Methods
        /// Displays an error or status message to the user via a native toast notification
        private void ShowToastAlert(object? sender, string msg)
        {
            isBusy = false;
            OnPropertyChanged(nameof(isBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long).Show();
            });
        }
        /// Navigates the user to the password reset page
        private void ForgotPassword(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new ResetPasswodPage();
            });
        }
        /// Delegates the Google login process to the user logic class
        private void GoogleLogin()
        {
            user.GoogleLogin();
        }
        /// Redirects the user to the home page upon successful authentication
        private void OnAuthComplete(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new HomePage();
            });
        }
        /// Sets the busy state and initiates the standard login process via user logic
        private void Login()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                user.Login();
            }
        }
        /// Toggles the boolean state for password masking and notifies the UI
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
        #endregion       
    }
}