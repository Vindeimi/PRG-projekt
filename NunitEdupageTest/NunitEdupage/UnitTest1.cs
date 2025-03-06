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
                Console.WriteLine("Navigov�no na p�ihla�ovac� str�nku.");

                // Vypln�n� p�ihla�ovac�ch �daj�
                driver.FindElement(By.Name("username")).SendKeys("SamuelHanzlik");
                driver.FindElement(By.Name("password")).SendKeys("48644969");

                // �ek�n� na tla��tko pro odesl�n� a kliknut� na n�j
                var submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("skgdFormSubmit")));
                submitButton.Click();
                Console.WriteLine("P�ihl�en� odesl�no.");

                // �ek�n� na p�esm�rov�n� po p�ihl�en�
                Console.WriteLine($"Aktu�ln� URL po p�ihl�en�: {driver.Url}");

                // Pokra�uj s p�echodem na str�nku doch�zky
                driver.Navigate().GoToUrl("https://sstebrno.edupage.org/dashboard/eb.php?mode=attendance");
                Console.WriteLine("P�echod na str�nku doch�zky.");

                // Po�kej na tabulku
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.dash_dochadzka")));
                Console.WriteLine("Tabulka doch�zky nalezena.");

                var scheduleTable = driver.FindElement(By.CssSelector("table.dash_dochadzka"));
                string scheduleHtml = scheduleTable.GetAttribute("outerHTML");
                Console.WriteLine($"HTML tabulky s rozvrhem:\n{scheduleHtml}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba: {ex.Message}");
                Console.WriteLine($"Zdrojov� k�d str�nky:\n{driver.PageSource}");
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
