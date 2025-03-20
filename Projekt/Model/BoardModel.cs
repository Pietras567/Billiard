using System;
using Logic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Model
{
    public class BoardModel : INotifyPropertyChanged
    {
        private Board board;
        private ObservableCollection<BallModel> balls;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<BallModel> Balls { get => balls; set => balls = value; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Height
        {
            get { return board.Height; }
            set { board.Height = value; }
        }

        public int Width
        {
            get { return board.Width; }
            set { board.Width = value; }
        }

        public BoardModel(Board board)
        {
            this.board = board;
            Balls = new ObservableCollection<BallModel>();

            foreach (var ball in board.Balls)
            {
                Balls.Add(new BallModel(ball));
            }
        }
    }
}