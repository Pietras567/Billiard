using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ballData
    {
        private int xCordinate;
        private int yCordinate;
        private int r;
        private int horizontalMove = 0;
        private int verticalMove = 0;
        private int mass = 20;


        public ballData(int x, int y, int r, int mass)
        {
            this.xCordinate = x;
            this.yCordinate = y;
            this.r = r;
            this.mass = mass;
        }


        public int Mass
        {
            get { return mass; }
            set { mass = value; OnPropertyChanged("Mass"); }
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









        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
