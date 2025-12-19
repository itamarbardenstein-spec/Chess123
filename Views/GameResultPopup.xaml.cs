using Chess.Models;
using CommunityToolkit.Maui.Views;

namespace Chess.Views
{
    public partial class GameResultPopup : Popup
    {
        public GameResultPopup(bool iWon)
        {
            InitializeComponent();

            TitleLabel.Text = iWon ? Strings.YouWon : Strings.YouLost;
            MessageLabel.Text = iWon
                ? Strings.Win: Strings.Lose;
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            Close();
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Application.Current!.MainPage = new HomePage();
            });
        }
    }}