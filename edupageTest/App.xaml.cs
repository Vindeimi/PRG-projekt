using edupageTest.Views;
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
        protected override void OnExit(ExitEventArgs e)
        {
            AppContext.Setup?.Dispose();
            base.OnExit(e);
        }
    }

}
