using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace NunitEdupage
{
    public class Tests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {
            _driver = new FirefoxDriver();
            _driver.Manage().Window.Maximize();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void TestLoginAndAttendancePage()
        {
            try
            {
                _driver.Navigate().GoToUrl("https://login1.edupage.org/");
                Console.WriteLine("Navigováno na přihlašovací stránku.");

                // Vyplnění přihlašovacích údajů
                _driver.FindElement(By.Name("username")).***REMOVED***;
                _driver.FindElement(By.Name("password")).***REMOVED***;

                // Čekání na tlačítko pro odeslání a kliknutí na něj
                var submitButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("skgdFormSubmit")));
                submitButton.Click();
                Console.WriteLine("Přihlášení odesláno.");

                // Čekání na přesměrování po přihlášení
                Console.WriteLine($"Aktuální URL po přihlášení: {_driver.Url}");

                // Pokračuj s přechodem na stránku docházky
                _driver.Navigate().GoToUrl("https://sstebrno.edupage.org/dashboard/eb.php?mode=attendance");
                Console.WriteLine("Přechod na stránku docházky.");

                // Počkej na tabulku
                _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.dash_dochadzka")));
                Console.WriteLine("Tabulka docházky nalezena.");

                _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.asc-ribbon-button")));
                IWebElement button = _driver.FindElement(By.CssSelector("div.asc-ribbon-button:nth-child(2)"));
                button.Click();

                _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.dt-container")));
                var scheduleTable = _driver.FindElement(By.CssSelector("div.dt-container"));
                string scheduleHtml = scheduleTable.GetAttribute("outerHTML");
                Console.WriteLine($"HTML tabulky s rozvrhem:\n{scheduleHtml}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba: {ex.Message}");
                Console.WriteLine($"Zdrojový kód stránky:\n{_driver.PageSource}");
                throw;
            }
        }

        [TearDown]
        public void Cleanup()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
}
