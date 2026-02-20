using Chess.ModelsLogic;

namespace Chess.Models
{
    public abstract class UserModels
    {
        #region Fields
        protected FbData fbd = new();
        #endregion
        #region Events
        public EventHandler? OnAuthCompleted;
        public EventHandler? OnPasswordResetCompleted;
        public EventHandler<string>? ShowToastAlert;
        #endregion
        #region Properties
        public string UserName { get; set; } = string.Empty;
        public bool IsRegistered => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age);
        public string Password { get; set; } = string.Empty;
        public string EmailForReset { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Age { get; set; }= string.Empty;
        #endregion
        #region Public Methods
        public abstract void Register();
        public abstract void Login();
        public abstract bool CanLogin();
        public abstract bool CanRegister();
        public abstract string GetFirebaseErrorMessage(string msg);
        public abstract void ResetEmailPassword();
        public abstract void GoogleLogin();
        public abstract void SignInWithGoogle(string idToken, Action<Task> OnComplete);
        #endregion
        #region Private Methods
        protected abstract void ShowAlert(string msg);
        protected abstract void SaveToPreferences();
        protected abstract void OnComplete(Task task);       
        protected abstract void OnResetComplete(Task task);
        #endregion
    }
}
