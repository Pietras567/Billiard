using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class GameApi : GameAbstractApi
    {
        private Board board;
        private CancellationTokenSource cancellationTokenSource; // Dodajemy pole przechowujące referencję do tokenu anulowania

        public delegate void MakeMoveDelegate(int height, int width, List<Ball> ballsCollisions, object lockObject, Ball bila);

        public MakeMoveDelegate MakeMoveMethod { get; set; }

        public void SetMakeMoveMethod(MakeMoveDelegate method)
        {
            MakeMoveMethod = method;
        }

        public delegate void CheckCollisionsDelegate(Ball kulka);

        public CheckCollisionsDelegate CheckCollisionsMethod { get; set; }

        public void SetCheckCollisionsMethod(CheckCollisionsDelegate method)
        {
            CheckCollisionsMethod = method;
        }


        public override DataAbstractApi DataApi
        {
            get; set;
        }
        public GameApi(DataAbstractApi? API = null)
        {
            if (API == null)
            {
                this.DataApi = DataAbstractApi.CreateApi();
            }
            else
            {
                this.DataApi = API;
            }
        }
        /*
        public override Ball createBall(int R = 0, int xCord = -5, int yCord = -5)
        {
            if(R!=0 && R>0)
            {
                if(xCord<0 || yCord<0)
                {
                    Random random = new Random();
                  //int mass = random.Next(10, 30); // Przykładowy promień
                    int radius = R; // Przykładowy promień
                    int x = random.Next(0, board.Width - radius); // Losowa współrzędna X
                    int y = random.Next(0, board.Height - radius); // Losowa współrzędna Y
                    return new Ball(DataApi.createBall(x, y, radius, radius));
                }
                else
                {
                    int radius = R; // Przykładowy promień
                    Random random = new Random();
                  //int mass = random.Next(10, 30); // Przykładowy promień
                    int x = xCord;
                    int y = yCord;
                    return new Ball(DataApi.createBall(x, y, radius, radius));
                }
            }
            else
            {
                Random random = new Random();
                if (xCord < 0 || yCord < 0)
                {
                    int radius = random.Next(20, 20); // Przykładowy promień
                  //int mass = random.Next(10, 30); // Przykładowy promień
                    int x = random.Next(0, board.Width - radius); // Losowa współrzędna X
                    int y = random.Next(0, board.Height - radius); // Losowa współrzędna Y
                    return new Ball(DataApi.createBall(x, y, radius, radius));
                }
                else
                {
                    int radius = random.Next(20, 20); // Przykładowy promień
                  //int mass = random.Next(10, 30); // Przykładowy promień
                    int x = xCord;
                    int y = yCord;
                    return new Ball(DataApi.createBall(x, y, radius, radius));
                }
            }
        }*/

        public override Ball createBall(int R = 0, int xCord = -5, int yCord = -5)
        {
            return new Ball(DataApi.createBall(R, xCord, yCord));
        }


        public override Board createBoard(int height, int width)
        {
            Board myBoard = new Board(DataApi.createBoard(height, width));
            board = myBoard;
            return myBoard;
        }

        public override List<Ball> GetBallsList()
        {
            return board.Balls;
        }

        public override void newGame()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            cancellationTokenSource = new CancellationTokenSource(); // Inicjujemy CancellationTokenSource
            DataApi.resetBufor();
            foreach (var ball in board.Balls)
            {
                // Deklaracja makeMoveAction
                Action<int, int, object, Ball> makeMoveAction = this.MakeMove;

                // Deklaracja checkCollisionsFunc
                //Func<Ball, List<Ball>> checkCollisionsFunc = checkCollisions;

                DataApi.newGame(makeMoveAction, ball, board.Height - ball.R, board.Width - ball.R, DataApi.getCommonLock(), cancellationTokenSource.Token);
            }
        }


        public override List<Ball> checkCollisions(Ball kulka)
        {
            List<Ball> ballsCollisions = new List<Ball>();

            // Utwórz obiekt blokady
            object lockObject = DataApi.getCommonLock();

            // Wejdź do sekcji krytycznej
            Monitor.Enter(lockObject);

            try
            {
                double XC = kulka.XCordinate + kulka.GethorizontalMove();
                double YC = kulka.YCordinate + kulka.GetverticalMove();
                foreach (var ball in board.Balls)
                {

                    if (ball != kulka)
                    {

                        double dist = Math.Sqrt((ball.XCordinate - XC) * (ball.XCordinate - XC) + (ball.YCordinate - YC) * (ball.YCordinate - YC));
                        double radiuses = (ball.R + kulka.R) / 2;
                        if(dist <= radiuses)
                        {
                            ballsCollisions.Add(ball);
                        }
                    }
                }
                //Thread.Sleep(100);

                //Task delayTask = Task.Delay(1000);
                //delayTask.Wait(); // Oczekuj na zakończenie opóźnienia

            }
            finally
            {
                // Wyjdź z sekcji krytycznej
                Monitor.Exit(lockObject);
            }

            return ballsCollisions;
        }

        /* public override void CreateBallsList(int balls)
         {
             board.Balls.Clear(); // Wyczyść obecne kule na planszy
             // Dodaj daną ilość nowych kulek o losowych współrzędnych
             for (int i = 0; i < balls; i++)
             {
                 Ball kulka = createBall();
                 board.Balls.Add(kulka);
                // board.BoardD.Balls.Add(kulka.BallD);
                 Random random = new Random();
                 int velocity = random.Next(-10, 10);
                 board.Balls.ElementAt(i).SetverticalMove(velocity);
                 board.Balls.ElementAt(i).SethorizontalMove(velocity);
             }
         }*/

        public override void CreateBallsList(int balls)
        {
            board.Balls.Clear(); // Wyczyść obecne kule na planszy
            DataApi.CreateBallsList(balls);
            
            for (int i = 0; i < balls; i++)
            {
                //Ball kulka = createBall();
                Ball kulka = new Ball(DataApi.getBoard().Balls.ElementAt(i));
                board.Balls.Add(kulka);
            }
        }

        public override void LoadGame(string saveName)
        {
            DataApi.LoadGame(saveName);
            board.Balls.Clear();

            for (int i = 0; i < DataApi.getBoard().Balls.Count(); i++)
            {
                //Ball kulka = createBall();
                Ball kulka = new Ball(DataApi.getBoard().Balls.ElementAt(i));
                board.Balls.Add(kulka);
            }

            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            cancellationTokenSource = new CancellationTokenSource(); // Inicjujemy CancellationTokenSource

            foreach (var ball in board.Balls)
            {
                // Deklaracja makeMoveAction
                Action<int, int, object, Ball> makeMoveAction = this.MakeMove;

                // Deklaracja checkCollisionsFunc
                //Func<Ball, List<Ball>> checkCollisionsFunc = checkCollisions;

                DataApi.newGame(makeMoveAction, ball, board.Height - ball.R, board.Width - ball.R, DataApi.getCommonLock(), cancellationTokenSource.Token);
            }
        }

        public override void SaveGame(string saveName)
        {
            DataApi.SaveGame(saveName);
        }



        public void MakeMove(int maxV, int maxH, object lockObject, Ball bila)
        {
            // Wejdź do sekcji krytycznej
            Monitor.Enter(lockObject);

            // Utwórz słownik do przechowywania nowych wektorów prędkości
            //Dictionary<Ball, (int, int)> newMoves = new Dictionary<Ball, (int, int)>();

            try
            {
                List<Ball> collisions = checkCollisions(bila);
                if (collisions.Count() > 0)
                {
                    bila.XCordinate += bila.GethorizontalMove();
                    bila.YCordinate += bila.GetverticalMove();

                    foreach (Ball b in collisions)
                    {
                        // Oblicz nowe wektory prędkości dla obu kulek po kolizji
                        double newHorizontalMove1 = (bila.GethorizontalMove() * (b.Mass - bila.Mass) + (2 * b.Mass * b.GethorizontalMove())) / (bila.Mass + b.Mass);
                        double newVerticalMove1 = (bila.GetverticalMove() * (b.Mass - bila.Mass) + (2 * b.Mass * b.GetverticalMove())) / (bila.Mass + b.Mass);

                        double newHorizontalMove2 = (b.GethorizontalMove() * (bila.Mass - b.Mass) + (2 * bila.Mass * bila.GethorizontalMove())) / (bila.Mass + b.Mass);
                        double newVerticalMove2 = (b.GetverticalMove() * (bila.Mass - b.Mass) + (2 * bila.Mass * bila.GetverticalMove())) / (bila.Mass + b.Mass);

                        string toSave = bila.Mass + " " + b.Mass + " " + bila.GethorizontalMove() + " " + bila.GetverticalMove() + " " + b.GethorizontalMove()
                            + " " + b.GetverticalMove() + " ";

                        Console.WriteLine("Stare XV1: " + bila.GethorizontalMove());
                        Console.WriteLine("Stare YV1: " + bila.GetverticalMove());
                        Console.WriteLine("Stare XV2: " + b.GethorizontalMove());
                        Console.WriteLine("Stare YV2: " + b.GetverticalMove());


                        bila.SethorizontalMove((int)newHorizontalMove1);
                        bila.SetverticalMove((int)newVerticalMove1);
                        b.SethorizontalMove((int)newHorizontalMove2);
                        b.SetverticalMove((int)newVerticalMove2);

                        toSave = toSave + bila.GethorizontalMove() + " " + bila.GetverticalMove() + " " + b.GethorizontalMove()
                            + " " + b.GetverticalMove();

                        Console.WriteLine("Nowe XV1: " + (int)newHorizontalMove1);
                        Console.WriteLine("Nowe YV1: " + (int)newVerticalMove1);
                        Console.WriteLine("Nowe XV2: " + (int)newHorizontalMove2);
                        Console.WriteLine("Nowe YV2: " + (int)newVerticalMove2);

                        DataApi.DodajDaneKolizjeKul(toSave);

                        //int val1 = newMoves[this].Item1;
                        //int val2 = newMoves[this].Item2;


                        // Dodaj nowe wektory prędkości do słownika
                        //newMoves[this] = (newHorizontalMove1, newVerticalMove1);
                        //newMoves[b] = (newHorizontalMove2, newVerticalMove2);

                        // Oblicz wektor od tej kuli do kuli b
                        int dx = b.XCordinate - bila.XCordinate;
                        int dy = b.YCordinate - bila.YCordinate;

                        // Oblicz odległość między kulami
                        double distance = Math.Sqrt(dx * dx + dy * dy);

                        // Oblicz minimalną odległość, jaką kule powinny mieć od siebie (suma ich promieni)
                        int minDistance = ((bila.R + b.R) / 2) + 1;

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
                        if (bila.XCordinate - (int)pushX <= 0)
                        {
                            bila.XCordinate = 0;
                            bila.SethorizontalMove(bila.GethorizontalMove() * -1);
                        }
                        else if (bila.XCordinate - (int)pushX >= maxH)
                        {
                            bila.XCordinate = maxH;
                            bila.SethorizontalMove(bila.GethorizontalMove() * -1);
                        }
                        else
                        {
                            bila.XCordinate -= (int)pushX;
                        }
                        if (bila.YCordinate - (int)pushY <= 0)
                        {
                            bila.YCordinate = 0;
                            bila.SetverticalMove(bila.GetverticalMove() * -1);
                        }
                        else if (bila.YCordinate - (int)pushY >= maxV)
                        {
                            bila.YCordinate = maxV;
                            bila.SetverticalMove(bila.GetverticalMove() * -1);
                        }
                        else
                        {
                            bila.YCordinate -= (int)pushY;
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
                    string toSave = bila.Mass + " " + bila.GethorizontalMove() + " " + bila.GetverticalMove() + " " + bila.XCordinate + " " + bila.YCordinate + " ";

                    if (bila.XCordinate + bila.GethorizontalMove() >= maxH)
                    {
                        int afterHit = bila.GethorizontalMove() - (maxH - bila.XCordinate);
                        bila.XCordinate = maxH;
                        bila.SethorizontalMove(bila.GethorizontalMove() * -1);
                        bila.XCordinate += afterHit;

                        toSave += bila.GethorizontalMove() + " " + bila.GetverticalMove() + " " + bila.XCordinate + " " + bila.YCordinate;
                        DataApi.DodajDaneKolizjeZeScianami(toSave);
                    }
                    else if (bila.XCordinate + bila.GethorizontalMove() <= 0)
                    {
                        int afterHit = -1 * (bila.GethorizontalMove() - (bila.XCordinate));
                        bila.XCordinate = 0;
                        bila.SethorizontalMove(bila.GethorizontalMove() * -1);
                        bila.XCordinate += afterHit;

                        toSave += bila.GethorizontalMove() + " " + bila.GetverticalMove() + " " + bila.XCordinate + " " + bila.YCordinate;
                        DataApi.DodajDaneKolizjeZeScianami(toSave);
                    }
                    else
                    {
                        bila.XCordinate += bila.GethorizontalMove();
                    }

                    string toSave2 = bila.Mass + " " + bila.GethorizontalMove() + " " + bila.GetverticalMove() + " " + bila.XCordinate + " " + bila.YCordinate + " ";
                    if (bila.YCordinate + bila.GetverticalMove() >= maxV)
                    {
                        int afterHit = bila.GetverticalMove() - (maxV - bila.YCordinate);
                        bila.YCordinate = maxV;
                        bila.SetverticalMove(bila.GetverticalMove() * -1);
                        bila.YCordinate += afterHit;

                        toSave2 += bila.GethorizontalMove() + " " + bila.GetverticalMove() + " " + bila.XCordinate + " " + bila.YCordinate;
                        DataApi.DodajDaneKolizjeZeScianami(toSave2);
                    }
                    else if (bila.YCordinate + bila.GetverticalMove() <= 0)
                    {
                        int afterHit = -1 * (bila.GetverticalMove() + (bila.YCordinate));
                        bila.YCordinate = 0;
                        bila.SetverticalMove(bila.GetverticalMove() * -1);
                        bila.YCordinate += afterHit;

                        toSave2 += bila.GethorizontalMove() + " " + bila.GetverticalMove() + " " + bila.XCordinate + " " + bila.YCordinate;
                        DataApi.DodajDaneKolizjeZeScianami(toSave2);
                    }
                    else
                    {
                        bila.YCordinate += bila.GetverticalMove();
                    }
                }
            }
            finally
            {
                // Wyjdź z sekcji krytycznej
                Monitor.Exit(lockObject);
            }
        }

    }
}
