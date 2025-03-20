using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        private int bilaNo= 0;

        public ballData(int x, int y, int r, int mass)
        {
            this.xCordinate = x;
            this.yCordinate = y;
            this.r = r;
            this.mass = mass;
        }

        [JsonConstructor]
        public ballData(int XCordinate, int YCordinate, int R, int Mass, int HorizontalMove, int VerticalMove)
        {
            this.xCordinate = XCordinate;
            this.yCordinate = YCordinate;
            this.r = R;
            this.mass = Mass;
            this.HorizontalMove = HorizontalMove;
            this.VerticalMove = VerticalMove;
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
        public int Mass
        {
            get { return mass; }
            set { mass = value; OnPropertyChanged("Mass"); }
        }
        public int HorizontalMove
        {
            get { return horizontalMove; }
            set { horizontalMove = value; OnPropertyChanged("horizontalMove"); }
        }
        public int VerticalMove
        {
            get { return verticalMove; }
            set { verticalMove = value; OnPropertyChanged("verticalMove"); }
        }

        public int BilaNo { get => bilaNo; set => bilaNo = value; }

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
