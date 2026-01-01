using Chess.ModelsLogic;
using Chess.Views;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class PawnPromotionPopupVM
    {
        private readonly PawnPromotionPopup _popup;
        public ICommand PromoteToQueenCommand { get; private set; }
        public ICommand PromoteToRookCommand { get; private set; }
        public ICommand PromoteToBishopCommand { get; private set; }
        public ICommand PromoteToKnightCommand { get; private set; }
        private int Row { get; set; }
        private int Column { get; set; }
        public PawnPromotionPopupVM(PawnPromotionPopup popup,int row,int column)
        {
            _popup = popup;
            Row = row;
            Column = column;
            PromoteToQueenCommand = new Command(QueenPromotion);
            PromoteToRookCommand = new Command(RookPromotion);
            PromoteToBishopCommand = new Command(BishopPromotion);
            PromoteToKnightCommand = new Command(KnightPromotion);
        }
        private void KnightPromotion(object obj)
        {
            
        }
        private void BishopPromotion(object obj)
        {
            
        }
        private void RookPromotion(object obj)
        {
           
        }
        private void QueenPromotion(object obj)
        {
           
        }
    }
}
