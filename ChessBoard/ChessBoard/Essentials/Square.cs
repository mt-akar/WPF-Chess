using ChessBoard.Pieces;
using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessBoard.Essentials
{
    public class Square : INotifyPropertyChanged
    {
        int row;
        public int Row { get { return row; } }

        int column;
        public int Column { get { return column; } }

        SolidColorBrush backColor;
        public SolidColorBrush BackColor { get { return backColor; } set { backColor = value; } }

        Piece piece;
        public Piece Piece { get { return piece; } set { piece = value; OnPropertyChanged("Piece"); } }

        ImageSource image;
        public ImageSource Im { get { return image; } set { image = value; OnPropertyChanged("Im"); } }
        
        public Square(int r, int c, Piece p)
        {
            row = r;
            column = c;
            piece = p;
            InitializeIm();
            backColor = new SolidColorBrush();
            backColor.Color = ((row + column) % 2 == 0) ? Color.FromArgb(255, 221, 221, 221) : Color.FromArgb(255, 255, 255, 255);
        }

        public void GoBackToDefaultColor()
        {
            backColor.Color = ((row + column) % 2 == 0) ? Color.FromArgb(255, 221, 221, 221) : Color.FromArgb(255, 255, 255, 255);
        }

        private void InitializeIm() // Initialize image
        {
            // rl: light rook, bd: dark bishop, gl: light king etc...
            if (piece == null)
            {
                image = new BitmapImage(new Uri(@"PP/ee.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.Type == "Rook")
            {
                image = (piece.Player == Player.White) ? new BitmapImage(new Uri(@"PP/rl.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri(@"PP/rd.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.Type == "Knight")
            { 
                image = (piece.Player == Player.White) ? new BitmapImage(new Uri(@"PP/kl.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri(@"PP/kd.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.Type == "Bishop")
            {
                image = (piece.Player == Player.White) ? new BitmapImage(new Uri(@"PP/bl.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri(@"PP/bd.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.Type == "Queen")
            {
                image = (piece.Player == Player.White) ? new BitmapImage(new Uri(@"PP/ql.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri(@"PP/qd.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.Type == "King")
            {
                image = (piece.Player == Player.White) ? new BitmapImage(new Uri(@"PP/gl.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri(@"PP/gd.png", UriKind.RelativeOrAbsolute));
            }
            else if (piece.Type == "Pawn")
            {
                image = (piece.Player == Player.White) ? new BitmapImage(new Uri(@"PP/pl.png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri(@"PP/pd.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                Console.WriteLine("Error 1");
            }
        }

        // ---- INotifyPropertyChanged ----
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        // ---- INotifyPropertyChanged ----
    }
}
