using System;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace CsharpTestProject1
{
    internal class AdminNewProdPage : Page
    {
        internal struct CurDateTime
        {
            public DateTime curDate;
            public int yyyy;
            public int mm;
            public int dd;
            public int h;
            public int m;
            public int s;

            public CurDateTime(DateTime date)
            {
                curDate = date;
                yyyy    = curDate.Year;
                mm      = curDate.Month;
                dd      = curDate.Day;
                h       = curDate.Hour;
                m       = curDate.Minute;
                s       = curDate.Second;
            }
        }

        internal static string GetProdPrefix(CurDateTime curDateTime)
        { 
            return WebDriverExtensions.PaddingLeft(curDateTime.h) + 
                   WebDriverExtensions.PaddingLeft(curDateTime.m) + 
                   WebDriverExtensions.PaddingLeft(curDateTime.s);
        }

        internal static string GetProdValidFrom(CurDateTime curDateTime)
        { 
            return PageParams.GetFullDateStrForBrowserDateControl(curDateTime.yyyy, curDateTime.mm, curDateTime.dd);
        }

        internal static string GetProdValidTo(CurDateTime curDateTime)
        { 
            return PageParams.GetFullDateStrForBrowserDateControl(curDateTime.yyyy + 2, curDateTime.mm, curDateTime.dd);
        }

        internal void Css_catalog_ElementClick() => PageParams.FindElmAndClick(By.CssSelector("[href*=catalog]"));

        internal void LinkText_ElementClick(string text) => PageParams.FindElmAndClick(By.LinkText(text));

        internal void Name_ElementClick(string text) => PageParams.FindElmAndClick(By.Name(text));

        internal void Name_ElementClear(string text) => PageParams.Driver.FindElmAndClear(By.Name(text));

        internal void Name_ElementSendKeys(string text, string keyText) => PageParams.Driver.FindElmAndSendKeys(By.Name(text), keyText);

        internal void XPath_categories_RubberDucks_ElementClick() => PageParams.FindElmAndClick(By.XPath("(//input[@name='categories[]'])[2]"));
        internal void XPath_categories_Unisex_ElementClick() => PageParams.FindElmAndClick(By.XPath("(//input[@name='product_groups[]'])[3]"));


        internal IList<IWebElement> Id_app_Elements => PageParams.Driver.FindElements(By.Id("app-"));


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