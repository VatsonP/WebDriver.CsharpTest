using System;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Events;


namespace CsharpTestProject1
{

    public abstract class DriverBase
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

        /*
        protected static AppDomain appDomain;
        */
        public static WebDriverWait wait
        {
            get;
            protected set;
        }

        protected LogWriter logWriter;

        // for write log file on error
        protected string CurrentTestName { get; set; }
        protected string CurrentTestFolder { get; set; }
        protected DirectoryInfo BaseLogFolder { get; set; }

        public static string CurrentIpStr { get; set; }

        // значение времени (в сек) общих неявных ожиданий, для явных ожиданий, для максимального времени неявного ожидания
        protected DriverBaseParams driverBaseParams { get; set; }

        public DriverBase(DriverBaseParams driverBaseParams)
        {
            this.driverBaseParams = driverBaseParams;
        }


        protected enum TestRunType { Local, Remote };

        protected static TestRunType testRunType { get; set; }

        public static WebDriverExtensions.WebDriverType webDriverType { get; set; }

        //must be initilize after the WebDriver create
        protected ICapabilities wdCapabilities { get; set; }

        //must be initilize after the WebDriver create
        protected ILogs wdLogs { get; set; }


        /*
        //Hook AppDomain events for attach an event handler to the current application domain's events:
        private void setHookAppDomainEvents(AppDomain aDom)
        {
            appDomain = aDom;
            appDomain.UnhandledException += new UnhandledExceptionEventHandler(domain_UnhandledExceptionHandler);
            appDomain.ProcessExit += new EventHandler(domain_ProcessExit);
            appDomain.DomainUnload += new EventHandler(domain_DomainUnload);
        }
        private void domain_UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Console.WriteLine("domain_UnhandledExceptionHandler caught: " + e.Message);

            logWriter.LogWrite("domain_UnhandledExceptionHandler",
                                    WebDriverExtensions.linkCurMethodMessage(MethodInfo.GetCurrentMethod(),
                                                                            e.GetType(),
                                                                            e.Message));
        }
        private void domain_ProcessExit(object sender, EventArgs e)
        {
            // Функционал из [TearDown] Stop() ... может быть закоментирован и заменен на этот (Hook AppDomain events),
            // для выполнения только один раз в конце всех тестов
            var eventName = e.ToString();
            var msg = "The End of Test";
            Console.WriteLine(msg+ " "+ eventName);
            saveLogOnAppEnd(currentTestName: "ProcessExit", eventName: eventName, eventMessage: msg);
        }
        
        private void domain_DomainUnload(object sender, EventArgs e)
        {
            webDrvQuit();
            driverQuit();
            // Функционал из [TearDown] Stop() ... может быть закоментирован и заменен на этот (Hook AppDomain events),
            // для выполнения только один раз в конце всех тестов
            var eventName = e.ToString();
            var msg = "The END of All Tests";
            Console.WriteLine(msg + " " + eventName);
            saveLogOnAppEnd(currentTestName: "DomainUnload", eventName: eventName, eventMessage: msg);
        }
        */

        public static void OneTimeSetUp()
        {
            tlDriver   = new ThreadLocal<IWebDriver>(true);
            eftlDriver = new ThreadLocal<EventFiringWebDriver>(true);
        }

        public abstract void SetUp();

        public abstract void TearDown();

        public static void OneTimeTearDown()
        {
            // Функционал теоретически может быть (?) перенесен в Hook AppDomain events (domain_ProcessExit) 
            // для выполнения только один раз в конце всех тестов
            driverQuit(driver);

            tlDriverDispose();
            eftlDriverDispose();
        }

        // ---------------------------------------------------------------------------------------------------------------------------

        public void TakeScreenshot(String fileNameWithoutExt)
        {
            driver.TakeScreenshot(CurrentTestFolder, CurrentTestName, fileNameWithoutExt);
        }

        public static void ClickElement(IWebElement element) => element.ClickElement(webDriverType);

        public static void FindElmAndClick(By locator) => driver.FindElmAndClick(locator, webDriverType);

        public static string GetFullDateStrForBrowserDateControl(int yyyy, int mm, int dd)
        {
            return driver.GetFullDateStrForBrowserDateControl(yyyy, mm, dd, webDriverType);
        }

    }

}
