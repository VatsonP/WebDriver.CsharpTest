using System;
using NUnit.Framework;
using CsharpTestProject1.TestApp;


namespace CsharpTestProject1.Test
{
    [TestFixture]
    /*Parallelizable(ParallelScope.Children)*/
    public class FirstDemoTests : TestBase
    {
        protected FirstDemoApp App
        {
            get;
            private set;
        }

        public FirstDemoTests() : base(CreateDriverBaseParams())
        {
            App = new FirstDemoApp();
        }

        [SetUp]
        public void InitSetUp()
        {
            App.InitPages(DrvBase); // For NEW PageObject realization
        }


        [Test]
        public void my01Test()
        {
            App.FirstTest();
        }

        [Test]
        public void my02Test()
        {
            App.SecondTest();
        }

        [Test]
        public void my03Test()
        {
            App.ThirdTest();
        }

        [Test]
        public void my04Test()
        {
            App.FourthTest();
        }

        [Test]
        public void my05Test()
        {
            App.FifthTest();
        }

        [Test]
        public void my06Test()
        {
            App.SixthTest_myCheckStiker();
        }
    }
}
