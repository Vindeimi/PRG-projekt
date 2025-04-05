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
using OpenQA.Selenium.Edge;
using System.Security.Cryptography;
using OpenQA.Selenium.Remote;
using System.Windows.Controls.Primitives;
using edupageTest.Views;

namespace edupageTest
{
    public partial class MainWindow : Window
    {
        //Side menu inicializace
        private Design _design;
        private MainSetup _setup;
        private string _password, _username;
        //public static MainWindow Instance { get; private set; }
        public bool CanLogin = false;
        //public Canvas GraphCanvas;


       public MainWindow(/*string username, string password*/)
        {
            InitializeComponent();
            //Instance = this;
            AppContext.MainWindow = this;
            AppContext.MenuBorder = menuBorder;
            AppContext.GraphCanvas = attendanceGraphCanvas;
            //_password = password;
            //_username = username;

            // Hlavní setup
            //this.Loaded += MainWindow_Loaded;            

            //Design 
            _design = new Design(menuBorder);
        }

        //private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    _setup = new MainSetup(_username, _password);
        //    await _setup.SetupAsync();
        //    _setup.CanLogin = CanLogin;
        //}

        #region ButtonClicks

        //Side menu private void
        private void BurgerButton_Click(object sender, RoutedEventArgs e) => _design.BurgerButton_Click(sender, e, MainFrame);

        //Side menu - Buttons 
        private void UvodButton(object sender, RoutedEventArgs e) => _design.UvodButton(sender, e, MainFrame);
        private void RozvrhButton(object sender, RoutedEventArgs e) => _design.RozvrhButton(sender, e, MainFrame);
        private void DochazkaButton(object sender, RoutedEventArgs e) => _design.DochazkaButton(sender, e, MainFrame);
        private void ZnamkyRozvrh(object sender, RoutedEventArgs e) => _design.ZnamkyRozvrh(sender, e, MainFrame);
        #endregion
    }
}
