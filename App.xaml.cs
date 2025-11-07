
using Chess.ModelsLogic;
using Chess.NewFolder;
using Chess.Views;

namespace Chess
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            User user = new();
            MainPage = user.IsRegistered ? new LoginPage() : new RegisterPage();
        }

    }
}
