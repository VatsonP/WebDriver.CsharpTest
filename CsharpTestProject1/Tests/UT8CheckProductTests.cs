using System;
using NUnit.Framework;


namespace CsharpTestProject1
{

    [TestFixture]
    public class UT8CheckProductTests : TestBase
    {
        private const int sleepTimeMSec = 1000;

        protected UT8CheckProductApp App
        {
            get;
            private set;
        }

        public UT8CheckProductTests() : base(new DriverBaseParams(drvImplWaitTime: 3, drvExplWaitTime: 5))
        {
            App = new UT8CheckProductApp(sleepTimeMSec);
        }

        [Test]
        public void TestMyCheckProduct()
        {
            App.InitPages(DrvBase); // For NEW PageObject realization

            App.myCheckProduct();
        }

    }
}