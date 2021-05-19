using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using CsharpWebDriverLib;
using CsharpTestProject1.Pages;


namespace CsharpTestProject1.TestApp
{

	public class FirstDemoApp
	{

		private const int sleepTimeMSec = 500;

		private PageParams pageParams;
		private AdminPanelLoginPage adminPanelLoginPage;

		public void InitPages(IDriverBase drvBase)
		{

			pageParams = new PageParams(drvBase);

			adminPanelLoginPage = new AdminPanelLoginPage(pageParams);
		}

		private void LoginAs(string usrText, string passText)
		{
			if (adminPanelLoginPage.Open().IsOnThisPage())
			{
				adminPanelLoginPage.EnterUsername(usrText).EnterPassword(passText).SubmitLogin();
			}
		}

		public void FirstTest()
		{
			PageParams.Driver.Navigate().GoToUrl("http://www.google.com/");
			IWebElement qElement = PageParams.Driver.FindElement(By.Name("q"));
			qElement.SendKeys("webdriver");
			qElement.SendKeys(Keys.Return);

			PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("webdriver"));

			PageParams.Driver.Sleep(sleepTimeMSec);
		}

		public void SecondTest()
		{
			//XAMPP litecart - "http://" + PageParams.CurrentIpStr + ":8080/litecart/en/"
			PageParams.Driver.Navigate().GoToUrl("http://" + PageParams.CurrentIpStr + ":8080/litecart/en/");

			IWebElement Element1 = PageParams.Driver.FindElement(By.CssSelector("div[class='input-wrapper'] [type='search']"));
			Element1.SendKeys("duck");
			Element1.SendKeys(Keys.Return);
			PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("duck"));

			PageParams.Driver.Sleep(sleepTimeMSec);
		}

		public void ThirdTest()
		{
			PageParams.Driver.Navigate().GoToUrl("http://www.google.com/");
			string searchingElementName = "q-q-q";

			//Пример исключения InvalidSelectorException - eсли передан неправильный локатор
			//isElementPresent(driver, By.CssSelector("1 2 3 notValidCss"), isWait: true);
			//Пример исключения NoSuchElementException - eсли элемент отсутствует в DOM на момент вызова
			//isElementPresent(driver, By.Name(searchingElementName), isWait: true);

			if (PageParams.Driver.isElementPresent(By.Name(searchingElementName), isWait: true))
			{
				IWebElement qElement = PageParams.Driver.FindElement(By.Name(searchingElementName));
				qElement.SendKeys("webdriver");
				qElement.SendKeys(Keys.Return);

				PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("webdriver"));
			}

			PageParams.Driver.Sleep(sleepTimeMSec);
		}

		public void FourthTest()
		{
			//XAMPP litecart - "http://" + PageParams.CurrentIpStr + ":8080/litecart/en/"
			PageParams.Driver.Navigate().GoToUrl("http://" + PageParams.CurrentIpStr + ":8080/litecart/en/");
			IWebElement Element1 = PageParams.Driver.FindElement(By.CssSelector("div[class='input-wrapper'] [type='search']"));
			Element1.SendKeys("duck");
			// метод-свойство Displayed
			Console.WriteLine("Element1.Displayed =" + Element1.Displayed);

			// использование функции GetAttribute() 
			Element1.checkAndPrintAttributeByName("value");
			Element1.checkAndPrintAttributeByName("placeholder");
			// атрибуты(свойства) типа boolean возвращают true (при их наличии) и null (при их отсутствии)
			Element1.checkAndPrintAttributeByName("spellcheck");
			Element1.checkAndPrintAttributeByName("draggable");
			Element1.checkAndPrintAttributeByName("a-a-a-a-a");

			Element1.SendKeys(Keys.Return);
			PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("duck"));

			PageParams.Driver.Sleep(sleepTimeMSec);
		}

		public void FifthTest()
		{
			//XAMPP litecart admin page - "http://" + PageParams.CurrentIpStr + ":8080/litecart/admin/?app=customers&doc=customers"
			var remoteAddress = "http://" + PageParams.CurrentIpStr + ":8080/litecart/admin/?app=customers&doc=customers"; //открыть страницу

			LoginAs(usrText: "admin", passText: "admin");
			PageParams.DriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("My Store"));
			//подождать пока не загрузится страница с заголовком "My Store"

			PageParams.Driver.Sleep(sleepTimeMSec);
		}

		public void SixthTest_myCheckStiker()
		{
			int prodQuantity, prodStickerQuantity;
			int allProdWithStickerQuantity = 0;
			IWebElement productUnit;
			IList<IWebElement> prodList, stickerList;

			//XAMPP litecart - "http://" + PageParams.CurrentIpStr + ":8080/litecart/en/"
			PageParams.Driver.Navigate().GoToUrl("http://" + PageParams.CurrentIpStr + ":8080/litecart/en/");

			PageParams.Driver.Navigate().GoToUrl("http://" + PageParams.CurrentIpStr + ":8080/litecart/en/rubber-ducks-c-1/");
			//открыть страницу магазина с товарами

			prodList = PageParams.Driver.FindElements(By.CssSelector("li.product"));
			// определение списка товаров на главной странице
			prodQuantity = prodList.Count; // сохраняем количество товаров

			Console.WriteLine("prodQuantity: " + prodQuantity);

			for (int i = 0; i < prodQuantity; i++)
			{  //проходим по списку товаров
				prodList = PageParams.Driver.FindElements(By.CssSelector("li.product"));
				productUnit = prodList[i];

				//определение списка стикеров (полосок) у товара

				stickerList = productUnit.FindElements(By.CssSelector("div.sticker"));
				//определение количества стикеров у товара
				prodStickerQuantity = stickerList.Count;

				Console.WriteLine("prodNum (i): " + i);
				Console.WriteLine("prodStickerQuantity: " + prodStickerQuantity);

				//проверка что у товара не более одного стикера
				NUnit.Framework.Assert.IsTrue(prodStickerQuantity <= 1);

				if (prodStickerQuantity == 1)
					allProdWithStickerQuantity = allProdWithStickerQuantity + 1;
			}

			Console.WriteLine("---------------------------------");
			Console.WriteLine("allProdWithStickerQuantity: " + allProdWithStickerQuantity);
		}

	}
}

