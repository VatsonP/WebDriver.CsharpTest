using System;
using NUnit.Framework;
using CsharpWebDriverLib;


namespace CsharpTestProject1.Test
{

    [TestFixture]
    public abstract class TestBase
    {
        public DriverBase DrvBase
        { 
            get;
            private set;
        }

        public TestBase(DriverBaseParams driverBaseParams) 
        {
            DrvBase = new DriverBaseReal(driverBaseParams);
        }

        public static DriverBaseParams CreateDriverBaseParams()
        { 
            return new DriverBaseParams();
        }

        public static DriverBaseParams CreateDriverBaseParams(int drvImplWaitTime)
        {
            return new DriverBaseParams(drvImplWaitTime);
        }

        public static DriverBaseParams CreateDriverBaseParams(int drvImplWaitTime, int drvExplWaitTime)
        {
            return new DriverBaseParams(drvImplWaitTime, drvExplWaitTime);
        }

        public static DriverBaseParams CreateDriverBaseParams(int drvImplWaitTime, int drvExplWaitTime, int drvMaxWaitTime)
        {
            return new DriverBaseParams(drvImplWaitTime, drvExplWaitTime, drvMaxWaitTime);
        }

        [OneTimeSetUp]
        protected static void OneTimeSetUp()
        {
            DriverBase.OneTimeSetUp();
        }

        [SetUp]
        protected void SetUp()
        {
            DrvBase.SetUp();
        }

        [TearDown]
        protected void TearDown()
        {
            DrvBase.TearDown();
        }

        [OneTimeTearDown]
        protected static void OneTimeTearDown()
        {
            DriverBase.OneTimeTearDown();
        }
    }
}