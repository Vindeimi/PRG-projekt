using System.Configuration;
using System.Data;
using System.Windows;
using edupageTest.Views;

namespace edupageTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var loginPage = new Views.LoginPage();

            if (loginPage.ShowDialog() == true)
            {
                // Create MainWindow with credentials
                var mainWindow = new MainWindow(loginPage.Username, loginPage.Password);
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }

}
