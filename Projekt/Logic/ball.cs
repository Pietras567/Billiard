using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data;
namespace Logic
{
    public class Ball : INotifyPropertyChanged
    {
        private ballData ball;

        public delegate void MakeMoveDelegate(int height, int width, List<Ball> ballsCollisions, object lockObject);

        public MakeMoveDelegate MakeMoveMethod { get; set; }

        public void SetMakeMoveMethod(MakeMoveDelegate method)
        {
            MakeMoveMethod = method;
        }

        public Ball(ballData kula)
        {
            this.ball = kula;
            SetMakeMoveMethod(this.MakeMove);
            ball.PropertyChanged += onUpdate;
        }

        private void onUpdate(object _object, PropertyChangedEventArgs args)
        {
            ballData ball = (ballData)_object;
            if (args.PropertyName == "XC")
            {
                OnPropertyChanged("XC");
            }
            if (args.PropertyName == "YC")
            {
                OnPropertyChanged("YC");
            }
            if (args.PropertyName == "Rad")
            {
                OnPropertyChanged("Rad");
            }
        }
        
        public int Mass
        {
            get { return ball.Mass; }
            set { ball.Mass = value; }
        }

        public int XCordinate
        {
            get { return ball.XCordinate; }
            set { ball.XCordinate = value; }
        }
        public int YCordinate
        {
            get { return ball.YCordinate; }
            set { ball.YCordinate = value; }
        }
        public int R
        {
            get { return ball.R; }
            set { ball.R = value; }
        }

        public ballData BallD { get => ball; set => ball = value; }

        public int GethorizontalMove()
        {
            return ball.GethorizontalMove();
        }

        public void SethorizontalMove(int value)
        {
            ball.SethorizontalMove(value);
        }

        public int GetverticalMove()
        {
            return ball.GetverticalMove();
        }

        public void SetverticalMove(int value)
        {
            ball.SetverticalMove(value);
        }

        public void MakeMove(int maxV, int maxH, List<Ball> collisions, object lockObject)
        {
            // Wejdź do sekcji krytycznej
            Monitor.Enter(lockObject);

            // Utwórz słownik do przechowywania nowych wektorów prędkości
            //Dictionary<Ball, (int, int)> newMoves = new Dictionary<Ball, (int, int)>();

            try
            {
                if (collisions.Count() > 0)
                {
                    XCordinate += GethorizontalMove();
                    YCordinate += GetverticalMove();

                    foreach (Ball b in collisions)
                    {
                        // Oblicz nowe wektory prędkości dla obu kulek po kolizji
                        double newHorizontalMove1 = (GethorizontalMove() * (b.Mass - Mass) + (2 * b.Mass * b.GethorizontalMove())) / (Mass + b.Mass);
                        double newVerticalMove1 = (GetverticalMove() * (b.Mass - Mass) + (2 * b.Mass * b.GetverticalMove())) / (Mass + b.Mass);

                        double newHorizontalMove2 = (b.GethorizontalMove() * (Mass - b.Mass) + (2 * Mass * GethorizontalMove())) / (Mass + b.Mass);
                        double newVerticalMove2 = (b.GetverticalMove() * (Mass - b.Mass) + (2 * Mass * GetverticalMove())) / (Mass + b.Mass);


                        Console.WriteLine("Stare XV1: " + newHorizontalMove1);
                        Console.WriteLine("Stare YV1: " + newVerticalMove1);
                        Console.WriteLine("Stare XV2: " + newHorizontalMove2);
                        Console.WriteLine("Stare YV2: " + newVerticalMove2);

                        SethorizontalMove((int)newHorizontalMove1);
                        SetverticalMove((int)newVerticalMove1);
                        b.SethorizontalMove((int)newHorizontalMove2);
                        b.SetverticalMove((int)newVerticalMove2);

                        Console.WriteLine("Nowe XV1: " + (int)newHorizontalMove1);
                        Console.WriteLine("Nowe YV1: " + (int)newVerticalMove1);
                        Console.WriteLine("Nowe XV2: " + (int)newHorizontalMove2);
                        Console.WriteLine("Nowe YV2: " + (int)newVerticalMove2);

                        //int val1 = newMoves[this].Item1;
                        //int val2 = newMoves[this].Item2;


                        // Dodaj nowe wektory prędkości do słownika
                        //newMoves[this] = (newHorizontalMove1, newVerticalMove1);
                        //newMoves[b] = (newHorizontalMove2, newVerticalMove2);

                        // Oblicz wektor od tej kuli do kuli b
                        int dx = b.XCordinate - XCordinate;
                        int dy = b.YCordinate - YCordinate;

                        // Oblicz odległość między kulami
                        double distance = Math.Sqrt(dx * dx + dy * dy);

                        // Oblicz minimalną odległość, jaką kule powinny mieć od siebie (suma ich promieni)
                        int minDistance = ((R + b.R) / 2) + 2;

                        // Oblicz wektor, który przesunie kule z powrotem na zewnątrz siebie
                        double pushX = dx / distance * (minDistance - distance);
                        double pushY = dy / distance * (minDistance - distance);
                        pushX = Math.Ceiling(pushX);
                        pushY = Math.Ceiling(pushY);
                        Console.WriteLine("Push X: " + pushX);
                        Console.WriteLine("Push Y: " + pushY);
                        Console.WriteLine("");
                        // Sprawdź, czy kula nie wyszła poza granice planszy po przesunięciu
                        // Przesuń kule z powrotem na zewnątrz siebie
                        if (XCordinate - (int)pushX <= 0)
                        {
                            XCordinate = 0;
                            SethorizontalMove(GethorizontalMove() * -1);
                        }
                        else if (XCordinate - (int)pushX >= maxH)
                        {
                            XCordinate = maxH;
                            SethorizontalMove(GethorizontalMove() * -1);
                        }
                        else
                        {
                            XCordinate -= (int)pushX;
                        }
                        if (YCordinate - (int)pushY <= 0)
                        {
                            YCordinate = 0;
                            SetverticalMove(GetverticalMove() * -1);
                        }
                        else if (YCordinate - (int)pushY >= maxV)
                        {
                            YCordinate = maxV;
                            SetverticalMove(GetverticalMove() * -1);
                        }
                        else
                        {
                            YCordinate -= (int)pushY;
                        }
                        //////////////
                        if (b.XCordinate + (int)pushX <= 0)
                        {
                            b.XCordinate = 0;
                            b.SethorizontalMove(b.GethorizontalMove() * -1);
                        }
                        else if (b.XCordinate + (int)pushX >= maxH)
                        {
                            b.XCordinate = maxH;
                            b.SethorizontalMove(b.GethorizontalMove() * -1);
                        }
                        else
                        {
                            b.XCordinate += (int)pushX;
                        }
                        if (b.YCordinate + (int)pushY <= 0)
                        {
                            b.YCordinate = 0;
                            b.SetverticalMove(b.GetverticalMove() * -1);
                        }
                        else if (b.YCordinate + (int)pushY >= maxV)
                        {
                            b.YCordinate = maxV;
                            b.SetverticalMove(b.GetverticalMove() * -1);
                        }
                        else
                        {
                            b.YCordinate += (int)pushY;
                        }

                        Console.WriteLine("Kolizja \n");
                    }

                    // Zaktualizuj wektory prędkości wszystkich kulek na raz
                    //foreach (var pair in newMoves)
                    //{
                    //    pair.Key.SethorizontalMove(pair.Value.Item1);
                    //    pair.Key.SetverticalMove(pair.Value.Item2);
                    //}
                }
                // Sprawdź czy kula uderzyła w krawędź planszy, a jeśli tak - zmień jej kierunek
                else
                {


                    if (XCordinate + GethorizontalMove() >= maxH)
                    {
                        int afterHit = GethorizontalMove() - (maxH - XCordinate);
                        XCordinate = maxH;
                        SethorizontalMove(GethorizontalMove() * -1);
                        XCordinate += afterHit;
                    }
                    else if (XCordinate + GethorizontalMove() <= 0)
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
            }
            finally
            {
                // Wyjdź z sekcji krytycznej
                Monitor.Exit(lockObject);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}


