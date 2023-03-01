using System;
using NUnit.Framework;
using CsharpTestProjCore.TestApp;


namespace CsharpTestProjCore.Test
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