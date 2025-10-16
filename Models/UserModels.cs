using Chess.ModelsLogic;

namespace Chess.Models
{
    internal abstract class UserModels
    {
        protected FbData fbd = new();
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Age { get; set; }= string.Empty;
        public bool RememberMe { get; set; } = false;
        public abstract void Register();
        public abstract void Login();
        public abstract bool CanLogin();
        public abstract bool CanRegister();
        public bool IsRegistered { get; set; } = false;
    }
}
