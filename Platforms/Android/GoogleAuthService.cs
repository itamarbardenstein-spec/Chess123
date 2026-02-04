using Android.Gms.Auth.Api.SignIn;
using Chess.Models;

namespace Chess.Platforms.Android
{
    public class GoogleAuthService : IGoogleAuthService
    {
        public static TaskCompletionSource<string> LoginTcs = null;

        public async Task<string> AuthenticateAsync()
        {
            LoginTcs = new TaskCompletionSource<string>();
            string webClientId = "203460691005-6j2un7i3vu9imfvlc9oili1bpbaainsq.apps.googleusercontent.com";

            var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(webClientId)
                .RequestEmail()
                .Build();

            var googleSignInClient = GoogleSignIn.GetClient(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity, gso);
            var intent = googleSignInClient.SignInIntent;
            Microsoft.Maui.ApplicationModel.Platform.CurrentActivity?.StartActivityForResult(intent, 1001);

            return await LoginTcs.Task;
        }
    }
}
