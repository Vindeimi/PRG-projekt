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
        public void Setup()
        {
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
            _login.LoginAndFetchAttendance();

            #endregion

            #region Attendance

            _attendance= new Attendance(_driver);
            _attendance.FindAttendance();
            #endregion


        }

    }
}
