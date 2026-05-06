using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace Chess.ViewModel
{
    /// ViewModel for the registration page, handling new user account creation
    public partial class RegisterPageVM : ObservableObject
    {
        #region Fields
        /// Internal user logic instance to manage registration data and processes
        private readonly User user = new();
        private bool isBusy;
        #endregion
        #region Commands
        /// Command to switch the visibility of the password entry field
        public ICommand ToggleIsPasswordCommand { get; }
        /// Command to execute the registration process
        public ICommand RegisterCommand { get; }
        #endregion
        #region Properties
        /// Determines if the password text should be masked or visible in the UI
        public bool IsPassword { get; set; } = true;
        /// Gets or sets the username, notifying the register command of state changes
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }
        /// Gets or sets the password and updates registration command availability
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }
        /// Gets or sets the email address and updates registration command availability
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }
        /// Gets or sets the user's age and updates registration command availability
        public string Age
        {
            get => user.Age;
            set
            {
                user.Age = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }
        /// Tracks if a registration background task is currently active
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
        /// Sets up commands and registers for logic-level events
        public RegisterPageVM()
        {
            RegisterCommand = new Command(Register, CanRegister);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            user.OnAuthCompleted += OnAuthComplete;
            user.ShowToastAlert += ShowToastAlert;
        }
        #endregion
        #region Public Methods
        /// Validates if the registration form is complete and the system is ready
        public bool CanRegister()
        {
            return !IsBusy && user.CanRegister();
        }
        #endregion
        #region Private Methods
        /// Shows status or error messages to the user via native toast notifications
        private void ShowToastAlert(object? sender, string msg)
        {
            isBusy = false;
            OnPropertyChanged(nameof(isBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long).Show();
            });
        }
        /// Handles UI navigation to the home page once registration is successful
        private void OnAuthComplete(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new HomePage();
            });
        }
        /// Toggles the boolean flag for password field masking
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
        /// Sets the busy state and triggers the registration logic
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