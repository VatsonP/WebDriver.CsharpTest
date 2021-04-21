using System;
using NUnit.Framework;


namespace CsharpTestProject1
{

    [TestFixture]
    public class UT5CheckCountriesTests : TestBase
    {
        protected UT5CheckCountriesApp App
        {
            get;
            private set;
        }

        public UT5CheckCountriesTests() : base(new DriverBaseParams(drvImplWaitTime: 3, drvExplWaitTime: 4))
        {
            App = new UT5CheckCountriesApp();
        }

        [Test]
        public void TestMyCheckCountries()
        {
            App.InitPages(DrvBase); // For NEW PageObject realization

            App.MyCheckCountries();
        }

    }
}