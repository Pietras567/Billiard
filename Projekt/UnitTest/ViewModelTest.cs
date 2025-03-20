using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Data;
using ViewModel;
using Model;
using NuGet.Frameworks;

namespace UnitTest
{
    [TestClass]
    public class ViewModelApiTest
    {
        [TestMethod]
        public void constructorTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            Assert.IsNotNull(mainViewModel);
            Assert.IsNotNull(mainViewModel.ModelAPI);
        }

        [TestMethod]
        public void setGetApiTest()
        {
            ModelAbstractApi modelAbstractApi = ModelAbstractApi.CreateApi();
            MainViewModel mainViewModel = new MainViewModel();
            Assert.IsNotNull(modelAbstractApi);
            Assert.IsNotNull(mainViewModel);
            mainViewModel.ModelAPI = modelAbstractApi;
            Assert.IsNotNull(mainViewModel.ModelAPI);
            Assert.AreSame(modelAbstractApi, mainViewModel.ModelAPI);
        }

        [TestMethod]
        public void InjectionTest()
        {
            ModelAbstractApi modelAbstractApi = ModelAbstractApi.CreateApi();
            MainViewModel mainViewModel = new MainViewModel(modelAbstractApi);
            Assert.IsNotNull(mainViewModel);
            Assert.IsNotNull(modelAbstractApi);
            Assert.IsNotNull(mainViewModel.ModelAPI);
            Assert.AreSame(modelAbstractApi, mainViewModel.ModelAPI);
        }
    }
}