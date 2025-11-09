using Chess.ViewModel;

namespace Chess
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageVm mpVM = new();
        public MainPage()
        {
            InitializeComponent();
            BindingContext = mpVM;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            mpVM.AddSnapshotListener();
        }

        protected override void OnDisappearing()
        {
            mpVM.RemoveSnapshotListener();
            base.OnDisappearing();
        }
    }       
    
}
