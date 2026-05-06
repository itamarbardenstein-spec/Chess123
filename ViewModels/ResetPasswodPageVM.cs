using Chess.Models;
using Chess.ModelsLogic;
using Chess.NewFolder;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace Chess.ViewModels
{
    /// ViewModel for the password reset page, managing recovery email requests
    public class ResetPasswodPageVM
    {
        #region Fields
        /// Internal user logic instance to handle authentication and password recovery actions
        private readonly User user = new();
        #endregion
        #region Commands
        /// Command to initiate the password reset email process
        public ICommand ResetEmail => new Command(ResetPass);
        /// Command to navigate the user back to the login screen
        public ICommand BackToLoginCommand => new Command(BackToLogin);
        #endregion
        #region Properties
        /// The email address provided by the user for receiving the reset link
        public string EmailForReset
        {
            get => user.EmailForReset;
            set => user.EmailForReset = value;
        }
        #endregion
        #region Constructor
        /// Subscribes to reset completion and alert events from the user logic
        public ResetPasswodPageVM()
        {
            user.OnPasswordResetCompleted += TrasnferToLogin;
            user.ShowToastAlert += ShowToastAlert;
        }
        #endregion
        #region Private Methods
        /// Displays feedback or error messages using native toast notifications
        private void ShowToastAlert(object? sender, string msg)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long).Show();
            });
        }
        /// Confirms the email was sent and redirects the user to the login page
        private void TrasnferToLogin(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.MessageSent, ToastDuration.Short).Show();
            });
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new LoginPage();
            });
        }
        /// Performs a direct navigation back to the login screen without resetting
        private void BackToLogin(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new LoginPage();
            });
        }
        /// Validates the current email input and triggers the logic to send the reset link
        private void ResetPass()
        {
            user.EmailForReset = EmailForReset;
            user.ResetEmailPassword();
        }
        #endregion               
    }
}