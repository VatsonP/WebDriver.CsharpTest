using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Events;
using System.Diagnostics;


namespace CsharpWebDriverLib.DriverBase
{
    public enum TestRunType { Local, RemoteWin, RemoteUbuntu };

    public interface IDriverBaseCore
    {
        public void SetUp();

        public void TearDown();

        public void TakeScreenshot(String fileNameWithoutExt);
    }

    public interface IDriverBase : IDriverBaseCore
    {
        //Для возможности повторного использования драйвера - инициализация хранилища драйверов tlDrver <IWebDriver>
        private static ThreadLocal<IWebDriver> _tlDriver;// = new ThreadLocal<IWebDriver>(true);
        private static ThreadLocal<IWebDriver> tlDriverCreate()
        {
            return new ThreadLocal<IWebDriver>(true);
        }
        private static void tlDriverDispose()
        {
            _tlDriver?.Value?.Dispose();
            _tlDriver?.Dispose();
        }
        protected static Boolean tlDriverIsValueCreated()
        {
            return _tlDriver.IsValueCreated;
        }
        protected static IWebDriver webDrv
        {
            get
            {
                if (!_tlDriver.IsValueCreated)
                    throw new ArgumentNullException("Driver <IWebDriver> not initialized!");
                return (IWebDriver)_tlDriver.Value;
            }
            set
            {
                if (!_tlDriver.IsValueCreated)
                    _tlDriver.Value = (IWebDriver)value;
            }
        }

        //Для возможности повторного использования драйвера - инициализация хранилища драйверов _eftlDriver <EventFiringWebDriver>
        private static ThreadLocal<EventFiringWebDriver> _eftlDriver;// = new ThreadLocal<EventFiringWebDriver>(true);
        private static ThreadLocal<EventFiringWebDriver> eftlDriverCreate()
        {
            return new ThreadLocal<EventFiringWebDriver>(true);
        }
        private static void eftlDriverDispose()
        {
            _eftlDriver?.Value?.Dispose();
            _eftlDriver?.Dispose();
        }
        protected static Boolean eftlDriverIsValueCreated()
        {
            return _eftlDriver.IsValueCreated;
        }

        public static EventFiringWebDriver driver
        {
            get
            {
                if (!_eftlDriver.IsValueCreated)
                    throw new ArgumentNullException("Driver <EventFiringWebDriver> not initialized!");
                return _eftlDriver.Value;
            }
            set
            {
                if (!_eftlDriver.IsValueCreated)
                    _eftlDriver.Value = value;
            }
        }

        private static void driverQuit(IWebDriver d)
        {
            d.Quit();
            d.Dispose();
        }

        private static void eventFiringCreate()
        {
            _tlDriver = tlDriverCreate();
            _eftlDriver = eftlDriverCreate();
        }

        private static void eventFiringDispose()
        {
            tlDriverDispose();
            eftlDriverDispose();
        }
        protected static Process selenoidProcess { get; set; }

        public static WebDriverWait wait
        {
            get;
            protected set;
        }

        public static string CurrentIpStr { get; set; }

        public static TestRunType testRunType { get; set; }

        public static WebDriverExtensions.WebDriverType webDriverType { get; set; }


        public static void BeforeOneTimeSetUp()
        {
            eventFiringCreate();
        }

        public static void BeforeOneTimeTearDown()
        {
            // Функционал теоретически может быть (?) перенесен в Hook AppDomain events (domain_ProcessExit) 
            // для выполнения только один раз в конце всех тестов
            driverQuit(driver);
            eventFiringDispose();
        }

        // ---------------------------------------------------------------------------------------------------------------------------

        public static void OneTimeTearDown()
        {
            TerminateLocalSeleniumBrowsersServer(IDriverBase.testRunType, IDriverBase.webDriverType);
            TerminateLocalSelenoidServerForIE(IDriverBase.testRunType, IDriverBase.webDriverType);
        }

        private static void TerminateLocalSeleniumBrowsersServer(TestRunType testRunType,
                                                          WebDriverExtensions.WebDriverType driverType)
        {
            Process[] processes;
            // Terminate the associated Selenium driver Windows process from memory
            switch (driverType)
            {
                case WebDriverExtensions.WebDriverType.IE:
                    processes = Process.GetProcessesByName(DriverBaseParams.ieDriverExeName);
                    foreach (Process p in processes)
                        p.Kill();

                    break;

                case WebDriverExtensions.WebDriverType.Chrome:
                    if (testRunType == TestRunType.Local)
                    {
                        processes = Process.GetProcessesByName(DriverBaseParams.chromeDriverExeName);
                        foreach (Process p in processes)
                            p.Kill();
                    }
                    break;

                case WebDriverExtensions.WebDriverType.Firefox:
                    if (testRunType == TestRunType.Local)
                    {
                        processes = Process.GetProcessesByName(DriverBaseParams.firefoxDriverExeName);
                        foreach (Process p in processes)
                            p.Kill();
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Not valid WebDriverType value: " + driverType);
            }
        }

        private static void TerminateLocalSelenoidServerForIE(TestRunType testRunType,
                                                       WebDriverExtensions.WebDriverType driverType)
        {
            if ((testRunType != TestRunType.Local) &
                (driverType == WebDriverExtensions.WebDriverType.IE) &
                (selenoidProcess != null)
               )
            {
                // Stop the Selenoid.exe process
                selenoidProcess.CloseMainWindow();
                if (!selenoidProcess.HasExited)
                {
                    selenoidProcess.Kill();
                }
                selenoidProcess.Dispose();

                // Terminate the associated Windows process from memory
                Process[] processes = Process.GetProcessesByName(DriverBaseParams.selenoidDriverExeName);
                foreach (Process p in processes)
                {
                    p.Kill();
                }
            }
        }

        // ---------------------------------------------------------------------------------------------------------------------------

        public static void ClickElement(IWebElement element) => element.ClickElement(webDriverType);

        public static void FindElmAndClick(By locator) => driver.FindElmAndClick(locator, webDriverType);

        public static string GetFullDateStrForBrowserDateControl(int yyyy, int mm, int dd) =>
            driver.GetFullDateStrForBrowserDateControl(yyyy, mm, dd, webDriverType);
    }

}
