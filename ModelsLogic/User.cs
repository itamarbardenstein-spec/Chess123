using Chess.Models;

namespace Chess.ModelsLogic
{
    internal class User : UserModels
    {
        public override void Register()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordNameKey, Password);
            Preferences.Set(Keys.EmailNameKey, Email);
            Preferences.Set(Keys.AgeNameKey, Age);

        }
        public override void Login()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordNameKey, Password);
            Preferences.Set(Keys.EmailNameKey, Email);
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
            Password = Preferences.Get(Keys.PasswordNameKey, string.Empty);
            Email = Preferences.Get(Keys.EmailNameKey, string.Empty);
            Age = Preferences.Get(Keys.AgeNameKey, string.Empty);
        }
    }
}
