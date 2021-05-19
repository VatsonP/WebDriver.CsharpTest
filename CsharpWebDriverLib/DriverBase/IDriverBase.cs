using System;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Events;


namespace CsharpWebDriverLib
{
    public interface IDriverBase
    {
        //Для возможности повторного использования драйвера - инициализация хранилища драйверов tlDrver <IWebDriver>
        private static ThreadLocal<IWebDriver> tlDriver;// = new ThreadLocal<IWebDriver>(true);
        private ThreadLocal<IWebDriver> tlDriverCreate()
        {
            return new ThreadLocal<IWebDriver>(true);
        }
        private static void tlDriverDispose()
        {
            tlDriver?.Dispose();
        }
        protected static Boolean tlDriverIsValueCreated()
        {
            return tlDriver.IsValueCreated;
        }
        protected static IWebDriver webDrv
        {
            get
            {
                if (!tlDriver.IsValueCreated)
                    throw new ArgumentNullException("Драйвер <IWebDriver> не проинициализирован!");
                return (IWebDriver)tlDriver.Value;
            }
            set
            {
                if (!tlDriver.IsValueCreated)
                    tlDriver.Value = (IWebDriver)value;
            }
        }

        //Для возможности повторного использования драйвера - инициализация хранилища драйверов eftlDriver <EventFiringWebDriver>
        private static ThreadLocal<EventFiringWebDriver> eftlDriver;// = new ThreadLocal<EventFiringWebDriver>(true);
        private ThreadLocal<EventFiringWebDriver> eftlDriverCreate()
        {
            return new ThreadLocal<EventFiringWebDriver>(true);
        }
        private static void eftlDriverDispose()
        {
            eftlDriver?.Dispose();
        }
        protected static Boolean eftlDriverIsValueCreated()
        {
            return eftlDriver.IsValueCreated;
        }

        public static EventFiringWebDriver driver
        {
            get
            {
                if (!eftlDriver.IsValueCreated)
                    throw new ArgumentNullException("Драйвер <EventFiringWebDriver> не проинициализирован!");
                return eftlDriver.Value;
            }
            set
            {
                if (!eftlDriver.IsValueCreated)
                    eftlDriver.Value = value;
            }
        }
        private static void driverQuit(IWebDriver d)
        {
            d.Quit();
            d.Dispose();
        }

        public static WebDriverWait wait
        {
            get;
            protected set;
        }

        public static string CurrentIpStr { get; set; }
        public static WebDriverExtensions.WebDriverType webDriverType { get; set; }

        /*

        // for write log file on error
        protected string CurrentTestName { get; set; }
        protected string CurrentTestFolder { get; set; }
        protected DirectoryInfo BaseLogFolder { get; set; }


        // значение времени (в сек) общих неявных ожиданий, для явных ожиданий, для максимального времени неявного ожидания
        protected DriverBaseParams driverBaseParams { get; set; }


        protected enum TestRunType { Local, Remote };

        protected static TestRunType testRunType { get; set; }

        //must be initilize after the WebDriver create
        protected ICapabilities wdCapabilities { get; set; }

        //must be initilize after the WebDriver create
        protected ILogs wdLogs { get; set; }
        */

        public static void OneTimeSetUp()
        {
            tlDriver = new ThreadLocal<IWebDriver>(true);
            eftlDriver = new ThreadLocal<EventFiringWebDriver>(true);
        }

        public void SetUp();

        public void TearDown();

        public static void OneTimeTearDown()
        {
            // Функционал теоретически может быть (?) перенесен в Hook AppDomain events (domain_ProcessExit) 
            // для выполнения только один раз в конце всех тестов
            driverQuit(driver);

            tlDriverDispose();
            eftlDriverDispose();
        }


        // ---------------------------------------------------------------------------------------------------------------------------

        public void TakeScreenshot(String fileNameWithoutExt);

        public static void ClickElement(IWebElement element) => element.ClickElement(webDriverType);

        public static void FindElmAndClick(By locator) => driver.FindElmAndClick(locator, webDriverType);

        public static string GetFullDateStrForBrowserDateControl(int yyyy, int mm, int dd) =>
            driver.GetFullDateStrForBrowserDateControl(yyyy, mm, dd, webDriverType);


        public static IDriverBase CreateDriverBase(DriverBaseParams driverBaseParams)
        {
            return new DriverBase(driverBaseParams);
        }
    }

}
