using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using OpenQA.Selenium;
using CsharpTestProjCore.Pages;
using CsharpWebDriverLib.DriverBase;


namespace CsharpTestProjCore.TestApp
{

    public class UT3CheckNewTabsApp
    {

        private PageParams pageParams;
        private AdminPanelLoginPage adminPanelLoginPage;
        private AdminLeftMenuPage   adminLeftMenuPage;

        private int countryQuantity, linksQuantity; // количество стран в списке
        private int randomIndex;

        private IWebElement        countryRow  { get; set; }  // строка по стране
        private IList<IWebElement> countryRows { get; set; }  // список стран 
        private IList<IWebElement> listLinks   { get; set; }  // список внешних ссылок
        
        private String originalWindow, newWindow;
        private ReadOnlyCollection<String> existingWindows { get; set; }

        private const int sleepTimeMSec = 1000;

        public void InitPages(IDriverBase drvBase)
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

        public void myCheckNewTabs()
        {
            LoginAs(usrText: "admin", passText: "admin");//открыть страницу и выполнить коннект под пользователем + ждем страницу "My Store"

            //открыть страницу со списком стран
            adminLeftMenuPage.OpenCountries().waitUntilCountries(); // и ждем загрузки страницы

            //определение списка строк в таблице стран
            countryRows = adminLeftMenuPage.Css_countries_row_Elements;// PageParams.Driver.FindElements(By.CssSelector("[name=countries_form] .row"));

            // сохраняем количество строк, т.е. стран в списке
            countryQuantity = countryRows.Count;

            var random = new Random();

            // выбираем номер случайно страны из списка
            randomIndex = random.Next(countryQuantity - 1);

            countryRow = countryRows[randomIndex];  // выбираем случайную страну
            
            adminLeftMenuPage.getCss_a_Elements(countryRow).Click();//countryRow.FindElement(By.CssSelector("a")).Click();

            // открываем страницу выбранной страны
            adminLeftMenuPage.waitUntilEditCountry();

            PageParams.Sleep(sleepTimeMSec);

            listLinks = adminLeftMenuPage.Css_form_fa_external_link_Elements;//PageParams.Driver.FindElements(By.CssSelector("form .fa-external-link"));
            // получаем список внешних ссылок

            linksQuantity = listLinks.Count;  // определяем количество ссылок

            for (int i = 0; i < linksQuantity; i++)
            {
                originalWindow = adminLeftMenuPage.getCurrentWindowHandle(); //PageParams.Driver.CurrentWindowHandle;
                // сохранили идентификатор текущего окна

                existingWindows = adminLeftMenuPage.getWindowHandles(); //PageParams.Driver.WindowHandles;
                // сохранили идентификаторы уже открытых окон

                listLinks[i].Click(); // кликаем по ссылке из найденного списка

                newWindow = adminLeftMenuPage.waitUntilEditCountry(existingWindows);
                // получаем идентификатор нового окна

                adminLeftMenuPage.SwitchToWindow(newWindow);  // переключаемся в новое окно

                PageParams.Sleep(sleepTimeMSec);

                adminLeftMenuPage.CloseCurWindow();  // закрываем окно

                adminLeftMenuPage.SwitchToWindow(originalWindow); // вернулись в исходное окно
            }
        }

    }
}
