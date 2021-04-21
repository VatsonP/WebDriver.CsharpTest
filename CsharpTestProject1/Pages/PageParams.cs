using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CsharpTestProject1
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

        public void ClickElement(IWebElement element) => DrvBase.ClickElement(element);

        public void FindElmAndClick(By locator) => DrvBase.FindElmAndClick(locator);

        public string GetFullDateStrForBrowserDateControl(int yyyy, int mm, int dd) => DrvBase.GetFullDateStrForBrowserDateControl(yyyy, mm, dd);
    }
}
