using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProgram.Tests {
    [TestClass()]
    public class SimpleCalculatorTests {
        [TestMethod()]
        public void AddTest() {
            SimpleCalculator calculator = new SimpleCalculator();
            int result = calculator.Add(7, 4);
            Assert.AreEqual(result, 11);

            result = calculator.Add(8, -4);
            Assert.AreEqual(result, 4);


            result = calculator.Add(-6, -4);
            Assert.AreEqual(result, -10);
        }

        [TestMethod()]
        public void NegativeAddTest()
        {
            SimpleCalculator calculator = new SimpleCalculator();
            int result = calculator.Add(5, 5);
            Assert.AreNotEqual(result, 99);

        }
    }
}