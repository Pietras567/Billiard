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
            Ball ball = new Ball(1, 2, 3);
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
            Ball ball = new Ball(1, 2, 3);
            ball.XCordinate = 4;
            Assert.AreEqual(ball.XCordinate, 4);
        }

        [TestMethod]
        public void setYCordinateTest()
        {
            Ball ball = new Ball(1, 2, 3);
            ball.YCordinate = 4;
            Assert.AreEqual(ball.YCordinate, 4);
        }

        [TestMethod]
        public void setRTest()
        {
            Ball ball = new Ball(1, 2, 3);
            ball.R = 4;
            Assert.AreEqual(ball.R, 4);
        }

        [TestMethod]
        public void sethorizontalMoveTest()
        {
            Ball ball = new Ball(1, 2, 3);
            ball.SethorizontalMove(4);
            Assert.AreEqual(ball.GethorizontalMove(), 4);
        }

        [TestMethod]
        public void setverticalMoveTest()
        {
            Ball ball = new Ball(1, 2, 3);
            ball.SetverticalMove(4);
            Assert.AreEqual(ball.GetverticalMove(), 4);
        }

        [TestMethod]
        public void setXCordinateTestWithEvent()
        {
            Ball ball = new Ball(1, 2, 3);
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

        [TestMethod]
        public void MakeMoveTest()
        {
            // if  (XCordinate + GethorizontalMove() >= maxH)
            Ball ball = new Ball(x: 1, y: 1, r: 3);
            ball.SethorizontalMove(11);
            ball.SetverticalMove(11);
            ball.MakeMove(10, 10);
            Assert.AreEqual(12, ball.XCordinate);
            Assert.AreEqual(12, ball.YCordinate);
            Assert.AreEqual(-11, ball.GethorizontalMove());
            Assert.AreEqual(-11, ball.GetverticalMove());

            // if  (XCordinate + GethorizontalMove() <= 0)
            ball = new Ball(x: 1, y: 1, r: 3);
            ball.SethorizontalMove(-11);
            ball.SetverticalMove(-11);
            ball.MakeMove(5, 5);
            Assert.AreEqual(12, ball.XCordinate);
            Assert.AreEqual(10, ball.YCordinate);
            Assert.AreEqual(11, ball.GethorizontalMove());
            Assert.AreEqual(11, ball.GetverticalMove());

            // Coordinate + moove > 0 but < max
            ball = new Ball(x: 1, y: 1, r: 3);
            ball.SethorizontalMove(1);
            ball.SetverticalMove(1);
            ball.MakeMove(5, 5);
            Assert.AreEqual(2, ball.XCordinate);
            Assert.AreEqual(2, ball.YCordinate);
            Assert.AreEqual(1, ball.GethorizontalMove());
            Assert.AreEqual(1, ball.GetverticalMove());
        }
    }
}
