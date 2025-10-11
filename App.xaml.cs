
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
            Page page=new RegisterPage();
            MainPage =page;
        }

    }
}
