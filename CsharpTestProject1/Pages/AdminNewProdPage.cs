using System;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace CsharpTestProject1
{
    internal class AdminNewProdPage : Page
    {
        internal IList<IWebElement> Id_app_Elements => PageParams.Driver.FindElements(By.Id("app-"));

        internal IWebElement Css_h1_Element => PageParams.Driver.FindElement(By.CssSelector("h1"));

        internal IList<IWebElement> getCss_menu_id_doc_Elements(IWebElement menuPoint)
        {
            return menuPoint.FindElements(By.CssSelector("[id^=doc-]"));
        }


        internal IWebElement Row_Css_a_Element(IWebElement row)
        { 
            return row.FindElement(By.CssSelector("a"));    
        }


        private static Func<IWebDriver, String> AnyWindowOtherThan(ReadOnlyCollection<String> oldWindows)
        {
            return (driver) =>
            {
                ReadOnlyCollection<string> handles = PageParams.Driver.WindowHandles;
                handles.Except(oldWindows);
                return handles.Count > 0 ? handles.AsEnumerable().Last() : null;
            };

        }

        internal String waitUntilEditCountry(ReadOnlyCollection<String> existingWindows)
        {
            return PageParams.DriverWait.Until(AnyWindowOtherThan(existingWindows)); // ждем загрузки окна
        }

        internal void SwitchToWindow(String strWindow)
        {
            PageParams.Driver.SwitchTo().Window(strWindow);  // переключаемся в новое окно
        }

        internal void CloseCurWindow()
        {
            PageParams.Driver.Close();  // закрываем окно
        }
        //-------------------------------------------------------------------------

        public AdminNewProdPage(PageParams _pageParams) : base(_pageParams) 
        {
            //Использование PageFactory устарело (Depricated ), 
            //т.к. заменяется свойствами, возвращающими нужный IWebElement
            //PageFactory.InitElements(PageParams.Driver, this);
        }
              
        internal AdminNewProdPage waitUntilMyStore()
        {
            PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleIs("My Store"));
            //подождать пока не загрузится страница с заголовком "My Store"
            return this;
        }
 
    }
}