using Android.App;
using Android.Runtime;

namespace Chess.Platforms.Android
{
    /// Main Android application class that initializes the MAUI framework
    [Application]
    public class MainApplication(nint handle, JniHandleOwnership ownership) : MauiApplication(handle, ownership)
    {
        /// Boots the MAUI application by calling the shared MauiProgram configuration
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}