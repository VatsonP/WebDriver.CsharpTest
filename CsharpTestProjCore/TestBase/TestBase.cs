using System;
using NUnit.Framework;
using CsharpWebDriverLib;


namespace CsharpTestProject1.Test
{

    [TestFixture]
    public abstract class TestBase
    {
        public IDriverBase DrvBase
        { 
            get;
            private set;
        }

        public TestBase(DriverBaseParams driverBaseParams) 
            => DrvBase = IDriverBase.CreateDriverBase(driverBaseParams);

        public static DriverBaseParams CreateDriverBaseParams()
            => DriverBaseParams.CreateDriverBaseParams();

        public static DriverBaseParams CreateDriverBaseParams(int drvImplWaitTime)
            => DriverBaseParams.CreateDriverBaseParams(drvImplWaitTime);

        public static DriverBaseParams CreateDriverBaseParams(int drvImplWaitTime, int drvExplWaitTime)
            => DriverBaseParams.CreateDriverBaseParams(drvImplWaitTime, drvExplWaitTime);

        public static DriverBaseParams CreateDriverBaseParams(int drvImplWaitTime, int drvExplWaitTime, int drvMaxWaitTime)
            => DriverBaseParams.CreateDriverBaseParams(drvImplWaitTime, drvExplWaitTime, drvMaxWaitTime);


        [OneTimeSetUp]
        protected static void OneTimeSetUp() => IDriverBase.OneTimeSetUp();

        [SetUp]
        protected void SetUp() => DrvBase.SetUp();

        [TearDown]
        protected void TearDown() => DrvBase.TearDown();

        [OneTimeTearDown]
        protected static void OneTimeTearDown() => IDriverBase.OneTimeTearDown();

    }
}