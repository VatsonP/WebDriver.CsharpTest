using System;
using NUnit.Framework;
using CsharpTestProjCore.TestApp;


namespace CsharpTestProjCore.Test
{

    [TestFixture]
    public class UT7CheckNewProductTests : TestBase
    {
        private const int sleepTimeMSec = 500;

        protected UT7CheckNewProductApp App
        {
            get;
            private set;
        }

        public UT7CheckNewProductTests() : base(CreateDriverBaseParams(drvImplWaitTime: 3, drvExplWaitTime: 5))
        {
            App = new UT7CheckNewProductApp(sleepTimeMSec);
        }

        [Test]
        public void TestMyCheckNewProduct()
        {
            App.InitPages(DrvBase); // For NEW PageObject realization

            App.myCheckNewProduct();
        }

    }
}