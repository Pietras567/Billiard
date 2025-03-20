using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Data;
using NuGet.Frameworks;

namespace UnitTest
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void constructorTest()
        {
            Board board = new Board(600, 800);
            Assert.IsNotNull(board);
            Assert.AreEqual(board.Height, 600);
            Assert.AreEqual(board.Width, 800);
        }

        [TestMethod]
        public void setGetHeightTest()
        {
            Board board = new Board(600, 800);
            board.Height = 700;
            Assert.AreEqual(board.Height, 700);
        }

        [TestMethod]
        public void setGetWidthTest()
        {
            Board board = new Board(600, 800);
            board.Width = 700;
            Assert.AreEqual(board.Width, 700);
        }

        [TestMethod]
        public void setGetBallsTest()
        {
            Board board = new Board(600, 800);
            Ball ball1 = new Ball(1, 2, 3);
            Ball ball2 = new Ball(4, 5, 6);
            board.Balls.Add(ball1);
            board.Balls.Add(ball2);
            Assert.AreEqual(board.Balls[0], ball1);
            Assert.AreEqual(board.Balls[1], ball2);
        }
    }
}