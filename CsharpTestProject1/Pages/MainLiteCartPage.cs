using System;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;


namespace CsharpTestProject1
{

    internal class MainLiteCartPage : Page 
    {
        public MainLiteCartPage(PageParams _pageParams) : base(_pageParams) { }


        internal MainLiteCartPage Open()
        {
            PageParams.Driver.Navigate().GoToUrl("http://" + PageParams.CurrentIpStr + ":8080/litecart/en/");
            //открыть главную страницу магазина
            return this;
        }

        internal MainLiteCartPage WaitUntilMainPage()
        {
            PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Online Store"));
            // ждем загрузки главной страницы
            return this;
        }

        internal IList<IWebElement> FindByCss_li_product_Elements => PageParams.Driver.FindElements(By.CssSelector("li.product"));

        internal bool AreElementsPresent_Css_li_shortcut_Elements => PageParams.Driver.areElementsPresent(By.CssSelector("li.shortcut"));

        internal IList<IWebElement> FindByCss_li_shortcut_Elements => PageParams.Driver.FindElements(By.CssSelector("li.shortcut"));

        internal IWebElement FindByName_optionsSize => PageParams.Driver.FindElement(By.Name("options[Size]"));

        internal IWebElement FindByName_add_cart_product => PageParams.Driver.FindElement(By.Name("add_cart_product"));

        internal IWebElement FindById_cart => PageParams.Driver.FindElement(By.Id("cart"));

        internal IWebElement FindbyName_remove_cart_item => PageParams.Driver.FindElement(By.Name("remove_cart_item"));

        internal string GetCss_div_name_Text(IWebElement productUnit){ return productUnit.FindElement(By.CssSelector("div.name")).Text; }

        internal void WaitUntilProdNameStr(String prodNameStr)
        {
            PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(prodNameStr)); // ждем загрузки страницы продукта
        }

        internal ReadOnlyCollection<IWebElement> PresenceOfElementLocatedById_cart =>
                 PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("cart"))); // нашли корзину

        internal bool WaitUntil_Checkout()
        {
            return PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Checkout"));// ожидаем открытия страницы корзины
        }
                                            
        
        public void WaitUntil_textToBePresentInElement_Css_span_quantity(int ii) {
            PageParams.DriverWait.Until(
                        SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElementLocated(
                                                   By.CssSelector("span.quantity"),
                                                   Convert.ToString(ii)
                                                   )
                      );
        }

        public ReadOnlyCollection<IWebElement> GetPresenceOfElementLocatedById_order_conf_wrapper() {
            return PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("order_confirmation-wrapper")));
            // находим таблицу товаров в корзине
        }

        public void WaitUntilStalenessOfProdTable(ReadOnlyCollection<IWebElement> prodTable)
        {
            PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(prodTable.GetEnumerator().Current));
        }

    }

}