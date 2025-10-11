using Chess.Models;

namespace Chess.ModelsLogic
{
    internal class User : UserModels
    {
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, OnComplete);
        }

        private void OnComplete(Task task)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    SaveToPreferences();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Registration Failed",
                        "An error occurred while creating your account. Please try again.",
                        "OK");
                }
            });
        }
        private void SaveToPreferences()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordKey, Password);
            Preferences.Set(Keys.EmailKey, Email);
            Preferences.Set(Keys.AgeKey, Age);
        }

        public override void Login()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordKey, Password);
            Preferences.Set(Keys.EmailKey, Email);
        }
        public override bool CanLogin()
        {
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email));
        }
        public override bool CanRegister()
        {
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age));
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
