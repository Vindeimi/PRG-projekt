﻿using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V131.DOM;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace edupageTest
{
    internal class Schedule
    {
        private readonly DriverInitialization _driverInitialization;

        private Dictionary<string, int> _subjectCount = new();
        private Dictionary<string, ScheduleInfo> _subjectSchedule = new();
        private Dictionary<string, RectInfo> _rectInfo = new();
        private List<PermanentScheduleInfo> _permanentSchedule = new();
        private List<string> _outerRecList = new();
        private Dictionary<string, SubjectInfo> _subjects;

        public Dictionary<string, string> SubjectShortcut = new (); // Title je key a Name (zkratka) je value
        public Dictionary<string, SubjectInfo> Subjects => _subjects;

        public Schedule(DriverInitialization driverInitialization)
        {
            _driverInitialization = driverInitialization;
        }

        #region Main Subject Info Class

        public class PermanentScheduleInfo()
        {
            public string Day {  get; set; }
            public int SubjectCount {  get; set; }
            public double PositionY {  get; set; }
            public double TextPositionY { get; set; }
            public double PositionX { get; set; }
            public int SchoolHours {  get; set; }
            public List<OneSubjectInfo> SubjectInfo = new();
        }
        #endregion

        #region Side Subjet Info Classy

        public class SubjectInfo()
        {
            public string Name { get; set; }
            public List<DayOfWeek> Days { get; set; } = new();
            public int HoursPerWeek { get; set; }
            public int TotalHours {  get; set; }
            public int Max20Absence {  get; set; }
            public int Max30Absence { get; set; }

            public bool OccursOnDay(DayOfWeek day) => Days.Contains(day);
        }

        private class ScheduleInfo()
        {
            public string Day { get; set; }
            public List<string> Subjects { get; set; }
            public int SubjectCount { get; set; }
            public double PositionY { get; set; }
        }

        private class RectInfo()
        {
            public double Width { get; set; }
            public double Height { get; set; }
            public double PositionY { get; set; }
            public List<RecSubjectInfo> SubInfo { get; set; } = new();
        }

        private class RecSubjectInfo()
        {
            public string Title { get; set; }
            public int SubjectCount { get; set; }
            public double PositionX { get; set; }
        }

        public class OneSubjectInfo()
        {
            public string Title { get; set; }
            public string Name { get; set; }
            public int Count { get; set; }
            public double PositionX { get; set; }
        }
        public void FindTimetable()

        {
            _driverInitialization.Driver.Navigate().GoToUrl("https://sstebrno.edupage.org/dashboard/eb.php?mode=timetable");
            Console.WriteLine("Přechod na stránku rozvrhu.");

            // Cekani na rozvrh
            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".print-nobreak")));
            Console.WriteLine("Rozvrh nalezen.");
        }

        #endregion

        #region Find Permanent TimeTable
        public List<PermanentScheduleInfo> FindPermanentTimeTable()
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

            return ExtractPermanentTimeTableData();
        }
        #endregion

        #region Extract Permanent TimeTable Data

        private List<PermanentScheduleInfo> ExtractPermanentTimeTableData()
        {
            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".print-sheet > svg:nth-child(1) > g:nth-child(2)")));
            var permanentTableData = _driverInitialization.Driver.FindElement(By.CssSelector(".print-sheet > svg:nth-child(1) > g:nth-child(2)"));
            string permanentTableDataHtml = permanentTableData.GetAttribute("outerHTML");
            Console.WriteLine(permanentTableDataHtml) ;
            string[] dayArr = ["Po1", "Út1", "St1", "Čt1", "Pá1", "Po2", "Út2", "St2", "Čt2", "Pá2"];
            double prevRecPos = 0;
            double prevPosY = 0;

            int rectNum = 0;
            IList<IWebElement> gElement = _driverInitialization.Driver.FindElements(By.TagName("g"));

            foreach (var element in gElement)
            {
                IList<IWebElement> cells = element.FindElements(By.TagName("text"));
                IList<IWebElement> rects = element.FindElements(By.TagName("rect"));
                #region Zjisteni rectu -> pole

                foreach (var rect in rects)
                {
                    string widthValue = rect.GetAttribute("width");
                    string heightValue = rect.GetAttribute("height");
                    string yValue = rect.GetAttribute("y");
                    string xValue = rect.GetAttribute("x");
                    string textTitle = "";

                    var titleElement = rect.GetAttribute("innerHTML");                    
                    var match = Regex.Match(titleElement, @"<title>(.*?)\r\n");
            
                    if (match.Success)
                    {
                        textTitle = match.Groups[1].Value; // Získání první skupiny (text mezi <title> a \r\n)
                    }
                    _outerRecList.Add(textTitle); // List pro debug

                    if (double.TryParse(widthValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double getWidthValue) &&
                        double.TryParse(heightValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double getHeightValue) &&
                        double.TryParse(yValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double getPositionY) && 
                        double.TryParse(xValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double getPositionX))
                    {
                        if (getWidthValue <= 110 && getHeightValue >= 225 && getHeightValue < 400)
                        {
                            rectNum++;
                            _rectInfo[dayArr[rectNum - 1]] = new RectInfo()
                            {
                                Height = getHeightValue,
                                Width = getWidthValue,
                                PositionY = getPositionY,
                                SubInfo = new List<RecSubjectInfo>()
                            };
                        }
                        
                        else if (getWidthValue > 115 && getHeightValue > 110 && getHeightValue < 245)
                        {
                            rectNum++; 
                            var item = _rectInfo.FirstOrDefault(pair => pair.Value.PositionY == getPositionY);
                            if(/*prevRecPos != getPositionX  || prevPosY != item.Value.PositionY*/ textTitle != "") 
                            {
                                /// pokud x a y není stejné jak předchozí hodnoty
                                /// -> nahrazeno textTitlem, protože jeden z rectů nemá textTitle, který potřebujeme

                                if (getWidthValue >= 225)
                                {
                                    item.Value.SubInfo.Add(new RecSubjectInfo
                                    {
                                        Title = textTitle,
                                        PositionX = getPositionX,
                                        SubjectCount = (int)Math.Floor(getWidthValue / 120)  // Kontroluje kolik hodin je hned po sobe
                                    });  
                                }
                                
                                /// Teoreticky tohle je zbytecne,
                                /// protoze to nahore kontroluje subjectCount
                                /// pomoci dynamicke operace WidthValue / 120,
                                /// ale jistota je jistota, takže tady nechám else na 1 predmet

                                else
                                {
                                    item.Value.SubInfo.Add(new RecSubjectInfo
                                    {
                                        Title = textTitle,
                                        PositionX = getPositionX,
                                        SubjectCount = 1
                                    });
                                }
                                prevRecPos = getPositionX;
                            }
                            prevPosY = item.Value.PositionY;
                        }

                    }

                }
                #endregion

                #region Zjisteni cell -> textu

                foreach (var cell in cells)
                {
                    string getBaseLine = cell.GetAttribute("dominant-baseline");
                    string getFontSize = cell.GetAttribute("font-size");
                    string yValue = cell.GetAttribute("y");

                    if (double.TryParse(yValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double getLocationY))
                    {
                        if (getLocationY >= 600)
                        {
                            if (!double.TryParse(cell.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                            {
                                if (getBaseLine == "central" && !getFontSize.Contains("41"))
                                {
                                    if (_subjectSchedule.ContainsKey(cell.Text.Trim() + 1))
                                    {
                                        _subjectSchedule[cell.Text.Trim() + 2] = new ScheduleInfo()
                                        {
                                            Day = cell.Text.Trim(),
                                            PositionY = getLocationY,
                                            SubjectCount = 0,
                                            Subjects = new List<string>(),
                                        };
                                    }
                                    else
                                    {
                                        _subjectSchedule[cell.Text.Trim() + 1] = new ScheduleInfo()
                                        {
                                            Day = cell.Text.Trim(),
                                            PositionY = getLocationY,
                                            SubjectCount = 0,
                                            Subjects = new List<string>(),

                                        };
                                    };
                                }
                                else if (getBaseLine == "central" && getFontSize.Contains("41"))
                                {
                                    var item = _subjectSchedule.FirstOrDefault(pair => pair.Value.PositionY == getLocationY);
                                    if (!string.IsNullOrEmpty(item.Key))
                                    {
                                        item.Value.Subjects.Add(cell.Text.Trim());
                                        item.Value.SubjectCount++;
                                    }
                                }
                            }
                        }
                    }
                } 
                for (int i = 0; i < _subjectSchedule.Count;i++)
                {
                    List<OneSubjectInfo> oneSubjectInfo = new();
                    double posX = 0;
                    int subCount = 0;
                    for (int n = 0; n < _subjectSchedule.ElementAt(i).Value.Subjects.Count(); n++)
                    {
                        oneSubjectInfo.Add(new OneSubjectInfo()
                        {
                            Title = _rectInfo.ElementAt(i).Value.SubInfo[n].Title,
                            Name = _subjectSchedule.ElementAt(i).Value.Subjects[n],
                            Count = _rectInfo.ElementAt(i).Value.SubInfo[n].SubjectCount,
                            PositionX = _rectInfo.ElementAt(i).Value.SubInfo.ElementAt(n).PositionX,
                        });
                        SubjectShortcut[_rectInfo.ElementAt(i).Value.SubInfo[n].Title] = _subjectSchedule.ElementAt(i).Value.Subjects[n];
                        subCount += _rectInfo.ElementAt(i).Value.SubInfo[n].SubjectCount;
                        posX = _rectInfo.ElementAt(i).Value.SubInfo.ElementAt(n).PositionX;
                    }
                    _permanentSchedule.Add(new PermanentScheduleInfo()
                    {
                        Day = _subjectSchedule.ElementAt(i).Key,
                        PositionX = posX,
                        PositionY = _rectInfo.ElementAt(i).Value.PositionY,
                        TextPositionY = _subjectSchedule.ElementAt(i).Value.PositionY,
                        SubjectCount = _rectInfo.ElementAt(1).Value.SubInfo.Count(),
                        SchoolHours = subCount,
                        SubjectInfo = oneSubjectInfo,
                    });                    
                }
                #endregion

                Console.WriteLine(_subjectCount);
            }
            AppContext.SubjectShortcut = SubjectShortcut;
            return _permanentSchedule;
        }
        #endregion
    }
}
