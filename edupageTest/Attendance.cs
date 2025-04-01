using Microsoft.VisualBasic;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Modules.Browser;
using OpenQA.Selenium.DevTools.V131.Console;
using OpenQA.Selenium.DevTools.V132.CSS;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using static edupageTest.Date;
using static edupageTest.Schedule;

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
        private Date _date;
        private Schedule _schedule;
        private Dictionary<string, AttendanceRecords> _attendanceData = new();
        private readonly DriverInitialization _driverInitialization;
        private Dictionary<int, (DateTime, DateTime)> _semesterDate = new();
        private List<string> _semesterDebugList = new();
        private DateTime _today = DateTime.Now;
        public int SemesterType = 1;

        public Attendance(DriverInitialization driverInitialization)
        {
            _driverInitialization = driverInitialization;
        }

        #region Attendance Setup

        public Dictionary<string, AttendanceRecords> FindAttendance()
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

            return ExtractAttendanceData();
        }
        #endregion

        #region Attendace Data Extract

        public Dictionary<string, AttendanceRecords> ExtractAttendanceData()
        {

            // Zisk body z tabulky dat 
            var tBody = _driverInitialization.Driver.FindElement(By.CssSelector(".dt-container > div:nth-child(2) > table:nth-child(1) > tbody:nth-child(2)"));
            var tHead= _driverInitialization.Driver.FindElement(By.CssSelector("div.asc-dt:nth-child(1) > table:nth-child(1) > thead:nth-child(2)"));
            string tBodyHtml = tBody.GetAttribute("outerHTML");
            var semNum = 0;

            // Hledani tr prvku
            IList<IWebElement> tRows = tBody.FindElements(By.TagName("tr"));
            var tHeadRow = tHead.FindElement(By.TagName("tr"));
            IList<IWebElement> thCells = tHeadRow.FindElements(By.TagName("th"));

            foreach(var cell in thCells)
            {
                try
                {
                    var spanElement = cell.GetAttribute("innerHTML");
                    string pattern = @"(\d{1,2}\. \d{1,2}\. \d{4}|\d{1,2}\. \d{1,2})-(\d{1,2}\. \d{1,2}\. \d{4})";
                    var match = Regex.Match(spanElement, pattern);

                    _semesterDebugList.Add(spanElement);
                    if (match.Success)
                    {
                        DateTime startDate;
                        DateTime endDate;

                        // Převod textu na DateTime
                        if (DateTime.TryParseExact(match.Groups[1].Value, "d. M. yyyy", null, System.Globalization.DateTimeStyles.None, out startDate) &&
                            DateTime.TryParseExact(match.Groups[2].Value, "d. M. yyyy", null, System.Globalization.DateTimeStyles.None, out endDate))
                        {
                            semNum++;
                            _semesterDate[semNum] = (startDate, endDate); // Uložení jako DateTime
                        }
                        else
                        {
                            Console.WriteLine("Nepodařilo se převést datum.");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Element nenalezen");
                }
            }
            if (_semesterDate.Count >= 2)
            {
                if (_semesterDate[1].Item2 == _semesterDate[2].Item1 || _semesterDate[2].Item1 == DateTime.MinValue || _semesterDate[2].Item2 == DateTime.MinValue)
                {
                    SemesterType = 1;
                }
                else if (_today.Date > new DateTime(_today.Year, 1, 30) || _semesterDate[2].Item1 > _semesterDate[1].Item2)
                {
                    SemesterType = 2;
                }
                else
                {
                    SemesterType = 1;
                }
            }

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
                    _attendanceData[rowData[0]+"1"] = new AttendanceRecords()
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
                }
            }
            return _attendanceData;
        }
        #endregion

        #region Výpočet Absence

        public Dictionary<string, (int Max20, int Max30)> CalculateAbsenceLimits(
            List<PermanentScheduleInfo> permanentTimeTable,
            List<Week> schoolDays)
        {
            Dictionary<string, int> subjectHours = new();

            foreach (var day in schoolDays)
            {
                string dayKey;

                if (day.WeekTypeNum % 2 == 0)
                {
                    dayKey = GetDayKey(day.EvenWeek.DateTime.DayOfWeek, day.WeekTypeNum);
                }
                else
                {
                    dayKey = GetDayKey(day.OddWeek.DateTime.DayOfWeek, day.WeekTypeNum);
                }
                var schedule = permanentTimeTable.FirstOrDefault(x => x.Day == dayKey);

                if (schedule != null)
                {
                    foreach (var subject in schedule.SubjectInfo)
                    {
                        var hours = subject.Count;

                        if (subjectHours.ContainsKey(subject.Name))
                        {
                            subjectHours[subject.Name] += hours;
                        }
                        else
                        {
                            subjectHours[subject.Name] = hours;
                        }
                    }
                }
            }

            Console.WriteLine(subjectHours);
            return CalculateLimits(subjectHours);
        }

        private string GetDayKey(DayOfWeek dayOfWeek, int weekType)
        {
            string dayPrefix = dayOfWeek switch
            {
                DayOfWeek.Monday => "Po",
                DayOfWeek.Tuesday => "Út",
                DayOfWeek.Wednesday => "St",
                DayOfWeek.Thursday => "Čt",
                DayOfWeek.Friday => "Pá",
                _ => throw new ArgumentException("Neplatný den v týdnu")
            };

            return $"{dayPrefix}{(weekType == 1 ? "1" : "2")}";
        }

        private Dictionary<string, (int Max20, int Max30)> CalculateLimits(
            Dictionary<string, int> subjectHours)
        {
            var limits = new Dictionary<string, (int, int)>();

            foreach (var subject in subjectHours)
            {
                var max20 = (int)Math.Floor(subject.Value * 0.2);
                var max30 = (int)Math.Floor(subject.Value * 0.3);

                limits.Add(subject.Key, (max20, max30));
            }

            return limits;
        }
        #endregion
    }
}
