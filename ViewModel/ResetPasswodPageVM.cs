using Chess.Models;
using Chess.ModelsLogic;
using Chess.NewFolder;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class ResetPasswodPageVM
    {
        #region Fields
        private readonly User user = new();
        #endregion
        #region Commands
        public ICommand ResetEmail => new Command(ResetPass);
        public ICommand BackToLoginCommand => new Command(BackToLogin);
        #endregion
        #region Properties
        public string EmailForReset
        {
            get => user.EmailForReset;
            set => user.EmailForReset = value;
        }
        #endregion
        #region Constructor
        public ResetPasswodPageVM()
        {
            user.OnPasswordResetCompleted += TrasnferToLogin;
        }
        #endregion
        #region Private Methods
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

        private void BackToLogin(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new LoginPage();
            });
        }
        private void ResetPass()
        {
            user.EmailForReset = EmailForReset;
            user.ResetEmailPassword();
        }
        #endregion             
    }
}
