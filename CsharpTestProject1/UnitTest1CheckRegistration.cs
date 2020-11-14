using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace CsharpTestProject1
{

    [TestFixture]
    public class CheckRegistration : DriverBase
    {

        String FirstName, LastName, eMailName, testString, taxId;

        private const int sleepTimeMSec = 5000;

        public CheckRegistration() : base(new DriverBaseParams()) { }

        [Test]
        public void MyCheckRegistration() {
            driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/en/"); //открыть главную страницу магазина

            // находим ссылку регистрации пользователя и щелкаем по ней
            driver.FindElement(By.CssSelector("[href*=create_account]")).Click();

            // читаем текущее время - добавляем его к фамилии и имеем уникальный e-mail и пароль каждый раз
            DateTime curDate = DateTime.Now;
            int yyyy =  curDate.Year;
            int mm =    curDate.Month;
            int dd =    curDate.Day;
            int h =     curDate.Hour;
            int m =     curDate.Minute;
            int s =     curDate.Second;

            FirstName = "Ivan";
            LastName = "Tankist";
            eMailName = FirstName + WebDriverExtensions.PaddingLeft(h) + WebDriverExtensions.PaddingLeft(m) + WebDriverExtensions.PaddingLeft(s);

            taxId = WebDriverExtensions.PaddingLeft(yyyy, 4) + "-" + WebDriverExtensions.PaddingLeft(mm) + "-" + WebDriverExtensions.PaddingLeft(dd) + "_" +
                    h.ToString() + m.ToString() + s.ToString();

            // заполняем форму - только обязательные поля
            driver.FindElmAndSendKeys(By.Name("tax_id"),    taxId);
            driver.FindElmAndSendKeys(By.Name("company"),   "MMM");
            driver.FindElmAndSendKeys(By.Name("firstname"), FirstName);
            driver.FindElmAndSendKeys(By.Name("lastname"),  LastName);
            driver.FindElmAndSendKeys(By.Name("address1"),  "3 Buiders st. 13");
            driver.FindElmAndSendKeys(By.Name("postcode"),  "66666");
            driver.FindElmAndSendKeys(By.Name("city"),      "Kyiv");
            driver.FindElmAndSendKeys(By.Name("email"),     eMailName + "@mail.com");
            driver.FindElmAndSendKeys(By.Name("phone"),     "223-322");
            driver.FindElmAndSendKeys(By.Name("password"),  eMailName);
            driver.FindElmAndSendKeys(By.Name("confirmed_password"), eMailName);

            //var currentTestFolder = TestContext.CurrentContext.TestDirectory;
            //var currentTestName = TestContext.CurrentContext.Test.Name;

            TakeScreenshot("ScreenOne");

            driver.Sleep(sleepTimeMSec);

            // нажимаем на кнопку Create Account
            driver.FindElm(By.Name("create_account")).Click();


            // Проверяем что мы залогинились - на странице должен быть соответствующий раздел
            // который имеет заголовок Account
            testString = driver.FindElm(By.CssSelector("[id=box-account] .title")).Text;

            Assert.AreEqual(testString.CompareTo("Account"), 0);

            // Отлогиниваемся
            driver.FindElm(By.CssSelector("[href*=logout]")).Click();


            // Проверяем что мы отлогинились - на странице должен быть соответствующий раздел
            // который имеет заголовок Login
            testString = driver.FindElm(By.CssSelector("[id=box-account-login] .title")).Text;

            Assert.AreEqual(testString.CompareTo("Login"), 0);

            // Логинимся под созданным пользователем
            driver.FindElmAndSendKeys(By.Name("email"), eMailName + "@mail.com");
            driver.FindElmAndSendKeys(By.Name("password"), eMailName);

            driver.FindElmAndClick(By.Name("login"), webDriverType);

            // Проверяем что мы залогинились - на странице должен быть соответствующий раздел
            // который имеет заголовок Account
            testString = driver.FindElm(By.CssSelector("[id=box-account] .title")).Text;
            Assert.AreEqual(testString.CompareTo("Account"), 0);

            TakeScreenshot("ScreenTwo");

            driver.Sleep(sleepTimeMSec);

            // Отлогиниваемся
            driver.FindElmAndClick(By.CssSelector("[href*=logout]"), webDriverType);

            // Проверяем что мы отлогинились - на странице должен быть соответствующий раздел
            // который имеет заголовок Login
            testString = driver.FindElm(By.CssSelector("[id=box-account-login] .title")).Text;

            Assert.AreEqual(testString.CompareTo("Login"), 0);
        }

    }
}