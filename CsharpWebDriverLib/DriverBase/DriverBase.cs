﻿using System;
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using static CsharpWebDriverLib.DriverBase.WebDriverExtensions;
using OpenQA.Selenium.Interactions;

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

        //must be initilize after the WebDriver create
        protected ICapabilities wdCapabilities { get; set; }

        //must be initilize after the WebDriver create
        private ILogs wdLogs { get; set; }
                
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

        private void saveLogOnAppEnd(string currentTestName, string eventName, string eventMessage)
        {
            // for write log file with Browser logging
            DirectoryInfo baseLogFolder = createBaseLogDir(TestContext.CurrentContext.TestDirectory);

            LogWriter lw = new LogWriter(wdLogs, baseLogFolder, currentTestName);

            lw.LogWrite("eventName", eventMessage);
        }
        */

        private void initWebDriverCapabilities(IWebDriver webDriver)
        {
            webDriver.Manage().Cookies.DeleteAllCookies();
            wdCapabilities = getWebDriverCapabilities(webDriver);
            driverCapabilitiesPrint(webDriver, wdCapabilities);//TODO
        }


        private void initLogWriter(IWebDriver webDriver)
        {
            wdLogs = webDriver.Manage().Logs;

            // for write log file on error
            CurrentTestName = TestContext.CurrentContext.Test.Name;
            CurrentTestFolder = TestContext.CurrentContext.TestDirectory;

            Console.WriteLine("Starting test: " + CurrentTestName);

            BaseLogFolder = createBaseLogDir(CurrentTestFolder);
            logWriter = new LogWriter(wdLogs, BaseLogFolder, CurrentTestName);

        }
        private void initWebDriverWait(IWebDriver webDriver)
        {
            // Для установки общих неявных ожиданий
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(driverBaseParams.drvImplWaitTime);

            // Для задания явных ожиданий
            IDriverBase.wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));
        }

        private String defineCurrentIpStr(TestRunType tRunType)
        {
            string retstr = "";

            switch (tRunType)
            {
                case TestRunType.Local:
                    retstr = driverBaseParams.localHostStr;
                    break;

                case TestRunType.RemoteWin:
                    retstr = driverBaseParams.localIpStr;
                    break;

                case TestRunType.RemoteUbuntu:
                    retstr = driverBaseParams.localIpStr;
                    break;
            }

            return retstr;
        }

        // ---------------------------------------------------------------------------------------------------------------------------

        public void TakeScreenshot(String fileNameWithoutExt)
        {
            IDriverBase.driver.TakeScreenshot(CurrentTestFolder, CurrentTestName, fileNameWithoutExt);
        }

        public void SetUp()
        {
            // SET initial parameter
            IDriverBase.testRunType   = DriverBaseParams.getTestRunType();
            IDriverBase.webDriverType = DriverBaseParams.getWebDriverType();
            IDriverBase.CurrentIpStr  = defineCurrentIpStr(IDriverBase.testRunType);

            if (IDriverBase.tlDriverIsValueCreated() & IDriverBase.eftlDriverIsValueCreated())
            {
                initLogWriter(IDriverBase.webDrv);
                initWebDriverCapabilities(IDriverBase.webDrv);
                initWebDriverWait(IDriverBase.webDrv);
                return;
            }

            if (IDriverBase.testRunType == TestRunType.Local)
            {
                IDriverBase.webDrv = newDriverSetOptions(IDriverBase.webDriverType);
            }
            else
            if ((IDriverBase.testRunType == TestRunType.RemoteWin) || (IDriverBase.testRunType == TestRunType.RemoteUbuntu))
            {
                IDriverBase.webDrv = newRemoteWebDriverSetOptions(driverType: IDriverBase.webDriverType);
            }

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
            initLogWriter(IDriverBase.webDrv);
            initWebDriverCapabilities(IDriverBase.webDrv);
            initWebDriverWait(IDriverBase.webDrv);

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

            saveBrowserLog(logWriter);
        }

        // ---------------------------------------------------------------------------------------------------------------------------

        private void saveBrowserLog(LogWriter lw)
        {
            Console.WriteLine("testRunType = " + IDriverBase.testRunType.ToString());
            Console.WriteLine("driverType  = " + IDriverBase.webDriverType.ToString());

            // for write log file with Browser logging
            if (lw != null)
            {
                lw.LogWrite("currentTestName", CurrentTestName);

                lw.LogWrite("Capabilities", wdCapabilities.ToString());

                lw.LogWrite("testRunType", IDriverBase.testRunType.ToString());
                lw.LogWrite("driverType", IDriverBase.webDriverType.ToString());

                lw.saveCurLogs(LogType.Browser);

                lw.FinalLogWrite();
            }
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
                    webDriver = new InternetExplorerDriver(internetExplorerDriverServerDirectory: @"C:\Tools", getIEOptions());
                    break;

                case WebDriverExtensions.WebDriverType.Chrome:
                    webDriver = new ChromeDriver(chromeDriverDirectory: @"C:\Tools", options: getChromeOptions());
                    break;

                case WebDriverExtensions.WebDriverType.Firefox:
                    webDriver = new FirefoxDriver(geckoDriverDirectory: @"C:\Tools", options: getFirefoxOptions());
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Not valid WebDriverType value: " + IDriverBase.webDriverType);
            }

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
            ieOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            return ieOptions;
        }


        private ChromeOptions getChromeOptions()
        {
            ChromeOptions chromeOptions = new ChromeOptions();

            // Для задания опции acceptInsecureCerts
            chromeOptions.AcceptInsecureCertificates = false;
            // Для задания опции UnhandledPromptBehavior
            chromeOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;
            chromeOptions.AddArgument("--lang=ru");
            // Для задания опции расположения EXE
            chromeOptions.BinaryLocation = getChromePathStr();
            //--Задаем опции командной строки соотв. браузера
            //chromeOptions.AddArguments("start-fullscreen");
            //Use custom profile(also called user data directory)
            chromeOptions.AddArguments("--profile-directory=Default");
            chromeOptions.AddArguments("--user-data-dir=C:/Temp/ChromeProfile");

            return chromeOptions;
        }

        private string getChromePathStr()
        {
            const string path_usr = @"%HOMEPATH%\Local Settings\Application Data\Google\Chrome\Application\chrome.exe";
            const string path_x86 = @"C:\Program Files (x86)\Google\Chrome\Application\Chrome.exe";
            const string path_x64 = @"C:\Program Files\Google\Chrome\Application\Chrome.exe";

            if (File.Exists(path_usr))
            {
                return path_usr;
            }
            else if(File.Exists(path_x86))
            {
                return path_x86;
            }
            else if (File.Exists(path_x64))
            {
                return path_x64;
            }
            else
                throw new FileNotFoundException("Chrome.exe file was not found.");
        }

        private FirefoxOptions getFirefoxOptions()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();

            var capabilityName = "acceptInsecureCerts";
            // Для задания опции acceptInsecureCerts
            firefoxOptions.SetPreference(capabilityName, false);
            // Для задания опции расположения EXE
            firefoxOptions.BrowserExecutableLocation = getFirefoxPathStr();
            //--Задаем опции командной строки соотв. браузера
            firefoxOptions.AddArguments("-private-window");

            //установка профиля пользователя для запуска браузера (копирует указанный профиль во временный для работы)
            string userProfileFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            /* string firefoxProfileFolderPath = Path.Combine(userProfileFolderPath, @"AppData\Roaming\Mozilla\Firefox\Profiles"); */
            string firefoxProfileFolderPath = Path.Combine(userProfileFolderPath, @"AppData\Local\Mozilla\Firefox\Profiles"); 
            string[] profileDirectories = Directory.GetDirectories(firefoxProfileFolderPath, "*.default");
            string fullProfilePath = profileDirectories[0];
            FirefoxProfile firefoxProfile = new FirefoxProfile(fullProfilePath);
            firefoxOptions.Profile = firefoxProfile;

            return firefoxOptions;
        }
        private string getFirefoxPathStr()
        {
            const string path_x86 = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
            const string path_x64 = @"C:\Program Files\Mozilla Firefox\firefox.exe";

            if (File.Exists(path_x86))
            {
                return path_x86;
            }
            else if (File.Exists(path_x64))
            {
                return path_x64;
            }
            else
                throw new FileNotFoundException("firefox.exe file was not found.");
        }

        private IWebDriver newRemoteWebDriverSetOptions(WebDriverExtensions.WebDriverType driverType)
        {
            IWebDriver webDriver;
            IDriverBase.webDriverType = driverType;

            string hostStr = "";

            if (driverType == WebDriverExtensions.WebDriverType.IE)
                hostStr = IDriverBase.CurrentIpStr;
            else
                hostStr = driverBaseParams.remoteIpStr;

            var uriString = "http://" + hostStr + ":4444/wd/hub/";

            switch (driverType)
            {
                case WebDriverExtensions.WebDriverType.IE:
                    IDriverBase.selenoidProcess = StartLocalSelenoidServerForIE();
                    webDriver = new RemoteWebDriver(new Uri(uriString), getRemoteIEOptions());
                    break;

                case WebDriverExtensions.WebDriverType.Chrome:
                    webDriver = new RemoteWebDriver(new Uri(uriString), getRemoteChromeOptions());
                    break;

                case WebDriverExtensions.WebDriverType.Firefox:
                    webDriver = new RemoteWebDriver(new Uri(uriString), getRemoteFirefoxOptions());
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Not valid WebDriverType value: " + driverType);
            }

            return webDriver;
        }

        private Process StartLocalSelenoidServerForIE()
        {
            // Start the Selenoid.exe server using the selenoid.bat file on local Windows machine
            var startInfo = new ProcessStartInfo
            {
                FileName = DriverBaseParams.selenoidBatFilePathName,
                Arguments = "start",
                WorkingDirectory = DriverBaseParams.selenoidBatFileWorkingDirectory,
                CreateNoWindow = false,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var process = new Process { StartInfo = startInfo };

            process.Start();
            //Sleep for the Selenoid.exe process to start up
            Thread.Sleep(1000);

            process.WaitForExitAsync();

            ProcessStartInfo processStartInfo = process.StartInfo;

            return process;
        }

        private InternetExplorerOptions getRemoteIEOptions()
        {
            InternetExplorerOptions ieOptions = new InternetExplorerOptions();

            ieOptions.BrowserVersion = "11";
            ieOptions.PlatformName = DriverBaseParams.driverSelenoidPlatformNameWindows;

            var runName = GetType().Assembly.GetName().Name;

            ieOptions.AddAdditionalOption("selenoid:options", new Dictionary<string, object>
            {
                ["name"] = runName,
                ["sessionTimeout"] = "1m"/* How to set session timeout */
            });

            // Для задания опции UnhandledPromptBehavior
            ieOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
            //установка опций для игнорирования отличия масштаба от 100%
            ieOptions.IgnoreZoomLevel = true;

            //установка опций для игнорирования отличия настройки защищенного режима в разных зонах
            // if (useSelenoidExe = true), enabling this options required !!!
            ieOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;

            return ieOptions;
        }


        private ChromeOptions getRemoteChromeOptions()
        {
            ChromeOptions chromeOptions = new ChromeOptions();

            chromeOptions.BrowserVersion = "112.0";
            chromeOptions.PlatformName = DriverBaseParams.driverSelenoidPlatformNameLinux;

            var runName = GetType().Assembly.GetName().Name;
            var timestamp = $"{DateTime.Now:yyyyMMdd.HHmm}";

            chromeOptions.AddAdditionalOption("selenoid:options", new Dictionary<string, object>
            {
                ["name"] = runName,
                ["sessionTimeout"] = "1m",/* How to set session timeout */
                ["videoName"] = $"{runName}.{timestamp}.mp4",
                ["enableVNC"] = true,
                ["enableVideo"] = true,
                ["videoScreenSize"] = "1280x720",
                ["logName"] = $"{runName}.{timestamp}.log",
                ["enableLog"] = true,
                ["screenResolution"] = "1920x1080x24"
            });
            // Для задания опции acceptInsecureCerts
            chromeOptions.AcceptInsecureCertificates = false;
            // Для задания опции UnhandledPromptBehavior
            chromeOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;
            //--Задаем опции командной строки соотв. браузера
            chromeOptions.AddArguments("start-fullscreen");
            chromeOptions.AddArgument("--lang=ru");
            chromeOptions.AddArgument("--no-sandbox");

            return chromeOptions;
        }


        private FirefoxOptions getRemoteFirefoxOptions()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions();

            firefoxOptions.BrowserVersion = "112.0";
            firefoxOptions.PlatformName = DriverBaseParams.driverSelenoidPlatformNameLinux;

            var runName = GetType().Assembly.GetName().Name;
            var timestamp = $"{DateTime.Now:yyyyMMdd.HHmm}";
            // * Variant 1 -------------------------------------------------------------
            firefoxOptions.AddAdditionalOption("selenoid:options", new Dictionary<string, object>
            {
                ["name"] = runName,
                ["sessionTimeout"] = "1m", /* How to set session timeout */
                ["videoName"] = $"{runName}.{timestamp}.mp4",
                ["enableVNC"] = true,
                ["enableVideo"] = true,
                ["videoScreenSize"] = "1280x720",
                ["logName"] = $"{runName}.{timestamp}.log",
                ["enableLog"] = true,
                ["screenResolution"] = "1920x1080x24"
            });
            /*
            // * Variant 2 -------------------------------------------------------------
            // Set the selenoid:options capability in the moz:firefoxOptions dictionary
            Dictionary<string, object> selenoidOptions = new Dictionary<string, object>();
            selenoidOptions.Add("videoName", $"{runName}.{timestamp}.mp4");
            selenoidOptions.Add("enableVNC", true);
            selenoidOptions.Add("enableVideo", true);
            selenoidOptions.Add("videoScreenSize", "1280x720");
            selenoidOptions.Add("logName", $"{runName}.{timestamp}.log");
            selenoidOptions.Add("enableLog"), true);
            firefoxOptions.AddAdditionalOption("selenoid:options", selenoidOptions);
            */
            // Для задания опции acceptInsecureCerts
            var preferenceName = "acceptInsecureCerts";
            firefoxOptions.SetPreference(preferenceName, false);

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


