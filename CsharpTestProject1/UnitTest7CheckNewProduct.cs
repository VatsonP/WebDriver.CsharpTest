using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Support.Extensions;

namespace CsharpTestProject1
{

    [TestFixture]
    public class CheckNewProduct : DriverBase
    {
        String Name, ProdName, validFrom, validTo, prefix;
       
        private const int sleepTimeMSec = 4000;

        public CheckNewProduct() : base(new DriverBaseParams()) { }

        [Test]
        public void myCheckNewProduct()
        {
            //XAMPP litecart admin page - "http://" + CurrentIpStr + ":8080/litecart/admin/"
            var remoteAddress = "http://" + CurrentIpStr + ":8080/litecart/admin/"; //открыть страницу

            LoginAs(webDriver: driver, remoteUri: new Uri(remoteAddress), driverType: webDriverType,
                    usrLocator: By.Name("username"), usrText: "admin",
                    passLocator: By.Name("password"), passText: "admin",
                    loginLocator: By.Name("login")
                   );

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleIs("My Store"));
            //подождать пока не загрузится страница с заголовком "My Store"
                        
            // читаем текущее время - добавляем его к фамилии и имеем уникальный e-mail и пароль каждый раз
            DateTime curDate = DateTime.Now;
            int yyyy = curDate.Year;
            int mm = curDate.Month;
            int dd = curDate.Day;
            int h = curDate.Hour;
            int m = curDate.Minute;
            int s = curDate.Second;
                                
            Name = "Donald McDown";
            prefix = WebDriverExtensions.PaddingLeft(h) + WebDriverExtensions.PaddingLeft(m) + WebDriverExtensions.PaddingLeft(s); 
            ProdName = Name + " " + prefix;

            validFrom = driver.GetFullDateStrForBrowserDateControl(yyyy, mm, dd, webDriverType);
            validTo = driver.GetFullDateStrForBrowserDateControl(yyyy + 2, mm, dd, webDriverType);

            driver.FindElmAndClick(By.CssSelector("[href*=catalog]"), webDriverType);
            // открыть каталог

            driver.FindElmAndClick(By.LinkText("Add New Product"), webDriverType);
            // открываем форму регистрации нового продукта

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

            driver.FindElmAndClick(By.Name("status"), webDriverType);
            // устанавливаем статус Enabled
            driver.FindElmAndClear(By.Name("name[en]"));
            // очистка
            driver.FindElmAndSendKeys(By.Name("name[en]"), ProdName);
            // вводим название товара
            driver.FindElmAndSendKeys(By.Name("code"), (prefix + Keys.Tab) );
            // вводим код товара
            driver.FindElmAndClick(By.XPath("(//input[@name='categories[]'])[2]"), webDriverType);
            // устанавливаем категорию Rubber Ducks

            driver.FindElmAndClick(By.XPath("(//input[@name='product_groups[]'])[3]"), webDriverType);
            // Устанавливаем группу Unisex

            driver.FindElmAndSendKeys(By.Name("quantity"), "1");
            // устанавливаем количество 1

            driver.FindElmAndSendKeys(By.Name("date_valid_from"), validFrom);
            // устанавливаем дату начала годности
            driver.FindElmAndSendKeys(By.Name("date_valid_to"), validTo);
            // устанавливаем дату конца годности


            driver.Sleep(sleepTimeMSec);

            driver.FindElmAndClick(By.LinkText("Information"), webDriverType);
            // переходим на вкладку Information

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

            // select the drop down list
            var manufacturerIdElm = driver.FindElement(By.Name("manufacturer_id"));
            //create select element object 
            var selectElementId = new SelectElement(manufacturerIdElm);
            // select by text
            selectElementId.SelectByText("ACME Corp.");

            // выбираем корпорацию
            driver.FindElmAndSendKeys(By.Name("keywords"), "Duck");
            // Ввводим ключевое слово
            driver.FindElmAndSendKeys(By.Name("short_description[en]"), "Duck");
            // задаем краткое описание
            driver.FindElmAndSendKeys(By.Name("description[en]"), (ProdName + " is cool!") );
            // задаем описание
            driver.FindElmAndSendKeys(By.Name("head_title[en]"), ProdName);
            // задаем заголовок
            driver.FindElmAndSendKeys(By.Name("meta_description[en]"), "666666666");
            // задаем метаописание

            driver.Sleep(sleepTimeMSec);

            driver.FindElmAndClick(By.LinkText("Data"), webDriverType);
            // переходим на вкладку Data

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

            driver.FindElmAndSendKeys(By.Name("sku"), prefix);
            // заполняем поле SKU
            driver.FindElmAndSendKeys(By.Name("gtin"), prefix);
            // заполняем поле GTIN
            driver.FindElmAndSendKeys(By.Name("taric"), prefix);
            // заполняем поле TARIC
            driver.FindElmAndSendKeys(By.Name("weight"), "1");
            // задаем вес
            driver.FindElmAndSendKeys(By.Name("dim_x"), "10");
            driver.FindElmAndSendKeys(By.Name("dim_y"), "11");
            driver.FindElmAndSendKeys(By.Name("dim_z"), "12");
            // задаем размеры
            driver.FindElmAndSendKeys(By.Name("attributes[en]"), "None");
            // задаем атрибуты

            driver.Sleep(sleepTimeMSec);

            driver.FindElmAndClick(By.LinkText("Prices"), webDriverType);
            // переходим на вкладку Prices

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

            driver.FindElmAndSendKeys(By.Name("purchase_price"), "13");
            // задаем цену

         
            // select the drop down list
            var currencyCodeElm = driver.FindElement(By.Name("purchase_price_currency_code"));
            //create select element object 
            var selectElementCurr = new SelectElement(currencyCodeElm);
            // select by text
            selectElementCurr.SelectByText("Euros");
            // выбираем валюту

            driver.FindElmAndSendKeys(By.Name("gross_prices[USD]"), "20");
            // задаем цену в долларах

            driver.Sleep(sleepTimeMSec);

            driver.FindElmAndClick(By.Name("save"), webDriverType);
            // сохраняем продукт

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

            // Проверяем наличие такого элемента на странице
            driver.FindElement(By.LinkText(ProdName));
        }

    }
}
