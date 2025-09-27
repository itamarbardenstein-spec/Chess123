namespace Chess.Models
{
    internal abstract class UserModels
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Age { get; set; }= string.Empty;
        public abstract void Register();
        public abstract void Login();
        public abstract bool CanLogin();
        public abstract bool CanRegister();
        public bool IsRegistered => false;
    }
}
