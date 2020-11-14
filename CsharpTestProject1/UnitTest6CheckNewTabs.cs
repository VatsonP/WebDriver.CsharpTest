using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;


namespace CsharpTestProject1
{

	[TestFixture]
	public class CheckNewTabs : DriverBase
	{
		private int countryQuantity, linksQuantity; // количество стран в списке
	    private int randomIndex;
	    private IWebElement countryRow;  // строка по стране
	    private IList<IWebElement> countryRows, listLinks;  // список стран, список внешних ссылок
	    private String originalWindow, newWindow;
	    private ReadOnlyCollection<String> existingWindows;
	
		private const int sleepTimeMSec = 2000;

		public CheckNewTabs() : base(new DriverBaseParams()) { }
		

		public static Func<IWebDriver, String> AnyWindowOtherThan(ReadOnlyCollection<String> oldWindows)
		{
			return (driver) =>
			{
				ReadOnlyCollection<string> handles = driver.WindowHandles;
				handles.Except(oldWindows);
				return handles.Count > 0 ? handles.AsEnumerable().Last() : null;
			};

		}
		

		[Test]
		public void myCheckNewTabs() {

			driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/admin/"); //открыть страницу
			if (driver.isElementPresent(By.Name("username")))
				driver.FindElmAndSendKeys(By.Name("username"), "admin"); //найти поле для ввода логина и ввести "admin"
			if (driver.isElementPresent(By.Name("password")))
				driver.FindElmAndSendKeys(By.Name("password"), "admin"); //найти поле для ввода пароля и ввести "admin"
			if (driver.isElementPresent(By.Name("login")))
				driver.FindElmAndClick(By.Name("login"), webDriverType); //найти кнопку логина и нажать на нее

			wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleIs("My Store"));
			//подождать пока не загрузится страница с заголовком "My Store"

			//открыть страницу со списком стран
			driver.Navigate().GoToUrl("http://" + CurrentIpStr + ":8080/litecart/admin/?app=countries&doc=countries");
	        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Countries")); // ждем загрузки страницы

			//определение списка строк в таблице стран
			countryRows = driver.FindElements(By.CssSelector("[name=countries_form] .row"));
	
	        // сохраняем количество строк, т.е. стран в списке
	        countryQuantity = countryRows.Count;
	
	        var random = new Random();
	
	        // выбираем номер случайно страны из списка
	        randomIndex = random.Next(countryQuantity-1);
	
	        countryRow = countryRows[randomIndex];  // выбираем случайную страну
	        countryRow.FindElement(By.CssSelector("a")).Click();

	        // открываем страницу выбранной страны
	        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Edit Country"));  // ждем загрузки страницы

			driver.Sleep(sleepTimeMSec);

			listLinks = driver.FindElements(By.CssSelector("form .fa-external-link"));
	        // получаем список внешних ссылок
	
	        linksQuantity = listLinks.Count;  // определяем количество ссылок

			for (int i=0; i<linksQuantity; i++) {
	
	            originalWindow = driver.CurrentWindowHandle;
				// сохранили идентификатор текущего окна

				existingWindows = driver.WindowHandles;
	            // сохранили идентификаторы уже открытых окон
	
	            listLinks[i].Click(); // кликаем по ссылке из найденного списка

				newWindow = wait.Until(AnyWindowOtherThan(existingWindows)); ;
				// получаем идентификатор нового окна

				driver.SwitchTo().Window(newWindow);  // переключаемся в новое окно

				driver.Sleep(sleepTimeMSec);

				driver.Close();  // закрываем окно

				driver.SwitchTo().Window(originalWindow); // вернулись в исходное окно
			}
	    }
	

	}
}