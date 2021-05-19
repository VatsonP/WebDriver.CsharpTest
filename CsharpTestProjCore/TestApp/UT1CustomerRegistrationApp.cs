using System;
using System.Collections.Generic;
using CsharpWebDriverLib;
using CsharpTestProject1.Pages;
using CsharpTestProject1.Model;


namespace CsharpTestProject1.TestApp
{

    public class UT1CustomerRegistrationApp 
    {
        private int sleepTimeMSec;

        private PageParams          pageParams;
        private RegistrationPage    registrationPage;
        private AdminPanelLoginPage adminPanelLoginPage;
        private CustomerListPage    customerListPage;

        public UT1CustomerRegistrationApp(int sleepTimeMSec)
        {
            this.sleepTimeMSec = sleepTimeMSec;
        }


        public void InitPages(IDriverBase drvBase)
        {
            pageParams          = new PageParams(drvBase);

            registrationPage    = new RegistrationPage(pageParams);
            adminPanelLoginPage = new AdminPanelLoginPage(pageParams);
            customerListPage    =  new CustomerListPage(pageParams);
        }


        internal void RegisterNewCustomer(Customer customer)
        {
            registrationPage.Open();
            registrationPage.Tax_idInput.SendKeys(customer.Tax_id); ;
            registrationPage.CompanyInput.SendKeys(customer.Company);
            registrationPage.FirstnameInput.SendKeys(customer.Firstname);
            registrationPage.LastnameInput.SendKeys(customer.Lastname);
            registrationPage.Address1Input.SendKeys(customer.Address1);
            registrationPage.PostcodeInput.SendKeys(customer.Postcode);
            registrationPage.CityInput.SendKeys(customer.City);
            registrationPage.SelectCountry(customer.Country);
            //registrationPage.SelectZone(customer.Zone);
            registrationPage.EmailInput.SendKeys(customer.Email);
            registrationPage.PhoneInput.SendKeys(customer.Phone);
            registrationPage.PasswordInput.SendKeys(customer.Password);
            registrationPage.ConfirmedPasswordInput.SendKeys(customer.Password);

            PageParams.Sleep(sleepTimeMSec);

            pageParams.TakeScreenshot("ScreenOne"); 

            registrationPage.CreateAccountButton.Click();

            pageParams.TakeScreenshot("ScreenTwo");
        }

        internal ISet<string> GetCustomerIds(bool isNeedPrint = false, string printHeader = "")
        {
            if (adminPanelLoginPage.Open().IsOnThisPage())
            {
                adminPanelLoginPage.EnterUsername("admin").EnterPassword("admin").SubmitLogin();
            }

            return customerListPage.Open().GetCustomerIds(isNeedPrint, printHeader);
        }
    }

}
 