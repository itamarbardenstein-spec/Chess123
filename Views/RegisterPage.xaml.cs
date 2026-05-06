using Chess.ViewModels;

namespace Chess.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
        BindingContext = new RegisterPageVM();
    }
}