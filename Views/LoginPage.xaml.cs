using Chess.ViewModels;
namespace Chess.NewFolder;
public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new LoginPageVM();
    }
}