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
        private Border _menuBorder;
        private Schedule _schedule;
        private Date _date;
        public string Username, Password;
        public bool CanLogin = false;
        public bool LoggedIn = false;

        public MainSetup(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public async Task SetupAsync()
        {
            if (MainWindow.Instance != null)
            {
                _menuBorder = MainWindow.Instance.MenuBorder;
            }


            await Task.Delay(500);

            #region Inicializace driveru
            if (_driver == null)
            {
                _driver = new DriverInitialization();
                _driver.DriverInit();
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
                _design = new Design(_menuBorder);
                #endregion

                #region Rozvrh
                _schedule = new Schedule(_driver);
                var permanentTimeTable = await Task.Run(() => _schedule.FindPermanentTimeTable());
                #endregion

                #region Datum
                _date = new Date();
                await _date.AutoDetectRegionAsync();
                var holidays = _date.GetHolidays(DateTime.Now.Year);
                var schoolDays = _date.GetSchoolDays(DateTime.Now.Year);
                var weeks = _date.GetWeekType(DateTime.Now.Year);
                #endregion
            }
            CanLogin = _login.CanLogin;

        }
    }

}
