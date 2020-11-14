﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CsharpTestProject1
{
    public class PageParams
    {
        public DriverBase DrvBase { get; private set; }

        public static IWebDriver    Driver { get => DriverBase.driver; }
        public static WebDriverWait DriverWait { get => DriverBase.wait; }
        public static string        CurrentIpStr { get => DriverBase.CurrentIpStr; }

        public PageParams(DriverBase drvBase)
        {
            DrvBase = drvBase;
        }
    }
}