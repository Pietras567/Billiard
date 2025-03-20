using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class ModelAbstractApi
    {
        public abstract GameAbstractApi GameAPI { get; set; }
        public static ModelAbstractApi CreateApi(GameAbstractApi? gameAPI = null)
        {
            return new ModelApi(gameAPI);
        }

        public abstract void newGame();
        public abstract BoardModel createBoard(int height, int width);
        public abstract BallModel createBall(int R = 0, int xCord = -5, int yCord = -5);
        public abstract ObservableCollection<BallModel> GetBallsList();
        public abstract void CreateBallsList(int balls);
        public abstract void LoadGame(string saveName);
        public abstract void SaveGame(string saveName);
    }
}
