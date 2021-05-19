using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using CsharpWebDriverLib;

namespace CsharpTestProject1.Pages
{
    public class PageParams
    {
        private DriverBase DrvBase { get; set; }

        public static IWebDriver Driver { get => DriverBase.driver; }
        public static WebDriverWait DriverWait { get => DriverBase.wait; }
        public static string CurrentIpStr { get => DriverBase.CurrentIpStr; }

        public static void Sleep(int sleep_Msec) => Driver.Sleep(sleep_Msec);

        public PageParams(DriverBase drvBase)
        {
            DrvBase = drvBase;
        }

        public void TakeScreenshot(string fileNameWithoutExt = "") => DrvBase.TakeScreenshot(fileNameWithoutExt);
        
        public static void ClickElement(IWebElement element) => DriverBase.ClickElement(element);
        public static void FindElmAndClick(By locator) => DriverBase.FindElmAndClick(locator);
        public static string GetFullDateStrForBrowserDateControl(int yyyy, int mm, int dd) => DriverBase.GetFullDateStrForBrowserDateControl(yyyy, mm, dd);
    }
}
