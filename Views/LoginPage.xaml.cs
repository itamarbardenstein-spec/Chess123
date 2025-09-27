using Chess.ViewModel;

namespace Chess.NewFolder;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new LoginPagVM();
    }
}