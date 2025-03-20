using System;
using Logic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Model
{
    public class BallModel : INotifyPropertyChanged
    {
        private Ball ball;

        public BallModel(Ball ball)
        {
            this.ball = ball;
            ball.PropertyChanged += onUpdate;
        }


        private void onUpdate(object _object, PropertyChangedEventArgs args)
        {
            Ball ball = (Ball)_object;
            if (args.PropertyName == "XC")
            {
                // this.x = ball.XCordinate;
                //Console.WriteLine("Zmiana X w model");
                OnPropertyChanged("XCordinate");
            }
            if (args.PropertyName == "YC")
            {
                // this.y = ball.YCordinate;
                //Console.WriteLine("Zmiana Y w model");
                OnPropertyChanged("YCordinate");
            }
            if (args.PropertyName == "Rad")
            {
              //  this.r = ball.R;
                OnPropertyChanged("R");
            }
        }

        public int XCordinate
        {
            get { return ball.XCordinate; }
            set
            {
                if (ball.XCordinate != value)
                {
                    ball.XCordinate = value;
                    OnPropertyChanged("XCordinate");
                }
            }
        }

        public int YCordinate
        {
            get { return ball.YCordinate; }
            set
            {
                if (ball.YCordinate != value)
                {
                    ball.YCordinate = value;
                    OnPropertyChanged("YCordinate");
                }
            }
        }

        public int R
        {
            get { return ball.R; }
            set
            {
                if (ball.R != value)
                {
                    ball.R = value;
                    OnPropertyChanged("R");
                }
            }
        }

        public int HorizontalMove
        {
            get { return ball.GethorizontalMove(); }
            set
            {
                if (ball.GethorizontalMove() != value)
                {
                    ball.SethorizontalMove(value);
                    OnPropertyChanged("HorizontalMove");
                }
            }
        }

        public int VerticalMove
        {
            get { return ball.GetverticalMove(); }
            set
            {
                if (ball.GetverticalMove() != value)
                {
                    ball.SetverticalMove(value);
                    OnPropertyChanged("VerticalMove");
                }
            }
        }

        public void MakeMove(int maxV, int maxH)
        {
            ball.MakeMove(maxV, maxH);
            OnPropertyChanged("XCordinate");
            OnPropertyChanged("YCordinate");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
