using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic
{
    public class Ball : INotifyPropertyChanged
    {
        private int xCordinate;
        private int yCordinate;
        private int r;
        private int horizontalMove;
        private int verticalMove;

        public Ball(int x, int y, int r)
        {
            this.xCordinate = x;
            this.yCordinate = y;
            this.r = r;
            this.SethorizontalMove(0);
            this.SetverticalMove(0);
        }


        public int XCordinate
        {
            get { return xCordinate; }
            set { xCordinate = value; OnPropertyChanged("XC"); }
        }
        public int YCordinate
        {
            get { return yCordinate; }
            set { yCordinate = value; OnPropertyChanged("YC"); }
        }
        public int R
        {
            get { return r; }
            set { r = value; OnPropertyChanged("Rad"); }
        }

        public int GethorizontalMove()
        {
            return horizontalMove;
        }

        public void SethorizontalMove(int value)
        {
            horizontalMove = value;
        }

        public int GetverticalMove()
        {
            return verticalMove;
        }

        public void SetverticalMove(int value)
        {
            verticalMove = value;
        }

        public void MakeMove(int maxV, int maxH)
        {
            // Sprawdź czy kula uderzyła w krawędź planszy, a jeśli tak - zmień jej kierunek
            if (XCordinate + GethorizontalMove() >= maxH)
            {
                int afterHit = GethorizontalMove() - (maxH - XCordinate);
                XCordinate = maxH;
                SethorizontalMove(GethorizontalMove() * -1);
                XCordinate += afterHit;
            }
            else if(XCordinate + GethorizontalMove() <= 0)
            {
                int afterHit = -1 * (GethorizontalMove() - (XCordinate));
                XCordinate = 0;
                SethorizontalMove(GethorizontalMove() * -1);
                XCordinate += afterHit;
            }
            else
            {
                XCordinate += GethorizontalMove();
            }
            if (YCordinate + GetverticalMove() >= maxV)
            {
                int afterHit = GetverticalMove() - (maxV - YCordinate);
                YCordinate = maxV;
                SetverticalMove(GetverticalMove() * -1);
                YCordinate += afterHit;
            }
            else if (YCordinate + GetverticalMove() <= 0)
            {
                int afterHit = -1 * (GetverticalMove() + (YCordinate));
                YCordinate = 0;
                SetverticalMove(GetverticalMove() * -1);
                YCordinate += afterHit;
            }
            else
            {
                YCordinate += GetverticalMove();
            }          
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}


