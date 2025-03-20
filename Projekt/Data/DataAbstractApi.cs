using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class DataAbstractApi
    {
        private boardData board;
        public static DataAbstractApi CreateApi()
        {
            return new DataApi();
        }

        public abstract object getCommonLock();
        public abstract ballData createBall(int xCord, int yCord, int R, int mass);
        public abstract boardData createBoard(int height, int width);
        public abstract void newGame<T>(Action<int, int, List<T>, object> makeMoveAction, Func<T, List<T>> checkCollisionsFunc, T ball, int height, int width, object lockObject, CancellationToken token);

        public abstract void CreateBallsList(int balls);

        public abstract ballData createBall(int R = 0, int xCord = -5, int yCord = -5);

        public abstract boardData getBoard();

    }
}
