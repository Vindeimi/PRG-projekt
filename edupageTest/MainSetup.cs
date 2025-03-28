using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //credentials field
        private string _username;
        private string _password;
        public async void Setup(string username, string password)
        {
            _username = username;
            _password = password;

            if (MainWindow.Instance != null)
            {
                _menuBorder = MainWindow.Instance.MenuBorder;
            }

            #region Inicializace driveru

            //Inicliazice hlavniho driveru
            _driver = new DriverInitialization();
            _driver.DriverInit();
            #endregion

            #region Design

            _design = new Design(_menuBorder);
            #endregion

            #region Login

            //Login, pak to bude jinak zatim jsem to dal sem
            _login = new Login(_driver);
            _login.LoginAndFetchAttendance(_username, _password);

            #endregion

            //#region Attendance

            //_attendance= new Attendance(_driver);
            //_attendance.FindAttendance();
            //#endregion

            #region Rozvrh

            _schedule = new Schedule(_driver);
            _schedule.FindPermanentTimeTable();
            #endregion

            #region Datum
            _date = new Date();
            await _date.AutoDetectRegionAsync();

            var holidays = _date.GetHolidays(DateTime.Now.Year);

            // PAK MUSIME PRIDAT IF JESTLI JE PRVNI NEBO DRUHE POLOLETI -> JESTLI JE PRVNI NOW.YEAR+1 JESTLI 2. TAK NOW.YEAR
            var schoolDays = _date.GetSchoolDays(DateTime.Now.Year);
            #endregion
        }
    }
}
