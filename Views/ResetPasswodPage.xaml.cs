using Chess.ViewModel;

namespace Chess.Views;

public partial class ResetPasswodPage : ContentPage
{
	public ResetPasswodPage()
	{
		InitializeComponent();
		BindingContext = new ResetPasswodPageVM();
    }
}