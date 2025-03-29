using System;
using System.ComponentModel;
using System.Security;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Reflection;
using LoginPage.Properties;
using edupageTest;
using static edupageTest.Views.LoginPage;
using System.IO;
using System.Runtime.CompilerServices;

namespace LoginPage.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public RelayCommand SubmitCommand { get; }
        public RelayCommand ToggleRememberMeCommand { get; }

        private readonly Login _loginService;
        private string _username;
        private SecureString _password;
        private bool _rememberMe;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                    SubmitCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public SecureString Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password?.Dispose();
                    _password = value;
                    OnPropertyChanged();
                    SubmitCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                if (_rememberMe != value)
                {
                    _rememberMe = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(RememberMeImage));
                }
            }
        }

        public ImageSource RememberMeImage =>
            RememberMe ? LoadImage("CheckBox_Checked.png") : LoadImage("CheckBox_Blank.png");

        


        public LoginViewModel()
        {
            _loginService = new Login(new DriverInitialization());
            SubmitCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            ToggleRememberMeCommand = new RelayCommand(() =>
            {
                RememberMe = !RememberMe;
                SubmitCommand.RaiseCanExecuteChanged(); // Now this will work
            });

            LoadCredentials();
        }

        private bool CanExecuteLogin() => !string.IsNullOrEmpty(Username) && Password?.Length > 0;

        private void ExecuteLogin()
        {
            try
            {
                string plainPassword = new System.Net.NetworkCredential(string.Empty, Password).Password;

                // Initialize and run setup with credentials
                var mainSetup = new MainSetup();
                mainSetup.Setup(Username, plainPassword);

                if (RememberMe)
                {
                    SaveCredentials();
                }

                MessageBox.Show("Login successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed: {ex.Message}");
            }
            finally
            {
                Password?.Dispose();
                Password = new SecureString();
            }
        }


        private void SaveCredentials()
        {
            try
            {
                Properties.Settings.Default.Username = Username;
                Properties.Settings.Default.Password = ConvertToInsecureString(Password);
                Properties.Settings.Default.RememberMe = RememberMe;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving credentials: {ex.Message}");
            }
        }

        private void LoadCredentials()
        {
            if (Properties.Settings.Default.RememberMe)
            {
                Username = Properties.Settings.Default.Username;
                Password = ConvertToSecureString(Properties.Settings.Default.Password);
                RememberMe = true;
            }
        }

        private void ClearSavedCredentials()
        {
            Properties.Settings.Default.Username = "";
            Properties.Settings.Default.Password = "";
            Properties.Settings.Default.RememberMe = false;
            Properties.Settings.Default.Save();
        }

        private BitmapImage LoadImage(string filename)
        {
            try
            {
                var uri = new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Assets/Icons/{filename}");
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = uri;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                return CreateFallbackImage();
            }
        }

        private BitmapImage CreateFallbackImage()
        {
            var width = 24;
            var height = 24;
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(
                    RememberMe ? Brushes.Green : Brushes.White,
                    new Pen(Brushes.Gray, 1),
                    new Rect(0, 0, width, height));

                if (RememberMe)
                {
                    var checkPen = new Pen(Brushes.DarkGreen, 2);
                    dc.DrawLine(checkPen, new Point(5, 12), new Point(10, 17));
                    dc.DrawLine(checkPen, new Point(10, 17), new Point(19, 6));
                }
            }

            var target = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            target.Render(visual);

            var bitmapImage = new BitmapImage();
            using (var stream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(target));
                encoder.Save(stream);
                stream.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }

        private SecureString ConvertToSecureString(string input)
        {
            var secure = new SecureString();
            foreach (char c in input ?? string.Empty)
                secure.AppendChar(c);
            return secure;
        }

        private string ConvertToInsecureString(SecureString securePassword)
        {
            return new System.Net.NetworkCredential(string.Empty, securePassword).Password;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}