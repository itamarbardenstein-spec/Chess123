namespace Chess.ModelsLogic
{
    /// Represents a group of captured pieces of the same type and color for UI display
    public class CapturedPieceGroup
    {
        /// The visual asset path representing the type of captured piece
        public string ImageSource { get; set; } = string.Empty;
        /// The total number of pieces of this type that have been captured
        public int Count { get; set; }
        /// Logic to determine if a numerical counter should be displayed
        public bool ShowCount => Count > 1;
    }
}