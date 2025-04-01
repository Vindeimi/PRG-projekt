using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Modules.Browser;
using OpenQA.Selenium.DevTools.V131.WebAuthn;
using OxyPlot;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace edupageTest
{
    internal class Grades
    {
        private readonly DriverInitialization _driverInitialization;

        private List<KeyValuePair<string, GradeRecords>> _gradeRecords = new();
        private List<string> _degugList = new();
        private List<string> _degugList1 = new();
        private List<string> _degugList2 = new();
        private List<string> _degugList3 = new();

        public Grades(DriverInitialization driverInitialization)
        {
            _driverInitialization = driverInitialization;
        }


        public class GradeRecords
        {
            public string Subject { get; set; }
            public List<KeyValuePair<string, double>> Grades { get; set; }
            public string Diameter { get; set; }
        }

        public List<KeyValuePair<string, GradeRecords>> FindGrades()
        {
            _driverInitialization.Driver.Navigate().GoToUrl("https://sstebrno.edupage.org/znamky/?eqa=d2hhdD1zdHVkZW50dmlld2VyJnBvaGxhZD1wcmVobGFkJnpuYW1reV95ZWFyaWQ9MjAyNCZ6bmFta3lfeWVhcmlkX25zPTEmbmFkb2Jkb2JpZT1QMiZyb2tvYmRvYmllPTIwMjQlM0ElM0FQMiZkb1JxPTEmd2hhdD1zdHVkZW50dmlld2VyJnVwZGF0ZUxhc3RWaWV3PTA%3D");
            Console.WriteLine("Přechod na stránku docházky.");

            // Cekani na tabulku
            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".znamkyTable")));
            Console.WriteLine("Tabulka známek nalezena.");

            _driverInitialization.Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".znamkyTable > tbody:nth-child(2)")));

            return ExtractGradesData();
        }

        public List<KeyValuePair<string, GradeRecords>> ExtractGradesData()
        {
            var gradeTable = _driverInitialization.Driver.FindElement(By.CssSelector(".znamkyTable > tbody:nth-child(2)"));
            var gradeRows = gradeTable.FindElements(By.TagName("tr"));

            string subjectName = "";
            List<KeyValuePair<string, double>> gradesList = new();
            string diameter = "";

            GradeRecords currentSubject = null;

            foreach (var row in gradeRows) // tr 
            {
                try
                {
                    List<string> grades = new();
                    var rowClass = row.GetAttribute("class") ?? "";

                    if (rowClass.Contains("predmetRow"))
                    {
                        var tdElements = row.FindElements(By.TagName("td"));
                        string originalTitle = "";

                        foreach (var data in tdElements) // jednotlivé td
                        {
                            IList<IWebElement> spanElements = new List<IWebElement>();  
                            IList<IWebElement> bElements = data.FindElements(By.TagName("b"));

                            var elementClass = data.GetAttribute("class") ?? "";

                            spanElements = data.FindElements(By.CssSelector("span.tips > span.znZnamka"));

                            if (spanElements.Count > 0)
                            {
                                grades.Add(spanElements[0].Text);
                            }

                            if (bElements.Count > 0)
                            {
                                _degugList.Add(bElements[0].GetAttribute("outerHTML"));
                                subjectName = bElements[0].Text.Split("\r")[0];
                                _degugList1.Add(subjectName);
                            }

                            if (elementClass.Contains("znPriemerCell"))
                            {

                                var aElement = data.FindElements(By.TagName("a"));
                                if (aElement.Count <= 0)
                                {
                                    originalTitle = "0";
                                    diameter = "0";
                                }
                                else
                                {
                                    _degugList2.Add(aElement[0].GetAttribute("outerHTML"));
                                    originalTitle = aElement[0].GetAttribute("original-title"); // Tohle je prumer
                                    diameter = aElement[0].Text;
                                }
                                _degugList3.Add(originalTitle);
                                gradesList = GradeCalculate(grades, originalTitle);
                            }
                        }
                        if(gradesList.Count > 0)
                        {
                            _gradeRecords.Add(new KeyValuePair<string, GradeRecords>(subjectName, new GradeRecords
                            {
                                Subject = subjectName,
                                Grades = gradesList,
                                Diameter = diameter
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Chyba při zpracování řádku: {ex.Message}");
                }
            }

            return _gradeRecords;
        }

        private List<KeyValuePair<string, double>> GradeCalculate(List<string> gradeString, string originalTitle)
        {
            List<KeyValuePair<string, double>> gradesList = new();

            if (gradeString.Count == 0)
            {
                gradesList.Add(new KeyValuePair<string, double>("0", 0));
                return gradesList;
            }

            int currentIndex = 0;
            foreach (var grades in gradeString)
            {
                var rawGrade = grades.Split('/');
                double weight = 1.0;
                string grade = rawGrade[0];
                string numberPart = grade.Split(new char[] { '+', '-' })[0];

                bool isNum = double.TryParse(numberPart, NumberStyles.Any, CultureInfo.InvariantCulture, out double gradeValue);

                if (!isNum && (!grade.Contains("-") || !grade.Contains("+")))
                {
                    gradesList.Add(new KeyValuePair<string, double>(grade, 0));
                    continue;
                }

                if (grade.Length > 1)
                {
                    if (grade.Contains("-"))
                    {
                        gradeValue += 0.3;
                    }
                    else if (grade.Contains("+"))
                    {
                        gradeValue -= 0.3;
                    }
                }
                else if (grade.Length == 1 && (grade.Contains("-") || grade.Contains("+")))
                {
                    gradeValue = 0;
                }

                double[] weights = ExtractWeights(originalTitle);
               
                if (currentIndex < weights.Length)
                {
                    weight = weights[currentIndex];
                    currentIndex++;
                }

                gradesList.Add(new KeyValuePair<string, double>(gradeValue.ToString(), weight));
            }

            return gradesList;
        }

        private double[] ExtractWeights(string originalTitle)
        {
            Match match = Regex.Match(originalTitle, @"\((.*?)\)\s*/\s*\((.*?)\)"); // NEUPRAVOVAT REGEX !!!

            if (!match.Success)
            {
                return new double[] { 1.0 };
            }

            string denominatorPart = match.Groups[2].Value;

            // Rozdělit složený výraz podle `+`
            string[] weightParts = denominatorPart.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(w => w.Trim())
                                                  .ToArray();

            // Parsování čísel s podporou různých desetinných oddělovačů
            double[] weights = weightParts.Select(w => double.TryParse(w, NumberStyles.Any, CultureInfo.InvariantCulture, out double val) ? val : 1.0).ToArray();

            return weights;
        }
    }
}