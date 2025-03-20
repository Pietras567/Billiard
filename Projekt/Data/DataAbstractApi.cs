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
        public abstract ballData addBall(int R = 0, int xCord = -5, int yCord = -5, int mass = -5);
        public abstract boardData createBoard(int height, int width);
        public abstract void newGame<T>(Action<int, int, object, T> makeMoveAction, T ball, int height, int width, object lockObject, CancellationToken token);

        public abstract void CreateBallsList(int balls);

        public abstract ballData createBall(int R = 0, int xCord = -5, int yCord = -5, int mass = -5);

        public abstract boardData getBoard();
        public abstract void LoadGame(string saveName);
        public abstract void SaveGame(string saveName);

        public abstract void SaveBallData(ballData data, string fileName, string saveName);
        public abstract ballData LoadBallData(string fileName, string saveName);
        public abstract void DodajDaneKolizjeKul(string daneTekstowe);

        public abstract void DodajDaneKolizjeZeScianami(string daneTekstowe);

        public abstract void resetBufor();

        public abstract void zapisz_bufor();


    }
}
