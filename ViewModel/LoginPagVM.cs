using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using System.Windows.Input;
namespace Chess.ViewModel
{
    public partial class LoginPagVM : ObservableObject
    {
#if ANDROID
        private readonly Platforms.Android.GoogleAuthService? _googleService = null;
#endif
        public ICommand GoogleLoginCommand { get; }
        public ICommand ForgotPaswordCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public bool IsPassword { get; set; } = true;
        public ICommand LoginCommand { get; }
        private readonly User user = new();
        public bool CanLogin()
        {
            return user.CanLogin();
        }
        public LoginPagVM()
        {
            LoginCommand = new Command(Login, CanLogin);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            ForgotPaswordCommand = new Command(ForgotPassword);
            user.OnAuthCompleted += OnAuthComplete;
            GoogleLoginCommand = new Command(GoogleLogin);
#if ANDROID
            _googleService = new Platforms.Android.GoogleAuthService();
#endif
        }
        private void ForgotPassword(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new ResetPasswodPage();
                }
            });
        }
        private async void GoogleLogin()
        {
            try
            {
#if ANDROID
                // שלב א': פתיחת חלונית גוגל וקבלת הטוקן
                string idToken = await _googleService!.AuthenticateAsync();
                if (!string.IsNullOrEmpty(idToken))
                {
                    // שלב ב': שליחת הטוקן ל-Firebase דרך המודל User
                    User.SignInWithGoogle(idToken, (task) =>
                    {
                        if (task.IsCompletedSuccessfully)
                        {
                            // שלב ג': הצלחה - מעבר לדף הבית (חייב לרוץ על ה-MainThread)
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                if (Application.Current != null)
                                {
                                    Application.Current.MainPage = new HomePage();
                                }
                            });
                        }
                        else
                        {
                            // טיפול בכישלון התחברות ל-Firebase
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                await Application.Current!.MainPage!.DisplayAlert("שגיאה", "ההתחברות ל-Firebase נכשלה", "אישור");
                            });
                        }
                    });
                }
#else
                await Application.Current!.MainPage!.DisplayAlert("Info", "Google Login זמין כרגע רק באנדרואיד", "OK");
#endif
            }
            catch (Exception ex)
            {
                // המשתמש ביטל את החלונית או שגיאה אחרת
                await Application.Current!.MainPage!.DisplayAlert("Login Error", ex.Message, "OK");
            }
        }
        private void OnAuthComplete(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new HomePage();
                }
            });
        }
        private void Login()
        {
            user.Login();
        }
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
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
    }
}
