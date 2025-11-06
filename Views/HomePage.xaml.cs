using Chess.ViewModel;

namespace Chess.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new HomePageVM();
    }
}