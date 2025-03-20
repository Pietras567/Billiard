using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Model
{
    internal class ModelApi : ModelAbstractApi
    {

        public override GameAbstractApi GameAPI { get; set; }
        private BoardModel boardModel;

        public ModelApi(GameAbstractApi? API = null)
        {
            if (API == null)
            {
                this.GameAPI = GameAbstractApi.CreateApi();
            }
            else
            {
                this.GameAPI = API;
            }
            this.boardModel = new BoardModel(GameAPI.createBoard(600, 800));
        }

        public override BallModel createBall(int R = 0, int xCord = -5, int yCord = -5)
        {
            BallModel tempBall = new BallModel(GameAPI.createBall(R, xCord, yCord));
            boardModel.Balls.Add(tempBall);
            return tempBall;
        }

        public override BoardModel createBoard(int height = 600, int width = 800)
        {
            BoardModel tempBoard = new BoardModel(GameAPI.createBoard(height, width));
            boardModel = tempBoard;
            return boardModel;
        }

        public override ObservableCollection<BallModel> GetBallsList()
        {

            return boardModel.Balls;
        }

        public override void newGame()
        {
            GameAPI.newGame();
            //System.Timers.Timer timer = new System.Timers.Timer(30);
            //timer.Elapsed += TimerCallback;
            //timer.AutoReset = true;
            //timer.Start();
        }

        //private void TimerCallback(object? sender, ElapsedEventArgs e)
        //{
        //    Console.WriteLine("XC M: " + boardModel.Balls[0].XCordinate);
        //    Console.WriteLine("YC M: " + boardModel.Balls[0].YCordinate);
        //}

        public override void CreateBallsList(int balls)
        {
            boardModel.Balls.Clear();
            GameAPI.CreateBallsList(balls);
            foreach (var ball in GameAPI.GetBallsList())
            {
                boardModel.Balls.Add(new BallModel(ball));
            }
        }

        public override void LoadGame(string saveName)
        {
            GameAPI.LoadGame(saveName);

            boardModel.Balls.Clear();
            
            foreach (var ball in GameAPI.GetBallsList())
            {
                boardModel.Balls.Add(new BallModel(ball));
            }
        }

        public override void SaveGame(string saveName)
        {
            GameAPI.SaveGame(saveName);
        }
    }
}
