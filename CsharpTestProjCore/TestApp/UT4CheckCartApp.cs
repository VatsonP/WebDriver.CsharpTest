using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using CsharpTestProject1.Pages;
using CsharpWebDriverLib;


namespace CsharpTestProject1.TestApp
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
        private MainLiteCartPage mainLiteCartPage;

        public void InitPages(IDriverBase drvBase)
        {
            pageParams = new PageParams(drvBase);

            mainLiteCartPage = new MainLiteCartPage(pageParams);
        }


        public void myCheckCart()
        {
            prodName = new String[prodCartCount];

            for (i = 0; i < prodCartCount; i++)
            {
                mainLiteCartPage.Open().WaitUntilMainPage(); //открыть главную страницу магазина и подождать загрузки

                prodList = mainLiteCartPage.FindByCss_li_product_Elements;

                // определение списка товаров на главной странице
                if (prodList.Count > 0)
                {
                    p = 1; j = 0;
                    while (p > 0)
                    {
                        k = 1; k1 = 1;
                        // выбираем конкретный продукт
                        productUnit = prodList[j];

                        prodName[i] = mainLiteCartPage.GetCss_div_name_Text(productUnit);
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

                    mainLiteCartPage.WaitUntilProdNameStr(prodNameStr: prodName[i]);

                    Cart = mainLiteCartPage.PresenceOfElementLocatedById_cart; // нашли корзину

                    k = String.Compare(prodName[i], "Yellow Duck", StringComparison.OrdinalIgnoreCase);
                    // Проверяем, что выбранный товар не Yellow Duck - требует доп. обработки
                    if (k == 0) // Обработка Yellow Duck - выбираем размер
                    {
                        // select the drop down list
                        var sizeElm = mainLiteCartPage.FindByName_optionsSize;
                        //create select element object 
                        var selectElement = new SelectElement(sizeElm);
                        // select by text
                        selectElement.SelectByText("Small");
                    }

                    mainLiteCartPage.FindByName_add_cart_product.Click(); // добавляем продукт в корзину

                    mainLiteCartPage.WaitUntil_textToBePresentInElement_Css_span_quantity(i + 1); // ждем изменения количества
                }
            }

            mainLiteCartPage.Open().WaitUntilMainPage(); //открыть главную страницу магазина и подождать загрузки

            mainLiteCartPage.FindById_cart.Click(); // открываем корзину
            mainLiteCartPage.WaitUntil_Checkout();  // ожидаем открытия страницы корзины

            for (int n = 1; n <= prodCartCount; n++)
            {
                prodTable = mainLiteCartPage.GetPresenceOfElementLocatedById_order_conf_wrapper();
                // находим таблицу товаров в корзине

                if (mainLiteCartPage.AreElementsPresent_Css_li_shortcut_Elements)
                {
                    PageParams.Driver.Sleep(sleepTimeMSec);
                    prodList = mainLiteCartPage.FindByCss_li_shortcut_Elements; 

                    if (prodList.Count > 0)
                    {
                        /*
                            Поскольку изначально картинки продуктов на экране сменяются, мы просто определяем
                            список маленьких изображений продуктов и щелкаем по нему.
                            При этом изображение продукта и все связанные с ним служебные кнопки фиксируются.
                        */
                        prodList[0].Click();
                    }

                    mainLiteCartPage.FindbyName_remove_cart_item.Click(); // кликнуть по кнопке удаления товара Remove
                    mainLiteCartPage.WaitUntilStalenessOfProdTable(prodTable);
                    // ожидаем обновления таблицы со списком товаров
                }
            }
            mainLiteCartPage.Open().WaitUntilMainPage(); //открыть главную страницу магазина и подождать загрузки
        }
    }
}
