using Chess.Models;
using Chess.Views;
namespace Chess.ModelsLogic
{
    public class User : UserModels
    {
        #region Fields
#if ANDROID
        private readonly Platforms.Android.GoogleAuthService? _googleService = null;
#endif
        #endregion
        #region Constructor
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
        #endregion
        #region Public Methods
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, OnComplete);
        }
        public override void Login()
        {
            fbd.SignInWithEmailAndPasswordAsync(Email, Password, OnComplete);           
        }
        public override string GetFirebaseErrorMessage(string msg)
        {
            string result = string.Empty;
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
                result = Strings.UnknownErrorMessage;
            return result;
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
            fbd.SignInWithGoogleAsync(idToken, OnComplete);
        }
        public override async void GoogleLogin()
        {
            try
            {
#if ANDROID
                string idToken = await Platforms.Android.GoogleAuthService.AuthenticateAsync();
                if (!string.IsNullOrEmpty(idToken)) 
                    SignInWithGoogle(idToken, (task) =>
                    {
                        if (task.IsCompletedSuccessfully)
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                if (Application.Current != null)
                                    Application.Current.MainPage = new HomePage();
                            });
                        else
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                await Application.Current!.MainPage!.DisplayAlert(Strings.Error, Strings.FireBaseLoginError, Strings.Ok);
                            });
                    });
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
        #endregion
        #region Private Methods
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
        protected override void OnResetComplete(Task task)
        {
            OnPasswordResetCompleted?.Invoke(this, EventArgs.Empty);            
        }
        #endregion
    }
}
