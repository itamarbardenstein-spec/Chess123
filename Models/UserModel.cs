using Chess.ModelsLogic;

namespace Chess.Models
{
    /// Base model for handling user authentication, profile data, and identity provider integration
    public abstract class UserModel
    {
        #region Fields
        /// Service for interacting with Firebase Authentication and Firestore
        protected FbData fbd = new();
        #endregion
        #region Events
        /// Triggered when a login or registration process finishes successfully
        public EventHandler? OnAuthCompleted;
        /// Triggered when the password reset email has been successfully sent
        public EventHandler? OnPasswordResetCompleted;
        /// Request to display a brief notification message in the UI
        public EventHandler<string>? ShowToastAlert;
        #endregion
        #region Properties
        /// The user's chosen display name
        public string UserName { get; set; } = string.Empty;
        /// Validation check to ensure all required profile fields are populated
        public bool IsRegistered => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age);
        /// The user's account password
        public string Password { get; set; } = string.Empty;
        /// The email address targeted for a password recovery operation
        public string EmailForReset { get; set; } = string.Empty;
        /// The user's account email address
        public string Email { get; set; } = string.Empty;
        /// The user's reported age
        public string Age { get; set; } = string.Empty;
        #endregion
        #region Public Methods
        /// Creates a new user account in the authentication system
        public abstract void Register();
        /// Authenticates the user using email and password credentials
        public abstract void Login();
        /// Checks if the provided credentials meet the minimum requirements for a login attempt
        public abstract bool CanLogin();
        /// Checks if the provided profile data meets the requirements for a registration attempt
        public abstract bool CanRegister();
        /// Translates raw Firebase exception codes into user-friendly localized messages
        public abstract string GetFirebaseErrorMessage(string msg);
        /// Initiates the password recovery process via email
        public abstract void ResetEmailPassword();
        /// Initiates the platform-specific Google Sign-In flow
        public abstract void GoogleLogin();
        /// Completes the authentication process using a Google ID token
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