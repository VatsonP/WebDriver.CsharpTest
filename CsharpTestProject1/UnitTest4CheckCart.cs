using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CsharpTestProject1
{

    [TestFixture]
    public class CheckCart : DriverBase
    {
        IList<IWebElement> prodList;
        ReadOnlyCollection<IWebElement> Cart, prodTable;
        IWebElement productUnit;
        int i, j, k, k1, p;
        String[] prodName;
        const int prodCartCount = 3;

        private const int sleepTimeMSec = 2000;

        public CheckCart() : base(new DriverBaseParams()) { }

        [Test]
        public void myCheckCart() {

            prodName = new String[prodCartCount];

            for (i = 0; i < prodCartCount; i++)
            {
                driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/en/"); //открыть главную страницу магазина
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Online Store"));

                prodList = driver.FindElements(By.CssSelector("li.product"));

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
                            k  = String.Compare(prodName[i], prodName[i - 1], StringComparison.OrdinalIgnoreCase);
                            k1 = String.Compare(prodName[i], prodName[i - 2], StringComparison.OrdinalIgnoreCase);

                        }

                        if ((k == 0) || (k1 == 0))
                        { j++; } // переходим на следующий продукт в списке
                        else
                        { p = 0; }  // подходящий продукт найден - прерываем цикл
                    }

                    productUnit.Click(); //щелкаем по странице продукта

                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains(prodName[i]));

                    Cart = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("cart"))); // нашли корзину

                    k = String.Compare(prodName[i], "Yellow Duck", StringComparison.OrdinalIgnoreCase);
                    // Проверяем, что выбранный товар не Yellow Duck - требует доп. обработки
                    if (k == 0) // Обработка Yellow Duck - выбираем размер
                    {
                        // select the drop down list
                        var sizeElm = driver.FindElement(By.Name("options[Size]"));
                        //create select element object 
                        var selectElement = new SelectElement(sizeElm);
                        // select by text
                        selectElement.SelectByText("Small");
                    }

                    driver.FindElement(By.Name("add_cart_product")).Click();
                    // добавляем продукт в корзину

                    wait.Until(
                                SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(
                                    driver.FindElement(By.CssSelector("span.quantity")),
                                    Convert.ToString(i + 1))
                              );
                    // ждем изменения количества
                }
        }

        driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/en/"); //открыть главную страницу магазина

        driver.FindElement(By.Id("cart")).Click(); // открываем корзину
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Checkout")); // ожидаем открытия страницы корзины

        for(int n=1; n<= prodCartCount; n++)
        {
            prodTable = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("order_confirmation-wrapper")));
                // находим таблицу товаров в корзине

            if (driver.areElementsPresent(By.CssSelector("li.shortcut")) )
                {
                    driver.Sleep(sleepTimeMSec);
                    prodList = driver.FindElements(By.CssSelector("li.shortcut"));

                    if (prodList.Count > 0)
                    {
                        /*
                            Поскольку изначально картинки продуктов на экране сменяются, мы просто определяем
                            список маленьких изображений продуктов и щелкаем по нему.
                            При этом изображение продукта и все связанные с ним служебные кнопки фиксируются.
                        */
                        prodList[0].Click();
                    }

                    driver.FindElement(By.Name("remove_cart_item")).Click();
                    // кликнуть по кнопке удаления товара Remove
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(prodTable.GetEnumerator().Current));
                    // ожидаем обновления таблицы со списком товаров
                }
        }

        driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/en/");
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Online Store"));

        }

    }
}