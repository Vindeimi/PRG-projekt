using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Navigation;

namespace edupageTest
{
    public class Design 
    {
        private bool _isMenuCollapsed = false;
        private Border _menuBorder;
        private double originalWidth;
        private double originalHeight;
        private GradesPage _gradesPage;
        public Design(Border menuBorder)
        {
            _menuBorder = menuBorder;
            originalWidth = _menuBorder.Width;
            originalHeight = _menuBorder.Height;
        }

        #region Buttony Funkce

        public void UvodButton(object sender, RoutedEventArgs e, Frame mainFrame)
        {
            mainFrame.Content = null;
        }
        public void RozvrhButton(object sender, RoutedEventArgs e, Frame mainFrame)
        {
            MessageBox.Show("Clicked on button 2");
        }
        public void DochazkaButton(object sender, RoutedEventArgs e, Frame mainFrame)
        {
            MessageBox.Show("Clicked on button 3");
        }
        public void ZnamkyRozvrh(object sender, RoutedEventArgs e, Frame mainFrame)
        {
            mainFrame.Navigate( _gradesPage ?? (_gradesPage = new GradesPage()));
        }
        public void BurgerButton_Click(object sender, RoutedEventArgs e, Frame mainFrame)
        {
            // Toggle the menu's visibility with animation
            if (_isMenuCollapsed)
            {
                // Expand the menu
                ExpandMenu();
            }
            else
            {
                // Collapse the menu
                CollapseMenu();
            }

            _isMenuCollapsed = !_isMenuCollapsed;
        }
        #endregion 

        #region Menu Funkce

        private void CollapseMenu()
        {
            // Create a DoubleAnimation to collapse the menu
            DoubleAnimation collapseAnimation = new DoubleAnimation
            {
                To = 0, // Collapse to 0 width
                Duration = TimeSpan.FromSeconds(0.3)
            };

            // Apply the animation to the menu's width
            _menuBorder.BeginAnimation(Border.WidthProperty, collapseAnimation);
            _menuBorder.BeginAnimation(Border.HeightProperty, collapseAnimation);
        }

        private void ExpandMenu()
        {
            // Create a DoubleAnimation to expand the menu
            DoubleAnimation expandWidthAnimation = new DoubleAnimation
            {
                To = originalWidth, // Expand to original width
                Duration = TimeSpan.FromSeconds(0.3)
            };

            DoubleAnimation expandHeightAnimation = new DoubleAnimation
            {
                To = originalHeight,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            // Apply the animation to the menu's width
            _menuBorder.BeginAnimation(Border.WidthProperty, expandWidthAnimation);
            _menuBorder.BeginAnimation(Border.HeightProperty, expandHeightAnimation);
        }

        #endregion
    }
}
