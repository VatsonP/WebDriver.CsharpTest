using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CsharpTestProjCore.Pages
{
    internal class Page
    {
        protected static PageParams pageParams;

        public Page(PageParams _pageParams)
        {
            pageParams = _pageParams;
        }

        internal struct CurDateTime
        {
            public DateTime curDate;
            public int yyyy;
            public int mm;
            public int dd;
            public int h;
            public int m;
            public int s;

            public CurDateTime(DateTime date)
            {
                curDate = date;
                yyyy = curDate.Year;
                mm = curDate.Month;
                dd = curDate.Day;
                h = curDate.Hour;
                m = curDate.Minute;
                s = curDate.Second;
            }
        }
    }

}