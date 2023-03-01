using System;
using System.IO;
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


namespace CsharpWebDriverLib.DriverBase
{
    internal class DriverBase : IDriverBase
    {
        internal DriverBase(DriverBaseParams driverBaseParams)
        {
            this.driverBaseParams = driverBaseParams;
        }

        /*
        protected static AppDomain appDomain;
        */

        protected LogWriter logWriter;

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
            IDriverBase.wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));
        }

        private String defineCurrentIpStr(TestRunType tRunType)
        {
            if (testRunType == TestRunType.Local)
                return driverBaseParams.localHostStr;
            else
            if (testRunType == TestRunType.Remote)
                return driverBaseParams.localIpStr;
            else
                return driverBaseParams.localIpStr;
        }

        // ---------------------------------------------------------------------------------------------------------------------------

        public void TakeScreenshot(String fileNameWithoutExt)
        {
            IDriverBase.driver.TakeScreenshot(CurrentTestFolder, CurrentTestName, fileNameWithoutExt);
        }

        public void SetUp()
        {
            testRunType = TestRunType.Local;
            IDriverBase.webDriverType = WebDriverExtensions.WebDriverType.Chrome;

            IDriverBase.CurrentIpStr = defineCurrentIpStr(testRunType);

            if (IDriverBase.tlDriverIsValueCreated() & IDriverBase.eftlDriverIsValueCreated())
            {
                initWebDriverCapabilities(IDriverBase.webDrv);
                initLogWriterAndWait(IDriverBase.webDrv);
                return;
            }

            if (testRunType == TestRunType.Local)
            {
                IDriverBase.webDrv = newDriverSetOptions(IDriverBase.webDriverType);
            }
            else
            if (testRunType == TestRunType.Remote)
            {   //RemoteWebDriver
                var uriString = "http://" + driverBaseParams.remoteIpStr + ":4444/wd/hub/";
                IDriverBase.webDrv = newRemoteWebDriverSetOptions(remoteAddress: new Uri(uriString), driverType: IDriverBase.webDriverType);
            }

            // Создаем обертку класса WebDriver для последующего сохранения  логов
            //A wrapper around an arbitrary IWebDriver instance which supports registering for events, e.g. for logging purposes. 
            IDriverBase.driver = new EventFiringWebDriver(IDriverBase.webDrv);

            IDriverBase.driver.FindingElement += (sender, e) => logWriter.LogWrite("FindingElement",
                                                                            WebDriverExtensions.linkCurMethodMessage(MethodInfo.GetCurrentMethod(),
                                                                                                                    e.GetType(),
                                                                                                                    String.Concat(e.FindMethod, " processing")));
            IDriverBase.driver.FindElementCompleted += (sender, e) => logWriter.LogWrite("FindElementCompleted",
                                                                            WebDriverExtensions.linkCurMethodMessage(MethodInfo.GetCurrentMethod(),
                                                                                                                    e.GetType(),
                                                                                                                    String.Concat(e.FindMethod, " is found")));
            IDriverBase.driver.ExceptionThrown += (sender, e) => logWriter.LogWrite("ExceptionThrown",
                                                                            WebDriverExtensions.linkCurMethodMessage(MethodInfo.GetCurrentMethod(),
                                                                                                                    e.GetType(),
                                                                                                                    e.ThrownException.Message));
            /*
            setHookAppDomainEvents(AppDomain.CurrentDomain);
            */
        }


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

            saveBrowserLog(testRunType, IDriverBase.webDriverType, IDriverBase.driver, wdCapabilities, CurrentTestName);
        }


        // ---------------------------------------------------------------------------------------------------------------------------

        private void saveBrowserLog(TestRunType testRunType, WebDriverExtensions.WebDriverType webDriverType,
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

        private void saveCurLogs(string logType, LogWriter lw)
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

        private void saveAllCurLogs(LogWriter lw)
        {
            saveCurLogs(LogType.Server, lw);
            saveCurLogs(LogType.Browser, lw);
            saveCurLogs(LogType.Client, lw);
            saveCurLogs(LogType.Driver, lw);
            saveCurLogs(LogType.Profiler, lw);
        }

        private DirectoryInfo createBaseLogDir(string currentTestFolder, string newSubFolder = "Log")
        {
            return Directory.CreateDirectory(Path.Combine(currentTestFolder, newSubFolder));
        }


        private IWebDriver newDriverSetOptions(WebDriverExtensions.WebDriverType driverType)
        {
            IWebDriver webDriver;

            IDriverBase.webDriverType = driverType;

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
                    IDriverBase.webDriverType = WebDriverExtensions.WebDriverType.IE;
                    break;
            }

            initWebDriverCapabilities(webDriver);
            initLogWriterAndWait(webDriver);
            return webDriver;
        }

        private InternetExplorerOptions getIEOptions()
        {
            InternetExplorerOptions ieOptions = new InternetExplorerOptions();

            // Для задания опции unexpectedAlertBehavior
            ieOptions.AddAdditionalInternetExplorerOption("unexpectedAlertBehavior", "ignore");

            // Для задания опции UnhandledPromptBehavior
            ieOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;

            //установка опций для игнорирования отличия масштаба от 100%
            ieOptions.IgnoreZoomLevel = true;
            //установка опций для игнорирования отличия настройки защищенного режима в разных зонах (не надежная работа)
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
            //--Задаем опции командной строки соотв. браузера
            firefoxOptions.AddArguments("-private-window");
            //установка профиля пользователя для запуска браузера (копирует указанный профиль во временный для работы)
            //FirefoxProfile firefoxProfile = new FirefoxProfile("C:\\Users\\AdminVadim\\AppData\\Local\\Mozilla\\Firefox\\Profiles\\ltebh6bi.default");
            //firefoxOptions.Profile = firefoxProfile;
            return firefoxOptions;
        }

        private IWebDriver newRemoteWebDriverSetOptions(Uri remoteAddress, WebDriverExtensions.WebDriverType driverType)
        {
            IWebDriver webDriver;

            IDriverBase.webDriverType = driverType;

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
                    IDriverBase.webDriverType = WebDriverExtensions.WebDriverType.IE;
                    break;
            }

            initWebDriverCapabilities(webDriver);
            initLogWriterAndWait(webDriver);
            return webDriver;
        }

        private InternetExplorerOptions getRemoteIEOptions()
        {
            InternetExplorerOptions ieOptions = new InternetExplorerOptions();

            ieOptions.PlatformName = "Windows 7";
            ieOptions.BrowserVersion = "109.0.5414.120";
            
            // Для задания опции UnhandledPromptBehavior
            ieOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
            //установка опций для игнорирования отличия масштаба от 100%
            ieOptions.IgnoreZoomLevel = true;
            //установка опций для игнорирования отличия настройки защищенного режима в разных зонах (не надежная работа)
            //ieOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            return ieOptions;
        }


        private ChromeOptions getRemoteChromeOptions()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.PlatformName = "Windows 7";
            chromeOptions.BrowserVersion = "109.0.5414.120";
            // Для задания опции UnhandledPromptBehavior
            chromeOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;
            chromeOptions.AddArgument("--lang=ru");

            var runName = GetType().Assembly.GetName().Name;
            var timestamp = $"{DateTime.Now:yyyyMMdd.HHmm}";

            chromeOptions.AddAdditionalChromeOption("name", runName);
            chromeOptions.AddAdditionalChromeOption("videoName", $"{runName}.{timestamp}.mp4");
            chromeOptions.AddAdditionalChromeOption("enableVNC", true);
            chromeOptions.AddAdditionalChromeOption("enableVideo", true);
            chromeOptions.AddAdditionalChromeOption("videoScreenSize", "1280x720");
            chromeOptions.AddAdditionalChromeOption("enableLog", true);
            chromeOptions.AddAdditionalChromeOption("screenResolution", "1920x1080x24");

            // Для задания опции расположения EXE
            //chromeOptions.BinaryLocation = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\Chrome.exe";
            //--Задаем опции командной строки соотв. браузера
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

            firefoxOptions.AddAdditionalFirefoxOption("name", runName);
            firefoxOptions.AddAdditionalFirefoxOption("videoName", $"{runName}.{timestamp}.mp4");
            firefoxOptions.AddAdditionalFirefoxOption("enableVNC", true);
            firefoxOptions.AddAdditionalFirefoxOption("enableVideo", true);
            firefoxOptions.AddAdditionalFirefoxOption("videoScreenSize", "1280x720");
            //firefoxOptions.AddAdditionalFirefoxOption("logName", $"{runName}.{timestamp}.log");
            firefoxOptions.AddAdditionalFirefoxOption("enableLog", true);
            firefoxOptions.AddAdditionalFirefoxOption("screenResolution", "1920x1080x24");

            // Для задания опции acceptInsecureCerts
            var preferenceName = "acceptInsecureCerts";
            firefoxOptions.SetPreference(preferenceName, false);
            // Для задания опции расположения EXE
            //firefoxOptions.BrowserExecutableLocation = "C:\\Program Files\\Mozilla Firefox\\firefox.exe";            
            //--Задаем опции командной строки соотв. браузера
            //firefoxOptions.AddArguments("-private-window");
            //установка профиля пользователя для запуска браузера (копирует указанный профиль во временный для работы)
            //FirefoxProfile firefoxProfile = new FirefoxProfile("C:\\Users\\AdminVadim\\AppData\\Local\\Mozilla\\Firefox\\Profiles\\ltebh6bi.default");
            //firefoxOptions.Profile = firefoxProfile;
            return firefoxOptions;
        }

        private void LoginAs(IWebDriver webDriver, Uri remoteUri, WebDriverExtensions.WebDriverType driverType,
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
        private static void driverCapabilitiesPrint(IWebDriver webDriver, ICapabilities capabilities)
        {
            Console.WriteLine("---------- printDriverCapabilities --------------------");
            if (capabilities != null)
                Console.WriteLine(capabilities.ToString());

            Console.WriteLine("---------- end print  ---------------------------------");
        }
        // -------------------------------------------------------------------------------------

        private static ICapabilities getWebDriverCapabilities(IWebDriver webDriver)
        {
            return ((IHasCapabilities)webDriver).Capabilities;
        }

        [Category("Functional"), Category("Release")]
        private void CategoriesPropertiesTestPrint()
        {
            foreach (var category in TestContext.CurrentContext.Test.Properties["Category"])
            {
                Console.WriteLine(category);
            }
        }
    }

}


