using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using NUnit.Framework;


namespace CsharpTestProject1
{

    public class UT5CheckCountriesApp
    {
        private PageParams          pageParams;
        private AdminPanelLoginPage adminPanelLoginPage;
        private AdminLeftMenuPage   adminLeftMenuPage;

        // количество стран в списке, зон в списке
        private int countryQuantity { get; set; }
        private int zoneQuantity    { get; set; }  
        private int geoZoneQuantity { get; set; }

        // массив количества зон для списка стран
        private int[] zones { get; set; }  
        private int a, az;
        
        // строка по стране и по зоне
        private IWebElement countryRow { get; set; }
        private IWebElement zoneRow    { get; set; }
        private IWebElement geoZoneRow { get; set; }

        // список стран, список зон
        private IList<IWebElement> countryRows { get; set; }
        private IList<IWebElement> zoneRows    { get; set; }
        private IList<IWebElement> geoZoneRows { get; set; }

        // имена стран, имена зон
        private String[] countryName { get; set; }
        private String[] zoneName    { get; set; }

		// функция проверки, что строки в массиве идут по алфавиту
		public static int testAlphabet(String[] testArr, int arrSize)
		{
			int isAlphab = 1;  // начальное знчение признака - алфавитный порядок

			for (int i = 1; i < arrSize; i++)
			{ // перебираем строковый массив
				int k; // переменная для результата сравнения строк
				k = String.Compare(testArr[i - 1], testArr[i], StringComparison.OrdinalIgnoreCase);
				if (k >= 0)
					isAlphab = -1; // алфавитный порядок нарушен - меняем признак
			}

			return isAlphab;
		}


		public void InitPages(DriverBase drvBase)
        {
            pageParams = new PageParams(drvBase);

            adminPanelLoginPage = new AdminPanelLoginPage(pageParams);
            adminLeftMenuPage   = new AdminLeftMenuPage(pageParams);
        }

        private void LoginAs(string usrText, string passText)
        {
            if (adminPanelLoginPage.Open().IsOnThisPage())
            {
                adminPanelLoginPage.EnterUsername(usrText).EnterPassword(passText).SubmitLogin();
            }

            adminLeftMenuPage.waitUntilMyStore(); //подождать пока не загрузится страница с заголовком "My Store"
        }

        public void MyCheckCountries()
        {
            LoginAs(usrText: "admin", passText: "admin");//открыть страницу и выполнить коннект под пользователем + ждем страницу "My Store"

            //открыть страницу со списком стран
            adminLeftMenuPage.OpenCountries().waitUntilCountries(); // и ждем загрузки страницы

			//определение списка строк в таблице стран
			countryRows = adminLeftMenuPage.Css_countries_row_Elements;

			// сохраняем количество строк, т.е. стран в списке
			countryQuantity = countryRows.Count;
			// массив строк с названиями стран
			countryName = new String[countryQuantity];
			zones = new int[countryQuantity];

			for (int i = 0; i < countryQuantity; i++)
			{
				// создание массива со списком названий стран
				countryRow = countryRows[i];
				countryName[i] = adminLeftMenuPage.Row_Css_a_Element(countryRow).Text;
				zones[i] = int.Parse(adminLeftMenuPage.CountryRow_Css_td_nth_child6_Element(countryRow).Text);
			}

			a = testAlphabet(countryName, countryQuantity);

            Assert.True(a == 1);
			// проверка алфавитного порядка - если он нарушен, тест провалится

			for (int i = 0; i < countryQuantity; i++)
			{
				// проверка стран с ненулевым количеством
				if (zones[i] > 0)
				{
					// опять определяем таблицу стран
					countryRows = adminLeftMenuPage.Css_countries_row_Elements;
					// получаем строку с заданным индексом
					countryRow = countryRows[i];
					// находим ссылку с названием страны и кликаем по ней
					adminLeftMenuPage.Row_Css_a_Element(countryRow).Click();
					
					// даем время на загрузку  // Для задания явных ожиданий     
					//wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

					// получаем список строк в таблице зон
					zoneRows = adminLeftMenuPage.Css_id_table_zones_tr_Elements;

					// количество строк с учетом строки заголовка и служебной снизу
					zoneQuantity = zoneRows.Count - 2;
					// массив с названиями зон
					zoneName = new String[zoneQuantity];

					for (int j = 1; j <= zoneQuantity; j++)
					{
						// создание массива со списком названий зон
						zoneRow = zoneRows[j];
						zoneName[j - 1] = adminLeftMenuPage.ZoneRow_Css_td_nth_child3_Element(zoneRow).Text;
					}
					
					az = testAlphabet(zoneName, zoneQuantity);
					Assert.True(az == 1);
					// проверка алфавитного порядка перечня зон

					/* опять возвращаемся на страницу со списком стран, поскольку
	                 при проверке списка зон мы зашли на страницу отдельной страны,
	                 и если не вернуться к списку стран, то мы не сможем проверить список
	                 зон у другой страны, у которой число зон больше 0 */
					adminLeftMenuPage.OpenCountries().waitUntilCountries(); // и ждем загрузки страницы
				}
			}

			// открываем страницу просмотра географических зон
			adminLeftMenuPage.OpenGeoZones();

			// создаем список строк в таблице зон
			geoZoneRows = adminLeftMenuPage.Css_geo_zones_row_Elements;
			geoZoneQuantity = geoZoneRows.Count; // количество строк в списке

			for (int i = 0; i < geoZoneQuantity; i++)
			{
				geoZoneRows = adminLeftMenuPage.Css_geo_zones_row_Elements;
				geoZoneRow = geoZoneRows[i];  // получаем строку из списка
											  // находим ссылку с названием страны и кликаем по ней
				adminLeftMenuPage.Row_Css_a_Element(geoZoneRow).Click();

				// даем время на загрузку  // Для задания явных ожиданий                 
				//wait = new WebDriverWait(driver, TimeSpan.FromSeconds(driverBaseParams.drvExplWaitTime));

				// получаем список строк в таблице зон
				zoneRows = adminLeftMenuPage.Css_id_table_zones_tr_Elements;
				// количество строк с учетом строки заголовка и служебной снизу
				zoneQuantity = zoneRows.Count - 2;
				// массив с названиями зон
				zoneName = new String[zoneQuantity];

				for (int j = 1; j <= zoneQuantity; j++)
				{
					// создание массива со списком названий зон
					zoneRow = zoneRows[j];
					zoneName[j - 1] = adminLeftMenuPage.ZoneRow_Css_id_table_zones_td_nth_child3_Element(zoneRow).GetAttribute("textContent");
				}

				az = testAlphabet(zoneName, zoneQuantity);
				Assert.True(az == 1);

				// возврат на страницу просмотра географических зон
				adminLeftMenuPage.OpenGeoZones();
			}
		}


	}
}
