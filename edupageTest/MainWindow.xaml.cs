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

namespace edupageTest
{
    public partial class MainWindow : Window
    {
        //Side menu inicializace
        private Design _design;
        private MainSetup _setup;
        public static MainWindow Instance { get; private set; }

       public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            // Hlavní setup
            _setup = new MainSetup();
            _setup.Setup();

            //Design 
            _design = new Design(menuBorder);
        }

        public Border MenuBorder => menuBorder;

        #region ButtonClicks

        //Side menu private void
        private void BurgerButton_Click(object sender, RoutedEventArgs e) => _design.BurgerButton_Click(sender, e);

        //Side menu - Buttons 
        private void Click_Test1(object sender, RoutedEventArgs e) => _design.Click_Test1(sender, e);
        private void Click_Test2(object sender, RoutedEventArgs e) => _design.Click_Test2(sender, e);
        private void Click_Test3(object sender, RoutedEventArgs e) => _design.Click_Test3(sender, e);
        private void Click_Test4(object sender, RoutedEventArgs e) => _design.Click_Test4(sender, e);
        #endregion
    }
}
