using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CsharpTestProject1
{
    internal class Page
    {
        protected static PageParams pageParams;

        public Page(PageParams _pageParams)
        {
            pageParams = _pageParams;
        }
    }

}