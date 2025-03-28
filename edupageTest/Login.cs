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
        public void LoginAndFetchAttendance(string username, string password)
        {
            try
            {
                _driverInitialization.Driver.Navigate().GoToUrl("https://login1.edupage.org/");

                // Wait for page to load completely
                _driverInitialization.Wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                // Wait for and fill login form
                var wait = new WebDriverWait(_driverInitialization.Driver, TimeSpan.FromSeconds(20));

                // Multiple possible selectors for robustness
                var usernameField = wait.Until(d =>
                    d.FindElement(By.CssSelector("input[name='username'], input[id='username'], input[type='text']")));

                var passwordField = _driverInitialization.Driver.FindElement(By.CssSelector(
                    "input[name='password'], input[id='password'], input[type='password']"));

                // Clear fields first in case of autofill
                usernameField.Clear();
                usernameField.SendKeys(username);

                passwordField.Clear();
                passwordField.SendKeys(password);

                // Type credentials slowly (more human-like)
                //SendKeysSlowly(usernameField, username);
                //SendKeysSlowly(passwordField, password);

                // Find and click submit button
                var submitButton = _driverInitialization.Driver.FindElement(By.CssSelector(
                    "button[type='submit'], input[type='submit'], #login-button"));
                submitButton.Click();

                // Wait for login to complete
                wait.Until(ExpectedConditions.UrlContains("dashboard"));





                //_driverInitialization.Driver.Navigate().GoToUrl("https://login1.edupage.org/");
                //Console.WriteLine("Navigováno na přihlašovací stránku.");

                //// Vyplnění přihlašovacích údajů
                //var wait = new WebDriverWait(_driverInitialization.Driver, TimeSpan.FromSeconds(10));
                //var usernameField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("username")));
                //var passwordField = _driverInitialization.Driver.FindElement(By.Id("password"));

                //usernameField.SendKeys(username);
                //passwordField.SendKeys(password);

                //_driverInitialization.Driver.FindElement(By.Id("login-button")).Click();

                //wait.Until(ExpectedConditions.UrlContains("dashboard"));

                // Čekání na tlačítko pro odeslání a kliknutí na něj
                //var submitButton = _driverInitialization.Wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("skgdFormSubmit")));
                //submitButton.Click();
                //Console.WriteLine("Přihlášení odesláno.");

                //// Čekání na přesměrování po přihlášení
                //Console.WriteLine($"Aktuální URL po přihlášení: {_driverInitialization.Driver.Url}");

                // Pokračuj s přechodem na stránku docházky
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba: {ex.Message}");
                Console.WriteLine($"Zdrojový kód stránky:\n{_driverInitialization.Driver.PageSource}");
                throw;
            }
        }
        //private void SendKeysSlowly(IWebElement element, string text)
        //{
        //    foreach (char c in text)
        //    {
        //        element.SendKeys(c.ToString());
        //        Thread.Sleep(new Random().Next(50, 150)); // Random typing speed
        //    }
        //}
        #endregion
    }
}
