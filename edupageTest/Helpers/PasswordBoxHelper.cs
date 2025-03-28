using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace LoginPage.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword",
            typeof(SecureString),
            typeof(PasswordBoxHelper),
            new FrameworkPropertyMetadata(null, OnBoundPasswordChanged));

        public static SecureString GetBoundPassword(DependencyObject dp) =>
            (SecureString)dp.GetValue(BoundPasswordProperty);

        public static void SetBoundPassword(DependencyObject dp, SecureString value) =>
            dp.SetValue(BoundPasswordProperty, value);

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox box)
            {
                box.PasswordChanged -= PasswordBox_PasswordChanged;
                box.Password = e.NewValue != null ?
                    new System.Net.NetworkCredential(string.Empty, (SecureString)e.NewValue).Password : "";
                box.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox box)
            {
                var secure = new SecureString();
                foreach (char c in box.Password) secure.AppendChar(c);
                SetBoundPassword(box, secure);
            }
        }
    }

}
