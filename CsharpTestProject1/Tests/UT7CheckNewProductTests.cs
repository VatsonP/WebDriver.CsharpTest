using System;
using NUnit.Framework;


namespace CsharpTestProject1
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

        public UT7CheckNewProductTests() : base(new DriverBaseParams(drvImplWaitTime: 3, drvExplWaitTime: 4))
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