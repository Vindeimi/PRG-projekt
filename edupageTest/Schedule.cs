using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edupageTest
{
    internal class Schedule
    {
        private readonly DriverInitialization _driverInitialization;
        private Dictionary<string, int> _subjectCount = new Dictionary<string, int>();

        public Schedule(DriverInitialization driverInitialization)
        {
            _driverInitialization = driverInitialization;
        }

        public void FindTimetable()
        {
            _driverInitialization.Driver.Navigate().GoToUrl("https://sstebrno.edupage.org/dashboard/eb.php?mode=timetable");
            Console.WriteLine("Přechod na stránku rozvrhu.");

            // Cekani na rozvrh
            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".print-nobreak")));
            Console.WriteLine("Rozvrh nalezen.");
        }

        public void FindPermanentTimeTable()
        {
            _driverInitialization.Driver.Navigate().GoToUrl("https://sstebrno.edupage.org/dashboard/eb.php?mode=timetable");
            Console.WriteLine("Přechod na stránku rozvrhu.");

            // Cekani na rozvrh
            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".print-nobreak")));
            Console.WriteLine("Rozvrh nalezen.");

            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#fitheight > div:nth-child(1) > span:nth-child(8)")));
            IWebElement button = _driverInitialization.Driver.FindElement(By.CssSelector("#fitheight > div:nth-child(1) > span:nth-child(8)"));
            button.Click();

            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".dropDownPanel > li:nth-child(1) > a:nth-child(1)")));
            IWebElement scheduleButton = _driverInitialization.Driver.FindElement(By.CssSelector(".dropDownPanel"));
            scheduleButton.Click();

            ExtractPermanentTimeTableData();
        }

        private void ExtractPermanentTimeTableData()
        {
            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".print-sheet > svg:nth-child(1) > g:nth-child(2)")));
            var permanentTableData = _driverInitialization.Driver.FindElement(By.CssSelector(".print-sheet > svg:nth-child(1) > g:nth-child(2)"));
            string permanentTableDataHtml = permanentTableData.GetAttribute("outerHTML");
            Console.WriteLine(permanentTableDataHtml) ;

            IList<IWebElement> gElement = _driverInitialization.Driver.FindElements(By.TagName("g"));

            foreach(var element in gElement)
            {
                IList<IWebElement> cells = element.FindElements(By.TagName("text"));


                foreach(var cell in cells)
                {
                    string getBaseLine = cell.GetAttribute("dominant-baseline");
                    string getFontSize = cell.GetAttribute("font-size");

                    if (getBaseLine == "central" && getFontSize.Contains("41"))
                    {
                        if (_subjectCount.ContainsKey(cell.Text.Trim()))
                        {
                            _subjectCount[cell.Text.Trim()] += 1;
                        }
                        else
                        {
                            _subjectCount[cell.Text.Trim()] = 1;
                        }
                    }
                }
                Console.WriteLine(_subjectCount);
            }
        }
    }
}
