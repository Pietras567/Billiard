using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Data;
using Model;
using NuGet.Frameworks;

namespace UnitTest
{
    [TestClass]
    public class ModelApiTest
    {
        [TestMethod]
        public void constructorTest()
        {
            ModelAbstractApi modelAbstractApi = ModelAbstractApi.CreateApi();
            Assert.IsNotNull(modelAbstractApi);
        }

        [TestMethod]
        public void setGetApiTest()
        {
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi();
            ModelAbstractApi modelAbstractApi = ModelAbstractApi.CreateApi();
            Assert.IsNotNull(modelAbstractApi);
            Assert.IsNotNull(gameAbstractApi);
            modelAbstractApi.GameAPI = gameAbstractApi;
            Assert.IsNotNull(modelAbstractApi.GameAPI);
            Assert.AreSame(gameAbstractApi, modelAbstractApi.GameAPI);
        }

        [TestMethod]
        public void InjectionTest()
        {
            GameAbstractApi gameAbstractApi = GameAbstractApi.CreateApi();
            ModelAbstractApi modelAbstractApi = ModelAbstractApi.CreateApi(gameAbstractApi);
            Assert.IsNotNull(modelAbstractApi);
            Assert.IsNotNull(gameAbstractApi);
            Assert.IsNotNull(modelAbstractApi.GameAPI);
            Assert.AreSame(gameAbstractApi, modelAbstractApi.GameAPI);
        }

    }
}