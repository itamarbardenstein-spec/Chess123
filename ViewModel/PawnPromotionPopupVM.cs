using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class PawnPromotionPopupVM
    {
        private readonly Game game;
        private readonly PawnPromotionPopup _popup;
        public ICommand PromoteToQueenCommand { get; private set; }
        public ICommand PromoteToRookCommand { get; private set; }
        public ICommand PromoteToBishopCommand { get; private set; }
        public ICommand PromoteToKnightCommand { get; private set; }
        private int Column { get; set; }
        private int Row { get; set; }
        public PawnPromotionPopupVM(PawnPromotionPopup popup, int row, int column, Game game)
        {
            _popup = popup;
            this.game = game;
            Column = column;
            Row = row;
            PromoteToQueenCommand = new Command(QueenPromotion);
            PromoteToRookCommand = new Command(RookPromotion);
            PromoteToBishopCommand = new Command(BishopPromotion);
            PromoteToKnightCommand = new Command(KnightPromotion);
        }
        private void KnightPromotion(object obj)
        {
            game.Promotion(Row, Column, Strings.Knight, true);
            _popup.Close();
        }
        private void BishopPromotion(object obj)
        {
            game.Promotion(Row, Column, Strings.Bishop, true);
            _popup.Close();
        }
        private void RookPromotion(object obj)
        {
            game.Promotion(Row, Column, Strings.Rook, true);
            _popup.Close();
        }
        private void QueenPromotion(object obj)
        {
            game.Promotion(Row, Column, Strings.Queen, true);
            _popup.Close();
        }
    }
}
