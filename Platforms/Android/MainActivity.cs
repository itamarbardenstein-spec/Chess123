using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Chess.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace Chess.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        MyTimer? mTimer;
        override protected void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RegisterTimerMessages();
            StartDeleteFbDocsService();
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // בדיקה שהקוד תואם לזה שהגדרנו ב-GoogleAuthService
            if (requestCode == 1001)
            {
                // קבלת המשימה מתוך ה-Intent שהתקבל מגוגל
                var task = global::Android.Gms.Auth.Api.SignIn.GoogleSignIn.GetSignedInAccountFromIntent(data);

                try
                {
                    // ביצוע המרה (Cast) כדי לגשת למאפייני החשבון
                    var account = (global::Android.Gms.Auth.Api.SignIn.GoogleSignInAccount)task.Result;

                    if (account != null)
                    {
                        // שליחת ה-Token בחזרה ל-Service ושחרור ה-await
                        Chess.Platforms.Android.GoogleAuthService.LoginTcs?.TrySetResult(account.IdToken);
                    }
                }
                catch (Exception ex)
                {
                    // במקרה של שגיאה (כמו SHA-1 לא תואם), נשלח את השגיאה ל-ViewModel
                    Chess.Platforms.Android.GoogleAuthService.LoginTcs?.TrySetException(ex);
                }
            }
        }
        private void StartDeleteFbDocsService()
        {
            Intent intent = new(this, typeof(DeleteFbDocsService));
            StartService(intent);
        }
        private void RegisterTimerMessages()
        {
            WeakReferenceMessenger.Default.Register<AppMessage<TimerSettings>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
            WeakReferenceMessenger.Default.Register<AppMessage<bool>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
        }

        private void OnMessageReceived(bool value)
        {
            if (value)
            {
                mTimer?.Cancel();
                mTimer = null;
            }
        }

        private void OnMessageReceived(TimerSettings value)
        {
            mTimer = new MyTimer(value.TotalTimeInMilliseconds, value.IntervalInMilliseconds);
            mTimer.Start();
        }
    }
}
