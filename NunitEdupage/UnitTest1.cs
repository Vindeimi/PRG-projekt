using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace NunitEdupage
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void TestLoginAndAttendancePage()
        {
            try
            {
                driver.Navigate().GoToUrl("https://login1.edupage.org/");
                Console.WriteLine("Navigováno na pøihlašovací stránku.");

                // Vyplnìní pøihlašovacích údajù
                driver.FindElement(By.Name("username")).SendKeys("SamuelHanzlik");
                driver.FindElement(By.Name("password")).SendKeys("48644969");

                // Èekání na tlaèítko pro odeslání a kliknutí na nìj
                var submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("skgdFormSubmit")));
                submitButton.Click();
                Console.WriteLine("Pøihlášení odesláno.");

                // Èekání na pøesmìrování po pøihlášení
                Console.WriteLine($"Aktuální URL po pøihlášení: {driver.Url}");

                // Pokraèuj s pøechodem na stránku docházky
                driver.Navigate().GoToUrl("https://sstebrno.edupage.org/dashboard/eb.php?mode=attendance");
                Console.WriteLine("Pøechod na stránku docházky.");

                // Poèkej na tabulku
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.dash_dochadzka")));
                Console.WriteLine("Tabulka docházky nalezena.");

                var scheduleTable = driver.FindElement(By.CssSelector("table.dash_dochadzka"));
                string scheduleHtml = scheduleTable.GetAttribute("outerHTML");
                Console.WriteLine($"HTML tabulky s rozvrhem:\n{scheduleHtml}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba: {ex.Message}");
                Console.WriteLine($"Zdrojový kód stránky:\n{driver.PageSource}");
                throw;
            }
        }

        [TearDown]
        public void Cleanup()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }
    }
}
