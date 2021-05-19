using System;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using CsharpWebDriverLib;

namespace CsharpTestProject1.Pages
{
    internal class AdminNewProdPage : Page
    {
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

        internal IList<IWebElement> Id_Elements(string text) => PageParams.Driver.FindElements(By.Id(text));

        internal void Css_catalog_ElementClick() => PageParams.FindElmAndClick(By.CssSelector("[href*=catalog]"));

        internal void LinkText_ElementClick(string text) => PageParams.FindElmAndClick(By.LinkText(text));

        internal void Name_ElementClick(string text) => PageParams.FindElmAndClick(By.Name(text));

        internal void Name_ElementClear(string text) => PageParams.Driver.FindElmAndClear(By.Name(text));

        internal void Name_ElementSendKeys(string text, string keyText) => PageParams.Driver.FindElmAndSendKeys(By.Name(text), keyText);

        internal void XPath_categories_RubberDucks_ElementClick() => PageParams.FindElmAndClick(By.XPath("(//input[@name='categories[]'])[2]"));
        internal void XPath_categories_Unisex_ElementClick() => PageParams.FindElmAndClick(By.XPath("(//input[@name='product_groups[]'])[3]"));

        internal IWebElement Name_manufacturerId_Element() => PageParams.Driver.FindElement(By.Name("manufacturer_id"));

        internal IWebElement Name_currencyCode_Element() => PageParams.Driver.FindElement(By.Name("purchase_price_currency_code"));

        internal void LinkText_FindElement(string text) => PageParams.Driver.FindElement(By.LinkText(text));

        //-------------------------------------------------------------------------

        internal IWebElement Css_FirstProduct_Campains_Element() => PageParams.Driver.FindElement(By.CssSelector("[id=box-campaigns] li.product"));

        internal IWebElement Css_Box_Product_Element() => PageParams.Driver.FindElement(By.CssSelector("[id=box-product]"));

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