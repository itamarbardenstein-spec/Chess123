using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Chess.Models;

namespace Chess.Platforms.Android
{
    /// Service to handle native Android Google Authentication flow
    public class GoogleAuthService
    {
        #region Properties
        /// Bridge to communicate between the intent result and the calling task
        public static TaskCompletionSource<string>? LoginTcs { get; set; }
        #endregion
        #region Public Methods
        /// Initiates the Google Sign-In intent and waits for the ID token response
        public static async Task<string> AuthenticateAsync()
        {
            // Initialize TCS to enable awaiting the result from an external activity callback
            LoginTcs = new TaskCompletionSource<string>();
            string webClientId = Keys.webClientId;
            // Configure Google Sign-In to request the ID token (for Firebase) and basic email profile
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(webClientId)
                .RequestEmail()
                .Build();
            // Get the native Android Google client using the current application activity
            GoogleSignInClient googleSignInClient = GoogleSignIn.GetClient(Platform.CurrentActivity, gso);
            // Prepare the intent that launches the Google account selection UI
            Intent intent = googleSignInClient.SignInIntent;
            // Start the activity with a specific request code (1001) to identify the result later
            Platform.CurrentActivity?.StartActivityForResult(intent, 1001);
            // Return the task which will be completed once the activity result is processed
            return await LoginTcs.Task;
        }
        #endregion
    }
}