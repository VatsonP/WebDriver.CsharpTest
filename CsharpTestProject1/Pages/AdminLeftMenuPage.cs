using System;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace CsharpTestProject1
{
    internal class AdminLeftMenuPage : Page
    {
        internal IList<IWebElement> Id_app_Elements => PageParams.Driver.FindElements(By.Id("app-"));

        internal IWebElement Css_h1_Element => PageParams.Driver.FindElement(By.CssSelector("h1"));

        internal IList<IWebElement> getCss_menu_id_doc_Elements(IWebElement menuPoint)
        {
            return menuPoint.FindElements(By.CssSelector("[id^=doc-]"));
        }

        //-- for UT5CheckCountriesApp ----------------------------------------------
        internal IList<IWebElement> Css_geo_zones_row_Elements => PageParams.Driver.FindElements(By.CssSelector("[name=geo_zones_form] .row"));
        internal IList<IWebElement> Css_id_table_zones_tr_Elements => PageParams.Driver.FindElements(By.CssSelector("[id=table-zones] tr"));

        // открываем страницу просмотра географических зон
        internal AdminLeftMenuPage OpenGeoZones()
        {
            PageParams.Driver.Url = "http://" + PageParams.CurrentIpStr + ":8080/litecart/admin/?app=geo_zones&doc=geo_zones";
            // открываем страницу просмотра географических зон

            return this;
        }

        internal IWebElement Row_Css_a_Element(IWebElement row)
        { 
            return row.FindElement(By.CssSelector("a"));    
        }

        internal IWebElement CountryRow_Css_td_nth_child6_Element(IWebElement countryRow)
        {
            return countryRow.FindElement(By.CssSelector("td:nth-child(6)"));
        }

        internal IWebElement ZoneRow_Css_td_nth_child3_Element(IWebElement zoneRow)
        {
            return zoneRow.FindElement(By.CssSelector("td:nth-child(3)"));
        }

        internal IWebElement ZoneRow_Css_id_table_zones_td_nth_child3_Element(IWebElement zoneRow)
        {
            return zoneRow.FindElement(By.CssSelector("[id=table-zones] tr td:nth-child(3) [selected=selected]"));
        }

        //-- for UT3CheckNewTabsApp ----------------------------------------------
        internal IList<IWebElement> Css_countries_row_Elements => PageParams.Driver.FindElements(By.CssSelector("[name=countries_form] .row"));

        internal IList<IWebElement> Css_form_fa_external_link_Elements => PageParams.Driver.FindElements(By.CssSelector("form .fa-external-link"));

        internal IWebElement getCss_a_Elements(IWebElement countryRow)
        {
            return countryRow.FindElement(By.CssSelector("a"));
        }

        internal String getCurrentWindowHandle()
        {
            return PageParams.Driver.CurrentWindowHandle; 
        }

        internal ReadOnlyCollection<String> getWindowHandles()
        {
            return PageParams.Driver.WindowHandles;
        }

        private static Func<IWebDriver, String> AnyWindowOtherThan(ReadOnlyCollection<String> oldWindows)
        {
            return (driver) =>
            {
                ReadOnlyCollection<string> handles = driver.WindowHandles;
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

        public AdminLeftMenuPage(PageParams _pageParams) : base(_pageParams) 
        {
            //Использование PageFactory устарело (Depricated ), 
            //т.к. заменяется свойствами, возвращающими нужный IWebElement
            //PageFactory.InitElements(PageParams.Driver, this);
        }
              
        internal AdminLeftMenuPage waitUntilMyStore()
        {
            PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleIs("My Store"));
            //подождать пока не загрузится страница с заголовком "My Store"
            return this;
        }

        internal AdminLeftMenuPage OpenCountries()
        {
            //XAMPP litecart admin page - "http://" + CurrentIpStr + ":8080/litecart/admin/?app=countries&doc=countries"
            PageParams.Driver.Url = "http://" + PageParams.CurrentIpStr + ":8080/litecart/admin/?app=countries&doc=countries";
            //открыть страницу со списком стран

            return this;
        }
        internal AdminLeftMenuPage waitUntilCountries()
        {
            PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Countries")); 
            // ждем загрузки страницы
            return this;
        }
        internal AdminLeftMenuPage waitUntilEditCountry()
        {
            // открываем страницу выбранной страны
            PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Edit Country"));  
            // ждем загрузки страницы
            return this;
        }
 
    }
}