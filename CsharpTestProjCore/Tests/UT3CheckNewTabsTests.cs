using System;
using NUnit.Framework;
using CsharpTestProjCore.TestApp;


namespace CsharpTestProjCore.Test
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