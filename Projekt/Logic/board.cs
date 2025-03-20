using System;
using System.Collections.Generic;
using System.ComponentModel;
using Data;
namespace Logic {
    public class Board : INotifyPropertyChanged
    {
        private boardData board;
        private List<Ball> balls = new List<Ball>();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public global::System.Int32 Height { get => board.Height; set => board.Height = value; }
        public global::System.Int32 Width { get => board.Width; set => board.Width = value; }
        public List<Ball> Balls { get => balls; set => balls = value; }
        public boardData BoardD { get => board; set => board = value; }

        public Board(boardData board) {
            this.board = board;
        }
    }
}


