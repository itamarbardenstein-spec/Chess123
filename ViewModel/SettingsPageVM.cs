using System.Windows.Input;

namespace Chess.ViewModel
{
    public class SettingsPageVM
    {
        private readonly INavigation _navigation;

        public SettingsPageVM(INavigation navigation)
        {
            _navigation = navigation;
            SaveCommand = new Command(OnSaveSettings);
            BackCommand = new Command(OnBack);
        }

        // Properties bound to UI
        public bool IsSoundOn { get; set; } = true;
        public bool AreNotificationsOn { get; set; } = true;
        public double GameTime { get; set; } = 10;

        public ICommand SaveCommand { get; }
        public ICommand BackCommand { get; }

        private async void OnSaveSettings()
        {
            // כאן אפשר לשמור את ההגדרות (Preferences, Database וכו')
            await Application.Current.MainPage.DisplayAlert("Settings", "Settings saved!", "OK");
        }

        private async void OnBack()
        {
            await _navigation.PopAsync();
        }
    }
}
