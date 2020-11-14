using System;
using System.Reflection;
using System.Threading;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework.Constraints;

namespace CsharpTestProject1
{
    public static class WebDriverExtensions
    {
        // константа времени (в сек) общих неявных ожиданий
        public const int drvImplWaitTime = 5;
        // константа времени (в сек) для явных ожиданий
        public const int drvExplWaitTime = 10;
        // константа времени (в сек) для максимального времени неявного ожидания
        public const int drvMaxWaitTime = 15;

        public enum WebDriverType { IE, Chrome, Firefox };

        //--------------------------------------------------------------------------------------------------------------
        public static IJavaScriptExecutor Scripts(this IWebDriver driver)
        {
            return (IJavaScriptExecutor)driver;
        } 
        // using:  driver.Scripts().ExecuteScript("JsStr");

        public static IWebElement FindElementByJs(this IWebDriver driver, string jsCommand)
        {
            return (IWebElement)(driver.Scripts()).ExecuteScript(jsCommand);
        }

        public static IWebElement FindElementByJsWithWait(this IWebDriver driver, string jsCommand, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(d => d.FindElementByJs(jsCommand));
            }
            return driver.FindElementByJs(jsCommand);
        }
        //--------------------------------------------------------------------------------------------------------------

        public static IWebElement FindElm(this IWebDriver webDriver, By locator)
        {
            return webDriver.FindElement(locator);
        }

        public static void FindElmAndClear(this IWebDriver webDriver, By locator)
        {
            FindElm(webDriver, locator).Clear();
        }

        public static void FindElmAndSendKeys(this IWebDriver webDriver, By locator, String keyText)
        {
            FindElm(webDriver, locator).SendKeys(keyText);
        }

        public static void ClickElement(this IWebElement element, WebDriverType driverType)
        {
            switch (driverType)
            {
                case WebDriverType.IE:
                    element.SendKeys(Keys.Return);
                    break;
                default:
                    element.Click();
                    break;
            }
        }

        public static void FindElmAndClick(this IWebDriver webDriver, By locator, WebDriverType driverType)
        {
            ClickElement(FindElm(webDriver, locator), driverType);
        }

        
        public static void Sleep(this IWebDriver webDriver, int sleep_Msec)
        {
            Thread.Sleep(sleep_Msec);
        }

        public static void TakeScreenshot(this IWebDriver webDriver, 
                                          string currentTestFolder, string currentTestName,
                                          string fileNameWithoutExt = "")
        {
            // for write Screenshot file 
            DirectoryInfo baseScrFolder = Directory.CreateDirectory(Path.Combine(currentTestFolder, "Screenshot"));
            DirectoryInfo curScrFolder  = Directory.CreateDirectory(Path.Combine(baseScrFolder.FullName, currentTestName));

            DateTime curDate = DateTime.Now;
            String uniqFileNameExt = "_" + PaddingLeft(curDate.Year, 4) + "-" + PaddingLeft(curDate.Month) + "-" + PaddingLeft(curDate.Day) +
                                     "_" + PaddingLeft(curDate.Hour) + PaddingLeft(curDate.Minute) + PaddingLeft(curDate.Second) + ".jpg";
            String fullFileName;
            if (fileNameWithoutExt == "")
                fullFileName = "ScreenShot" + uniqFileNameExt;
            else
                fullFileName = fileNameWithoutExt + uniqFileNameExt;

            var curScrFilePathName = Path.Combine(curScrFolder.FullName, fullFileName);

            Screenshot ss;
            try
            {
                ss = ((ITakesScreenshot)webDriver).GetScreenshot();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to capture screenshot: " + e.Message);
                throw;
            }

            try
            {
                ss.SaveAsFile(curScrFilePathName, ScreenshotImageFormat.Jpeg);
            }
            catch (IOException e)
            {
                Console.WriteLine("Failed to save screenshot: " + e.Message);
                throw;
            }
        }

        public static string GetFullDateStrForBrowserDateControl(this IWebDriver webDriver, int yyyy, int mm, int dd, WebDriverType driverType)
        {
            switch (driverType)
            {
                case WebDriverType.Chrome:
                    return PaddingLeft(dd) + "." +
                           PaddingLeft(mm) + "." +
                           PaddingLeft(yyyy, 4);


                default:
                    return PaddingLeft(yyyy, 4) + "-" +
                           PaddingLeft(mm) + "-" +
                           PaddingLeft(dd);
            }
        }

        public static string PaddingLeft(int intS, int totalWidth = 2, char paddingChar = '0')
        {
            return intS.ToString().PadLeft(totalWidth, paddingChar);
        }


        public static Boolean isAttributePresent(this IWebElement element, String attributeName)
        {
            Boolean result = false;
            try
            {
                String value = element.GetAttribute(attributeName);
                if (value != null)
                {
                    result = true;
                }
            }
            catch (Exception) { }

            return result;
        }

        public static Boolean isElementPresent(this IWebDriver webDriver, By locator, Boolean isWait = false,
                                                int localExplWaitTime = drvExplWaitTime)
        {

            try
            {
                if (isWait)
                {
                    WebDriverWait localWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(localExplWaitTime));
                    // Задаем явное ожидание - вариант с применением лямбда выражения (с передачей ф-ции в качестве параметра)
                    IWebElement element = localWait.Until(d => d.FindElement(locator));

                    // АЛЬТЕРНАТИВНЫЙ Вариант - с использованием устаревшего пакета DotNetSeleniumExtras.WaitHelpers.3.11.0
                    //IWebElement element = localWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
                }
                else
                {
                    webDriver.FindElement(locator);
                }
                return true;
            }
            // Если передан неправильный локатор
            catch (InvalidSelectorException ex)
            {
                WriteCurMethodMessage(MethodInfo.GetCurrentMethod(), ex.GetType(), ex.Message);
                throw ex;
            }
            //Если элемент отсутствует в DOM на момент вызова
            catch (NoSuchElementException ex)
            {
                WriteCurMethodMessage(MethodInfo.GetCurrentMethod(), ex.GetType(), ex.Message);
                return false;
            }
            //Если элемент доступен в DOM на момент поиска, но спустя время, в момент его вызова, DOM может измениться
            catch (StaleElementReferenceException ex)
            {
                WriteCurMethodMessage(MethodInfo.GetCurrentMethod(), ex.GetType(), ex.Message);
                return false;
            }
            //Если элемент был найдем в DOM, но не видим на странице
            catch (ElementNotVisibleException ex)
            {
                WriteCurMethodMessage(MethodInfo.GetCurrentMethod(), ex.GetType(), ex.Message);
                return false;
            }
        }

        private static void WriteCurMethodMessage(MethodBase method, Type classType, String messageStr)
        {
            Console.WriteLine(linkCurMethodMessage(method, classType, messageStr));
        }

        public static string linkCurMethodMessage(MethodBase method, Type classType, String messageStr)
        {
            return $"{method.Name}(): " + classType.ToString() + " - " + messageStr;
        }


        public static Boolean areElementsPresent(this IWebDriver webDriver, By locator, int localImplWaitTime = drvImplWaitTime)
        {
            try
            {
                // Для установки MAX общих неявных ожиданий drvImplWaitTime
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(drvMaxWaitTime);

                return webDriver.FindElements(locator).Count > 0;
            }
            finally
            {
                // Для возврата опции общих неявных ожиданий localImplWaitTime
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(localImplWaitTime);
            }
        }

        public static Boolean areElementsNOTPresent(this IWebDriver webDriver, By locator, int localImplWaitTime = drvImplWaitTime)
        {
            try
            {
                // Для установки общих неявных ожиданий = 0
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

                return webDriver.FindElements(locator).Count == 0;
            }
            finally
            {
                // Для возврата опции общих неявных ожиданий localImplWaitTime
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(localImplWaitTime);
            }
        }

        public static void checkAndPrintAttributeByName(this IWebElement element, String attributeName)
        {
            if (isAttributePresent(element, attributeName))
            {
                Console.Write("element('name')= " + element.GetAttribute("name") + ", ");
                Console.WriteLine("element.GetAttribute('" + attributeName + "')= " + element.GetAttribute(attributeName));
            }
            else
            {
                Console.WriteLine("Attribute '" + attributeName + "' is absent");
            }
        }

    }
}
