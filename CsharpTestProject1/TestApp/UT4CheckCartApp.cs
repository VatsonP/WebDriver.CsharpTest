using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;


namespace CsharpTestProject1
{

    public class UT4CheckCartApp
    {
        IList<IWebElement> prodList;
        ReadOnlyCollection<IWebElement> Cart, prodTable;
        IWebElement productUnit;
        int i, j, k, k1, p;
        String[] prodName;
        const int prodCartCount = 3;

        private const int sleepTimeMSec = 500;

        private PageParams pageParams;
        private AdminPanelLoginPage adminPanelLoginPage;

        public void InitPages(DriverBase drvBase)
        {
            pageParams = new PageParams(drvBase);

            adminPanelLoginPage = new AdminPanelLoginPage(pageParams);
        }

        private void LoginAs(string usrText, string passText)
        {
            if (adminPanelLoginPage.Open().IsOnThisPage())
            {
                adminPanelLoginPage.EnterUsername(usrText).EnterPassword(passText).SubmitLogin();
            }

        }

        public void myCheckCart()
        {
            prodName = new String[prodCartCount];

            for (i = 0; i < prodCartCount; i++)
            {
                PageParams.Driver.Navigate().GoToUrl("http://" + PageParams.CurrentIpStr + ":8080/litecart/en/"); //открыть главную страницу магазина
                PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Online Store"));

                prodList = PageParams.Driver.FindElements(By.CssSelector("li.product"));

                // определение списка товаров на главной странице
                if (prodList.Count > 0)
                {
                    p = 1; j = 0;
                    while (p > 0)
                    {
                        k = 1; k1 = 1;
                        // выбираем конкретный продукт
                        productUnit = prodList[j];

                        prodName[i] = productUnit.FindElement(By.CssSelector("div.name")).Text;
                        // получаем имя продукта

                        if (i == 1)
                        { // для 2-го товара
                          // проверяем, что выбранный товар не совпадает с предыдущим
                            k = String.Compare(prodName[i], prodName[i - 1], StringComparison.OrdinalIgnoreCase);

                        }

                        if (i == 2)
                        { // для 3-го товара
                          // проверяем, что выбранный товар не совпадает с предыдущими
                            k = String.Compare(prodName[i], prodName[i - 1], StringComparison.OrdinalIgnoreCase);
                            k1 = String.Compare(prodName[i], prodName[i - 2], StringComparison.OrdinalIgnoreCase);

                        }

                        if ((k == 0) || (k1 == 0))
                        { j++; } // переходим на следующий продукт в списке
                        else
                        { p = 0; }  // подходящий продукт найден - прерываем цикл
                    }

                    productUnit.Click(); //щелкаем по странице продукта

                    PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(prodName[i]));

                    Cart = PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("cart"))); // нашли корзину

                    k = String.Compare(prodName[i], "Yellow Duck", StringComparison.OrdinalIgnoreCase);
                    // Проверяем, что выбранный товар не Yellow Duck - требует доп. обработки
                    if (k == 0) // Обработка Yellow Duck - выбираем размер
                    {
                        // select the drop down list
                        var sizeElm = PageParams.Driver.FindElement(By.Name("options[Size]"));
                        //create select element object 
                        var selectElement = new SelectElement(sizeElm);
                        // select by text
                        selectElement.SelectByText("Small");
                    }

                    PageParams.Driver.FindElement(By.Name("add_cart_product")).Click();
                    // добавляем продукт в корзину

                    PageParams.DriverWait.Until(
                                SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(
                                    PageParams.Driver.FindElement(By.CssSelector("span.quantity")),
                                    Convert.ToString(i + 1))
                              );
                    // ждем изменения количества
                }
            }

            PageParams.Driver.Navigate().GoToUrl("http://" + PageParams.CurrentIpStr + ":8080/litecart/en/"); //открыть главную страницу магазина

            PageParams.Driver.FindElement(By.Id("cart")).Click(); // открываем корзину
            PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Checkout")); // ожидаем открытия страницы корзины

            for (int n = 1; n <= prodCartCount; n++)
            {
                prodTable = PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("order_confirmation-wrapper")));
                // находим таблицу товаров в корзине

                if (PageParams.Driver.areElementsPresent(By.CssSelector("li.shortcut")))
                {
                    PageParams.Driver.Sleep(sleepTimeMSec);
                    prodList = PageParams.Driver.FindElements(By.CssSelector("li.shortcut"));

                    if (prodList.Count > 0)
                    {
                        /*
                            Поскольку изначально картинки продуктов на экране сменяются, мы просто определяем
                            список маленьких изображений продуктов и щелкаем по нему.
                            При этом изображение продукта и все связанные с ним служебные кнопки фиксируются.
                        */
                        prodList[0].Click();
                    }

                    PageParams.Driver.FindElement(By.Name("remove_cart_item")).Click();
                    // кликнуть по кнопке удаления товара Remove
                    PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(prodTable.GetEnumerator().Current));
                    // ожидаем обновления таблицы со списком товаров
                }
            }

            PageParams.Driver.Navigate().GoToUrl("http://" + PageParams.CurrentIpStr + ":8080/litecart/en/");
            PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Online Store"));

        }

    }
}
