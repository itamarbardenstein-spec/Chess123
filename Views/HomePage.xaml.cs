using Chess.ViewModels;
namespace Chess.Views;
public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new HomePageVM();
    }
}