using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Data;
using NuGet.Frameworks;

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
    }
}