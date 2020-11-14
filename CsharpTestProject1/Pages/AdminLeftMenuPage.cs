using System;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace CsharpTestProject1
{
    internal class AdminLeftMenuPage : Page
    {
        internal IList<IWebElement> Id_app_Elements => PageParams.Driver.FindElements(By.Id("app-"));

        internal IWebElement Css_h1_Element => PageParams.Driver.FindElement(By.CssSelector("h1"));


        public IList<IWebElement> getCss_menu_id_doc_Elements(IWebElement menuPoint)
        {
            return menuPoint.FindElements(By.CssSelector("[id^=doc-]"));
        }

        public AdminLeftMenuPage(PageParams _pageParams) : base(_pageParams) 
        {
            //Использование PageFactory устарело (Depricated ), 
            //т.к. заменяется свойствами, возвращающими нужный IWebElement
            //PageFactory.InitElements(PageParams.Driver, this);
        }

        internal AdminLeftMenuPage Open()
        {
            //XAMPP litecart admin page - "http://" + CurrentIpStr + ":8080/litecart/admin/?app=customers&doc=customers"
            PageParams.Driver.Url = "http://" + PageParams.CurrentIpStr + ":8080/litecart/admin/?app=customers&doc=customers"; //открыть страницу

            return this;
        }

    }
}