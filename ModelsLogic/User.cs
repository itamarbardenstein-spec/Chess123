using Chess.Models;

namespace Chess.ModelsLogic
{
    internal class User : UserModels
    {
        public override void Register()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordNameKey, UserName);
            Preferences.Set(Keys.EmailNameKey, UserName);
            Preferences.Set(Keys.AgeNameKey, UserName);

        }
        public override void Login()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordNameKey, UserName);
            Preferences.Set(Keys.EmailNameKey, UserName);
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
