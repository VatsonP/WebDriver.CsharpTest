using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace CsharpTestProject1
{
    [TestFixture]
    public class CheckCountries : DriverBase
	{
        private int                  countryQuantity, zoneQuantity, geoZoneQuantity;  // количество стран в списке, зон в списке
	    private int[]                zones;                                           // массив количества зон для списка стран
	    private int                  a, az;
	    private IWebElement          countryRow, zoneRow, geoZoneRow;      // строка по стране и по зоне
	    private IList<IWebElement>   countryRows, zoneRows, geoZoneRows;   // список стран, список зон
	    private String[]             countryName, zoneName;                // имена стран, имена зон


		public CheckCountries() : base(new DriverBaseParams()) { }

		// функция проверки, что строки в массиве идут по алфавиту
		private static int testAlphabet (String[] testArr, int arrSize)
        {
	        int isAlphab=1;  // начальное знчение признака - алфавитный порядок

	        for (int i=1; i<arrSize; i++)
            { // перебираем строковый массив
	            int k; // переменная для результата сравнения строк
	            k = String.Compare(testArr[i - 1], testArr[i], StringComparison.OrdinalIgnoreCase); 
	            if(k>=0)
                    isAlphab =-1; // алфавитный порядок нарушен - меняем признак
	        }

	        return isAlphab;
	    }

        [Test]
        public void myCheckCountries()
        {
	        driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/admin/"); //открыть страницу
			if (driver.isElementPresent(By.Name("username")))
				driver.FindElement(By.Name("username")).SendKeys("admin");//найти поле для ввода логина и ввести "admin"
			if (driver.isElementPresent(By.Name("password"))) 
				driver.FindElement(By.Name("password")).SendKeys("admin");//найти поле для ввода пароля и ввести "admin"
			if (driver.isElementPresent(By.Name("login")))
				driver.FindElement(By.Name("login")).Click();

	        //найти кнопку логина и нажать на нее
	        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleIs("My Store"));
            //подождать пока не загрузится страница с заголовком "My Store"

            //открыть страницу со списком стран
            driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/admin/?app=countries&doc=countries");
	
	        //определение списка строк в таблице стран
	        countryRows = driver.FindElements(By.CssSelector("[name=countries_form] .row"));
	
	        // сохраняем количество строк, т.е. стран в списке
	        countryQuantity = countryRows.Count;
	        // массив строк с названиями стран
	        countryName = new String[countryQuantity];
	        zones = new int[countryQuantity];
	
	        for (int i=0; i<countryQuantity; i++) {
	            // создание массива со списком названий стран
	            countryRow     = countryRows[i];
	            countryName[i] = countryRow.FindElement(By.CssSelector("a")).Text;
	            zones[i]       = int.Parse(countryRow.FindElement(By.CssSelector("td:nth-child(6)")).Text);
	        }
	
	        a = testAlphabet(countryName,countryQuantity);
	
	        Assert.True(a==1);
	        // проверка алфавитного порядка - если он нарушен, тест провалится
	
	        for (int i=0; i<countryQuantity; i++) {
	            // проверка стран с ненулевым количеством
	            if (zones[i]>0) {
	                // опять определяем таблицу стран
	                countryRows = driver.FindElements(By.CssSelector("[name=countries_form] .row"));
	                // получаем строку с заданным индексом
	                countryRow=countryRows[i];
	                // находим ссылку с названием страны и кликаем по ней
	                countryRow.FindElement(By.CssSelector("a")).Click();

                    // даем время на загрузку  // Для задания явных ожиданий                 
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

                    // получаем список строк в таблице зон
                    zoneRows = driver.FindElements(By.CssSelector("[id=table-zones] tr"));
	                // количество строк с учетом строки заголовка и служебной снизу
	                zoneQuantity = zoneRows.Count - 2;
	                // массив с названиями зон
	                zoneName = new String[zoneQuantity];
	
	                for (int j=1; j<=zoneQuantity; j++) {
	                    // создание массива со списком названий зон
	                    zoneRow=zoneRows[j];
	                    zoneName[j-1]=zoneRow.FindElement(By.CssSelector("td:nth-child(3)")).Text;
	                }
	                az = testAlphabet(zoneName,zoneQuantity);
	                Assert.True(az==1);
	                // проверка алфавитного порядка перечня зон
	
	                /* опять возвращаемся на страницу со списком стран, поскольку
	                 при проверке списка зон мы зашли на страницу отдельной страны,
	                 и если не вернуться к списку стран, то мы не сможем проверить список
	                 зон у другой страны, у которой число зон больше 0 */
	                driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/admin/?app=countries&doc=countries");
	            }
	        }
	
	        // открываем страницу просмотра географических зон
	        driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/admin/?app=geo_zones&doc=geo_zones");
	
	        // создаем список строк в таблице зон
	        geoZoneRows = driver.FindElements(By.CssSelector("[name=geo_zones_form] .row"));
	        geoZoneQuantity = geoZoneRows.Count; // количество строк в списке
	
	        for (int i=0; i<geoZoneQuantity; i++) {
	            geoZoneRows = driver.FindElements(By.CssSelector("[name=geo_zones_form] .row"));
	            geoZoneRow = geoZoneRows[i];  // получаем строку из списка
	            // находим ссылку с названием страны и кликаем по ней
	            geoZoneRow.FindElement(By.CssSelector("a")).Click();

                // даем время на загрузку  // Для задания явных ожиданий                 
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));
	
	            // получаем список строк в таблице зон
	            zoneRows = driver.FindElements(By.CssSelector("[id=table-zones] tr"));
	            // количество строк с учетом строки заголовка и служебной снизу
	            zoneQuantity = zoneRows.Count - 2;
	            // массив с названиями зон
	            zoneName = new String[zoneQuantity];
	
	            for (int j=1; j<=zoneQuantity; j++) {
	                // создание массива со списком названий зон
	                zoneRow=zoneRows[j];
	                zoneName[j-1] = zoneRow.FindElement(
	                                            By.CssSelector("[id=table-zones] tr td:nth-child(3) [selected=selected]")).
	                                                    GetAttribute("textContent");
	            }

	            az = testAlphabet(zoneName,zoneQuantity);
	            Assert.True(az==1);

	            // возврат на страницу просмотра географических зон
	            driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/admin/?app=geo_zones&doc=geo_zones");
	        }
	    }

	}
}