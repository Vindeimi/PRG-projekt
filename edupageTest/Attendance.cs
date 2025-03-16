using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V131.Console;
using OpenQA.Selenium.DevTools.V132.CSS;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace edupageTest
{
    #region Attendance Data Classes

    public class AttendanceRecords
    {
        public string Subject { get; set; }
        public Semester FirstSemester { get; set; } = new();
        public Semester SecondSemester { set; get; } = new();
        public Semester Total { get; set; } = new();
    }

    public class Semester
    {
        public string Missed { get; set; }
        public string Total { get; set; }
        public string Percent { get; set; }
    }
    #endregion

    internal class Attendance
    {
        private Dictionary<string, AttendanceRecords> _attendanceData = new();
        private readonly DriverInitialization _driverInitialization;

        public Attendance(DriverInitialization driverInitialization)
        {
            _driverInitialization = driverInitialization;
        }

        #region Attendance Setup

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

            // Zisk dat z tabulky
            string scheduleHtml = scheduleTable.GetAttribute("outerHTML");
            Console.WriteLine($"HTML tabulky s rozvrhem:\n{scheduleHtml}");

            ExtractAttendanceData();
        }
        #endregion

        #region Attendace Data Extract

        public void ExtractAttendanceData()
        {

            // Zisk body z tabulky dat 
            var tBody = _driverInitialization.Driver.FindElement(By.CssSelector(".dt-container > div:nth-child(2) > table:nth-child(1) > tbody:nth-child(2)"));
            string tBodyHtml = tBody.GetAttribute("outerHTML");

            // Hledani tr prvku
            IList<IWebElement> tRows = tBody.FindElements(By.TagName("tr"));

            // Hledani td prvku
            foreach (var element in tRows)
            {
                IList<IWebElement> cells = element.FindElements(By.TagName("td"));

                List<string> rowData = new List<string>();
                string correctText = "";

                foreach (var cell in cells)
                {

                    // Odendani procent
                    if (cell.Text.Trim().Contains('%'))
                    {
                        correctText = cell.Text.Trim().Substring(0, cell.Text.Count() - 1);
                    }
                    else
                    {
                        correctText = cell.Text.Trim();
                    }
                    rowData.Add(correctText);
                }

                /// Pridani do class
                /// 
                /// Pridani do class
                 
                // Kontrola stejneho klice
                if (_attendanceData.ContainsKey(rowData[0]))
                {
                    _attendanceData[rowData[0]+"SK"] = new AttendanceRecords()
                    {
                        Subject = rowData[0],
                        FirstSemester = new Semester { Missed = rowData[1], Total = rowData[2], Percent = rowData[3] },
                        SecondSemester = new Semester { Missed = rowData[4], Total = rowData[5], Percent = rowData[6] },
                        Total = new Semester { Missed = rowData[7], Total = rowData[8], Percent = rowData[9] },
                    };
                    Console.WriteLine(rowData);
                }

                // Pokud klic neexistuje
                else
                {
                    _attendanceData[rowData[0]] = new AttendanceRecords()
                    {
                        Subject = rowData[0],
                        FirstSemester = new Semester { Missed = rowData[1], Total = rowData[2], Percent = rowData[3] },
                        SecondSemester = new Semester { Missed = rowData[4], Total = rowData[5], Percent = rowData[6] },
                        Total = new Semester { Missed = rowData[7], Total = rowData[8], Percent = rowData[9] },
                    };
                    Console.WriteLine(rowData);
                }
            }
        }
        #endregion
    }
}
