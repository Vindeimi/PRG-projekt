﻿using OpenQA.Selenium.DevTools.V131.DOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Security.Principal;
using System.Text.Json;

namespace edupageTest
{
    public enum CzechRegion
    {
        /// <summary>
        /// UZ NIKDY VIC ODKRUTI OCI OD TOHOTO SOUBORU, TOHLE JE CISTA BOLEST, WIKIPEDIE A JEN ALGORITMY. KDYZ JSEM TEN KOD PSAL TAK JSEM VEDEL NA PUL CO DELAM, TED TO VI JEN BUH.
        /// </summary>


        // Kody kraju podle NUTS 3
        #region Kody kraju

        [RegionInfo("CZ010", "Praha")]
        Praha = 10,

        [RegionInfo("CZ020", "Středočeský kraj")]
        Stredocesky = 20,

        [RegionInfo("CZ031", "Jihočeský kraj")]
        Jihocesky = 31,

        [RegionInfo("CZ032", "Plzeňský kraj")]
        Plzensky = 32,

        [RegionInfo("CZ041", "Karlovarský kraj")]
        Karlovarsky = 41,

        [RegionInfo("CZ042", "Ústecký kraj")]
        Ustecky = 42,

        [RegionInfo("CZ051", "Liberecký kraj")]
        Liberecky = 51,

        [RegionInfo("CZ052", "Královéhradecký kraj")]
        Kralovehradecky = 52,

        [RegionInfo("CZ053", "Pardubický kraj")]
        Pardubicky = 53,

        [RegionInfo("CZ063", "Kraj Vysočina")]
        Vysocina = 63,

        [RegionInfo("CZ064", "Jihomoravský kraj")]
        Jihomoravsky = 64,

        [RegionInfo("CZ071", "Olomoucký kraj")]
        Olomoucky = 71,

        [RegionInfo("CZ072", "Zlínský kraj")]
        Zlinsky = 72,

        [RegionInfo("CZ080", "Moravskoslezský kraj")]
        Moravskoslezsky = 80
    }
    #endregion

    [AttributeUsage(AttributeTargets.Field)]
    public class RegionInfoAttribute : Attribute
    {
        public string NutsCode { get; }
        public string OfficialName { get; }

        public RegionInfoAttribute(string nutsCode, string officialName)
        {
            NutsCode = nutsCode;
            OfficialName = officialName;
        }
    }

    public class RegionDetector
    {
        #region Kody Regionu

        private static readonly Dictionary<int, CzechRegion> _postalCodeMapping = new()
        {
            { 10, CzechRegion.Praha },
            { 20, CzechRegion.Stredocesky },
            { 31, CzechRegion.Jihocesky },
            { 32, CzechRegion.Plzensky },
            { 41, CzechRegion.Karlovarsky },
            { 42, CzechRegion.Ustecky },
            { 51, CzechRegion.Liberecky },
            { 52, CzechRegion.Kralovehradecky },
            { 53, CzechRegion.Pardubicky },
            { 63, CzechRegion.Vysocina },
            { 64, CzechRegion.Jihomoravsky },
            { 71, CzechRegion.Olomoucky },
            { 72, CzechRegion.Zlinsky },
            { 80, CzechRegion.Moravskoslezsky }
        };
        #endregion

        // Parsuje PSC
        public CzechRegion DetectFromPostalCode(string postalCode)
        {
            if (int.TryParse(postalCode.Substring(0, 2), out int regionCode))
            {
                return _postalCodeMapping.TryGetValue(regionCode, out var region) ? region : CzechRegion.Jihomoravsky;
            }
            return CzechRegion.Jihomoravsky;
        }

        #region Zisk IP Adresy Pro Detekci Regionu

        public async Task<CzechRegion> DetectFromIpAsync()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetStringAsync("http://ip-api.com/json/");
                var data = JsonSerializer.Deserialize<IpApiResponse>(response);

                return data.Region switch
                {
                    "Prague" => CzechRegion.Praha,
                    "Central Bohemian" => CzechRegion.Stredocesky,
                    "South Bohemian" => CzechRegion.Jihocesky,
                    "Plzeň" => CzechRegion.Plzensky,
                    "Karlovy Vary" => CzechRegion.Karlovarsky,
                    "Ústí nad Labem" => CzechRegion.Ustecky,
                    "Liberec" => CzechRegion.Liberecky,
                    "Hradec Králové" => CzechRegion.Kralovehradecky,
                    "Pardubice" => CzechRegion.Pardubicky,
                    "Vysočina" => CzechRegion.Vysocina,
                    "South Moravian" => CzechRegion.Jihomoravsky,
                    "Olomouc" => CzechRegion.Olomoucky,
                    "Zlín" => CzechRegion.Zlinsky,
                    "Moravian-Silesian" => CzechRegion.Moravskoslezsky,
                    _ => CzechRegion.Jihomoravsky
                };
            }
            catch
            {
                return CzechRegion.Jihomoravsky;
            }
        }
        #endregion

        private class IpApiResponse
        {
            public string Region { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
        }
    }

    internal class Date
    {
        private CzechRegion _region;
        private readonly RegionDetector _regionDetector = new();

        public Date(CzechRegion region = CzechRegion.Jihomoravsky)
        {
            _region = region;
        }

        public async Task AutoDetectRegionAsync()
        {
            _region = await _regionDetector.DetectFromIpAsync();
        }

        #region Prvni Skolni Den

        public DateTime GetFirstSchoolDay(int year)
        {
            DateTime firstDay = new DateTime(year, 9, 1);

            // Zmena pro zacatek o vikendu
            while (firstDay.DayOfWeek == DayOfWeek.Saturday ||
                   firstDay.DayOfWeek == DayOfWeek.Sunday)
            {
                firstDay = firstDay.AddDays(1);
            }

            return firstDay;
        }
        #endregion

        #region Pocet Skolnich Dnu

        public IEnumerable<DateTime> GetSchoolDays(int year)
        {
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            if (AppContext.SemesterType == 1)
            {
                start = new DateTime(year-1, 9, 1);
                end = new DateTime(year, 1, 26);
            }
            else if (AppContext.SemesterType == 2)
            {
                start = new DateTime(year, 1, 26);
                end = new DateTime(year, 6, 30);
            }
            else
            {
                start = new DateTime(year - 1, 9, 1);
                end = new DateTime(year, 1, 26);
            }

            var allDays = GenerateDateRange(start, end);
            var holidays = GetHolidays(year);

            return allDays
                .Where(d => d.DayOfWeek != DayOfWeek.Saturday &&
                            d.DayOfWeek != DayOfWeek.Sunday)
                .Except(holidays);
        }
        #endregion

        #region Vsechny Prazdniny

        public List<DateTime> GetHolidays(int year)
        {
            List<DateTime> holidays = new();

            // Podzimni prazdniny
            holidays.AddRange(GetAutumnHolidays(year-1));

            // Vanocni prazdniny
            holidays.AddRange(GetChristmasHolidays(year-1));

            // Pololetni prazdniny
            holidays.AddRange(GetMidTermHolidays(year));

            // Jarni prazdniny
            holidays.AddRange(GetSpringHolidays(year));

            // Velikonocni prazdniny
            holidays.AddRange(GetEasterHolidays(year));

            // Statni prazdniny
            holidays.AddRange(GetPublicHolidays(year));

            // Letni prazdniny
            // if (year == DateTime.Now.Year) -> kdyby byly chyby 
            holidays.AddRange(GetSummerHolidays(year));

            return holidays.Distinct().ToList();
        }
        #endregion

        #region Jarni Prazdniny

        private List<DateTime> GetSpringHolidays(int year)
        {
            var base2024 = new Dictionary<CzechRegion, DateTime>
    {
        { CzechRegion.Praha, new DateTime(2024, 2, 5) },
        { CzechRegion.Jihomoravsky, new DateTime(2024, 2, 5) },
        { CzechRegion.Stredocesky, new DateTime(2024, 2, 19) },
        { CzechRegion.Ustecky, new DateTime(2024, 2, 12) },
        { CzechRegion.Liberecky, new DateTime(2024, 2, 26) },
        { CzechRegion.Kralovehradecky, new DateTime(2024, 2, 19) },
        { CzechRegion.Jihocesky, new DateTime(2024, 3, 11) },
        { CzechRegion.Plzensky, new DateTime(2024, 2, 5) },
        { CzechRegion.Karlovarsky, new DateTime(2024, 2, 12) },
        { CzechRegion.Pardubicky, new DateTime(2024, 3, 11) },
        { CzechRegion.Vysocina, new DateTime(2024, 2, 26) },
        { CzechRegion.Olomoucky, new DateTime(2024, 3, 4) },
        { CzechRegion.Zlinsky, new DateTime(2024, 2, 19) },
        { CzechRegion.Moravskoslezsky, new DateTime(2024, 2, 12) }
    };

            // 1. Posun o požadovaný počet týdnů v rámci roku 2024
            int yearDifference = year - 2024;
            DateTime shiftedDate = base2024[_region].AddDays(yearDifference * 7);

            // 2. Vytvořit datum v cílovém roce se stejným měsícem a dnem
            DateTime targetDate = new DateTime(year, shiftedDate.Month, shiftedDate.Day);

            // 3. Najít pondělí v TOMTÉŽ TÝDNU jako targetDate
            int daysToSubtract = (targetDate.DayOfWeek == DayOfWeek.Monday) ? 0 :
                                (targetDate.DayOfWeek - DayOfWeek.Monday + 7) % 7;
            DateTime startDate = targetDate.AddDays(-daysToSubtract);

            return GenerateDateRange(startDate, startDate.AddDays(6)).ToList();
        }


        // Generuje rozsah data
        private static IEnumerable<DateTime> GenerateDateRange(DateTime start, DateTime end)
        {
            for (DateTime date = start; date <= end; date = date.AddDays(1))
                yield return date;
        }
        #endregion


        #region Ostatni Prazdniny
        // Pomocne metody pro ostatni prazdniny
        private List<DateTime> GetAutumnHolidays(int year)
        {
            DateTime autumnStart = new DateTime(year, 10, 26);
            DateTime autumnEnd = new DateTime(year, 10, 29);
            return GenerateDateRange(autumnStart, autumnEnd).ToList();
        }

        private List<DateTime> GetChristmasHolidays(int year)
        {
            DateTime xmasStart = new DateTime(year, 12, 23);
            DateTime xmasEnd = new DateTime(year, 1, 2).AddYears(1); // Presah do noveho roku
            return GenerateDateRange(xmasStart, xmasEnd).ToList();
        }

        private List<DateTime> GetMidTermHolidays(int year)
        {
            return new List<DateTime> { new DateTime(year, 1, 31) };
        }

        private List<DateTime> GetEasterHolidays(int year)
        {
            DateTime easter = CalculateEaster(year);
            return new List<DateTime>
            {
                easter.AddDays(-3), // Zeleny čtvrtek
                easter.AddDays(-2), // Velky pátek
                easter.AddDays(1)   // Velikonocni pondeli
            };
        }

        private List<DateTime> GetPublicHolidays(int year)
        {
            return new List<DateTime>
            {
                new DateTime(year, 1, 1),    // Novy rok
                new DateTime(year, 5, 1),     // Svatek prace
                new DateTime(year, 5, 8),     // Den osvobozeni
                new DateTime(year, 7, 5),     // Cyril a Metodej
                new DateTime(year, 7, 6),     // Jan Hus
                new DateTime(year - 1, 9, 28),    // Den ceske statnosti
                new DateTime(year - 1, 10, 28),   // Vznik CSR
                new DateTime(year - 1, 11, 17),   // Den boje za svobodu
                new DateTime(year - 1, 12, 24),   // Stedry den
                new DateTime(year - 1, 12, 25),   // 1. svatek vanocni
                new DateTime(year - 1, 12, 26)    // 2. svatek vanocni
            };
        }

        private List<DateTime> GetSummerHolidays(int year)
        {
            DateTime summerStart = new DateTime(year, 7, 1);
            DateTime summerEnd = new DateTime(year, 8, 31);
            return GenerateDateRange(summerStart, summerEnd).ToList();
        }

        // Vypocet Velikonoc (metoda podle Gaussova algoritmu) -> ABSOLUTNE NEVIM JAK TO FUNGUJE -> PROSTE TO JE VZOREC -> RADSI NESAHAT!!!!
        private static DateTime CalculateEaster(int year)
        {
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 1;
            return new DateTime(year, month, day);
        }
        #endregion

        #region Výpočet Týdne

        public class Week()
        {
            public int WeekTypeNum { get; set; }
            public OddWeek OddWeek = new();
            public EvenWeek EvenWeek = new();
        }

        public class OddWeek()
        {
            public DateTime DateTime { get; set; }
            public int WeekNumber { get; set; }
            public DayOfWeek Day { get; set; }
        }

        public class EvenWeek()
        {
            public DateTime DateTime { get; set; }
            public int WeekNumber { get; set; }
            public DayOfWeek Day {  get; set; }
        }

        public List<Week> GetWeekType(int year)
        {
            var schoolDays = GetSchoolDays(year).ToArray();
            List<Week> weeks = new List<Week>();

            foreach(var day in schoolDays)
            {
                int weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(day, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                if (weekNumber % 2 == 0)
                {
                    weeks.Add(new Week { EvenWeek = { Day = day.DayOfWeek, DateTime = day, WeekNumber = weekNumber}, WeekTypeNum = 2});
                }
                else
                {
                    weeks.Add(new Week { OddWeek = { Day = day.DayOfWeek, DateTime = day, WeekNumber = weekNumber }, WeekTypeNum = 1});
                }
            }
            return weeks;
        }
        #endregion
    }
}   