﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using System.Security.Cryptography;
using OpenQA.Selenium.Remote;

namespace edupageTest
{
    public partial class MainWindow : Window
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        //Side menu inicializace
        private Design _design;

        public MainWindow()
        {
            InitializeComponent();
            Setup();
            LoginAndFetchAttendance();

            //Design
            _design = new Design(MenuBorder);
        }

        private void Setup()
        {

            var options = new EdgeOptions();

            ///
            /// Nastaveni pak se zmeni
            ///

            //options.AddArgument("--headless=new");
            //var service = EdgeDriverService.CreateDefaultService(@"H:\PRG C# 3\WPFpokracovaci\edupageRemake\PRG-projekt\msedgedriver.exe");
            //service.LogPath = "msedgedriver.log";
            //service.EnableVerboseLogging = true;

            ///
            /// Kdyz funguje lokalne -> preferovane
            /// 

            //_driver = new EdgeDriver(service, options);

            ///
            /// Kdyz nefunguje lokalne, pouzit remote server na port dany driverem
            ///
            _driver = new RemoteWebDriver(new Uri("http://localhost:58182"), options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private void LoginAndFetchAttendance()
        {
            try
            {
                _driver.Navigate().GoToUrl("https://login1.edupage.org/");
                Console.WriteLine("Navigováno na přihlašovací stránku.");

                // Vyplnění přihlašovacích údajů
                _driver.FindElement(By.Name("username")).SendKeys("");
                _driver.FindElement(By.Name("password")).SendKeys("");

                // Čekání na tlačítko pro odeslání a kliknutí na něj
                var submitButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("skgdFormSubmit")));
                submitButton.Click();
                Console.WriteLine("Přihlášení odesláno.");

                // Čekání na přesměrování po přihlášení
                Console.WriteLine($"Aktuální URL po přihlášení: {_driver.Url}");

                // Pokračuj s přechodem na stránku docházky
                _driver.Navigate().GoToUrl("https://sstebrno.edupage.org/dashboard/eb.php?mode=attendance");
                Console.WriteLine("Přechod na stránku docházky.");

                // Počkej na tabulku
                _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table.dash_dochadzka")));
                Console.WriteLine("Tabulka docházky nalezena.");

                _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.asc-ribbon-button")));
                IWebElement button = _driver.FindElement(By.CssSelector("div.asc-ribbon-button:nth-child(2)"));
                button.Click();

                _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.dt-container")));
                var scheduleTable = _driver.FindElement(By.CssSelector("div.dt-container"));
                string scheduleHtml = scheduleTable.GetAttribute("outerHTML");
                Console.WriteLine($"HTML tabulky s rozvrhem:\n{scheduleHtml}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba: {ex.Message}");
                Console.WriteLine($"Zdrojový kód stránky:\n{_driver.PageSource}");
                throw;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }

        //Side menu private void
        private void BurgerButton_Click(object sender, RoutedEventArgs e)
        {
            _design.BurgerButton_Click(sender, e);
        }
        //Side menu - Buttons 
        private void Click_Test1(object sender, RoutedEventArgs e)
        {
            _design.Click_Test1(sender, e);
        }

        private void Click_Test2(object sender, RoutedEventArgs e)
        {
            _design.Click_Test2(sender, e);
        }

        private void Click_Test3(object sender, RoutedEventArgs e)
        {
            _design.Click_Test3(sender, e);
        }

        private void Click_Test4(object sender, RoutedEventArgs e)
        {
            _design.Click_Test4(sender, e);
        }
    }
}
