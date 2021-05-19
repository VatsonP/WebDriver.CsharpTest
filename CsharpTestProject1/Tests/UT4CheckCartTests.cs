using System;
using NUnit.Framework;
using CsharpTestProject1.TestApp;


namespace CsharpTestProject1.Test
{

    [TestFixture]
    public class UT4CheckCartTests : TestBase
    {
        protected UT4CheckCartApp App
        {
            get;
            private set;
        }

        public UT4CheckCartTests() : base(CreateDriverBaseParams())
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