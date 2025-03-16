using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace edupageTest
{
    internal class Login
    {
        private readonly DriverInitialization _driverInitialization;
        
        public Login(DriverInitialization driver)
        {
            _driverInitialization = driver;
        }

        #region Funkce Login
        public void LoginAndFetchAttendance()
        {
            try
            {
                _driverInitialization.Driver.Navigate().GoToUrl("https://login1.edupage.org/");
                Console.WriteLine("Navigováno na přihlašovací stránku.");

                // Vyplnění přihlašovacích údajů
                _driverInitialization.Driver.FindElement(By.Name("username")).***REMOVED***;
                _driverInitialization.Driver.FindElement(By.Name("password")).***REMOVED***;

                // Čekání na tlačítko pro odeslání a kliknutí na něj
                var submitButton = _driverInitialization.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("skgdFormSubmit")));
                submitButton.Click();
                Console.WriteLine("Přihlášení odesláno.");

                // Čekání na přesměrování po přihlášení
                Console.WriteLine($"Aktuální URL po přihlášení: {_driverInitialization.Driver.Url}");

                // Pokračuj s přechodem na stránku docházky
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba: {ex.Message}");
                Console.WriteLine($"Zdrojový kód stránky:\n{_driverInitialization.Driver.PageSource}");
                throw;
            }
        }
        #endregion
    }
}
