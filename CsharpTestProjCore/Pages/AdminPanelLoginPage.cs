using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CsharpTestProject1.Pages
{
    internal class AdminPanelLoginPage : Page
    {
        public AdminPanelLoginPage(PageParams _pageParams) : base(_pageParams) { }

        internal AdminPanelLoginPage Open()
        {
            PageParams.Driver.Url = "http://" + PageParams.CurrentIpStr + ":8080/litecart/admin";
            return this;
        }

        internal bool IsOnThisPage()
        {
            return PageParams.Driver.FindElements(By.Id("box-login")).Count > 0;
        }

        internal AdminPanelLoginPage EnterUsername(string username)
        {
            PageParams.Driver.FindElement(By.Name("username")).SendKeys(username);
            return this;
        }

        internal AdminPanelLoginPage EnterPassword(string password)
        {
            PageParams.Driver.FindElement(By.Name("password")).SendKeys(password);
            return this;
        }

        internal void SubmitLogin()
        {
            PageParams.Driver.FindElement(By.Name("login")).Click();
            PageParams.DriverWait.Until(d => d.FindElement(By.Id("box-apps-menu")));
        }

    }
}