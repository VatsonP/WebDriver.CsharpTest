using System;
using NUnit.Framework;
using CsharpTestProjCore.TestApp;


namespace CsharpTestProjCore.Test
{

    [TestFixture]
    public class UT2LeftMenuClickTests : TestBase
    {
        private const int sleepTimeMenuMSec = 100;
        private const int sleepTimeSubmenuMSec = 200;
        private const int sleepTimeMSec = 300;

        protected UT2LeftMenuClickApp App
        {
            get;
            private set;
        }

        public UT2LeftMenuClickTests() : base(CreateDriverBaseParams(drvImplWaitTime: 3, drvExplWaitTime: 5))
        {
            App = new UT2LeftMenuClickApp(sleepTimeMenuMSec: sleepTimeMenuMSec, 
                                          sleepTimeSubmenuMSec: sleepTimeSubmenuMSec,
                                          sleepTimeMSec: sleepTimeMSec
                                         );
        }

        [Test]
        public void TestMyLeftMenuClick()
        {
            App.InitPages(DrvBase); // For NEW PageObject realization

            App.MyLeftMenuClick();
        }

    }
}