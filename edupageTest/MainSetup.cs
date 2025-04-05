using edupageTest.Views;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace edupageTest
{
    internal class MainSetup
    {
        private DriverInitialization _driver;
        private Login _login;
        private Attendance _attendance;
        private Design _design;
        private Graph _graph;
        private Border _menuBorder;
        private Schedule _schedule;
        private Date _date;
        //private Canvas _canvas;
        private Grades _grades;

        public string Username { get; set; }
        public string Password { get; set; }
        public bool CanLogin { get; set; }
        public bool LoggedIn { get; set; }

        public MainSetup(string username, string password/*, Canvas canvas*/ )
        {
            Username = username;
            Password = password;
            AppContext.Setup = this;
        }

        public async Task SetupAsync()
        {
            //if (MainWindow.Instance != null)
            //{
            //    _menuBorder = AppContext.MenuBorder;
            //}

            await Task.Delay(500);

            #region Inicializace driveru

            if (_driver == null)
            {
                _driver = new DriverInitialization();
                _driver.DriverInit();
                AppContext.Driver = _driver;
            }
            #endregion

            #region Login

            if (!LoggedIn)
            {
                _login = new Login(_driver);
                await Task.Run(() => _login.LoginAndFetchAttendance(Username, Password));
            }
            #endregion

            if (_login.CanLogin)
            {
                #region Design

                _design = new Design(AppContext.MenuBorder);
                #endregion

                #region Rozvrh

                _schedule = new Schedule(_driver);
                var permanentTimeTable = await Task.Run(() => _schedule.FindPermanentTimeTable());
                var subjectShortcut = _schedule.SubjectShortcut;
                #endregion

                #region Datum

                _date = new Date();
                await _date.AutoDetectRegionAsync();
                var holidays = _date.GetHolidays(DateTime.Now.Year);
                var schoolDays = _date.GetSchoolDays(DateTime.Now.Year);
                var weeks = _date.GetWeekType(DateTime.Now.Year);
                #endregion

                #region Attendance

                _attendance = new Attendance(_driver);
                Dictionary<string, AttendanceRecords> attendanceData = _attendance.FindAttendance();
                var absenceLimit = _attendance.CalculateAbsenceLimits(permanentTimeTable, weeks);
                #endregion

                #region Graf

                _graph = new Graph(attendanceData, _attendance.SemesterType, AppContext.GraphCanvas, subjectShortcut);
                Thread.Sleep(300);
                _graph.DrawMainMenuGraph();
                #endregion

                #region Zn�mky

                _grades = new Grades(_driver);
                AppContext.Grades = _grades.FindGrades();
                #endregion
            }
            CanLogin = _login.CanLogin;

        }
    }

}
