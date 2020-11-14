using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;


namespace CsharpTestProject1
{
    [TestFixture]
    /*Parallelizable(ParallelScope.Children)*/
    public class DemoTests : DriverBase
    {
        private const int sleepTimeMSec = 2000;

        public DemoTests() : base(new DriverBaseParams()) { }

        [Test]
        public void FirstTest()
        {
            driver.Navigate().GoToUrl("http://www.google.com/");
            IWebElement qElement = driver.FindElement(By.Name("q"));
            qElement.SendKeys("webdriver");
            qElement.SendKeys(Keys.Return);

            CategoriesPropertiesTestPrint();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("webdriver"));

            driver.Sleep(sleepTimeMSec);
        }

        [Test]
        public void SecondTest() 
        {
            //XAMPP litecart - "http://" + CurrentIpStr + ":8080/litecart/en/"
            driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/en/");

            IWebElement Element1 = driver.FindElement(By.CssSelector("div[class='input-wrapper'] [type='search']"));
            Element1.SendKeys("duck");
            Element1.SendKeys(Keys.Return);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("duck"));

            driver.Sleep(sleepTimeMSec);
        }

        [Test]
        public void ThirdTest()
        {
            driver.Navigate().GoToUrl("http://www.google.com/");
            string searchingElementName = "q-q-q";

            //Пример исключения InvalidSelectorException - eсли передан неправильный локатор
            //DriverBase.isElementPresent(driver, By.CssSelector("1 2 3 notValidCss"), isWait: true);
            //Пример исключения NoSuchElementException - eсли элемент отсутствует в DOM на момент вызова
            //DriverBase.isElementPresent(driver, By.Name(searchingElementName), isWait: true);

            if (driver.isElementPresent(By.Name(searchingElementName), isWait: true) )
            {
                IWebElement qElement = driver.FindElement(By.Name(searchingElementName));
                qElement.SendKeys("webdriver");
                qElement.SendKeys(Keys.Return);

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("webdriver"));
            }

            driver.Sleep(sleepTimeMSec);
        }

        [Test]
        public void FourthTest()
        {
            //XAMPP litecart - "http://" + CurrentIpStr + ":8080/litecart/en/"
            driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/en/");
            IWebElement Element1 = driver.FindElement(By.CssSelector("div[class='input-wrapper'] [type='search']"));
            Element1.SendKeys("duck");
            // метод-свойство Displayed
            Console.WriteLine("Element1.Displayed =" + Element1.Displayed);

            // использование функции GetAttribute() 
            Element1.checkAndPrintAttributeByName("value");
            Element1.checkAndPrintAttributeByName("placeholder");
            // атрибуты(свойства) типа boolean возвращают true (при их наличии) и null (при их отсутствии)
            Element1.checkAndPrintAttributeByName("spellcheck");
            Element1.checkAndPrintAttributeByName("draggable");
            Element1.checkAndPrintAttributeByName("a-a-a-a-a");

            Element1.SendKeys(Keys.Return);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("duck"));

            driver.Sleep(sleepTimeMSec);
        }

        [Test]
        public void FifthTest()
        {
            //XAMPP litecart admin page - "http://" + CurrentIpStr + ":8080/litecart/admin/?app=customers&doc=customers"
            var remoteAddress = "http://" + CurrentIpStr + ":8080/litecart/admin/?app=customers&doc=customers"; //открыть страницу

            LoginAs(webDriver   : driver, remoteUri   : new Uri(remoteAddress), driverType: webDriverType,
                    usrLocator  : By.Name("username"),  usrText: "admin",
                    passLocator : By.Name("password"), passText: "admin",
                    loginLocator: By.Name("login")
                   );

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("My Store"));
            //подождать пока не загрузится страница с заголовком "My Store"

            driver.Sleep(sleepTimeMSec);
        }

        [Test]
        public void SeventhTest_myCheckStiker()
        {
            int prodQuantity, prodStickerQuantity;
            int allProdWithStickerQuantity = 0;
            IWebElement productUnit;
            IList<IWebElement> prodList, stickerList;

            //XAMPP litecart - "http://" + CurrentIpStr + ":8080/litecart/en/"
            driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/en/");

            driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/en/rubber-ducks-c-1/"); 
            //открыть страницу магазина с товарами

            prodList = driver.FindElements(By.CssSelector("li.product"));
            // определение списка товаров на главной странице
            prodQuantity = prodList.Count; // сохраняем количество товаров

            Console.WriteLine("prodQuantity: " + prodQuantity);

            for (int i = 0; i < prodQuantity; i++)
            {  //проходим по списку товаров
                prodList = driver.FindElements(By.CssSelector("li.product"));
                productUnit = prodList[i];

                //определение списка стикеров (полосок) у товара

                stickerList = productUnit.FindElements(By.CssSelector("div.sticker"));
                //определение количества стикеров у товара
                prodStickerQuantity = stickerList.Count;

                Console.WriteLine("prodNum (i): " + i);
                Console.WriteLine("prodStickerQuantity: " + prodStickerQuantity);

                //проверка что у товара не более одного стикера
                Assert.IsTrue(prodStickerQuantity <= 1);

                if (prodStickerQuantity == 1)
                    allProdWithStickerQuantity = allProdWithStickerQuantity + 1;
            }

            Console.WriteLine("---------------------------------");
            Console.WriteLine("allProdWithStickerQuantity: " + allProdWithStickerQuantity);

        }

    }
}
