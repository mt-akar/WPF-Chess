using ChessBoard.Essentials;
using ChessBoard.Pieces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

/*
 * To Do List:
 * 50 Move Rule
 * Threefold Repetition Rule
 */

namespace ChessBoard
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        static BitmapImage emptyImage = new BitmapImage(new Uri(@"PP/ee.png", UriKind.RelativeOrAbsolute)); // Transparent image

        Square[,] table;        // Chess table, 2D Square Array
        int t;                  // If t = 2k, it is White's k'th turn, if t = 2k + , it is Black's k'th turn
        Square cur;             // Currently selected square that contains the currently selected piece
        Square[,,] history;     // History of the board. history[t,,] is table[]
        bool gameEnded;         // Store wheather if game is ended or not. Don't allow continuation after complation.

        string statusMessage;
        public string StatusMessage { get { return statusMessage; } set { statusMessage = value; OnPropertyChanged("StatusMessage"); } }

        public MainWindow()
        {
            // Initialiations
            InitializeComponent();

            table = new Square[9, 9];           // (0 indexes won't be used.)
            // Construct a default chess table
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (i == 1 && (j == 1 || j == 8))
                    {
                        table[i, j] = new Square(i, j, new Piece("Rook", Player.White));
                    }
                    else if (i == 8 && (j == 1 || j == 8))
                    {
                        table[i, j] = new Square(i, j, new Piece("Rook", Player.Black));
                    }
                    else if (i == 1 && (j == 2 || j == 7))
                    {
                        table[i, j] = new Square(i, j, new Piece("Knight", Player.White));
                    }
                    else if (i == 8 && (j == 2 || j == 7))
                    {
                        table[i, j] = new Square(i, j, new Piece("Knight", Player.Black));
                    }
                    else if (i == 1 && (j == 3 || j == 6))
                    {
                        table[i, j] = new Square(i, j, new Piece("Bishop", Player.White));
                    }
                    else if (i == 8 && (j == 3 || j == 6))
                    {
                        table[i, j] = new Square(i, j, new Piece("Bishop", Player.Black));
                    }
                    else if (i == 1 && j == 4)
                    {
                        table[i, j] = new Square(i, j, new Piece("Queen", Player.White));
                    }
                    else if (i == 8 && j == 4)
                    {
                        table[i, j] = new Square(i, j, new Piece("Queen", Player.Black));
                    }
                    else if (i == 1 && j == 5)
                    {
                        table[i, j] = new Square(i, j, new Piece("King", Player.White));
                    }
                    else if (i == 8 && j == 5)
                    {
                        table[i, j] = new Square(i, j, new Piece("King", Player.Black));
                    }
                    else if (i == 2)
                    {
                        table[i, j] = new Square(i, j, new Piece("Pawn", Player.White));
                    }
                    else if (i == 7)
                    {
                        table[i, j] = new Square(i, j, new Piece("Pawn", Player.Black));
                    }
                    else
                    {
                        table[i, j] = new Square(i, j, null);
                    }
                }
            }
            t = -1;                             // t is set to -1 because UpdateHistory() is going to increase it severeal lines later. Optimizations can be done to management of t.
            cur = null;                         // No square is chosen initially
            history = new Square[1000, 9, 9];   // Game may have max of 1000 moves as it is.
            UpdateHistory();                    // Proccess the initial board to history.
            gameEnded = false;                  // gameEnded is false untill game ends.
            this.StatusMessage = "Welcome";

            /* On the board on the UI every square contains a Stack Panel that contains an Image.
             * Stack Panel handles the background color of the square while Image handles the image of the piece that is on the square.
             * For each square, both Stack Panel and Image are bound to the the same Square object.
             */

            #region Stack Panels' DataContaxt Bindings

            sp11.DataContext = table[1, 1];
            sp12.DataContext = table[1, 2];
            sp13.DataContext = table[1, 3];
            sp14.DataContext = table[1, 4];
            sp15.DataContext = table[1, 5];
            sp16.DataContext = table[1, 6];
            sp17.DataContext = table[1, 7];
            sp18.DataContext = table[1, 8];

            sp21.DataContext = table[2, 1];
            sp22.DataContext = table[2, 2];
            sp23.DataContext = table[2, 3];
            sp24.DataContext = table[2, 4];
            sp25.DataContext = table[2, 5];
            sp26.DataContext = table[2, 6];
            sp27.DataContext = table[2, 7];
            sp28.DataContext = table[2, 8];

            sp31.DataContext = table[3, 1];
            sp32.DataContext = table[3, 2];
            sp33.DataContext = table[3, 3];
            sp34.DataContext = table[3, 4];
            sp35.DataContext = table[3, 5];
            sp36.DataContext = table[3, 6];
            sp37.DataContext = table[3, 7];
            sp38.DataContext = table[3, 8];

            sp41.DataContext = table[4, 1];
            sp42.DataContext = table[4, 2];
            sp43.DataContext = table[4, 3];
            sp44.DataContext = table[4, 4];
            sp45.DataContext = table[4, 5];
            sp46.DataContext = table[4, 6];
            sp47.DataContext = table[4, 7];
            sp48.DataContext = table[4, 8];

            sp51.DataContext = table[5, 1];
            sp52.DataContext = table[5, 2];
            sp53.DataContext = table[5, 3];
            sp54.DataContext = table[5, 4];
            sp55.DataContext = table[5, 5];
            sp56.DataContext = table[5, 6];
            sp57.DataContext = table[5, 7];
            sp58.DataContext = table[5, 8];

            sp61.DataContext = table[6, 1];
            sp62.DataContext = table[6, 2];
            sp63.DataContext = table[6, 3];
            sp64.DataContext = table[6, 4];
            sp65.DataContext = table[6, 5];
            sp66.DataContext = table[6, 6];
            sp67.DataContext = table[6, 7];
            sp68.DataContext = table[6, 8];

            sp71.DataContext = table[7, 1];
            sp72.DataContext = table[7, 2];
            sp73.DataContext = table[7, 3];
            sp74.DataContext = table[7, 4];
            sp75.DataContext = table[7, 5];
            sp76.DataContext = table[7, 6];
            sp77.DataContext = table[7, 7];
            sp78.DataContext = table[7, 8];

            sp81.DataContext = table[8, 1];
            sp82.DataContext = table[8, 2];
            sp83.DataContext = table[8, 3];
            sp84.DataContext = table[8, 4];
            sp85.DataContext = table[8, 5];
            sp86.DataContext = table[8, 6];
            sp87.DataContext = table[8, 7];
            sp88.DataContext = table[8, 8];

            #endregion

            #region Images' DataContext Bindings

            im11.DataContext = table[1, 1];
            im12.DataContext = table[1, 2];
            im13.DataContext = table[1, 3];
            im14.DataContext = table[1, 4];
            im15.DataContext = table[1, 5];
            im16.DataContext = table[1, 6];
            im17.DataContext = table[1, 7];
            im18.DataContext = table[1, 8];

            im21.DataContext = table[2, 1];
            im22.DataContext = table[2, 2];
            im23.DataContext = table[2, 3];
            im24.DataContext = table[2, 4];
            im25.DataContext = table[2, 5];
            im26.DataContext = table[2, 6];
            im27.DataContext = table[2, 7];
            im28.DataContext = table[2, 8];

            im31.DataContext = table[3, 1];
            im32.DataContext = table[3, 2];
            im33.DataContext = table[3, 3];
            im34.DataContext = table[3, 4];
            im35.DataContext = table[3, 5];
            im36.DataContext = table[3, 6];
            im37.DataContext = table[3, 7];
            im38.DataContext = table[3, 8];

            im41.DataContext = table[4, 1];
            im42.DataContext = table[4, 2];
            im43.DataContext = table[4, 3];
            im44.DataContext = table[4, 4];
            im45.DataContext = table[4, 5];
            im46.DataContext = table[4, 6];
            im47.DataContext = table[4, 7];
            im48.DataContext = table[4, 8];

            im51.DataContext = table[5, 1];
            im52.DataContext = table[5, 2];
            im53.DataContext = table[5, 3];
            im54.DataContext = table[5, 4];
            im55.DataContext = table[5, 5];
            im56.DataContext = table[5, 6];
            im57.DataContext = table[5, 7];
            im58.DataContext = table[5, 8];

            im61.DataContext = table[6, 1];
            im62.DataContext = table[6, 2];
            im63.DataContext = table[6, 3];
            im64.DataContext = table[6, 4];
            im65.DataContext = table[6, 5];
            im66.DataContext = table[6, 6];
            im67.DataContext = table[6, 7];
            im68.DataContext = table[6, 8];

            im71.DataContext = table[7, 1];
            im72.DataContext = table[7, 2];
            im73.DataContext = table[7, 3];
            im74.DataContext = table[7, 4];
            im75.DataContext = table[7, 5];
            im76.DataContext = table[7, 6];
            im77.DataContext = table[7, 7];
            im78.DataContext = table[7, 8];

            im81.DataContext = table[8, 1];
            im82.DataContext = table[8, 2];
            im83.DataContext = table[8, 3];
            im84.DataContext = table[8, 4];
            im85.DataContext = table[8, 5];
            im86.DataContext = table[8, 6];
            im87.DataContext = table[8, 7];
            im88.DataContext = table[8, 8];

            #endregion

            // statusMessageBox contains information about the recent action in the game.
            statusMessageBox.DataContext = this;

            // Set the icon of the window. Credits: Google
            this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/kl.ico", UriKind.RelativeOrAbsolute));
        }

        /* The glorious methot of the game. It is called whenever a square is clicked.
         * Parameters are given by the IDE.
         */
        private void LMDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Last sender image is stored in the Square.sel
            Square sen = ((Square)((sender as DependencyObject).GetValue(FrameworkElement.DataContextProperty)));

            // Whole method returns, if gameEnded is true
            if (gameEnded)
            {
                return;
            }

            if (sen == cur)
            {
                cur = null;
                Repaint();
                return;
            }

            // If a player have choosen a piece and changes his mind to make a move with another one of his pieces.
            if (cur != null && sen.Piece != null && sen.Piece.Player == cur.Piece.Player)
            {
                cur = null;
                Repaint();
            }

            // If player hasn't choose a piece and tries to choose an oponent piece or an empty square, method returns.
            if (cur == null && (sen.Piece == null || (t % 2 == 0 && sen.Piece.Player == Player.Black) || (t % 2 == 1 && sen.Piece.Player == Player.White)))
            {
                return;
            }

            // PREVIEW! Picks a piece to preview the square that the piece can possibly move. The squares that the chosen piece can possibly move will be painted.
            else if (cur == null && ((t % 2 == 0 && sen.Piece.Player == Player.White) || (t % 2 == 1 && sen.Piece.Player == Player.Black)))
            {
                // Paint the chosen piece too so that even if there are no possible moves, UI looks responsive
                sen.BackColor.Color = Color.FromArgb(255, 204, 255, 220);

                ArrayList moves = PossibleMoves(sen.Row, sen.Column); // In order to not run the PossibleMoves() multiple times
                for (int i = 0; i < moves.Count; i += 2)
                {
                    table[(int)moves[i], (int)moves[i + 1]].BackColor.Color = Color.FromArgb(255, 204, 255, 220);
                }

                // Set the currently selected square to the sender square
                cur = sen;
            }

            //////////////////////////////////////////////////////////////
            // Move (If a piece is already chosen, make the actual move //
            //////////////////////////////////////////////////////////////
            else if (cur != null)
            {
                // If destination square represents a valid move, which means the square is painted, Move()
                if (sen.BackColor.Color == Color.FromArgb(255, 204, 255, 220))
                {
                    // Set the statusMessage to show the notation
                    this.StatusMessage = "" + ToLetter(cur.Column) + cur.Row + " - " + ToLetter(sen.Column) + sen.Row;
                    cur.Piece.Moved = true; // A rook would not have been moved in case of a castle but it is irrelevant at that point anyways.
                    Move(cur, sen);
                }

                else
                {
                    // Inform the player that he has made an invalid move
                    this.StatusMessage = "Invalid move";
                }
                // Empty the currently selected square and repaint the board so that the game is ready to receive a new piece to move
                cur = null;
                Repaint();

                ////////////////////
                // End Game Check //
                ////////////////////

                // Loop the whole board. On each loop, check if you find a possible move for current player, return the method.
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        if (table[i, j].Piece != null && ((table[i, j].Piece.Player == Player.White && t % 2 == 0) || (table[i, j].Piece.Player == Player.Black && t % 2 == 1)) && PossibleMoves(i, j).Count != 0)
                        {
                            return;
                        }
                    }
                }
                // If loop exit, set gameEnded = true
                gameEnded = true;
                // Then loop the whole board again. On each loop, check if the current player's king is UnderThreat(). 
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        if (table[i, j].Piece != null && table[i, j].Piece.Type == "King" &&
                            ((t % 2 == 0 && table[i, j].Piece.Player == Player.White && UnderThreat(i, j, Player.White)) ||
                            (t % 2 == 1 && table[i, j].Piece.Player == Player.Black && UnderThreat(i, j, Player.Black))))
                        {
                            // If so, it is checkmate, return the method.
                            this.StatusMessage = t % 2 == 1 ? "Checkmate - White Wins" : "Checkmate - Black Wins";
                            return;
                        }
                    }
                }
                // If not, it is stalemate.
                this.StatusMessage = "Stalemate";
            }

            else
            {
                Console.WriteLine("Error 6");
            }
        }

        /* Check if a square is under thread for a player.
         * This boolean value will be used to check checks and castle conditions.
         * @param ir is the row of the square that is wanted to control.
         * @param ic is the column of the square that is wanted to control.
         * @param p is the player that is supposedly threatened by his player on the given square
         */
        private bool UnderThreat(int ir, int ic, Player p)
        {
            // Check is the square is under threat by a Knight
            {
                if (ir + 2 <= 8 && ic + 1 <= 8 && table[ir + 2, ic + 1].Piece != null && table[ir + 2, ic + 1].Piece.Type == "Knight" && table[ir + 2, ic + 1].Piece.Player != p)
                {
                    return true;
                }
                if (ir + 1 <= 8 && ic + 2 <= 8 && table[ir + 1, ic + 2].Piece != null && table[ir + 1, ic + 2].Piece.Type == "Knight" && table[ir + 1, ic + 2].Piece.Player != p)
                {
                    return true;
                }
                if (ir - 1 >= 1 && ic + 2 <= 8 && table[ir - 1, ic + 2].Piece != null && table[ir - 1, ic + 2].Piece.Type == "Knight" && table[ir - 1, ic + 2].Piece.Player != p)
                {
                    return true;
                }
                if (ir - 2 >= 1 && ic + 1 <= 8 && table[ir - 2, ic + 1].Piece != null && table[ir - 2, ic + 1].Piece.Type == "Knight" && table[ir - 2, ic + 1].Piece.Player != p)
                {
                    return true;
                }
                if (ir - 2 >= 1 && ic - 1 >= 1 && table[ir - 2, ic - 1].Piece != null && table[ir - 2, ic - 1].Piece.Type == "Knight" && table[ir - 2, ic - 1].Piece.Player != p)
                {
                    return true;
                }
                if (ir - 1 >= 1 && ic - 2 >= 1 && table[ir - 1, ic - 2].Piece != null && table[ir - 1, ic - 2].Piece.Type == "Knight" && table[ir - 1, ic - 2].Piece.Player != p)
                {
                    return true;
                }
                if (ir + 1 <= 8 && ic - 2 >= 1 && table[ir + 1, ic - 2].Piece != null && table[ir + 1, ic - 2].Piece.Type == "Knight" && table[ir + 1, ic - 2].Piece.Player != p)
                {
                    return true;
                }
                if (ir + 2 <= 8 && ic - 1 >= 1 && table[ir + 2, ic - 1].Piece != null && table[ir + 2, ic - 1].Piece.Type == "Knight" && table[ir + 2, ic - 1].Piece.Player != p)
                {
                    return true;
                }
            }

            // Check is the square is under threat by a Bishop or a Queen, diagonally
            {
                int r = ir;
                int c = ic;
                while (r + 1 <= 8 && c + 1 <= 8)
                {
                    if (table[++r, ++c].Piece != null && table[r, c].Piece.Player != p && (table[r, c].Piece.Type == "Bishop" || table[r, c].Piece.Type == "Queen"))
                    {
                        return true;
                    }
                    else if (table[r, c].Piece != null)
                        break;
                }
                r = ir;
                c = ic;
                while (r - 1 >= 1 && c + 1 <= 8)
                {
                    if (table[--r, ++c].Piece != null && table[r, c].Piece.Player != p && (table[r, c].Piece.Type == "Bishop" || table[r, c].Piece.Type == "Queen"))
                    {
                        return true;
                    }
                    else if (table[r, c].Piece != null)
                        break;
                }
                r = ir;
                c = ic;
                while (r - 1 >= 1 && c - 1 >= 1)
                {
                    if (table[--r, --c].Piece != null && table[r, c].Piece.Player != p && (table[r, c].Piece.Type == "Bishop" || table[r, c].Piece.Type == "Queen"))
                    {
                        return true;
                    }
                    else if (table[r, c].Piece != null)
                        break;
                }
                r = ir;
                c = ic;
                while (r + 1 <= 8 && c - 1 >= 1)
                {
                    if (table[++r, --c].Piece != null && table[r, c].Piece.Player != p && (table[r, c].Piece.Type == "Bishop" || table[r, c].Piece.Type == "Queen"))
                    {
                        return true;
                    }
                    else if (table[r, c].Piece != null)
                        break;
                }
                r = ir;
                c = ic;
            }

            // Check is the square is under threat by a Rook and Queen, vertically or horizontally
            {
                int r = ir;
                while (r + 1 <= 8)
                {
                    if (table[++r, ic].Piece != null && table[r, ic].Piece.Player != p && (table[r, ic].Piece.Type == "Rook" || table[r, ic].Piece.Type == "Queen"))
                    {
                        return true;
                    }
                    else if (table[r, ic].Piece != null)
                        break;
                }
                r = ir;
                while (r - 1 >= 1)
                {
                    if (table[--r, ic].Piece != null && table[r, ic].Piece.Player != p && (table[r, ic].Piece.Type == "Rook" || table[r, ic].Piece.Type == "Queen"))
                    {
                        return true;
                    }
                    else if (table[r, ic].Piece != null)
                        break;
                }
                r = ir;
                int c = ic;
                while (c + 1 <= 8)
                {
                    if (table[ir, ++c].Piece != null && table[ir, c].Piece.Player != p && (table[ir, c].Piece.Type == "Rook" || table[ir, c].Piece.Type == "Queen"))
                    {
                        return true;
                    }
                    else if (table[ir, c].Piece != null)
                        break;
                }
                c = ic;
                while (c - 1 >= 1)
                {
                    if (table[ir, --c].Piece != null && table[ir, c].Piece.Player != p && (table[ir, c].Piece.Type == "Rook" || table[ir, c].Piece.Type == "Queen"))
                    {
                        return true;
                    }
                    else if (table[ir, c].Piece != null)
                        break;
                }
                c = ic;
            }

            // Check is the square is under threat by a King
            {
                if (ir + 1 <= 8 && table[ir + 1, ic].Piece != null && table[ir + 1, ic].Piece.Type == "King" && table[ir + 1, ic].Piece.Player != p)
                {
                    return true;
                }
                if (ir + 1 <= 8 && ic + 1 <= 8 && table[ir + 1, ic + 1].Piece != null && table[ir + 1, ic + 1].Piece.Type == "King" && table[ir + 1, ic + 1].Piece.Player != p)
                {
                    return true;
                }
                if (ic + 1 <= 8 && table[ir, ic + 1].Piece != null && table[ir, ic + 1].Piece.Type == "King" && table[ir, ic + 1].Piece.Player != p)
                {
                    return true;
                }
                if (ir - 1 >= 1 && ic + 1 <= 8 && table[ir - 1, ic + 1].Piece != null && table[ir - 1, ic + 1].Piece.Type == "King" && table[ir - 1, ic + 1].Piece.Player != p)
                {
                    return true;
                }
                if (ir - 1 >= 1 && table[ir - 1, ic].Piece != null && table[ir - 1, ic].Piece.Type == "King" && table[ir - 1, ic].Piece.Player != p)
                {
                    return true;
                }
                if (ir - 1 >= 1 && ic - 1 >= 1 && table[ir - 1, ic - 1].Piece != null && table[ir - 1, ic - 1].Piece.Type == "King" && table[ir - 1, ic - 1].Piece.Player != p)
                {
                    return true;
                }
                if (ic - 1 >= 1 && table[ir, ic - 1].Piece != null && table[ir, ic - 1].Piece.Type == "King" && table[ir, ic - 1].Piece.Player != p)
                {
                    return true;
                }
                if (ir + 1 <= 8 && ic - 1 >= 1 && table[ir + 1, ic - 1].Piece != null && table[ir + 1, ic - 1].Piece.Type == "King" && table[ir + 1, ic - 1].Piece.Player != p)
                {
                    return true;
                }
            }

            // Check is the square is under threat by a Pawn
            {
                if (ir + 1 <= 8 && ic + 1 <= 8 && table[ir + 1, ic + 1].Piece != null && table[ir + 1, ic + 1].Piece.Type == "Pawn" && table[ir + 1, ic + 1].Piece.Player == Player.Black && p == Player.White)
                {
                    return true;
                }
                if (ir + 1 <= 8 && ic - 1 >= 1 && table[ir + 1, ic - 1].Piece != null && table[ir + 1, ic - 1].Piece.Type == "Pawn" && table[ir + 1, ic - 1].Piece.Player == Player.Black && p == Player.White)
                {
                    return true;
                }
                if (ir - 1 >= 1 && ic + 1 <= 8 && table[ir - 1, ic + 1].Piece != null && table[ir - 1, ic + 1].Piece.Type == "Pawn" && table[ir - 1, ic + 1].Piece.Player == Player.White && p == Player.Black)
                {
                    return true;
                }
                if (ir - 1 >= 1 && ic - 1 >= 1 && table[ir - 1, ic - 1].Piece != null && table[ir - 1, ic - 1].Piece.Type == "Pawn" && table[ir - 1, ic - 1].Piece.Player == Player.White && p == Player.Black)
                {
                    return true;
                }
            }
            return false;
        }

        /* Move the piece on the @param.from to the square @param.to
         * Don't ask if the move is possible in the context of chess.
         * @param from is the square that contains the piece that is wanted to move
         * @param to is the square that is the destination for the piece that is wanted to move
         */
        private void Move(Square from, Square to)
        {
            // Check if the move is Promotion
            if (from.Piece.Type == "Pawn" && ((to.Row == 8 && from.Piece.Player == Player.White) || (to.Row == 1 && from.Piece.Player == Player.Black)))
            {
                to.Piece = new Piece("Queen", from.Piece.Player); // Promotes to a queen as default
                to.Im = (from.Piece.Player == Player.White) ? to.Im = new BitmapImage(new Uri(@"PP/ql.png", UriKind.RelativeOrAbsolute)) :
                    to.Im = new BitmapImage(new Uri(@"PP/qd.png", UriKind.RelativeOrAbsolute));
                from.Piece = null;
                from.Im = emptyImage;
            }

            // Check if the move is Castle
            else if (from.Piece.Type == "King" && Math.Abs(from.Column - to.Column) > 1)
            {
                // Move King
                to.Piece = from.Piece;
                to.Im = from.Im;
                from.Piece = null;
                from.Im = emptyImage;

                // Move Rook
                if (to.Column == 7)
                {
                    table[to.Row, 6].Piece = table[to.Row, 8].Piece;
                    table[to.Row, 6].Im = table[to.Row, 8].Im;
                    table[to.Row, 8].Piece = null;
                    table[to.Row, 8].Im = emptyImage;
                }
                else if (to.Column == 3)
                {
                    table[to.Row, 4].Piece = table[to.Row, 1].Piece;
                    table[to.Row, 4].Im = table[to.Row, 1].Im;
                    table[to.Row, 1].Piece = null;
                    table[to.Row, 1].Im = emptyImage;
                }
            }

            // Check if the move is En Passant
            else if (from.Piece.Type == "Pawn" && from.Column != to.Column && to.Piece == null)
            {
                to.Piece = from.Piece;
                to.Im = from.Im;
                from.Piece = null;
                from.Im = emptyImage;
                table[from.Row, to.Column].Piece = null;
                table[from.Row, to.Column].Im = emptyImage;
            }

            // Just a normal slide move or a capture
            else
            {
                to.Piece = from.Piece;
                to.Im = from.Im;
                from.Piece = null;
                from.Im = emptyImage;
            }

            // Increase the m_t and update the m_history
            UpdateHistory();
        }

        /* Find all possible moves for a given piece.
         * Use it later for painting.
         * Returns ArrayList has always even number of elements.
         * If PossibleMoves(int r, int c).Count == 2k, there are k possible moves.
         * 2k'th element is the k'th possible move's desination row.
         * 2k+1'th element is the k'th possible move's desination column.
         * For each k [PossibleMoves(int r, int c)[2k], PossibleMoves(int r, int c)[2k + 1]] is the possible destination coordinate of k'th move
         * @param r is the row of the piece that is given.
         * @param c is the column of the piece that is given.
         */
        private ArrayList PossibleMoves(int to_Row, int to_Column)
        {
            ArrayList m = new ArrayList();
            int moveNumber = 0;
            Square s = table[to_Row, to_Column]; // Only to keep things short, no functionality here

            // Make sure you don't get the mighty NullPointerException
            if (s.Piece == null)
            {
                return m;
            }

            // Find the possible moves if the given piece is a Kinght
            if (s.Piece.Type == "Knight")
            {
                if (to_Row + 2 <= 8 && to_Column + 1 <= 8 && (table[to_Row + 2, to_Column + 1].Piece == null || table[to_Row + 2, to_Column + 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row + 2);
                    m.Add(to_Column + 1);
                }
                if (to_Row + 1 <= 8 && to_Column + 2 <= 8 && (table[to_Row + 1, to_Column + 2].Piece == null || table[to_Row + 1, to_Column + 2].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row + 1);
                    m.Add(to_Column + 2);
                }
                if (to_Row - 1 >= 1 && to_Column + 2 <= 8 && (table[to_Row - 1, to_Column + 2].Piece == null || table[to_Row - 1, to_Column + 2].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row - 1);
                    m.Add(to_Column + 2);
                }
                if (to_Row - 2 >= 1 && to_Column + 1 <= 8 && (table[to_Row - 2, to_Column + 1].Piece == null || table[to_Row - 2, to_Column + 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row - 2);
                    m.Add(to_Column + 1);
                }
                if (to_Row - 2 >= 1 && to_Column - 1 >= 1 && (table[to_Row - 2, to_Column - 1].Piece == null || table[to_Row - 2, to_Column - 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row - 2);
                    m.Add(to_Column - 1);
                }
                if (to_Row - 1 >= 1 && to_Column - 2 >= 1 && (table[to_Row - 1, to_Column - 2].Piece == null || table[to_Row - 1, to_Column - 2].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row - 1);
                    m.Add(to_Column - 2);
                }
                if (to_Row + 1 <= 8 && to_Column - 2 >= 1 && (table[to_Row + 1, to_Column - 2].Piece == null || table[to_Row + 1, to_Column - 2].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row + 1);
                    m.Add(to_Column - 2);
                }
                if (to_Row + 2 <= 8 && to_Column - 1 >= 1 && (table[to_Row + 2, to_Column - 1].Piece == null || table[to_Row + 2, to_Column - 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row + 2);
                    m.Add(to_Column - 1);
                }
            }

            // Find the possible diagonal moves if the given piece is a Bishop or a Queen
            if (s.Piece.Type == "Bishop" || s.Piece.Type == "Queen")
            {
                while (to_Row + 1 <= 8 && to_Column + 1 <= 8 && (table[to_Row + 1, to_Column + 1].Piece == null || table[to_Row + 1, to_Column + 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(++to_Row);
                    m.Add(++to_Column);
                    if (table[to_Row, to_Column].Piece != null)
                        break;
                }
                to_Row = s.Row;
                to_Column = s.Column;
                while (to_Row - 1 >= 1 && to_Column + 1 <= 8 && (table[to_Row - 1, to_Column + 1].Piece == null || table[to_Row - 1, to_Column + 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(--to_Row);
                    m.Add(++to_Column);
                    if (table[to_Row, to_Column].Piece != null)
                        break;
                }
                to_Row = s.Row;
                to_Column = s.Column;
                while (to_Row - 1 >= 1 && to_Column - 1 >= 1 && (table[to_Row - 1, to_Column - 1].Piece == null || table[to_Row - 1, to_Column - 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(--to_Row);
                    m.Add(--to_Column);
                    if (table[to_Row, to_Column].Piece != null)
                        break;
                }
                to_Row = s.Row;
                to_Column = s.Column;
                while (to_Row + 1 <= 8 && to_Column - 1 >= 1 && (table[to_Row + 1, to_Column - 1].Piece == null || table[to_Row + 1, to_Column - 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(++to_Row);
                    m.Add(--to_Column);
                    if (table[to_Row, to_Column].Piece != null)
                        break;
                }
                to_Row = s.Row;
                to_Column = s.Column;
            }

            // Find the possible vertical or horizontal moves if the given piece is a Rook or a Queen
            if (s.Piece.Type == "Rook" || s.Piece.Type == "Queen")
            {
                while (to_Row + 1 <= 8 && (table[to_Row + 1, to_Column].Piece == null || table[to_Row + 1, to_Column].Piece.Player != s.Piece.Player))
                {
                    m.Add(++to_Row);
                    m.Add(to_Column);
                    if (table[to_Row, to_Column].Piece != null)
                        break;
                }
                to_Row = s.Row;
                while (to_Row - 1 >= 1 && (table[to_Row - 1, to_Column].Piece == null || table[to_Row - 1, to_Column].Piece.Player != s.Piece.Player))
                {
                    m.Add(--to_Row);
                    m.Add(to_Column);
                    if (table[to_Row, to_Column].Piece != null)
                        break;
                }
                to_Row = s.Row;
                while (to_Column + 1 <= 8 && (table[to_Row, to_Column + 1].Piece == null || table[to_Row, to_Column + 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row);
                    m.Add(++to_Column);
                    if (table[to_Row, to_Column].Piece != null)
                        break;
                }
                to_Column = s.Column;
                while (to_Column - 1 >= 1 && (table[to_Row, to_Column - 1].Piece == null || table[to_Row, to_Column - 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row);
                    m.Add(--to_Column);
                    if (table[to_Row, to_Column].Piece != null)
                        break;
                }
                to_Column = s.Column;
            }

            // Find the possible moves if the given piece is a King
            if (s.Piece.Type == "King")
            {
                if (s.Row + 1 <= 8 && (table[to_Row + 1, to_Column].Piece == null || table[to_Row + 1, to_Column].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row + 1);
                    m.Add(to_Column);
                }
                if (s.Row + 1 <= 8 && to_Column + 1 <= 8 && (table[to_Row + 1, to_Column + 1].Piece == null || table[to_Row + 1, to_Column + 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row + 1);
                    m.Add(to_Column + 1);
                }
                if (s.Column + 1 <= 8 && (table[to_Row, to_Column + 1].Piece == null || table[to_Row, to_Column + 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row);
                    m.Add(to_Column + 1);
                }
                if (s.Row - 1 >= 1 && to_Column + 1 <= 8 && (table[to_Row - 1, to_Column + 1].Piece == null || table[to_Row - 1, to_Column + 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row - 1);
                    m.Add(to_Column + 1);
                }
                if (s.Row - 1 >= 1 && (table[to_Row - 1, to_Column].Piece == null || table[to_Row - 1, to_Column].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row - 1);
                    m.Add(to_Column);
                }
                if (s.Row - 1 >= 1 && to_Column - 1 >= 1 && (table[to_Row - 1, to_Column - 1].Piece == null || table[to_Row - 1, to_Column - 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row - 1);
                    m.Add(to_Column - 1);
                }
                if (s.Column - 1 >= 1 && (table[to_Row, to_Column - 1].Piece == null || table[to_Row, to_Column - 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row);
                    m.Add(to_Column - 1);
                }
                if (s.Row + 1 <= 8 && to_Column - 1 >= 1 && (table[to_Row + 1, to_Column - 1].Piece == null || table[to_Row + 1, to_Column - 1].Piece.Player != s.Piece.Player))
                {
                    m.Add(to_Row + 1);
                    m.Add(to_Column - 1);
                }

                // Special cases: Castling

                // White short castle
                if (to_Row == 1 && to_Column == 5 && !s.Piece.Moved && s.Piece.Player == Player.White &&
                    table[1, 6].Piece == null && !UnderThreat(1, 6, Player.White) &&
                    table[1, 7].Piece == null && !UnderThreat(1, 7, Player.White) &&
                    table[1, 8].Piece != null && table[1, 8].Piece.Type == "Rook" && table[1, 8].Piece.Player == Player.White && !table[1, 8].Piece.Moved)
                {
                    m.Add(to_Row);
                    m.Add(to_Column + 2);
                }
                // White long castle
                if (to_Row == 1 && to_Column == 5 && !s.Piece.Moved && s.Piece.Player == Player.White &&
                    table[1, 4].Piece == null && !UnderThreat(1, 4, Player.White) &&
                    table[1, 3].Piece == null && !UnderThreat(1, 3, Player.White) &&
                    table[1, 2].Piece == null &&
                    table[1, 1].Piece != null && table[1, 1].Piece.Type == "Rook" && table[1, 1].Piece.Player == Player.White && !table[1, 1].Piece.Moved)
                {
                    m.Add(to_Row);
                    m.Add(to_Column - 2);
                }
                // Black short castle
                if (to_Row == 8 && to_Column == 5 && !s.Piece.Moved && s.Piece.Player == Player.Black &&
                    table[8, 6].Piece == null && !UnderThreat(8, 6, Player.Black) &&
                    table[8, 7].Piece == null && !UnderThreat(8, 7, Player.Black) &&
                    table[8, 8].Piece != null && table[8, 8].Piece.Type == "Rook" && table[8, 8].Piece.Player == Player.Black && !table[8, 8].Piece.Moved)
                {
                    m.Add(to_Row);
                    m.Add(to_Column + 2);
                }
                // Black long castle
                if (to_Row == 8 && to_Column == 5 && !s.Piece.Moved && s.Piece.Player == Player.Black &&
                    table[8, 4].Piece == null && !UnderThreat(8, 4, Player.Black) &&
                    table[8, 3].Piece == null && !UnderThreat(8, 3, Player.Black) &&
                    table[8, 2].Piece == null &&
                    table[8, 1].Piece != null && table[8, 1].Piece.Type == "Rook" && table[8, 1].Piece.Player == Player.Black && !table[8, 1].Piece.Moved)
                {
                    m.Add(to_Row);
                    m.Add(to_Column - 2);
                }
            }

            // Find the possible moves if the given piece is a Pawn
            if (s.Piece.Type == "Pawn")
            {
                if (s.Piece.Player == Player.White)
                {
                    if (to_Row + 1 <= 8 && table[to_Row + 1, to_Column].Piece == null)
                    {
                        m.Add(to_Row + 1);
                        m.Add(to_Column);
                    }
                    if (to_Row == 2 && table[to_Row + 1, to_Column].Piece == null && table[to_Row + 2, to_Column].Piece == null)
                    {
                        m.Add(to_Row + 2);
                        m.Add(to_Column);
                    }
                    if (to_Row + 1 <= 8 && to_Column + 1 <= 8 && table[to_Row + 1, to_Column + 1].Piece != null && table[to_Row + 1, to_Column + 1].Piece.Player == Player.Black)
                    {
                        m.Add(to_Row + 1);
                        m.Add(to_Column + 1);
                    }
                    if (to_Row + 1 <= 8 && to_Column - 1 >= 1 && table[to_Row + 1, to_Column - 1].Piece != null && table[to_Row + 1, to_Column - 1].Piece.Player == Player.Black)
                    {
                        m.Add(to_Row + 1);
                        m.Add(to_Column - 1);
                    }
                    // En Passant
                    if (to_Row == 5 && to_Column + 1 <= 8 && t > 0 && history[t - 1, 7, to_Column + 1].Piece != null && history[t - 1, 7, to_Column + 1].Piece.Type == "Pawn" && history[t - 1, 7, to_Column + 1].Piece.Player == Player.Black &&
                        history[t - 1, 6, to_Column + 1].Piece == null && history[t - 1, 5, to_Column + 1].Piece == null &&
                        table[5, to_Column + 1].Piece != null && table[5, to_Column + 1].Piece.Type == "Pawn" && table[5, to_Column + 1].Piece.Player == Player.Black)
                    {
                        m.Add(6);
                        m.Add(to_Column + 1);
                    }
                    else if (to_Row == 5 && to_Column - 1 >= 1 && t > 0 && history[t - 1, 7, to_Column - 1].Piece != null && history[t - 1, 7, to_Column - 1].Piece.Type == "Pawn" && history[t - 1, 7, to_Column - 1].Piece.Player == Player.Black &&
                        history[t - 1, 6, to_Column - 1].Piece == null && history[t - 1, 5, to_Column - 1].Piece == null &&
                        table[5, to_Column - 1].Piece != null && table[5, to_Column - 1].Piece.Type == "Pawn" && table[5, to_Column - 1].Piece.Player == Player.Black)
                    {
                        m.Add(6);
                        m.Add(to_Column - 1);
                    }
                }
                if (s.Piece.Player == Player.Black)
                {
                    if (to_Row - 1 >= 1 && table[to_Row - 1, to_Column].Piece == null)
                    {
                        m.Add(to_Row - 1);
                        m.Add(to_Column);
                        moveNumber++;
                    }
                    if (to_Row == 7 && table[to_Row - 1, to_Column].Piece == null && table[to_Row - 2, to_Column].Piece == null)
                    {
                        m.Add(to_Row - 2);
                        m.Add(to_Column);
                    }
                    if (to_Row - 1 >= 1 && to_Column + 1 <= 8 && table[to_Row - 1, to_Column + 1].Piece != null && table[to_Row - 1, to_Column + 1].Piece.Player == Player.White)
                    {
                        m.Add(to_Row - 1);
                        m.Add(to_Column + 1);
                    }
                    if (to_Row - 1 >= 1 && to_Column - 1 >= 1 && table[to_Row - 1, to_Column - 1].Piece != null && table[to_Row - 1, to_Column - 1].Piece.Player == Player.White)
                    {
                        m.Add(to_Row - 1);
                        m.Add(to_Column - 1);
                    }
                    // En Passant
                    if (to_Row == 4 && to_Column + 1 <= 8 && t > 0 && history[t - 1, 2, to_Column + 1].Piece != null && history[t - 1, 2, to_Column + 1].Piece.Type == "Pawn" && history[t - 1, 2, to_Column + 1].Piece.Player == Player.White &&
                        history[t - 1, 3, to_Column + 1].Piece == null && history[t - 1, 4, to_Column + 1].Piece == null &&
                        table[4, to_Column + 1].Piece != null && table[4, to_Column + 1].Piece.Type == "Pawn" && table[4, to_Column + 1].Piece.Player == Player.White)
                    {
                        m.Add(3);
                        m.Add(to_Column + 1);
                    }
                    else if (to_Row == 4 && to_Column - 1 <= 8 && t > 0 && history[t - 1, 2, to_Column - 1].Piece != null && history[t - 1, 2, to_Column - 1].Piece.Type == "Pawn" && history[t - 1, 2, to_Column - 1].Piece.Player == Player.White &&
                        history[t - 1, 3, to_Column - 1].Piece == null && history[t - 1, 4, to_Column - 1].Piece == null &&
                        table[4, to_Column - 1].Piece != null && table[4, to_Column - 1].Piece.Type == "Pawn" && table[4, to_Column - 1].Piece.Player == Player.White)
                    {
                        m.Add(3);
                        m.Add(to_Column - 1);
                    }
                }
            }

            /* Eliminate all moves that don't satisfy the King safety requirement.
             * Checking for this requirement before every addition felt like too many lines. (30 x 24) Execution takes more time in this implementation.
             */

            /* Loop the ArrayList m for possible moves.
             * Actually do the move with the intention of controlling the king and reverting the move.
             * Loop the entire board, check if the current player's king is still safe after the imaginary move
             * If not, get rid of that possible move.
             * Else take the move back, set the piece to be not moved, continue loopin.
             * Return the World.
             * 
             * If this checking the King safety requirement is done while adding instead of having added all of them, code would be more optimized
             */

            for (int k = 0; k < m.Count;)
            {
                Move(table[to_Row, to_Column], table[(int)m[k], (int)m[k + 1]]);
                bool possible = true;
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        // The reason why i didn't want to keep kings' locaiton is that I might have more that one king on the board in the future (which is stupid.)
                        if (table[i, j].Piece != null && table[i, j].Piece.Type == "King" &&
                            ((t % 2 == 1 && table[i, j].Piece.Player == Player.White && UnderThreat(i, j, Player.White)) ||
                            (t % 2 == 0 && table[i, j].Piece.Player == Player.Black && UnderThreat(i, j, Player.Black))))
                        {
                            possible = false;
                        }
                    }
                }
                if (!possible)
                {
                    m.RemoveAt(k);
                    m.RemoveAt(k);
                }
                else
                {
                    k += 2;
                }
                DeupdateHistory();
            }
            return m;
        }

        /* Undo method, activates when the undo button is clicked.
         * Pretty much self explanatory.
         */
        private void UndoClicked(object sender, RoutedEventArgs e)
        {
            if (t > 0)
            {
                DeupdateHistory();
                cur = null;
                Repaint();
                StatusMessage = "Undo";
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // This method is under development for optimization.
        private bool IsKingSafe(Square initial, int to_Row, int to_Column)
        {
            Move(initial, table[to_Row, to_Column]);
            bool possible = true;
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    // The reason why i didn't want to keep kings' locaiton is that i might have more that one king on the board in the future (which is kinda stupid, but not really.)
                    if (table[i, j].Piece != null && table[i, j].Piece.Type == "King" &&
                        ((t % 2 == 1 && table[i, j].Piece.Player == Player.White && UnderThreat(i, j, Player.White)) ||
                        (t % 2 == 0 && table[i, j].Piece.Player == Player.Black && UnderThreat(i, j, Player.Black))))
                    {
                        possible = false;
                    }
                }
            }
            return possible;
        }

        /* Update the m_t.
         * Pass everything on the m_table[x, y] to m_history[t, x, y]
         */
        private void UpdateHistory()
        {
            t++;
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    history[t, i, j] = new Square(table[i, j].Row, table[i, j].Column, table[i, j].Piece) { Im = table[i, j].Im };
                }
            }
        }

        /* Deupdate the m_t.
         * Revert everything on the m_table[x, y] to history[t, x, y]
         * Empty the m_history[t + 1, x, y]
         */
        private void DeupdateHistory()
        {
            t--;
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (history[t, i, j].Piece != null)
                    {
                        table[i, j].Piece = history[t, i, j].Piece.Duplicate();
                    }
                    else
                    {
                        table[i, j].Piece = null;
                    } // Try to change this null part.
                    table[i, j].Im = history[t, i, j].Im;
                    history[t + 1, i, j] = null;
                }
            }
        }

        // Repaint the board to its default colors.
        private void Repaint()
        {
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    table[i, j].BackColor.Color = ((i + j) % 2 == 0) ? Color.FromArgb(255, 221, 221, 221) : Color.FromArgb(255, 255, 255, 255);
                }
            }
        }

        /* Express the column as letter for notation purposes.
         * @param i is the column number to be changed to letter
         */
        private Char ToLetter(int i)
        {
            switch (i)
            {
                case 1:
                    return 'A';
                case 2:
                    return 'B';
                case 3:
                    return 'C';
                case 4:
                    return 'D';
                case 5:
                    return 'E';
                case 6:
                    return 'F';
                case 7:
                    return 'G';
                case 8:
                    return 'H';
                default:
                    Console.WriteLine("Error 4");
                    return '?';
            }
        }


        // ---- INotifyPropertyChanged ---- //

        // This property is the INotifyPropertyChanged interface's requirement.
        public event PropertyChangedEventHandler PropertyChanged;

        /* This is the event that notifies the UI when the @param.prop is changed
         * @param prop is the property that is changed. UI is going to be notified using this method.
         */
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        // ---- INotifyPropertyChanged ---- //

        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}