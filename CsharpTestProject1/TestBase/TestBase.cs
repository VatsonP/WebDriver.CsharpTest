using System;
using NUnit.Framework;


namespace CsharpTestProject1
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
            DrvBase = new DriverBase(driverBaseParams);
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