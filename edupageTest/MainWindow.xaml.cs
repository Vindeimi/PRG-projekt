using System.Text;
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

namespace edupageTest
{
    public partial class MainWindow : Window
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        public MainWindow()
        {
            InitializeComponent();
            Setup();
            LoginAndFetchAttendance();
        }

        private void Setup()
        {

            var options = new FirefoxOptions();
            options.AddArgument("--headless=new");
            _driver = new FirefoxDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private void LoginAndFetchAttendance()
        {
            try
            {
                _driver.Navigate().GoToUrl("https://login1.edupage.org/");
                Console.WriteLine("Navigováno na přihlašovací stránku.");

                // Vyplnění přihlašovacích údajů
                _driver.FindElement(By.Name("username")).***REMOVED***;
                _driver.FindElement(By.Name("password")).***REMOVED***;

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
    }
}