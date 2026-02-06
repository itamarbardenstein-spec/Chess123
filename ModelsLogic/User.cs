using Chess.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
namespace Chess.ModelsLogic
{
    public class User : UserModels
    {
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
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long).Show();
            });
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
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email));
        }
        public override bool CanRegister()
        {
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age));
        }
        public void SignInWithGoogle(string idToken, Action<Task> OnComplete)
        {
            FbData.SignInWithGoogleAsync(idToken, OnComplete);
        }
        public override void ResetEmailPassword()
        {
            fbd.ResetEmailPasswordAsync(EmailForReset, OnResetComplete);
        }
        protected override void OnResetComplete(Task task)
        {
            
        }
        public User()
        {           
            UserName = Preferences.Get(Keys.UserNameKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordKey, string.Empty);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
            Age = Preferences.Get(Keys.AgeKey, string.Empty);
        }
    }
}
