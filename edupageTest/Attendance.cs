using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edupageTest
{
    internal class Attendance
    {
        private readonly DriverInitialization _driverInitialization;

        public Attendance(DriverInitialization driverInitialization)
        {
            _driverInitialization = driverInitialization;
        }

        public void FindAttendance()
        {
            _driverInitialization.Driver.Navigate().GoToUrl("https://sstebrno.edupage.org/dashboard/eb.php?mode=attendance");
            Console.WriteLine("Přechod na stránku docházky.");

            // Cekani na tabulku
            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.dash_dochadzka")));
            Console.WriteLine("Tabulka docházky nalezena.");

            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.asc-ribbon-button")));
            IWebElement button = _driverInitialization.Driver.FindElement(By.CssSelector("div.asc-ribbon-button:nth-child(2)"));
            button.Click();

            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.dt-container")));
            var scheduleTable = _driverInitialization.Driver.FindElement(By.CssSelector("div.dt-container"));
            string scheduleHtml = scheduleTable.GetAttribute("outerHTML");
            Console.WriteLine($"HTML tabulky s rozvrhem:\n{scheduleHtml}");
        }

    }
}
