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
            Assert.AreEqual(ballData.XCordinate, 1);
            Assert.AreEqual(ballData.YCordinate, 2);
            Assert.AreEqual(ballData.R, 3);
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
            var mockMakeMoveAction = new Mock<Action<int, int, List<Ball>, object>>();
            var mockCheckCollisionsFunc = new Mock<Func<Ball, List<Ball>>>();

            dataAbstractApi.newGame(mockMakeMoveAction.Object, mockCheckCollisionsFunc.Object, ball, 600, 800, new object(), cancellationTokenSource.Token);

            cancellationTokenSource.CancelAfter(100); // Stop po 100ms

            // Asercje
            mockCheckCollisionsFunc.Verify(mock => mock(It.IsAny<Ball>()), Times.AtLeastOnce());
            mockMakeMoveAction.Verify(mock => mock(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<Ball>>(), It.IsAny<object>()), Times.AtLeastOnce());
        }

    }
}