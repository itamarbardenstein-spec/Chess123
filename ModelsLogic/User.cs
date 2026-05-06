using Chess.Models;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Chess.ModelsLogic
{
    /// Manages user authentication, profile data, and Firebase integration
    public class User : UserModel
    {
        #region Fields
#if ANDROID
        /// Android-specific service for Google authentication
        private readonly Platforms.Android.GoogleAuthService? _googleService = null;
#endif
        #endregion
        #region Constructor
        /// Initializes a new user by loading saved credentials from local storage
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
        /// Registers a new user account with Firebase using email and password
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, OnComplete);
        }
        /// Authenticates an existing user via Firebase email/password sign-in
        public override void Login()
        {
            fbd.SignInWithEmailAndPasswordAsync(Email, Password, OnComplete);
        }
        /// Maps raw Firebase error strings to user-friendly localized messages
        public override string GetFirebaseErrorMessage(string msg)
        {
            string result = string.Empty;
            if (msg.Contains(Strings.ErrMessageReason))
            {
                if (msg.Contains(Strings.EmailExists))
                    result = Strings.EmailExistsErrMsg;
                if (msg.Contains(Strings.InvalidEmailAddress))
                    result = Strings.InvalidEmailErrMessage;
                if (msg.Contains(Strings.WeakPassword))
                    result = Strings.WeakPasswordErrMessage;
                if (msg.Contains(Strings.InvalidLogin))
                    result = Strings.InvalidLoginErrMsg;
            }
            else
                result = Strings.UnknownErrorMessage;
            return result;
        }
        /// Checks if the minimum required fields are filled for logging in
        public override bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email);
        }
        /// Validates that all registration fields, including age, are provided
        public override bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age);
        }
        /// Finalizes the Google authentication process within Firebase
        public override void SignInWithGoogle(string idToken, Action<Task> OnComplete)
        {
            fbd.SignInWithGoogleAsync(idToken, OnComplete);
        }
        /// Initiates the platform-specific Google login flow and handles navigation on success
        public override async void GoogleLogin()
        {
            try
            {
#if ANDROID
                // Request native Android Google authentication token
                string idToken = await Platforms.Android.GoogleAuthService.AuthenticateAsync();
                if (!string.IsNullOrEmpty(idToken))
                    SignInWithGoogle(idToken, (task) =>
                    {
                        if (task.IsCompletedSuccessfully)
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                // Redirect to home page upon successful cloud sign-in
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
        /// Requests a password reset email from Firebase for the specified address
        public override void ResetEmailPassword()
        {
            fbd.ResetEmailPasswordAsync(EmailForReset, OnResetComplete);
        }
        #endregion
        #region Private Methods
        /// Handles the result of registration and login tasks
        protected override void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                // Persist user data locally and notify observers of success
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
        /// Triggers a toast or UI alert through registered event handlers
        protected override void ShowAlert(string msg)
        {
            ShowToastAlert?.Invoke(this, msg);
        }
        /// Saves current user properties to persistent device storage
        protected override void SaveToPreferences()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordKey, Password);
            Preferences.Set(Keys.EmailKey, Email);
            Preferences.Set(Keys.AgeKey, Age);
        }
        /// Handles the completion of the password reset email request
        protected override void OnResetComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnPasswordResetCompleted?.Invoke(this, EventArgs.Empty);
            else
                ShowAlert(Strings.validEmail);
        }
        #endregion
    }
}