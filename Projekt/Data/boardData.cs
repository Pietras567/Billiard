using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class boardData
    {
        private int height;
        private int width;
        private List<ballData> balls = new List<ballData>();

        public boardData(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        public global::System.Int32 Height { get => height; set => height = value; }
        public global::System.Int32 Width { get => width; set => width = value; }
        public List<ballData> Balls { get => balls; set => balls = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
