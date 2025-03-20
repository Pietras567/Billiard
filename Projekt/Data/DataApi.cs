using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    internal sealed class DataApi : DataAbstractApi
    {
        private static object lockObject = new object();
        private boardData board;
        private CancellationTokenSource cancellationTokenSource; // Dodajemy pole przechowujące referencję do tokenu anulowania

        public override boardData getBoard()
        {
            return board;
        }

        public override object getCommonLock()
        {
            return DataApi.lockObject;
        }

        public override ballData createBall(int xCord, int yCord, int R, int mass)
        {
            ballData kula = new ballData(xCord, yCord, R, mass);
            board.Balls.Add(kula);
            return kula;
        }

        public override boardData createBoard(int height, int width)
        {
            boardData myBoard = new boardData(height, width);
            board = myBoard;
            return myBoard;
        }

        public override void  newGame<T>(Action<int, int, List<T>, object> makeMoveAction, Func<T, List<T>> checkCollisionsFunc, T ball, int height, int width, object lockObject, CancellationToken token)
        {
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    List<T> ballsCollisions = checkCollisionsFunc(ball);
                    makeMoveAction(height, width, ballsCollisions, lockObject);
                    await Task.Delay(30, token); // Częstotliwość aktualizacji ruchu (10ms)
                }
            }, token);

        }


        public override void CreateBallsList(int balls)
        {
            board.Balls.Clear(); // Wyczyść obecne kule na planszy
            // Dodaj daną ilość nowych kulek o losowych współrzędnych
            for (int i = 0; i < balls; i++)
            {
                ballData kulka = createBall();
                board.Balls.Add(kulka);
                
                Random random = new Random();
                int velocityX = random.Next(-10, 10);
                int velocityY = random.Next(-10, 10);

                while (velocityX >= -2 && velocityX <= 2)
                {
                    velocityX = random.Next(-10, 10);
                }

                while (velocityY >= -2 && velocityY <= 2)
                {
                    velocityY = random.Next(-10, 10);
                }
                board.Balls.ElementAt(i).SetverticalMove(velocityY);
                board.Balls.ElementAt(i).SethorizontalMove(velocityX);
            }
        }

        public override ballData createBall(int R = 0, int xCord = -5, int yCord = -5)
        {
            if (R != 0 && R > 0)
            {
                if (xCord < 0 || yCord < 0)
                {
                    Random random = new Random();
                    //int mass = random.Next(10, 30); // Przykładowy promień
                    int radius = R; // Przykładowy promień
                    int x = random.Next(0, board.Width - radius); // Losowa współrzędna X
                    int y = random.Next(0, board.Height - radius); // Losowa współrzędna Y
                    return new ballData(x, y, radius, radius);
                }
                else
                {
                    int radius = R; // Przykładowy promień
                    Random random = new Random();
                    //int mass = random.Next(10, 30); // Przykładowy promień
                    int x = xCord;
                    int y = yCord;
                    return new ballData(x, y, radius, radius);
                }
            }
            else
            {
                Random random = new Random();
                if (xCord < 0 || yCord < 0)
                {
                    int radius = random.Next(30, 30); // Przykładowy promień
                                                      //int mass = random.Next(10, 30); // Przykładowy promień
                    int x = random.Next(0, board.Width - radius); // Losowa współrzędna X
                    int y = random.Next(0, board.Height - radius); // Losowa współrzędna Y
                    return new ballData(x, y, radius, radius);
                }
                else
                {
                    int radius = random.Next(30, 30); // Przykładowy promień
                                                      //int mass = random.Next(10, 30); // Przykładowy promień
                    int x = xCord;
                    int y = yCord;
                    return new ballData(x, y, radius, radius);
                }
            }
        }

    }
}
