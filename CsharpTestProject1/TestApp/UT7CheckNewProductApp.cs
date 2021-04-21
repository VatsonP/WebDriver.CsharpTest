using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace CsharpTestProject1
{

	public class UT7CheckNewProductApp
	{
		private int sleepTimeMSec;

		private PageParams          pageParams;
		private AdminPanelLoginPage adminPanelLoginPage;
		private AdminNewProdPage	adminNewProdPage;

		String Name, ProdName, validFrom, validTo, prefix;

		// количество стран в списке, зон в списке
		private int countryQuantity { get; set; }


		// строка по стране и по зоне
		private IWebElement countryRow { get; set; }

		// список стран, список зон
		private IList<IWebElement> countryRows { get; set; }


		public UT7CheckNewProductApp(int sleepTimeMSec)
		{
			this.sleepTimeMSec = sleepTimeMSec;
		}


		public void InitPages(DriverBase drvBase)
		{
			pageParams = new PageParams(drvBase);

			adminPanelLoginPage = new AdminPanelLoginPage(pageParams);
			adminNewProdPage = new AdminNewProdPage(pageParams);
		}

		private void LoginAs(string usrText, string passText)
		{
			if (adminPanelLoginPage.Open().IsOnThisPage())
			{
				adminPanelLoginPage.EnterUsername(usrText).EnterPassword(passText).SubmitLogin();
			}

			adminNewProdPage.waitUntilMyStore(); //подождать пока не загрузится страница с заголовком "My Store"
		}

		public void myCheckNewProduct()
		{
			LoginAs(usrText: "admin", passText: "admin");//открыть страницу и выполнить коннект под пользователем + ждем страницу "My Store"

			// читаем текущее время - добавляем его к фамилии и имеем уникальный e-mail и пароль каждый раз
			DateTime curDate = DateTime.Now;
			int yyyy = curDate.Year;
			int mm = curDate.Month;
			int dd = curDate.Day;
			int h = curDate.Hour;
			int m = curDate.Minute;
			int s = curDate.Second;

			Name = "Donald McDown";
			prefix = WebDriverExtensions.PaddingLeft(h) + WebDriverExtensions.PaddingLeft(m) + WebDriverExtensions.PaddingLeft(s);
			ProdName = Name + " " + prefix;

			validFrom = pageParams.GetFullDateStrForBrowserDateControl(yyyy, mm, dd);
			validTo   = pageParams.GetFullDateStrForBrowserDateControl(yyyy + 2, mm, dd);

			pageParams.FindElmAndClick(By.CssSelector("[href*=catalog]"));
			// открыть каталог

			pageParams.FindElmAndClick(By.LinkText("Add New Product"));
			// открываем форму регистрации нового продукта

			// даем время на загрузку  // Для задания явных ожиданий         
			//wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

			pageParams.FindElmAndClick(By.Name("status"));
			// устанавливаем статус Enabled
			PageParams.Driver.FindElmAndClear(By.Name("name[en]"));
			// очистка
			PageParams.Driver.FindElmAndSendKeys(By.Name("name[en]"), ProdName);
			// вводим название товара
			PageParams.Driver.FindElmAndSendKeys(By.Name("code"), (prefix + Keys.Tab));
			// вводим код товара
			pageParams.FindElmAndClick(By.XPath("(//input[@name='categories[]'])[2]"));
			// устанавливаем категорию Rubber Ducks

			pageParams.FindElmAndClick(By.XPath("(//input[@name='product_groups[]'])[3]"));
			// Устанавливаем группу Unisex

			PageParams.Driver.FindElmAndSendKeys(By.Name("quantity"), "1");
			// устанавливаем количество 1

			PageParams.Driver.FindElmAndSendKeys(By.Name("date_valid_from"), validFrom);
			// устанавливаем дату начала годности
			PageParams.Driver.FindElmAndSendKeys(By.Name("date_valid_to"), validTo);
			// устанавливаем дату конца годности


			PageParams.Sleep(sleepTimeMSec);

			pageParams.FindElmAndClick(By.LinkText("Information"));
			// переходим на вкладку Information

			// даем время на загрузку  // Для задания явных ожиданий         
			//wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

			// select the drop down list
			var manufacturerIdElm = PageParams.Driver.FindElement(By.Name("manufacturer_id"));
			//create select element object 
			var selectElementId = new SelectElement(manufacturerIdElm);
			// select by text
			selectElementId.SelectByText("ACME Corp.");

			// выбираем корпорацию
			PageParams.Driver.FindElmAndSendKeys(By.Name("keywords"), "Duck");
			// Ввводим ключевое слово
			PageParams.Driver.FindElmAndSendKeys(By.Name("short_description[en]"), "Duck");
			// задаем краткое описание
			PageParams.Driver.FindElmAndSendKeys(By.Name("description[en]"), (ProdName + " is cool!"));
			// задаем описание
			PageParams.Driver.FindElmAndSendKeys(By.Name("head_title[en]"), ProdName);
			// задаем заголовок
			PageParams.Driver.FindElmAndSendKeys(By.Name("meta_description[en]"), "666666666");
			// задаем метаописание

			PageParams.Sleep(sleepTimeMSec);

			pageParams.FindElmAndClick(By.LinkText("Data"));
			// переходим на вкладку Data

			// даем время на загрузку  // Для задания явных ожиданий         
			//wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

			PageParams.Driver.FindElmAndSendKeys(By.Name("sku"), prefix);
			// заполняем поле SKU
			PageParams.Driver.FindElmAndSendKeys(By.Name("gtin"), prefix);
			// заполняем поле GTIN
			PageParams.Driver.FindElmAndSendKeys(By.Name("taric"), prefix);
			// заполняем поле TARIC
			PageParams.Driver.FindElmAndSendKeys(By.Name("weight"), "1");
			// задаем вес
			PageParams.Driver.FindElmAndSendKeys(By.Name("dim_x"), "10");
			PageParams.Driver.FindElmAndSendKeys(By.Name("dim_y"), "11");
			PageParams.Driver.FindElmAndSendKeys(By.Name("dim_z"), "12");
			// задаем размеры
			PageParams.Driver.FindElmAndSendKeys(By.Name("attributes[en]"), "None");
			// задаем атрибуты

			PageParams.Sleep(sleepTimeMSec);

			pageParams.FindElmAndClick(By.LinkText("Prices"));
			// переходим на вкладку Prices

			// даем время на загрузку  // Для задания явных ожиданий         
			//wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

			PageParams.Driver.FindElmAndSendKeys(By.Name("purchase_price"), "13");
			// задаем цену


			// select the drop down list
			var currencyCodeElm = PageParams.Driver.FindElement(By.Name("purchase_price_currency_code"));
			//create select element object 
			var selectElementCurr = new SelectElement(currencyCodeElm);
			// select by text
			selectElementCurr.SelectByText("Euros");
			// выбираем валюту

			PageParams.Driver.FindElmAndSendKeys(By.Name("gross_prices[USD]"), "20");
			// задаем цену в долларах

			PageParams.Sleep(sleepTimeMSec);

			pageParams.FindElmAndClick(By.Name("save"));
			// сохраняем продукт

			// даем время на загрузку  // Для задания явных ожиданий         
			//wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

			// Проверяем наличие такого элемента на странице
			PageParams.Driver.FindElement(By.LinkText(ProdName));

		}


	}
}
