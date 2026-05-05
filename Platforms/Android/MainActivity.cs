using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Chess.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace Chess.Platforms.Android
{
    /// Main entry point for the Android application, handling lifecycle events and message registration
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        #region Fields
        /// Reference to the custom countdown timer for game logic
        MyTimer? mTimer;
        #endregion
        #region Private Methods
        /// Initializes the activity, sets up message listeners, and starts background services
        override protected void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RegisterTimerMessages();
            StartDeleteFbDocsService();
        }
        /// Handles results from external activities, specifically processing Google Sign-In tokens
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            // Request code 1001 matches the Google login attempt
            if (requestCode == 1001)
            {
                global::Android.Gms.Tasks.Task task = global::Android.Gms.Auth.Api.SignIn.GoogleSignIn.GetSignedInAccountFromIntent(data);
                try
                {
                    global::Android.Gms.Auth.Api.SignIn.GoogleSignInAccount account = (global::Android.Gms.Auth.Api.SignIn.GoogleSignInAccount)task.Result;
                    if (account != null)
                        GoogleAuthService.LoginTcs?.TrySetResult(account.IdToken);
                }
                catch (Exception ex)
                {
                    GoogleAuthService.LoginTcs?.TrySetException(ex);
                }
            }
        }
        /// Launches the Android service responsible for cleaning up old Firebase documents
        private void StartDeleteFbDocsService()
        {
            Intent intent = new(this, typeof(DeleteFbDocsService));
            StartService(intent);
        }
        /// Subscribes to application-wide messages to control the timer state
        private void RegisterTimerMessages()
        {
            // Register for timer initialization messages
            WeakReferenceMessenger.Default.Register<AppMessage<TimerSettings>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
            // Register for timer cancellation messages
            WeakReferenceMessenger.Default.Register<AppMessage<bool>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
        }
        /// Stops and clears the timer when a boolean cancel message is received
        private void OnMessageReceived(bool value)
        {
            if (value)
            {
                mTimer?.Cancel();
                mTimer = null;
            }
        }
        /// Creates and starts a new timer instance based on provided settings
        private void OnMessageReceived(TimerSettings value)
        {
            mTimer = new MyTimer(value.TotalTimeInMilliseconds, value.IntervalInMilliseconds);
            mTimer.Start();
        }
        #endregion
    }
}