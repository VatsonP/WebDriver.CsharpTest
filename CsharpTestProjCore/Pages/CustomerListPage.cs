using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace CsharpTestProjCore.Pages
{
    internal class CustomerListPage : Page
    {
        public CustomerListPage(PageParams _pageParams) : base(_pageParams)
        {
            //Использование PageFactory устарело, т.к. заменяется свойствами, возвращающими нужный IWebElement
            //PageFactory.InitElements(PageParams.Driver, this);
        }

        internal CustomerListPage Open()
        {
            PageParams.Driver.Url = "http://" + PageParams.CurrentIpStr + ":8080/litecart/admin/?app=customers&doc=customers";
            return this;
        }

        /* 
        //Depricated ------------------------------------------------------------------------
        // Using PageFactory Object, initilized in constructor

        [FindsBy(How = How.CssSelector, Using = "table.dataTable tr.row")]
        IList<IWebElement> customerRows;
        internal ISet<string> GetCustomerIds_mod()
        {
            return new HashSet<string>(
                customerRows.Select(e => e.FindElements(By.TagName("td"))[2].Text).ToList());
        }
        //------------------------------------------------------------------------------------
        */

        public ISet<string> GetCustomerIds(bool isNeedPrint, string printHeader)
        {
            IReadOnlyCollection<IWebElement> tableRows = PageParams.Driver.FindElements(By.CssSelector("table.dataTable tr.row")).ToList();

            if (isNeedPrint)
                Console.WriteLine(printHeader);

            var hSet = new HashSet<string>();
            foreach (var tRow in tableRows)
            {
                if (isNeedPrint)
                    Console.WriteLine(tRow.Text);

                hSet.Add(tRow.Text);
            }

            return hSet;
        }

    }
}