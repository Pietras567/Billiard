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
            boardData boardData = new boardData(600, 800);
            Board board = new Board(boardData);
            Assert.IsNotNull(board);
            Assert.AreEqual(board.Height, 600);
            Assert.AreEqual(board.Width, 800);
        }

        [TestMethod]
        public void setGetHeightTest()
        {
            boardData boardData = new boardData(600, 800);
            Board board = new Board(boardData);
            board.Height = 700;
            Assert.AreEqual(board.Height, 700);
        }

        [TestMethod]
        public void setGetWidthTest()
        {
            boardData boardData = new boardData(600, 800);
            Board board = new Board(boardData);
            board.Width = 700;
            Assert.AreEqual(board.Width, 700);
        }

        [TestMethod]
        public void setGetBallsTest()
        {
            boardData boardData = new boardData(600, 800);
            Board board = new Board(boardData);
            ballData ballData1 = new ballData(1, 2, 3, 4);
            ballData ballData2 = new ballData(4, 5, 6, 7);
            Ball ball1 = new Ball(ballData1);
            Ball ball2 = new Ball(ballData2);
            board.Balls.Add(ball1);
            board.Balls.Add(ball2);
            Assert.AreEqual(board.Balls[0], ball1);
            Assert.AreEqual(board.Balls[1], ball2);
        }
    }
}