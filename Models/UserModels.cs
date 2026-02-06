using Chess.ModelsLogic;

namespace Chess.Models
{
    public abstract class UserModels
    {
        protected FbData fbd = new();
        public EventHandler? OnAuthCompleted;
        public string UserName { get; set; } = string.Empty;
        public bool IsRegistered => (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age));
        public string Password { get; set; } = string.Empty;
        public string EmailForReset = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Age { get; set; }= string.Empty;
        public abstract void Register();
        public abstract void Login();
        public abstract bool CanLogin();
        public abstract bool CanRegister();
        protected abstract void ShowAlert(string msg);
        protected abstract void SaveToPreferences();
        protected abstract void OnComplete(Task task);
        public abstract string GetFirebaseErrorMessage(string msg);
        public abstract void ResetEmailPassword();
        protected abstract void OnResetComplete(Task task);
    }
}
