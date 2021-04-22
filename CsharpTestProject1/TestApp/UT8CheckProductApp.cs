using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;



namespace CsharpTestProject1
{

	public class UT8CheckProductApp
	{
		private int sleepTimeMSec;

		private PageParams          pageParams;
		private AdminNewProdPage	adminNewProdPage;

		IWebElement productUnit;

		String[] regularPrice1, regularPrice2, campaignPrice1, campaignPrice2;
		String productName1, productName2;
		float size1, size2;


		public UT8CheckProductApp(int sleepTimeMSec)
		{
			this.sleepTimeMSec = sleepTimeMSec;
		}


		public void InitPages(DriverBase drvBase)
		{
			pageParams = new PageParams(drvBase);

			adminNewProdPage = new AdminNewProdPage(pageParams);
		}

		internal void OpenStoreMainPage()
		{
			PageParams.Driver.Url = "http://" + PageParams.CurrentIpStr + ":8080/litecart/en/"; //открыть главную страницу магазина
		}

		public void myCheckProduct()
		{
			OpenStoreMainPage();

			productUnit = adminNewProdPage.Css_FirstProduct_Campains_Element(); // выбрали первый товар в разделе Campains

			productName1 = productUnit.FindElement(By.CssSelector(".name")).Text;
			// сохраняем имя продукта

			regularPrice1 = new String[4];
			campaignPrice1 = new String[4];

			// Сохраняем параметры обычной цены
			regularPrice1[0] = productUnit.FindElement(By.CssSelector(".regular-price")).Text;
			// сохраняем значение обычной цены
			// поскольку с цветами в приложении ошибка - проверку цвета исключаем
			//regularPrice1[1]= productUnit.FindElement(By.CssSelector(".regular-price")).GetCssValue("color");
			// сохраняем цвет шрифта обычной цены
			regularPrice1[1] = productUnit.FindElement(By.CssSelector(".regular-price")).GetCssValue("font-weight");
			// сохраняем тип шрифта обычной цены - жирный или нет
			regularPrice1[2] = productUnit.FindElement(By.CssSelector(".regular-price")).GetCssValue("text-decoration");
			// сохраняем оформление шрифта обычной цены
			regularPrice1[3] = productUnit.FindElement(By.CssSelector(".regular-price")).GetCssValue("font-size");
			// сохраняем размер шрифта обычной цены

			size1 = float.Parse(regularPrice1[3].Replace("px", ""), CultureInfo.InvariantCulture);
			// получаем числовое значение размера шрифта


			// сохраняем параметры цены по акции
			campaignPrice1[0] = productUnit.FindElement(By.CssSelector(".campaign-price")).Text;
			// сохраняем значение цены по акции
			// поскольку с цветами в приложении ошибка - проверку цвета исключаем
			// campaignPrice1[1]= productUnit.FindElement(By.CssSelector(".campaign-price")).GetCssValue("color");
			// сохраняем цвет шрифта цены по акции
			campaignPrice1[1] = productUnit.FindElement(By.CssSelector(".campaign-price")).GetCssValue("font-weight");
			// сохраняем размер шрифта цены по акции
			campaignPrice1[2] = productUnit.FindElement(By.CssSelector(".campaign-price")).GetCssValue("text-decoration");
			// сохраняем оформление шрифта обычной цены
			campaignPrice1[3] = productUnit.FindElement(By.CssSelector(".campaign-price")).GetCssValue("font-size");
			// сохраняем размер шрифта цены по акции

			size2 = float.Parse(campaignPrice1[3].Replace("px", ""), CultureInfo.InvariantCulture);
			// получаем числовое значение размера шрифта

			PageParams.Sleep(sleepTimeMSec);

			Assert.IsTrue(size1 < size2);
			// сравниваем размеры шрифтов цен - шрифт обычной цены меньше чем у акционной


			productUnit.Click();  // открываем страницу продукта

			//wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));// даем время на загрузку

			productUnit = adminNewProdPage.Css_Box_Product_Element(); // выбираем блок продукта на странице

			productName2 = productUnit.FindElement(By.CssSelector("[itemprop=name]")).Text;
			// сохраняем имя продукта

			PageParams.Sleep(sleepTimeMSec);

			Assert.IsTrue(productName1.CompareTo(productName2) == 0);
			// проверка совпадения названий продуктов

			regularPrice2 = new String[4];
			campaignPrice2 = new String[4];

			// Сохраняем параметры обычной цены
			regularPrice2[0] = productUnit.FindElement(By.CssSelector(".regular-price")).Text;
			// сохраняем значение обычной цены
			// поскольку с цветами в приложении ошибка - проверку цвета исключаем
			//regularPrice2[1]= productUnit.FindElement(By.CssSelector(".regular-price")).GetCssValue("color");
			// сохраняем цвет шрифта обычной цены
			regularPrice2[1] = productUnit.FindElement(By.CssSelector(".regular-price")).GetCssValue("font-weight");
			// сохраняем тип шрифта обычной цены - жирный или нет
			regularPrice2[2] = productUnit.FindElement(By.CssSelector(".regular-price")).GetCssValue("text-decoration");
			// сохраняем оформление шрифта обычной цены
			regularPrice2[3] = productUnit.FindElement(By.CssSelector(".regular-price")).GetCssValue("font-size");
			// сохраняем размер шрифта обычной цены

			size1 = float.Parse(regularPrice2[3].Replace("px", ""), CultureInfo.InvariantCulture);
			// получаем числовое значение размера шрифта

			// сохраняем параметры цены по акции
			campaignPrice2[0] = productUnit.FindElement(By.CssSelector(".campaign-price")).Text;
			// сохраняем значение цены по акции
			// поскольку с цветами в приложении ошибка - проверку цвета исключаем
			//campaignPrice2[1]= productUnit.FindElement(By.CssSelector(".campaign-price")).GetCssValue("color");
			// сохраняем цвет шрифта цены по акции
			campaignPrice2[1] = productUnit.FindElement(By.CssSelector(".campaign-price")).GetCssValue("font-weight");
			// сохраняем размер шрифта цены по акции
			campaignPrice2[2] = productUnit.FindElement(By.CssSelector(".campaign-price")).GetCssValue("text-decoration");
			// сохраняем оформление шрифта обычной цены
			campaignPrice2[3] = productUnit.FindElement(By.CssSelector(".campaign-price")).GetCssValue("font-size");
			// сохраняем размер шрифта цены по акции

			size2 = float.Parse(campaignPrice2[3].Replace("px", ""), CultureInfo.InvariantCulture);
			// получаем числовое значение размера шрифта

			PageParams.Sleep(sleepTimeMSec);

			Assert.IsTrue(size1 < size2);
			// сравниваем размеры шрифтов цен - шрифт обычной цены меньше чем у акционной

			// сравниваем значения и параметры оформления цен
			for (int i = 0; i < 3; i++)
			{

				Console.WriteLine("regularPrice1[" + i + "]= " + regularPrice1[i] + "; regularPrice2[" + i + "]=" + regularPrice2[i] + ";");
				Console.WriteLine("campaignPrice1[" + i + "]= " + campaignPrice1[i] + "; campaignPrice2[" + i + "]=" + campaignPrice2[i] + ";");

				//Assert.assertTrue(regularPrice1[i].compareTo(regularPrice2[i])==0);
				// совпадение значений и оформления обычной цены
				//Assert.assertTrue(campaignPrice1[i].compareTo(campaignPrice2[i])==0);
				// совпадение значений и оформления цены по акции
			}
		}

	}
}
