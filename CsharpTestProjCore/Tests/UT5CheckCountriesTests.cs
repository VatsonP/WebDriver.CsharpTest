using System;
using NUnit.Framework;
using CsharpTestProject1.TestApp;


namespace CsharpTestProject1.Test
{

    [TestFixture]
    public class UT5CheckCountriesTests : TestBase
    {
        protected UT5CheckCountriesApp App
        {
            get;
            private set;
        }

        public UT5CheckCountriesTests() : base(CreateDriverBaseParams())
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