using Chess.ViewModel;

namespace Chess.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
        BindingContext = new SettingsPageVM(this.Navigation);
    }
}