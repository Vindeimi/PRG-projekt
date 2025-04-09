using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using SeleniumExtras.WaitHelpers;
using System.Windows;
using OpenQA.Selenium.DevTools.V131.DOM;
using Accessibility;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Chrome;
using System.Windows.Media.Animation;
using OpenQA.Selenium.Remote;
using System.IO;
using System.Diagnostics;
using OpenQA.Selenium.DevTools.V131.Browser;

namespace edupageTest
{
    internal class DriverInitialization : IDisposable
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private EdgeOptions _edgeOption;
        private Process _process;

        public IWebDriver Driver => _driver;
        public WebDriverWait Wait => _wait;

        public (WebDriverWait driverWait, IWebDriver driver) DriverInit()
        {
            Initialize();
            return (_wait, _driver);
        }

        #region Nastaveni Driveru

        private void Initialize()
        {
            DriverOptions[] options = [new FirefoxOptions(), new EdgeOptions(), new SafariOptions(), new ChromeOptions()];

            foreach (var option in options)
            {
                try
                {
                    _driver = CreateDriver(option);
                    if (_driver != null) break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Chyba při spuštění s {option.GetType().Name}: {e.Message}");
                }
            }

            // 2. POKUS: Spustit driver přes remote driver

            if (_driver == null)
            {
                try
                {
                    _edgeOption = new EdgeOptions();

                    #region Option List

                    //_edgeOption.AddArgument("--headless=new");
                    _edgeOption.AddArgument("--disable-features=EdgeSignin");
                    _edgeOption.AddArgument("--guest");
                    _edgeOption.AddArgument("--disable-features=EdgeSignIn,MSAccountSignIn,SyncBackendData");
                    _edgeOption.AddArgument("--disable-single-sign-on");
                    _edgeOption.AddArgument("--disable-autofill-assistant");
                    _edgeOption.AddArgument("--disable-account-consistency");
                    _edgeOption.AddArgument("--disable-signin-scoped-device-id");
                    _edgeOption.AddArgument("--disable-sync");
                    _edgeOption.AddArgument("--no-sandbox");
                    _edgeOption.AddArgument("--disable-gpu");
                    _edgeOption.AddArgument("--disable-extensions");
                    _edgeOption.AddArgument("--disable-blink-features=AutomationControlled");
                    _edgeOption.AddArgument("--log-level=3");
                    _edgeOption.AddArgument("--silent");

                    #endregion

                    string tempProfilePath = Path.Combine(Path.GetTempPath(), "EdgeProfile");
                    _edgeOption.AddArgument($"--user-data-dir={tempProfilePath}");

                    var service = EdgeDriverService.CreateDefaultService(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                    service.LogPath = "msedgeriver.log";
                    service.HideCommandPromptWindow = true;
                    service.EnableVerboseLogging = true;

                    service.Start();

                    string uriAddress = "http://localhost:" + service.Port;
                    _driver = new RemoteWebDriver(new Uri(uriAddress), _edgeOption);
                    _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Remote WebDriver selhal: {e.Message}");

                    // 3. POKUS: Spustit Edge přes Debugging Port
                    try
                    {                        
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = "msedge.exe",
                            Arguments = "--remote-debugging-port=9222 --user-data-dir=\"C:\\temp\\edge-profile\"",
                            CreateNoWindow = true,
                            UseShellExecute = true,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };
                        Process.Start(psi);

                        System.Threading.Thread.Sleep(3000);

                        // Připojit se k němu přes Selenium
                        EdgeOptions debugOptions = new EdgeOptions();
                        debugOptions.DebuggerAddress = "localhost:9222";

                        _driver = new EdgeDriver(debugOptions);
                        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Spuštění Edge s debugging portem selhalo: {ex.Message}");
                    }
                }
            }

            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        }

        #endregion

        #region CreateDriver 

        private IWebDriver CreateDriver(DriverOptions options)
        {
            switch (options)
            {
                case FirefoxOptions firefoxOptions:
                    //firefoxOptions.AddArgument("--headless");
                    var firefoxService = FirefoxDriverService.CreateDefaultService();
                    //firefoxService.HideCommandPromptWindow = true;
                    return new FirefoxDriver(firefoxService, firefoxOptions);

                case ChromeOptions chromeOptions:
                    chromeOptions.AddArgument("--headless=new");
                    var chromeService = ChromeDriverService.CreateDefaultService();
                    chromeService.HideCommandPromptWindow = true; 
                    return new ChromeDriver(chromeService, chromeOptions);

                case EdgeOptions edgeOptions:
                    edgeOptions.AddArgument("--headless");
                    var edgeService = EdgeDriverService.CreateDefaultService();
                    edgeService.HideCommandPromptWindow = true; 
                    return new EdgeDriver(edgeService, edgeOptions);

                case SafariOptions safariOptions:
                    return new SafariDriver(safariOptions);

                default:
                    throw new ArgumentException("Nepodporovany typ options");
            }
        }

        #endregion

        #region Window Closing Funkce
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cleanup();
        }
        #endregion

        #region Cleanup Funkce
        private void Cleanup()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }

        public void Dispose()
        {
            try
            {
                _driver?.Quit();
                _driver?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error quitting driver: {ex.Message}");
            }

            try
            {
                if (_process != null && !_process.HasExited)
                {
                    _process.Kill();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error killing browser process: {ex.Message}");
            }
        }
        #endregion
    }
}
