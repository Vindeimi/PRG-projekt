using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace edupageTest.Views
{
    /// <summary>
    /// Interakční logika pro AttendancePage.xaml
    /// </summary>
    public partial class AttendancePage : Page
    {
        private Graph _graph;
        private GradesAttendance _gradesAttendance;
        public AttendancePage()
        {
            InitializeComponent();
            AppContext.GradesCanvas = gradesCanvas;
            AppContext.AttendanceOverlayCanvas = overlayCanvas;

            if (AppContext.AttendanceGraph == null)
            {
                _graph = new Graph(AppContext.AttendanceData, AppContext.Attendance.SemesterType, AppContext.GradesCanvas, AppContext.SubjectShortcut);
                Thread.Sleep(300);
                _graph.DrawMainMenuGraph();
                AppContext.AttendanceGraph = _graph;
            }
            else
            {
                _graph = AppContext.AttendanceGraph;
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;

            // Posun o 45% šířky obsahu
            double offset = scrollViewer.HorizontalOffset - (e.Delta * 0.45);

            // Omezení offsetu na platný rozsah
            offset = Math.Max(0, Math.Min(offset, scrollViewer.ScrollableWidth));

            scrollViewer.ScrollToHorizontalOffset(offset);
            e.Handled = true;
        }

        private void SubjectClick(object sender, RoutedEventArgs e)
        {
            AppContext.MainFrame.Navigate(_gradesAttendance ?? (_gradesAttendance = new GradesAttendance()));
        }       
        private void GraphClick(object sender, RoutedEventArgs e)
        {
            AppContext.MainFrame.Navigate(this);
        }

    }
}
