using System;
using NUnit.Framework;
using CsharpTestProject1.TestApp;


namespace CsharpTestProject1.Test
{

    [TestFixture]
    public class UT3CheckNewTabsTests : TestBase
    {
        protected UT3CheckNewTabsApp App
        {
            get;
            private set;
        }

        public UT3CheckNewTabsTests() : base(CreateDriverBaseParams())
        {
            App = new UT3CheckNewTabsApp(); 
        }
    

        [Test]
        public void TestMyCheckNewTabs()
        {
            App.InitPages(DrvBase); // For NEW PageObject realization

            App.myCheckNewTabs();
        }
    }
}