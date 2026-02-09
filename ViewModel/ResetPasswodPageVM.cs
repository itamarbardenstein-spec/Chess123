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
        private readonly User user = new();
        public ICommand ResetEmail => new Command(ResetPass);
        public ICommand BackToLoginCommand => new Command(BackToLogin);
        public ResetPasswodPageVM()
        {
            user.OnPasswordResetCompleted += TrasnferToLogin;
        }

        private void TrasnferToLogin(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.MessageSent, ToastDuration.Short).Show();
            });
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new LoginPage();
                }
            });
        }

        private void BackToLogin(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new LoginPage();
                }
            });
        }

        public string EmailForReset
        {
            get => user.EmailForReset;
            set
            {
                user.EmailForReset = value;
            }
        }
        private void ResetPass()
        {
            user.EmailForReset = EmailForReset;
            user.ResetEmailPassword();
        }
    }
}
