using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.WebRequestMethods;

namespace edupageTest
{
    internal class Login
    {
        private readonly DriverInitialization _driverInitialization;
        public bool CanLogin = false;

        public Login(DriverInitialization driver)
        {
            _driverInitialization = driver;
        }

        #region Funkce Login

        public void LoginAndFetchAttendance(string username, string password)
        {
            try
            {
                _driverInitialization.Driver.Navigate().GoToUrl("https://login1.edupage.org/");
                Console.WriteLine("Navigováno na přihlašovací stránku.");

                // Vyčištění a vyplnění přihlašovacích údajů
                var usernameField = _driverInitialization.Driver.FindElement(By.Name("username"));
                usernameField.Clear();
                usernameField.SendKeys(username);

                var passwordField = _driverInitialization.Driver.FindElement(By.Name("password"));
                passwordField.Clear();
                passwordField.SendKeys(password);

                // Čekání na tlačítko pro odeslání a kliknutí na něj
                IWebElement submitButton = null;
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        submitButton = _driverInitialization.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("skgdFormSubmit")));
                        submitButton.Click();
                        break;
                    }
                    catch (StaleElementReferenceException)
                    {
                        // Re-fetch elementu
                        submitButton = _driverInitialization.Driver.FindElement(By.ClassName("skgdFormSubmit"));
                    }
                }
                Console.WriteLine("Přihlášení odesláno.");
                // Čekání na přesměrování po přihlášení
                Console.WriteLine($"Aktuální URL po přihlášení: {_driverInitialization.Driver.Url}");
                Thread.Sleep(500);
                if (_driverInitialization.Driver.Url.Contains("login"))
                {
                    Console.WriteLine("Přihlášení se nezdařilo");
                }
                else
                {
                    CanLogin = true;
                }
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
