namespace Chess.Models
{
    static class Strings
    {
        public const string Register = "Register";
        public const string UserName = "User name";
        public const string Password = "Password";
        public const string Email = "Email";
        public const string Age = "Age";
        public const string Login = "Login";
        public const string RegistrationFailed = "Registration Failed";
        public const string Ok = "OK";
        public const string UnknownErrorMessage = "Unknown Error";
        public const string WeakPassword = "WeakPassword";
        public const string WeakPasswordErrMessage = "Password needs to be at least six charecters long";
        public const string InvalidEmailErrMessage = "Invalid Email address";
        public const string ErrMessageReason = "Reason";
        public const string EmailExists = "EmailExists";
        public const string InvalidEmailAddress = "InvalidEmailAddress";
        public const string EmailExistsErrMsg = "This email is already in use";
        public const string InvalidLogin = "INVALID_LOGIN_CREDENTIALS";
        public const string InvalidLoginErrMsg = "Email or password is incorrect";
        public const string Play = "Play";
        public const string Instructions = "Instructions";
        public static string JoinGameErr = "Error in joining game";
        public static string GameDeleted ="Game has been deleted";
        public const string SelectGameTime = "Select Game Time";
        public const string AppTitle = "Chess    ©Itamar.B.S";
        public const string WaitMessage = "Please wait";
        public const string PlayMessage = "Play please";
        public const string BlackKing = "chess_king_svgrepo_com.svg";
        public const string WhiteKing = "white_king.png";
        public const string BlackQueen = "black_queen.svg";
        public const string WhiteQueen = "white_queen.png";
        public const string BlackRook = "black_rook.png";
        public const string WhiteRook = "white_rook.png";
        public const string BlackPawn = "black_pawn.png";
        public const string WhitePawn = "white_pawn.png";
        public const string BlackBishop = "black_bishop.png";
        public const string WhiteBishop = "white_bishop.png";   
        public const string BlackKnight = "black_knight.png";
        public const string WhiteKnight = "white_knight.png";
        public const string InvalidMove = "Illegal Move! try again";
        public const string BoardColorWhite = "#F0D9B5";
        public const string BoardColorBlack = "#B58863";
        public const string Checkmate = "Checkmate";
        public const string YouWon = "You Won!!";
        public const string YouLost = "You Lost";
        public const string Win = "You won the game by checkmate, well done!";
        public const string Lose = "You lost the game by checkmate, try again";
        public const string InsructionsTxt = "White and black\r\nThe player controlling the white pieces is named White; the player controlling the black pieces is named Black. White moves first, then players alternate moves. Making a move is required it is not legal to skip a move, even when having to move is detrimental. Play continues until a king is checkmated, a player resigns, or a draw is declared, as explained below. In addition, if the game is being played under a time control, a player who exceeds the time limit loses the game unless they cannot be checkmated. \r\nThe official chess rules do not include a procedure for determining who plays White. Instead, this decision is left open to tournament-specific rules (e.g. a Swiss system tournament or round-robin tournament) or, in the case of casual play, mutual agreement, in which case some kind of random choice such as flipping a coin can be employed. A common method is for one player to conceal a pawn of each color in either hand; the other player chooses a hand to open and receives the color of the piece that is revealed. \r\nMovement\r\nBasic moves\r\nEach type of chess piece has its own method of movement. A piece moves to a vacant square except when capturing an opponent's piece. \r\nExcept for any move of the knight and castling, pieces cannot jump over other pieces. A piece is captured (or taken) when an attacking enemy piece replaces it on its square. The captured piece is thereby permanently removed from the game.[a] The king can be put in check but cannot be captured (see below).\r\n•\tThe king moves exactly one square adjacent to it. A special move with the king known as castling is allowed only once per player, per game (see below).\r\n•\tA rook moves any number of vacant squares horizontally or vertically. It also is moved when castling.\r\n•\tA bishop moves any number of vacant squares diagonally. (Thus a bishop can move to only light or dark squares, not both.)\r\n•\tThe queen moves any number of vacant squares horizontally, vertically, or diagonally.\r\n•\tA knight moves to one of the nearest squares not on the same rank, file, or diagonal. (This can be thought of as moving two squares horizontally then one square vertically, or moving one square horizontally then two squares vertically—i.e. in an L pattern.) The knight is not blocked by other pieces; it jumps to the new location.\r\n•\tPawns have the most complex rules of movement:\r\n•\tA pawn moves straight forward one square, if that square is vacant. If it has not yet moved, a pawn also has the option of moving two squares straight forward, provided both squares are vacant. Pawns cannot move backwards.\r\n•\tA pawn, unlike other pieces, captures differently from how it moves. A pawn can capture an enemy piece on either of the two squares diagonally in front of the pawn. It cannot move to those squares when vacant except when capturing en passant.\r\nThe pawn is also involved in the two special moves en passant and promotion. \r\nCastling\r\nCastling consists of moving the king two squares towards a rook, then placing the rook on the other side of the king, adjacent to it. Castling is only permissible if all of the following conditions hold:\r\nThe king and rook involved in castling must not have previously moved;\r\n•\tThere must be no pieces between the king and the rook;\r\n•\tThe king may not currently be under attack, nor may the king pass through or end up in a square that is under attack by an enemy piece (though the rook is permitted to be under attack and to pass over an attacked square);\r\n•\tThe castling rook must be on the same rank as the king\r\nAn unmoved king and an unmoved rook of the same color on the same rank are said to have castling rights. \r\nEn passant\r\nWhen a pawn advances two squares on its initial move and ends the turn adjacent to an enemy pawn on the same rank, it may be captured en passant by the enemy pawn as if it had moved only one square. This capture is legal only on the move immediately following the pawn's advance. The diagrams demonstrate an instance of this: if the white pawn moves from a2 to a4, the black pawn on b4 can capture it en passant, moving from b4 to a3, and the white pawn on a4 is removed from the board. \r\nPromotion\r\nIf a player advances a pawn to its eighth rank, the pawn is then promoted (converted) to a queen, rook, bishop, or knight of the same color at the choice of the player (a queen is most common). The choice is not limited to previously captured pieces. Hence it is theoretically possible for a player to have up to nine queens or up to ten rooks, bishops, or knights if all of the player's pawns are promoted. \r\nCheck\r\nBlack's king is in check. It can get out of check by moving to an unattacked square. Black can also parry the check by moving the bishop to e5 or the knight to f4 to block the check, or by capturing the white bishop with the knight. \r\nA king is in check when it is under attack by at least one enemy piece. A piece unable to move because it would place its own king in check (it is pinned against its own king) may still deliver check to the opposing player. \r\nIt is illegal to make a move that places or leaves one's king in check. The possible ways to get out of check are:\r\n•\tMove the king to a square where it is not in check.\r\n•\tCapture the checking piece.\r\n•\tInterpose a piece between the king and the opponent's threatening piece (block the check).\r\n•\tIn informal games, it is customary to announce \"check\" when making a move that puts the opponent's king in check. In formal competitions, however, check is rarely announced. \r\nEnd of the game\r\nWhite is checkmated; Black wins. \r\nIf a player's king is placed in check and there is no legal move that player can make to escape check, then the king is said to be checkmated, the game ends, and that player loses. Unlike the other pieces, the king is never captured. \r\nThe diagram shows an example checkmate position. The white king is threatened by the black queen; the empty square to which the king could move is also threatened; and the king cannot capture the queen, because it would then be in check by the rook.\r\n\r\n";
    }
}
