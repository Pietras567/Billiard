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
            
            foreach (var ball in board.Balls)
            {
                // Deklaracja makeMoveAction
                Action<int, int, List<Ball>, object> makeMoveAction = ball.MakeMove;

                // Deklaracja checkCollisionsFunc
                Func<Ball, List<Ball>> checkCollisionsFunc = checkCollisions;

                DataApi.newGame(makeMoveAction, checkCollisionsFunc, ball, board.Height - ball.R, board.Width - ball.R, DataApi.getCommonLock(), cancellationTokenSource.Token);
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
    }


}
