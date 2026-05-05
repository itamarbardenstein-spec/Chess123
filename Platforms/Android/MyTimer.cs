using Android.OS;
using Chess.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace Chess.Platforms.Android
{
    /// Custom Android countdown timer that broadcasts progress and completion via messaging
    public class MyTimer(long millisInFuture, long countDownInterval) : CountDownTimer(millisInFuture, countDownInterval)
    {
        /// Sends a completion signal through the messenger when the timer reaches zero
        public override void OnFinish()
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<long>(Keys.FinishedSignal));
        }
        /// Broadcasts the remaining time at each specified interval tick
        public override void OnTick(long millisUntilFinished)
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<long>(millisUntilFinished));
        }
    }
}