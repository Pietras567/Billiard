using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Logic {
    public class Board : INotifyPropertyChanged
    {
        private int height;
        private int width;
        private List<Ball> balls = new List<Ball>();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public global::System.Int32 Height { get => height; set => height = value; }
        public global::System.Int32 Width { get => width; set => width = value; }
        public List<Ball> Balls { get => balls; set => balls = value; }

        public Board(int height, int width) {
            this.height = height;
            this.width = width;
        }
    }
}


