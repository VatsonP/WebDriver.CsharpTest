using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Firefox;
using System.Reflection;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Events;
using NUnit.Framework.Interfaces;


namespace CsharpTestProject1
{
    [TestFixture]
    public class DriverBase
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
            private set
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
            private set
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
        public string CurrentTestName { get; protected set; }
        public string CurrentTestFolder { get; protected set; }
        protected DirectoryInfo BaseLogFolder { get; set; }


        protected static string localHostStr = "localhost";    // LocalHost  
        protected static string localIpStr = "192.168.0.101"; // Local Host Ip 
        protected static string remoteIpStr = "192.168.203.128";

        // Remote WinServer2019 with Docker "192.168.0.91"
        // Remote Ubuntu 20.4   with Docker "192.168.203.128"
        public static string CurrentIpStr
        {
            get;
            private set;
        }

        // значение времени (в сек) общих неявных ожиданий, для явных ожиданий, ля максимального времени неявного ожидания
        protected DriverBaseParams driverBaseParams;

        public DriverBase(DriverBaseParams driverBaseParams)
        {
            this.driverBaseParams = driverBaseParams;
        }

        public enum TestRunType { Local, Remote };
        public TestRunType testRunType
        {
            get;
            private set;
        }

        public WebDriverExtensions.WebDriverType webDriverType
        {
            get;
            private set;
        }

        protected ICapabilities wdCapabilities
        {   //must be initilize after the WebDriver create
            get;
            private set;
        }

        protected ILogs wdLogs
        {   //must be initilize after the WebDriver create
            get;
            private set;
        }


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
        private void saveLogOnAppEnd(string currentTestName, string eventName, string eventMessage)
        {
            // for write log file with Browser logging
            DirectoryInfo baseLogFolder = createBaseLogDir(TestContext.CurrentContext.TestDirectory);

            LogWriter lw = new LogWriter(baseLogFolder, currentTestName);

            lw.LogWrite("eventName", eventMessage);
        }


        private void initWebDriverCapabilities(IWebDriver webDriver)
        {
            webDriver.Manage().Cookies.DeleteAllCookies();
            wdCapabilities = getWebDriverCapabilities(webDriver);
            driverCapabilitiesPrint(webDriver, wdCapabilities);//TODO
        }

        private void initLogWriterAndWait(IWebDriver webDriver)
        {
            wdLogs = webDriver.Manage().Logs;

            // for write log file on error
            CurrentTestName = TestContext.CurrentContext.Test.Name;
            CurrentTestFolder = TestContext.CurrentContext.TestDirectory;

            Console.WriteLine("Starting test: " + CurrentTestName);

            BaseLogFolder = createBaseLogDir(CurrentTestFolder);
            logWriter = new LogWriter(BaseLogFolder, CurrentTestName);

            // Для установки общих неявных ожиданий
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(driverBaseParams.drvImplWaitTime);

            // Для задания явных ожиданий
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));
        }

        private String defineCurrentIpStr(TestRunType tRunType)
        {
            if (testRunType == TestRunType.Local)
                return localHostStr;
            else
            if (testRunType == TestRunType.Remote)
                return localIpStr;
            else
                return localIpStr;
        }

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            tlDriver   = new ThreadLocal<IWebDriver>(true);
            eftlDriver = new ThreadLocal<EventFiringWebDriver>(true);
        }

        [SetUp]
        public void SetUp()
        {
            testRunType   = TestRunType.Local;
            webDriverType = WebDriverExtensions.WebDriverType.Chrome;

            CurrentIpStr = defineCurrentIpStr(testRunType);

            if (tlDriverIsValueCreated() & eftlDriverIsValueCreated())
            {
                initWebDriverCapabilities(webDrv);
                initLogWriterAndWait(webDrv);
                return;
            }

            if (testRunType == TestRunType.Local)
            {
                webDrv = newDriverSetOptions(webDriverType);
            }
            else
            if (testRunType == TestRunType.Remote)
            {   //RemoteWebDriver
                var uriString = "http://" + remoteIpStr + ":4444/wd/hub/";
                webDrv = newRemoteWebDriverSetOptions(remoteAddress : new Uri(uriString), driverType : webDriverType);
            }

            // Создаем обертку класса WebDriver для последующего сохранения  логов
            //A wrapper around an arbitrary IWebDriver instance which supports registering for events, e.g. for logging purposes. 
            driver = new EventFiringWebDriver(webDrv);

            driver.FindingElement += (sender, e) => logWriter.LogWrite("FindingElement",
                                                                            WebDriverExtensions.linkCurMethodMessage(MethodInfo.GetCurrentMethod(),
                                                                                                                    e.GetType(),
                                                                                                                    String.Concat(e.FindMethod, " processing")));
            driver.FindElementCompleted += (sender, e) => logWriter.LogWrite("FindElementCompleted",
                                                                            WebDriverExtensions.linkCurMethodMessage(MethodInfo.GetCurrentMethod(),
                                                                                                                    e.GetType(),
                                                                                                                    String.Concat(e.FindMethod, " is found")));
            driver.ExceptionThrown += (sender, e) => logWriter.LogWrite("ExceptionThrown",
                                                                            WebDriverExtensions.linkCurMethodMessage(MethodInfo.GetCurrentMethod(),
                                                                                                                    e.GetType(),
                                                                                                                    e.ThrownException.Message));
            /*
            setHookAppDomainEvents(AppDomain.CurrentDomain);
            */
        }


        [TearDown]
        public void TearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;
            var testMessage = "Stop() - OK";
            if (Equals(testResult, ResultState.Failure) ||
                Equals(testResult == ResultState.Error))
            {
                testMessage = "Stop() - ResultState.Failure or ResultState.Error";
            }
            Console.WriteLine(testMessage);
            Console.WriteLine("Finish test: " + CurrentTestName);

            saveBrowserLog(testRunType, webDriverType, driver, wdCapabilities, CurrentTestName);
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            // Функционал теоретически может быть (?) перенесен в Hook AppDomain events (domain_ProcessExit) 
            // для выполнения только один раз в конце всех тестов
            driverQuit(driver);

            tlDriverDispose();
            eftlDriverDispose();
        }

        // ---------------------------------------------------------------------------------------------------------------------------

        protected void saveBrowserLog(TestRunType testRunType, WebDriverExtensions.WebDriverType webDriverType,
                                  IWebDriver webDrv, ICapabilities capabilities, string currentTestName)
        {
            if (webDriverType == WebDriverExtensions.WebDriverType.Chrome | webDriverType == WebDriverExtensions.WebDriverType.Firefox)
            {
                Console.WriteLine("testRunType = " + testRunType.ToString());
                Console.WriteLine("driverType  = " + webDriverType.ToString());

                // for write log file with Browser logging
                DirectoryInfo baseLogFolder = createBaseLogDir(TestContext.CurrentContext.TestDirectory);

                LogWriter lw = new LogWriter(baseLogFolder, currentTestName);
                lw.LogWrite("currentTestName", currentTestName);

                lw.LogWrite("Capabilities", capabilities.ToString());

                lw.LogWrite("testRunType", testRunType.ToString());
                lw.LogWrite("driverType", webDriverType.ToString());

                saveCurLogs(LogType.Browser, lw);

                lw.FinalLogWrite();
            }
        }

        protected void saveCurLogs(string logType, LogWriter lw)
        {
            try
            {
                if (wdLogs != null)
                {
                    var browserLogs = wdLogs.GetLog(logType);
                    if (browserLogs.Count > 0)
                    {
                        foreach (var log in browserLogs)
                        {
                            lw.LogWrite(logType, log.Message);
                        }
                    }
                }
            }
            catch
            {
                //There are no log types present
            }
        }

        protected void saveAllCurLogs(LogWriter lw)
        {
            saveCurLogs(LogType.Server, lw);
            saveCurLogs(LogType.Browser, lw);
            saveCurLogs(LogType.Client, lw);
            saveCurLogs(LogType.Driver, lw);
            saveCurLogs(LogType.Profiler, lw);
        }

        protected DirectoryInfo createBaseLogDir(string currentTestFolder, string newSubFolder = "Log")
        {
            return Directory.CreateDirectory(Path.Combine(currentTestFolder, newSubFolder));
        }


        protected IWebDriver newDriverSetOptions(WebDriverExtensions.WebDriverType driverType)
        {
            IWebDriver webDriver;

            webDriverType = driverType;

            switch (driverType)
            {
                case WebDriverExtensions.WebDriverType.IE:
                    webDriver = new InternetExplorerDriver(getIEOptions());
                    break;

                case WebDriverExtensions.WebDriverType.Chrome:
                    webDriver = new ChromeDriver(chromeDriverDirectory: "C:\\Tools", options: getChromeOptions());
                    //webDriver.Navigate().GoToUrl("chrome://settings/clearBrowserData");
                    //webDriver.FindElmAndSendKeys(By.XPath("//settings-ui"), Keys.Return);
                    break;

                case WebDriverExtensions.WebDriverType.Firefox:
                    webDriver = new FirefoxDriver(geckoDriverDirectory: "C:\\Tools", options: getFirefoxOptions());
                    break;

                default:
                    webDriver = new InternetExplorerDriver(getIEOptions());
                    webDriverType = WebDriverExtensions.WebDriverType.IE;
                    break;
            }
            
            initWebDriverCapabilities(webDriver);
            initLogWriterAndWait(webDriver);
            return webDriver;
        }

        private InternetExplorerOptions getIEOptions()
        {
            InternetExplorerOptions ieOptions = new InternetExplorerOptions();
            var capabilityName = "unexpectedAlertBehavior";
            ieOptions.AddAdditionalCapability(capabilityName, "ignore");
            // Для задания опции UnhandledPromptBehavior
            ieOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
            //установка опций для игнорировния отличия масштаба от 100%
            ieOptions.IgnoreZoomLevel = true;
            //установка опций для игнорировния отличия настройки защищенного режима в разных зонах (не надежная работа)
            //ieOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            return ieOptions;
        }


        private ChromeOptions getChromeOptions()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            // Для задания опции UnhandledPromptBehavior
            chromeOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;
            chromeOptions.AddArgument("--lang=ru");
            // Для задания опции расположения EXE
            chromeOptions.BinaryLocation = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\Chrome.exe";
            //--Задаем опции коммандной строки соотв. браузера
            //chromeOptions.AddArguments("start-fullscreen");
            //Use custom profile(also called user data directory)
            chromeOptions.AddArguments("user-data-dir=c:\\Users\\AdminVadim\\AppData\\Local\\Google\\Chrome\\User Data");

            return chromeOptions;
        }


        private FirefoxOptions getFirefoxOptions()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            var capabilityName = "acceptInsecureCerts";
            // Для задания опции acceptInsecureCerts
            firefoxOptions.SetPreference(capabilityName, false);
            // Для задания опции расположения EXE
            firefoxOptions.BrowserExecutableLocation = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";
            //--Задаем опции коммандной строки соотв. браузера
            firefoxOptions.AddArguments("-private-window");
            //установка профиля пользователя для запуска браузера (копирует указанный профиль во временный для работы)
            //FirefoxProfile firefoxProfile = new FirefoxProfile("C:\\Users\\AdminVadim\\AppData\\Local\\Mozilla\\Firefox\\Profiles\\ltebh6bi.default");
            //firefoxOptions.Profile = firefoxProfile;
            return firefoxOptions;
        }

        protected IWebDriver newRemoteWebDriverSetOptions(Uri remoteAddress, WebDriverExtensions.WebDriverType driverType)
        {
            IWebDriver webDriver;

            webDriverType = driverType;

            switch (driverType)
            {
                case WebDriverExtensions.WebDriverType.IE:
                    webDriver = new RemoteWebDriver(remoteAddress, getRemoteIEOptions());
                    break;

                case WebDriverExtensions.WebDriverType.Chrome:
                    webDriver = new RemoteWebDriver(remoteAddress, getRemoteChromeOptions());
                    //webDriver.Navigate().GoToUrl("chrome://settings/clearBrowserData");
                    //webDriver.FindElmAndSendKeys(By.XPath("//settings-ui"), Keys.Return);
                    break;

                case WebDriverExtensions.WebDriverType.Firefox:
                    webDriver = new RemoteWebDriver(remoteAddress, getRemoteFirefoxOptions());
                    break;

                default:
                    webDriver = new RemoteWebDriver(remoteAddress, getIEOptions());
                    webDriverType = WebDriverExtensions.WebDriverType.IE;
                    break;
            }

            initWebDriverCapabilities(webDriver);
            initLogWriterAndWait(webDriver);
            return webDriver;
        }

        private InternetExplorerOptions getRemoteIEOptions()
        {
            InternetExplorerOptions ieOptions = new InternetExplorerOptions();
            var capabilityName = "platform";
            ieOptions.AddAdditionalCapability(capabilityName, new Platform(PlatformType.Vista)); ;

            // Для задания опции UnhandledPromptBehavior
            ieOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
            //установка опций для игнорировния отличия масштаба от 100%
            ieOptions.IgnoreZoomLevel = true;
            //установка опций для игнорировния отличия настройки защищенного режима в разных зонах (не надежная работа)
            //ieOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            return ieOptions;
        }


        private ChromeOptions getRemoteChromeOptions()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.BrowserVersion = "85.0";

            var runName = GetType().Assembly.GetName().Name;
            var timestamp = $"{DateTime.Now:yyyyMMdd.HHmm}";
            chromeOptions.AddAdditionalCapability("name", runName, true);
            chromeOptions.AddAdditionalCapability("videoName", $"{runName}.{timestamp}.mp4", true);
            //chromeOptions.AddAdditionalCapability("logName", $"{runName}.{timestamp}.log", true);
            chromeOptions.AddAdditionalCapability("enableVNC", true, true);
            chromeOptions.AddAdditionalCapability("enableVideo", true, true);
            chromeOptions.AddAdditionalCapability("videoScreenSize", "1280x720", true);
            //chromeOptions.AddAdditionalCapability("enableLog", true, true);
            chromeOptions.AddAdditionalCapability("screenResolution", "1920x1080x24", true);

            // Для задания опции UnhandledPromptBehavior
            chromeOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;
            chromeOptions.AddArgument("--lang=ru");
            // Для задания опции расположения EXE
            //chromeOptions.BinaryLocation = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\Chrome.exe";
            //--Задаем опции коммандной строки соотв. браузера
            //chromeOptions.AddArguments("start-fullscreen");
            //Use custom profile(also called user data directory)
            //chromeOptions.AddArguments("user-data-dir=c:\\Users\\AdminVadim\\AppData\\Local\\Google\\Chrome\\User Data");

            return chromeOptions;
        }


        private FirefoxOptions getRemoteFirefoxOptions()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.BrowserVersion = "80.0";

            var runName = GetType().Assembly.GetName().Name;
            var timestamp = $"{DateTime.Now:yyyyMMdd.HHmm}";
            firefoxOptions.AddAdditionalCapability("name", runName, true);
            firefoxOptions.AddAdditionalCapability("videoName", $"{runName}.{timestamp}.mp4", true);
            //firefoxOptions.AddAdditionalCapability("logName", $"{runName}.{timestamp}.log", true);
            firefoxOptions.AddAdditionalCapability("enableVNC", true, true);
            firefoxOptions.AddAdditionalCapability("enableVideo", true, true);
            firefoxOptions.AddAdditionalCapability("videoScreenSize", "1280x720", true);
            //firefoxOptions.AddAdditionalCapability("enableLog", true, true);
            firefoxOptions.AddAdditionalCapability("screenResolution", "1920x1080x24", true);

            // Для задания опции acceptInsecureCerts
            var preferenceName = "acceptInsecureCerts";
            firefoxOptions.SetPreference(preferenceName, false);
            // Для задания опции расположения EXE
            //firefoxOptions.BrowserExecutableLocation = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";            
            //--Задаем опции коммандной строки соотв. браузера
            //firefoxOptions.AddArguments("-private-window");
            //установка профиля пользователя для запуска браузера (копирует указанный профиль во временный для работы)
            //FirefoxProfile firefoxProfile = new FirefoxProfile("C:\\Users\\AdminVadim\\AppData\\Local\\Mozilla\\Firefox\\Profiles\\ltebh6bi.default");
            //firefoxOptions.Profile = firefoxProfile;
            return firefoxOptions;
        }

        protected void LoginAs(IWebDriver webDriver, Uri remoteUri, WebDriverExtensions.WebDriverType driverType,
                               By usrLocator, string usrText, By passLocator, string passText, By loginLocator)
        {
            webDriver.Navigate().GoToUrl(remoteUri); //открыть страницу

            if (webDriver.isElementPresent(usrLocator))
                webDriver.FindElmAndSendKeys(usrLocator, usrText); //найти поле для ввода логина и ввести "admin"
            if (webDriver.isElementPresent(passLocator))
                webDriver.FindElmAndSendKeys(passLocator, passText); //найти поле для ввода пароля и ввести "admin"
            if (webDriver.isElementPresent(loginLocator))
                webDriver.FindElmAndClick(loginLocator, driverType); //найти кнопку логина и нажать на нее
        }

        // -------------------------------------------------------------------------------------
        //TODO
        protected static void driverCapabilitiesPrint(IWebDriver webDriver, ICapabilities capabilities)
        {
            Console.WriteLine("---------- printDriverCapabilities --------------------");
            if (capabilities != null)
                Console.WriteLine(capabilities.ToString());

            Console.WriteLine("---------- end print  ---------------------------------");
        }
        // -------------------------------------------------------------------------------------

        protected static ICapabilities getWebDriverCapabilities(IWebDriver webDriver)
        { 
            return  ((IHasCapabilities)webDriver).Capabilities;
        }

        [Category("Functional"), Category("Release")]
        protected void CategoriesPropertiesTestPrint()
        {
            foreach (var category in TestContext.CurrentContext.Test.Properties["Category"])
            {
                Console.WriteLine(category);
            }
        }

        public void TakeScreenshot(String fileNameWithoutExt)
        {
            driver.TakeScreenshot(CurrentTestFolder, CurrentTestName, fileNameWithoutExt);
        }

    }

}
