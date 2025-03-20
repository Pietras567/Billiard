using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Data;
using NuGet.Frameworks;
using Moq;
using System.Threading;

namespace UnitTest
{
    [TestClass]
    public class DataApiTest
    {
        [TestMethod]
        public void constructorTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            Assert.IsNotNull(dataAbstractApi);
        }

        [TestMethod]
        public void createBoardTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            boardData boardData = dataAbstractApi.createBoard(600, 800);
            Assert.IsNotNull(boardData);
            Assert.AreEqual(boardData.Height, 600);
            Assert.AreEqual(boardData.Width, 800);
        }

        [TestMethod]
        public void createBallTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            boardData boardData = dataAbstractApi.createBoard(600, 800);
            ballData ballData = dataAbstractApi.createBall(1, 2, 3, 4);
            Assert.IsNotNull(ballData);
            Assert.AreEqual(ballData.XCordinate, 2);
            Assert.AreEqual(ballData.YCordinate, 3);
            Assert.AreEqual(ballData.R, 1);
            Assert.AreEqual(ballData.Mass, 4);
        }

        [TestMethod]
        public void createBallsListTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            dataAbstractApi.createBoard(600, 800);
            dataAbstractApi.CreateBallsList(5);
            Assert.AreEqual(dataAbstractApi.getBoard().Balls.Count, 5);
        }

        [TestMethod]
        public void getCommonLockTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            Assert.IsNotNull(dataAbstractApi.getCommonLock());
        }

        [TestMethod]
        public void newGameCallsCheckCollisionsAndMakeMoveTest()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            cancellationTokenSource = new CancellationTokenSource();

            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();   
            dataAbstractApi.createBoard(600, 800);
            dataAbstractApi.CreateBallsList(1);
            Ball ball = new Ball(dataAbstractApi.createBall(1, 2, 3, 4));

            // Tworzymy MOCK dla makeMoveAction i checkCollisionsFunc
            var mockMakeMoveAction = new Mock<Action<int, int, object, object>>();
            var mockCheckCollisionsFunc = new Mock<Func<Ball, List<Ball>>>();

            dataAbstractApi.newGame(mockMakeMoveAction.Object, ball, 600, 800, new object(), cancellationTokenSource.Token);

            cancellationTokenSource.CancelAfter(100); // Stop po 100ms

            //// Asercje
            //mockCheckCollisionsFunc.Verify(mock => mock(It.IsAny<Ball>()), Times.AtLeastOnce());
            //mockMakeMoveAction.Verify(mock => mock(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<Ball>>(), It.IsAny<object>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void saveGameTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            dataAbstractApi.createBoard(600, 800);
            dataAbstractApi.CreateBallsList(5);
            List<ballData> balls = dataAbstractApi.getBoard().Balls;
            dataAbstractApi.SaveGame("test");
            dataAbstractApi.CreateBallsList(5);

            dataAbstractApi.LoadGame("test");
            Assert.AreEqual(dataAbstractApi.getBoard().Balls.Count, 5);
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(dataAbstractApi.getBoard().Balls[i].XCordinate, balls[i].XCordinate);
                Assert.AreEqual(dataAbstractApi.getBoard().Balls[i].YCordinate, balls[i].YCordinate);
                Assert.AreEqual(dataAbstractApi.getBoard().Balls[i].R, balls[i].R);
                Assert.AreEqual(dataAbstractApi.getBoard().Balls[i].Mass, balls[i].Mass);
            }

            dataAbstractApi.CreateBallsList(3);
            dataAbstractApi.SaveGame("test2");
            dataAbstractApi.CreateBallsList(5);

            dataAbstractApi.LoadGame("test2");
            Assert.AreEqual(dataAbstractApi.getBoard().Balls.Count, 3);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(dataAbstractApi.getBoard().Balls[i].XCordinate, balls[i].XCordinate);
                Assert.AreEqual(dataAbstractApi.getBoard().Balls[i].YCordinate, balls[i].YCordinate);
                Assert.AreEqual(dataAbstractApi.getBoard().Balls[i].R, balls[i].R);
                Assert.AreEqual(dataAbstractApi.getBoard().Balls[i].Mass, balls[i].Mass);
            }
        }

        [TestMethod]
        public void saveBallDataTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            dataAbstractApi.createBoard(600, 800);
            ballData testBall= dataAbstractApi.createBall(1, 2, 3, 4);
            dataAbstractApi.SaveBallData(testBall, "testBall", "testBall");

            ballData ballData = dataAbstractApi.LoadBallData("testBall", "saves/testBall");
            Assert.AreEqual(ballData.XCordinate, 2);
            Assert.AreEqual(ballData.YCordinate, 3);
            Assert.AreEqual(ballData.R, 1);
            Assert.AreEqual(ballData.Mass, 4);
        }

        [TestMethod]        
        public void dodajDaneKolizjeKulTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            dataAbstractApi.resetBufor();
            string dane = "1 2 3 4 5 6 7 8 9 10";
            // assert no exception is thrown
            dataAbstractApi.DodajDaneKolizjeKul(dane);
        }

        [TestMethod]
        public void dodajDaneKolizjeZeScianamiTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            dataAbstractApi.resetBufor();
            string dane = "1 2 3 4 5 6 7 8 9";
            // assert no exception is thrown
            dataAbstractApi.DodajDaneKolizjeZeScianami(dane);
        }

        [TestMethod]
        public void zapiszDaneDoPlikuTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            dataAbstractApi.resetBufor();
            dataAbstractApi.DodajDaneKolizjeKul("1 2 3 4 5 6 7 8 9 10");
            dataAbstractApi.DodajDaneKolizjeZeScianami("1 2 3 4 5 6 7 8 9");
        }
    }
}