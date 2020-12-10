using System;
using NUnit.Framework;


namespace CsharpTestProject1
{

    [TestFixture]
    public class UT4CheckCartTests : TestBase
    {
        protected UT4CheckCartApp App
        {
            get;
            private set;
        }

        public UT4CheckCartTests() : base(new DriverBaseParams())
        {
            App = new UT4CheckCartApp(); 
        }
    

        [Test]
        public void TestMyCheckCart()
        {
            App.InitPages(DrvBase); // For NEW PageObject realization

            App.myCheckCart();
        }
    }
}