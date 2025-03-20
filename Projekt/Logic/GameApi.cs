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

        public override Ball createBall(int R = 0, int xCord = -5, int yCord = -5)
        {
            if(R!=0 && R>0)
            {
                if(xCord<0 || yCord<0)
                {
                    Random random = new Random();
                    int radius = R; // Przykładowy promień
                    int x = random.Next(0, board.Width - radius); // Losowa współrzędna X
                    int y = random.Next(0, board.Height - radius); // Losowa współrzędna Y
                    return new Ball(x, y, radius);
                }
                else
                {
                    int radius = R; // Przykładowy promień
                    int x = xCord;
                    int y = yCord;
                    return new Ball(x, y, radius);
                }
            }
            else
            {
                Random random = new Random();
                if (xCord < 0 || yCord < 0)
                {
                    int radius = random.Next(10, 40); ; // Przykładowy promień
                    int x = random.Next(0, board.Width - radius); // Losowa współrzędna X
                    int y = random.Next(0, board.Height - radius); // Losowa współrzędna Y
                    return new Ball(x, y, radius);
                }
                else
                {
                    int radius = random.Next(10, 40); ; // Przykładowy promień
                    int x = xCord;
                    int y = yCord;
                    return new Ball(x, y, radius);
                }
            }
        }

        public override Board createBoard(int height, int width)
        {
            Board myBoard = new Board(height, width);
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

            Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    foreach (var ball in board.Balls)
                    {
                        ball.MakeMove(board.Height - ball.R, board.Width - ball.R);

                        //if(ball == board.Balls[0])
                        //{
                            //Console.WriteLine("XC: " + board.Balls[0].XCordinate);
                            //Console.WriteLine("YC: " + board.Balls[0].YCordinate);
                        //}

                    }
                    await Task.Delay(30, cancellationTokenSource.Token); // Częstotliwość aktualizacji ruchu (10ms)
                }
            }, cancellationTokenSource.Token);
        }


        public override void CreateBallsList(int balls)
        {
            board.Balls.Clear(); // Wyczyść obecne kule na planszy
            // Dodaj 5 nowych kulek o losowych współrzędnych
            for (int i = 0; i < balls; i++)
            {
                
                board.Balls.Add(createBall());

                board.Balls.ElementAt(i).SetverticalMove(5);
                board.Balls.ElementAt(i).SethorizontalMove(5);
            }
        }
    }


}
