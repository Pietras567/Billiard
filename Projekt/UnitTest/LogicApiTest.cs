using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Data;
using NuGet.Frameworks;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class LogicApiTest
    {
        [TestMethod]
        public void constructorTest()
        {
            // No dataApi injection
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi();
            Assert.IsNotNull(gameAbstractApi);
        }

        [TestMethod]
        public void setGetApiTest()
        {
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi();
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            Assert.IsNotNull(gameAbstractApi);
            Assert.IsNotNull(dataAbstractApi);
            gameAbstractApi.DataApi = dataAbstractApi;
            Assert.IsNotNull(gameAbstractApi.DataApi);
            Assert.AreSame(dataAbstractApi, gameAbstractApi.DataApi);
        }

        [TestMethod]
        public void InjectionTest()
        {
            // Injecting DataAbstractApi
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi(dataAbstractApi); // Injecting dataAbstractApi
            Assert.IsNotNull(gameAbstractApi);
            Assert.IsNotNull(dataAbstractApi);
            Assert.IsNotNull(gameAbstractApi.DataApi);
            Assert.AreSame(dataAbstractApi, gameAbstractApi.DataApi);
        }

        [TestMethod]
        public void createBoardTest()
        {
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi();
            Board board = gameAbstractApi.createBoard(600, 800);
            Assert.IsNotNull(board);
            Assert.AreEqual(board.Height, 600);
            Assert.AreEqual(board.Width, 800);
        }

        [TestMethod]
        public void createBallTest()
        {
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi();
            gameAbstractApi.createBoard(600, 800);

            // R!= 0, xCord! = -5, yCord! = -5
            Ball ball = gameAbstractApi.createBall(10, -5, -5);
            Assert.IsNotNull(ball);
            //Assert.AreEqual(ball.R, 10);
            Assert.IsTrue(ball.XCordinate >= 0 && ball.XCordinate <= 800 - ball.R);
            Assert.IsTrue(ball.YCordinate >= 0 && ball.YCordinate <= 600 - ball.R);

            // R != 0, coords positive
            ball = gameAbstractApi.createBall(10, 11, 12);
            Assert.IsNotNull(ball);
            Assert.AreEqual(ball.R, 10);
            Assert.AreEqual(ball.XCordinate, 11);
            Assert.AreEqual(ball.YCordinate, 12);

            // R == 0, xCord == -5, yCord == -5
            ball = gameAbstractApi.createBall();
            Assert.IsNotNull(ball);
            Assert.IsTrue(ball.R >= 10 && ball.R <= 40);
            Assert.IsTrue(ball.XCordinate >= 0 && ball.XCordinate <= 800 - ball.R);
            Assert.IsTrue(ball.YCordinate >= 0 && ball.YCordinate <= 600 - ball.R);

            // R == 0, coords positive
            ball = gameAbstractApi.createBall(0, 11, 12);
            Assert.IsNotNull(ball);
            Assert.IsTrue(ball.R >= 10 && ball.R <= 40);
            Assert.AreEqual(ball.XCordinate, 11);
            Assert.AreEqual(ball.YCordinate, 12);
        }

        [TestMethod]
        public void createGetBallsListTest()
        {
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi();
            gameAbstractApi.createBoard(600, 800);
            gameAbstractApi.CreateBallsList(5);
            Assert.IsNotNull(gameAbstractApi.GetBallsList());
            Assert.AreEqual(expected: 5, actual: gameAbstractApi.GetBallsList().Count);
            foreach (var ball in gameAbstractApi.GetBallsList())
            {
                Assert.IsNotNull(ball);
            }
        }

        [TestMethod]
        public void newGameTest()
        {
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi();
            gameAbstractApi.createBoard(600, 800);
            gameAbstractApi.CreateBallsList(5);
            gameAbstractApi.newGame();
            // Success if no exceptions are thrown
        }

        [TestMethod]
        public void lockObjectTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi(dataAbstractApi);
            Assert.IsNotNull(dataAbstractApi.getCommonLock());

            Monitor.Enter(dataAbstractApi.getCommonLock());
            // Asercja ze jest zablokowany, innym watkiem
            Task.Run(() =>
            {
                bool isLocked = Monitor.TryEnter(dataAbstractApi.getCommonLock());
                Assert.IsFalse(isLocked);
            });
        }

        [TestMethod]
        public void checkCollisionsTest()
        {
            DataAbstractApi dataAbstractApi = DataAbstractApi.CreateApi();
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi(dataAbstractApi);
            gameAbstractApi.createBoard(600, 800);
            gameAbstractApi.CreateBallsList(2);

            // zmieniamy parametry kul tak zeby sie zderzyly
            gameAbstractApi.GetBallsList()[0].XCordinate = 0;
            gameAbstractApi.GetBallsList()[0].YCordinate = 0;
            gameAbstractApi.GetBallsList()[1].XCordinate = 0;
            gameAbstractApi.GetBallsList()[1].YCordinate = 0;

            List <Ball> collisions = gameAbstractApi.checkCollisions(gameAbstractApi.GetBallsList()[0]);

            // 1 kolizja
            Assert.AreEqual(expected: 1, actual: collisions.Count);
        }
    }
}