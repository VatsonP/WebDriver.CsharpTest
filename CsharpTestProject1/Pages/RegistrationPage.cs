using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace CsharpTestProject1
{
    internal class RegistrationPage : Page
    {
        public RegistrationPage(PageParams _pageParams) : base(_pageParams) 
        {
            //Использование PageFactory устарело (Depricated ), 
            //т.к. заменяется свойствами, возвращающими нужный IWebElement
            //PageFactory.InitElements(PageParams.Driver, this);
        }

        internal void Open()
        {
            PageParams.Driver.Url = "http://" + PageParams.CurrentIpStr + ":8080/litecart/en/create_account";
        }

        //[FindsBy(How = How.Name, Using = "tax_id")]
        internal IWebElement Tax_idInput => PageParams.Driver.FindElement(By.Name("tax_id"));

        //[FindsBy(How = How.Name, Using = "company")]
        internal IWebElement CompanyInput => PageParams.Driver.FindElement(By.Name("company"));

        //[FindsBy(How = How.Name, Using = "address1")]
        internal IWebElement Address1Input => PageParams.Driver.FindElement(By.Name("address1"));

        //[FindsBy(How = How.Name, Using = "city")]
        internal IWebElement CityInput => PageParams.Driver.FindElement(By.Name("city"));

        //[FindsBy(How = How.Name, Using = "email")]
        internal IWebElement EmailInput => PageParams.Driver.FindElement(By.Name("email"));

        //[FindsBy(How = How.Name, Using = "firstname")]
        internal IWebElement FirstnameInput => PageParams.Driver.FindElement(By.Name("firstname"));

        //[FindsBy(How = How.Name, Using = "lastname")]
        internal IWebElement LastnameInput => PageParams.Driver.FindElement(By.Name("lastname"));

        //[FindsBy(How = How.Name, Using = "phone")]
        internal IWebElement PhoneInput => PageParams.Driver.FindElement(By.Name("phone"));

        //[FindsBy(How = How.Name, Using = "postcode")]
        internal IWebElement PostcodeInput => PageParams.Driver.FindElement(By.Name("postcode"));

        //[FindsBy(How = How.Name, Using = "password")]
        internal IWebElement PasswordInput => PageParams.Driver.FindElement(By.Name("password"));

        //[FindsBy(How = How.Name, Using = "confirmed_password")]
        internal IWebElement ConfirmedPasswordInput => PageParams.Driver.FindElement(By.Name("confirmed_password"));

        //[FindsBy(How = How.Name, Using = "create_account")]
        internal IWebElement CreateAccountButton => PageParams.Driver.FindElement(By.Name("create_account"));

        internal void SelectCountry(string country)
        {
            PageParams.Driver.FindElement(By.CssSelector("[id ^= select2-country_code]")).Click();
            PageParams.Driver.FindElement(By.CssSelector(
                    String.Format(".select2-results__option[id $= {0}]", country))).Click();
        }

        internal void SelectZone(string zone)
        {
            PageParams.DriverWait.Until(d => d.FindElement(
                    By.CssSelector(String.Format("select[name=zone_code] option[value={0}]", zone))));
            new SelectElement(PageParams.Driver.FindElement(By.CssSelector("select[name=zone_code]"))).SelectByValue(zone);
        }

    }
}