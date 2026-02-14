namespace Chess.ModelsLogic
{
    public class CapturedPieceGroup
    {
        public string ImageSource { get; set; }=string.Empty;
        public int Count { get; set; }
        public bool ShowCount => Count > 1; // להציג מספר רק אם יש יותר מ-1
    }
}
