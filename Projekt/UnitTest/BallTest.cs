using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Data;
using NuGet.Frameworks;

namespace UnitTest
{
    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void constructorTest()
        {
            ballData ballData = new ballData(1, 2, 3, 4);
            Ball ball = new Ball(ballData);
            Assert.IsNotNull(ball);
            Assert.AreEqual(ball.XCordinate, 1);
            Assert.AreEqual(ball.YCordinate, 2);
            Assert.AreEqual(ball.R, 3);
            Assert.AreEqual(ball.GethorizontalMove(), 0);
            Assert.AreEqual(ball.GetverticalMove(), 0);
        }

        [TestMethod]
        public void setXCordinateTest()
        {
            ballData ballData = new ballData(1, 2, 3, 4);
            Ball ball = new Ball(ballData);
            ball.XCordinate = 4;
            Assert.AreEqual(ball.XCordinate, 4);
        }

        [TestMethod]
        public void setYCordinateTest()
        {
            ballData ballData = new ballData(1, 2, 3, 4);
            Ball ball = new Ball(ballData);
            ball.YCordinate = 4;
            Assert.AreEqual(ball.YCordinate, 4);
        }

        [TestMethod]
        public void setRTest()
        {
            ballData ballData = new ballData(1, 2, 3, 4);
            Ball ball = new Ball(ballData);
            ball.R = 4;
            Assert.AreEqual(ball.R, 4);
        }

        [TestMethod]
        public void sethorizontalMoveTest()
        {
            ballData ballData = new ballData(1, 2, 3, 4);
            Ball ball = new Ball(ballData);
            ball.SethorizontalMove(4);
            Assert.AreEqual(ball.GethorizontalMove(), 4);
        }

        [TestMethod]
        public void setverticalMoveTest()
        {
            ballData ballData = new ballData(1, 2, 3, 4);
            Ball ball = new Ball(ballData);
            ball.SetverticalMove(4);
            Assert.AreEqual(ball.GetverticalMove(), 4);
        }

        [TestMethod]
        public void setXCordinateTestWithEvent()
        {
            ballData ballData = new ballData(1, 2, 3, 4);
            Ball ball = new Ball(ballData);
            bool eventRaised = false;
            ball.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "XC")
                {
                    eventRaised = true;
                }
            };
            ball.XCordinate = 4;
            Assert.IsTrue(eventRaised);
        }

        //[TestMethod]
        //public void MakeMoveTest()
        //{
        //    // empty list of collisions
        //    List<Ball> collisions = new List<Ball>();
        //    // if  (XCordinate + GethorizontalMove() >= maxH)
        //    ballData ballData = new ballData(1, 1, 3, 4);
        //    Ball ball = new Ball(ballData);
        //    ball.SethorizontalMove(11);
        //    ball.SetverticalMove(11);
        //    //ball.MakeMove(10, 10, collisions, new object());

        //    Assert.AreEqual(12, ball.XCordinate);
        //    Assert.AreEqual(12, ball.YCordinate);
        //    Assert.AreEqual(-11, ball.GethorizontalMove());
        //    Assert.AreEqual(-11, ball.GetverticalMove());

        //    // if  (XCordinate + GethorizontalMove() <= 0)
        //    ballData = new ballData(1, 1, 3, 4);
        //    ball = new Ball(ballData);
        //    ball.SethorizontalMove(-11);
        //    ball.SetverticalMove(-11);
        //    ball.MakeMove(5, 5, collisions, new object());
        //    Assert.AreEqual(12, ball.XCordinate);
        //    Assert.AreEqual(10, ball.YCordinate);
        //    Assert.AreEqual(11, ball.GethorizontalMove());
        //    Assert.AreEqual(11, ball.GetverticalMove());

        //    // Coordinate + moove > 0 but < max
        //    ballData = new ballData(1, 1, 3, 4);
        //    ball = new Ball(ballData);
        //    ball.SethorizontalMove(1);
        //    ball.SetverticalMove(1);
        //    ball.MakeMove(5, 5, collisions, new object());
        //    Assert.AreEqual(2, ball.XCordinate);
        //    Assert.AreEqual(2, ball.YCordinate);
        //    Assert.AreEqual(1, ball.GethorizontalMove());
        //    Assert.AreEqual(1, ball.GetverticalMove());
        //}
    }
}
