
using Chess.Models;
namespace Chess.ModelsLogic
{
    internal class Connectivity : ConnectivityModel
    {
        public Connectivity()
        {
            Microsoft.Maui.Networking.Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
            IsConnected = Microsoft.Maui.Networking.Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }

        protected override void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
