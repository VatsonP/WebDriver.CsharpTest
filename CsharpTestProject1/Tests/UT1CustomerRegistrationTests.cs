using System;
using System.Collections.Generic;
using NUnit.Framework;


namespace CsharpTestProject1
{

    [TestFixture]
    public class UT1CustomerRegistrationTests : TestBase
    {
        protected UT1CustomerRegistrationApp App
        {
            get;
            private set;
        }

        public UT1CustomerRegistrationTests() : base(new DriverBaseParams())
        {
            App = new UT1CustomerRegistrationApp();
        }

        [Test, TestCaseSource(typeof(DataProviders), "ValidCustomers")]
        public void CanRegisterCustomer(Customer customer)
        {
            App.InitPages(DrvBase); // For NEW PageObject realization

            ISet<String> oldIds = App.GetCustomerIds(isNeedPrint: true, printHeader: "Before enter new Customer");

            App.RegisterNewCustomer(customer);

            ISet<String> newIds = App.GetCustomerIds(isNeedPrint: true, printHeader: "After insert new Customer");

            Assert.IsTrue(newIds.IsSupersetOf(oldIds));
            Assert.IsTrue(newIds.Count == oldIds.Count + 1);
            Console.WriteLine("Old Customers Count= {0}, New Customers Count= {1}", oldIds.Count, newIds.Count);
        }
    }
}