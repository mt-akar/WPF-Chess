using ChessBoard.Essentials;

namespace ChessBoard.Pieces
{
    public class Piece
    {
        string type;
        public string Type { get { return type; } set { type = value; } }

        Player player;
        public Player Player { get { return player; } set { player = value; } }

        bool moved;
        public bool Moved { get { return moved; } set { moved = value; } }

        public Piece(string t, Player p)
        {
            type = t;
            player = p;
            moved = false;
        }

        public Piece Duplicate()
        {
            return new Piece(type, player);
        }
    }
}
