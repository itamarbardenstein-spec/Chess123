using Chess.ModelsLogic;
using Chess.ViewModel;
using CommunityToolkit.Maui.Views;

namespace Chess.Views
{
    public partial class PawnPromotionPopup : Popup
    {
        public PawnPromotionPopup(int row, int column, Game game)
        {
            InitializeComponent();
            BindingContext = new PawnPromotionPopupVM(this, row, column, game);
        }
    }
}

