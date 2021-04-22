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
			var curDateTime = new AdminNewProdPage.CurDateTime(DateTime.Now);

			Name      = "Donald McDown";
			prefix    = AdminNewProdPage.GetProdPrefix(curDateTime);
			ProdName  = Name + " " + prefix;

			validFrom = AdminNewProdPage.GetProdValidFrom(curDateTime); 
			validTo   = AdminNewProdPage.GetProdValidTo(curDateTime);

			adminNewProdPage.Css_catalog_ElementClick();// открыть каталог

			adminNewProdPage.LinkText_ElementClick("Add New Product");// открываем форму регистрации нового продукта

			// даем время на загрузку  // Для задания явных ожиданий         
			//wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

			adminNewProdPage.Name_ElementClick("status"); // устанавливаем статус Enabled

			adminNewProdPage.Name_ElementClear("name[en]"); // очистка

			adminNewProdPage.Name_ElementSendKeys("name[en]", ProdName); // вводим название товара

			adminNewProdPage.Name_ElementSendKeys("code", (prefix + Keys.Tab));// вводим код товара

			adminNewProdPage.XPath_categories_RubberDucks_ElementClick(); // устанавливаем категорию Rubber Ducks

			adminNewProdPage.XPath_categories_Unisex_ElementClick(); // Устанавливаем группу Unisex

			adminNewProdPage.Name_ElementSendKeys("quantity", "1"); // устанавливаем количество 1

			adminNewProdPage.Name_ElementSendKeys("date_valid_from", validFrom); // устанавливаем дату начала годности
			adminNewProdPage.Name_ElementSendKeys("date_valid_to",   validTo);   // устанавливаем дату конца годности


			PageParams.Sleep(sleepTimeMSec);

			adminNewProdPage.LinkText_ElementClick("Information");// переходим на вкладку Information

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

			PageParams.FindElmAndClick(By.LinkText("Data"));
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

			PageParams.FindElmAndClick(By.LinkText("Prices"));
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

			PageParams.FindElmAndClick(By.Name("save"));
			// сохраняем продукт

			// даем время на загрузку  // Для задания явных ожиданий         
			//wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

			// Проверяем наличие такого элемента на странице
			PageParams.Driver.FindElement(By.LinkText(ProdName));

		}


	}
}
