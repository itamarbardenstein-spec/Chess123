using Chess.Models;
using Chess.NewFolder;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
namespace Chess.ModelsLogic
{
    public class User : UserModels
    {
#if ANDROID
        private readonly Platforms.Android.GoogleAuthService? _googleService = null;
#endif
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, OnComplete);
        }
        public override void Login()
        {
            fbd.SignInWithEmailAndPasswordAsync(Email, Password, OnComplete);           
        }
        protected override void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                SaveToPreferences();
                OnAuthCompleted?.Invoke(this, EventArgs.Empty);
            }
            else if (task.Exception != null)
            {                
                string msg = task.Exception.Message;
                ShowAlert(GetFirebaseErrorMessage(msg));
            }
            else
                ShowAlert(Strings.RegistrationFailed);
        }
        public override string GetFirebaseErrorMessage(string msg)
        {
            string result= string.Empty;
            if (msg.Contains(Strings.ErrMessageReason))
            {
                if (msg.Contains(Strings.EmailExists))
                    result= Strings.EmailExistsErrMsg;
                if (msg.Contains(Strings.InvalidEmailAddress))
                    result= Strings.InvalidEmailErrMessage;
                if (msg.Contains(Strings.WeakPassword))
                    result= Strings.WeakPasswordErrMessage;
                if(msg.Contains(Strings.InvalidLogin))
                    result= Strings.InvalidLoginErrMsg;
            }
            else
                result= Strings.UnknownErrorMessage;
            return result;
        }
        protected override void ShowAlert(string msg)
        {
            ShowToastAlert?.Invoke(this, msg);           
        }
        protected override void SaveToPreferences()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordKey, Password);
            Preferences.Set(Keys.EmailKey, Email);
            Preferences.Set(Keys.AgeKey, Age);          
        }    
        public override bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email);
        }
        public override bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age);
        }
        public override void SignInWithGoogle(string idToken, Action<Task> OnComplete)
        {
            FbData.SignInWithGoogleAsync(idToken, OnComplete);
        }
        public override async void GoogleLogin()
        {
            try
            {
#if ANDROID
                // שלב א': פתיחת חלונית גוגל וקבלת הטוקן
                string idToken = await Platforms.Android.GoogleAuthService.AuthenticateAsync();
                if (!string.IsNullOrEmpty(idToken)) 
                {
                    // שלב ב': שליחת הטוקן ל-Firebase דרך המודל User
                    SignInWithGoogle(idToken, (task) =>
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
                                await Application.Current!.MainPage!.DisplayAlert(Strings.Error, Strings.FireBaseLoginError, Strings.Ok);
                            });
                        }
                    });
                }
#endif
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert(Strings.LoginError, ex.Message, Strings.Ok);
            }
        }
        public override void ResetEmailPassword()
        {
            fbd.ResetEmailPasswordAsync(EmailForReset, OnResetComplete);
        }
        protected override void OnResetComplete(Task task)
        {
            OnPasswordResetCompleted?.Invoke(this, EventArgs.Empty);
            
        }
        public User()
        {           
            UserName = Preferences.Get(Keys.UserNameKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordKey, string.Empty);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
            Age = Preferences.Get(Keys.AgeKey, string.Empty);
#if ANDROID
            _googleService = new Platforms.Android.GoogleAuthService();
#endif
        }
    }
}
