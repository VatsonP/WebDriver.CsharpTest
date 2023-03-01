using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using CsharpTestProjCore.Pages;
using CsharpWebDriverLib.DriverBase;


namespace CsharpTestProjCore.TestApp
{

    public class UT2LeftMenuClickApp
    {
        private int sleepTimeMenuMSec;
        private int sleepTimeSubmenuMSec;
        private int sleepTimeMSec;

        private PageParams pageParams;
        private AdminPanelLoginPage adminPanelLoginPage;
        private AdminLeftMenuPage   adminLeftMenuPage;

        private IList<IWebElement> menuPoints { get; set; }
        private IList<IWebElement> submenuPoints { get; set; }

        private IWebElement menuPoint { get; set; }
        private IWebElement submenuPoint { get; set; }

        private int menuQuantity, submenuQuantity;

        public UT2LeftMenuClickApp(int sleepTimeMenuMSec, int sleepTimeSubmenuMSec, int sleepTimeMSec) 
        {
            this.sleepTimeMenuMSec = sleepTimeMenuMSec;
            this.sleepTimeSubmenuMSec = sleepTimeSubmenuMSec;
            this.sleepTimeMSec = sleepTimeMSec;
        }

        public void InitPages(IDriverBase drvBase)
        {
            pageParams = new PageParams(drvBase);

            adminPanelLoginPage = new AdminPanelLoginPage(pageParams);
            adminLeftMenuPage = new AdminLeftMenuPage(pageParams);
        }

        private void LoginAs(string usrText, string passText)
        {
            if (adminPanelLoginPage.Open().IsOnThisPage())
            {
                adminPanelLoginPage.EnterUsername(usrText).EnterPassword(passText).SubmitLogin();
            }

            adminLeftMenuPage.waitUntilMyStore(); //подождать пока не загрузится страница с заголовком "My Store"
        }

        public void MyLeftMenuClick()
        {
            LoginAs(usrText: "admin", passText: "admin");//открыть страницу и выполнить коннект под пользователем

            menuPoints = adminLeftMenuPage.Id_app_Elements; 

            // определение списка пунктов меню
            menuQuantity = menuPoints.Count; // сохраняем количество пунктов меню

            // определение списка пунктов меню
            for (int i = 0; i < menuQuantity; i++) //проходим по пунктам меню
            {
                menuPoints = adminLeftMenuPage.Id_app_Elements;
                menuPoint = menuPoints[i]; //выбираем пункт меню

                menuPoint.Click();  // кликаем по меню

                menuPoints = adminLeftMenuPage.Id_app_Elements; //обновляем список
                menuPoint = menuPoints[i]; //выбираем пункт меню
                // определение списка пунктов подменю
                submenuPoints = adminLeftMenuPage.getCss_menu_id_doc_Elements(menuPoint); //menuPoint.FindElements(By.CssSelector("[id^=doc-]"));
                submenuQuantity = submenuPoints.Count; // сохраняем количество пунктов подменю

                PageParams.Sleep(sleepTimeMenuMSec);

                if (submenuQuantity > 0)
                { //подменю есть
                    for (int j = 0; j < submenuQuantity; j++)
                    {
                        menuPoints = adminLeftMenuPage.Id_app_Elements;  //обновляем список
                        menuPoint = menuPoints[i]; //выбираем пункт меню
                        // определение списка пунктов подменю
                        submenuPoints = adminLeftMenuPage.getCss_menu_id_doc_Elements(menuPoint);//menuPoint.FindElements(By.CssSelector("[id^=doc-]"));
                        submenuPoint = submenuPoints[j]; //выбираем пункт подменю

                        PageParams.ClickElement(submenuPoint);//кликаем по подменю submenuPoint.Click();  

                        var IsDisplayed = adminLeftMenuPage.Css_h1_Element.Displayed; // PageParams.Driver.FindElement(By.CssSelector("h1"));  //проверка наличия заголовка

                        PageParams.Sleep(sleepTimeSubmenuMSec);
                    }
                }
                else
                {   // подменю нет
                    var IsDisplayed = adminLeftMenuPage.Css_h1_Element.Displayed; //PageParams.Driver.FindElement(By.CssSelector("h1"));  //проверка наличия заголовка
                }

            }

            PageParams.Sleep(sleepTimeMSec);
        }

    }
}
