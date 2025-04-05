using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
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
using static edupageTest.Views.LoginPage;
using System.Reflection;
using System.IO;
using System;
namespace edupageTest.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        //private MainViewModel ViewModel { get; }
        public string Username => UsernameTextBox.Text;
        public string Password => PasswordTextBox.Password;

        private bool _canLogin = false;
        private MainSetup _setup;
        private MainWindow _mainWindow; 
        private Design _design;

        public LoginPage()
        {
            InitializeComponent();
            _mainWindow = new MainWindow();
            _setup = new MainSetup(Username, Password/*, _mainWindow.GraphCanvas*/);
            //ViewModel = new MainViewModel();
            //this.DataContext = ViewModel;
            this.PreviewMouseDown += Window_PreviewMouseDown;
            this.Loaded += SetupLoad;           
        }

        private async void SetupLoad(object sender, RoutedEventArgs e)
        {
            await _setup.SetupAsync();
            _canLogin = _setup.CanLogin;
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateCredentials())
                return;
            try
            {
                await Task.Run(async () =>
                {
                    await Application.Current.Dispatcher.Invoke(async () =>
                    {
                        _setup.Username = Username;
                        _setup.Password = Password;
                        await _setup.SetupAsync();
                        _canLogin = _setup.CanLogin;
                    });
                });

                if (_canLogin)
                {
                    _setup.LoggedIn = true;
                    _mainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Neplatné přihlašovací údaje");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba: {ex.Message}");
            }
        }

        #region Vyplnění Polí

        private bool ValidateCredentials()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ShowValidationError("Please enter username", UsernameTextBox);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ShowValidationError("Please enter password", PasswordTextBox);
                return false;
            }

            return true;
        }
        private void ShowValidationError(string message, Control control)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            control.Focus();
        }
        #endregion

        #region TextBox focus

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Hide the placeholder when the TextBox gains focus
            PasswordPlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Show the placeholder if the TextBox is empty
            if (string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                PasswordPlaceholderText.Visibility = Visibility.Visible;
            }
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Hide the placeholder when the TextBox gains focus
            UsernamePlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                UsernamePlaceholderText.Visibility = Visibility.Visible;
            }
        }
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Check if click is outside both text boxes
            if (!UsernameTextBox.IsMouseOver && !PasswordTextBox.IsMouseOver)
            {
                // Clear focus from both text boxes
                FocusManager.SetFocusedElement(this, null);
                Keyboard.ClearFocus();
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == UsernameTextBox)
                {
                    PasswordTextBox.Focus();
                }
                else if (sender == PasswordTextBox)
                {
                    SubmitButton_Click(sender, e);
                }
            }
        }
        #endregion

        //    #region Relay Command
        //public class RelayCommand : ICommand
        //{
        //    private readonly Action _execute;
        //    private readonly Func<bool> _canExecute;
        //    private event EventHandler _canExecuteChanged;

        //    public RelayCommand(Action execute, Func<bool> canExecute = null)
        //    {
        //        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        //        _canExecute = canExecute;
        //    }

        //    public event EventHandler CanExecuteChanged
        //    {
        //        add
        //        {
        //            _canExecuteChanged += value;
        //            CommandManager.RequerySuggested += value;
        //        }
        //        remove
        //        {
        //            _canExecuteChanged -= value;
        //            CommandManager.RequerySuggested -= value;
        //        }
        //    }

        //    public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        //    public void Execute(object parameter) => _execute();

        //    public void RaiseCanExecuteChanged()
        //    {
        //        _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        //    }
        //}

        //#endregion

        //#region Remember me image change
        //public class MainViewModel : INotifyPropertyChanged
        //{
        //    private bool _isChecked;
        //    public bool IsChecked
        //    {
        //        get => _isChecked;
        //        set
        //        {
        //            if (_isChecked != value)
        //            {
        //                _isChecked = value;
        //                OnPropertyChanged();
        //                OnPropertyChanged(nameof(CurrentImage));

        //            }
        //        }
        //    }

        //    public BitmapImage CurrentImage =>
        //_isChecked
        //    ? LoadImage("CheckBox_Checked.png")
        //    : LoadImage("CheckBox_Blank.png");



        //    private BitmapImage LoadImage(string filename)
        //    {
        //        try
        //        {
        //            var uri = new Uri($"pack://application:,,,/Assets/Icons/{filename}");
        //            return new BitmapImage(uri);
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"Error loading image: {ex.Message}");
        //            return CreateErrorImage();
        //        }
        //    }

        //    #region Creating Error Image
        //    private BitmapImage CreateErrorImage()
        //    {
        //        // Fallback: Generate a red X image programmatically
        //        var drawingVisual = new DrawingVisual();
        //        using (var drawingContext = drawingVisual.RenderOpen())
        //        {
        //            drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, 100, 100));
        //            drawingContext.DrawLine(new Pen(Brushes.Red, 3), new Point(10, 10), new Point(90, 90));
        //            drawingContext.DrawLine(new Pen(Brushes.Red, 3), new Point(90, 10), new Point(10, 90));
        //        }

        //        var bitmap = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
        //        bitmap.Render(drawingVisual);
        //        return BitmapToBitmapImage(bitmap);
        //    }

        //    private BitmapImage BitmapToBitmapImage(BitmapSource bitmapSource)
        //    {
        //        var bitmapImage = new BitmapImage();
        //        var memoryStream = new MemoryStream();

        //        var encoder = new PngBitmapEncoder();
        //        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
        //        encoder.Save(memoryStream);

        //        memoryStream.Position = 0;
        //        bitmapImage.BeginInit();
        //        bitmapImage.StreamSource = memoryStream;
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmapImage.EndInit();
        //        memoryStream.Close();

        //        return bitmapImage;
        //    }

        //    #endregion 
        //    public ICommand ToggleRememberMeCommand { get; }

        //    public MainViewModel()
        //    {
        //        ToggleRememberMeCommand = new RelayCommand(() =>
        //        {
        //            Debug.WriteLine($"Toggling from {IsChecked} to {!IsChecked}");
        //            IsChecked = !IsChecked;
        //            Debug.WriteLine($"Current image path: {(IsChecked ? "Checked" : "Blank")}");
        //        });
        //        Debug.WriteLine($"Initial image: {CurrentImage}");
        //    }

        //    public event PropertyChangedEventHandler PropertyChanged;

        //    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //    {
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
        //#endregion

    }
}




