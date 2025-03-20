using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public abstract class GameAbstractApi
    {
        public abstract DataAbstractApi DataApi { get; set; }
        public static GameAbstractApi CreateApi(DataAbstractApi? API = null)
        {
            return new GameApi(API);
        }

        public abstract void newGame();
        public abstract Board createBoard(int height, int width);
        public abstract Ball createBall(int R = 0, int xCord = -5, int yCord = -5);
        public abstract List<Ball> GetBallsList();
        public abstract void CreateBallsList(int balls);
        public abstract List<Ball> checkCollisions(Ball kulka);
        public abstract void LoadGame(string saveName);
        public abstract void SaveGame(string saveName);

    }
}
