using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using CsharpWebDriverLib;

namespace CsharpTestProject1.Pages
{
    public record PageParams
    {
        private IDriverBase DrvBase { get; init; }

        public static IWebDriver Driver { get => IDriverBase.driver; }
        public static WebDriverWait DriverWait { get => IDriverBase.wait; }
        public static string CurrentIpStr { get => IDriverBase.CurrentIpStr; }

        public static void Sleep(int sleep_Msec) => Driver.Sleep(sleep_Msec);

        public PageParams(IDriverBase drvBase)
        {
            DrvBase = drvBase;
        }

        public void TakeScreenshot(string fileNameWithoutExt = "") => DrvBase.TakeScreenshot(fileNameWithoutExt);
        
        public static void ClickElement(IWebElement element) => IDriverBase.ClickElement(element);
        public static void FindElmAndClick(By locator) => IDriverBase.FindElmAndClick(locator);
        public static string GetFullDateStrForBrowserDateControl(int yyyy, int mm, int dd) => IDriverBase.GetFullDateStrForBrowserDateControl(yyyy, mm, dd);
    }
}
