using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Chess.Models;

namespace Chess.Platforms.Android
{
    public class GoogleAuthService
    {
        #region Properties
        public static TaskCompletionSource<string>? LoginTcs { get; set; }
        #endregion
        #region Public Methods
        public static async Task<string> AuthenticateAsync()    
        {
            LoginTcs = new TaskCompletionSource<string>();
            string webClientId = Keys.webClientId;
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(webClientId)
                .RequestEmail()
                .Build();
            GoogleSignInClient googleSignInClient = GoogleSignIn.GetClient(Platform.CurrentActivity, gso);
            Intent intent = googleSignInClient.SignInIntent;
            Platform.CurrentActivity?.StartActivityForResult(intent, 1001);
            return await LoginTcs.Task;
        }
        #endregion
    }
}
