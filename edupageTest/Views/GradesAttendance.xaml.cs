using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
    /// Interakční logika pro GradesAttendance.xaml
    /// </summary>
    public partial class GradesAttendance : Page
    {
        public ObservableCollection<SubjectItem> SubjectItem { get; } = new ObservableCollection<SubjectItem>();
        private Dictionary<string, AttendanceRecords> _attendance;

        public GradesAttendance()
        {
            InitializeComponent();
            _attendance = AppContext.AttendanceData;

            GetSubjectData();
            DataContext = this;
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;

            // Správný směr skrolování
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (e.Delta / 3));

            e.Handled = true;
        }


        public ObservableCollection<SubjectItem> GetSubjectData()
        {
            foreach (var aten in _attendance)
            {
                if (aten.Key.Equals("chování", StringComparison.OrdinalIgnoreCase) ||
                    aten.Key.Equals("chovani", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string subject;
                if (AppContext.SubjectShortcut != null && AppContext.SubjectShortcut.TryGetValue(aten.Key, out subject))
                {
                    Console.WriteLine(subject); // normální předmět
                }
                else if (AppContext.SubjectShortcut != null && AppContext.SubjectShortcut.TryGetValue(aten.Key + "1", out subject))
                {
                    Console.WriteLine(subject); // předmět s 1
                }
                else if (AppContext.SubjectShortcut != null && AppContext.SubjectShortcut.TryGetValue(aten.Key + "2", out subject))
                {
                    Console.WriteLine(subject); // předmět s 2
                }
                else
                {
                    subject = aten.Key;
                }
                
                string missing = aten.Value.FirstSemester.Missed.ToString();
                string total = AppContext.Attendance.SubjectHours[subject].ToString();
                string percent = aten.Value.FirstSemester.Percent.ToString();

                SubjectItem.Add(new SubjectItem
                {
                    Subject = subject,
                    Missing = missing,
                    Total = total,
                    Percent = percent,
                });
            }
            return SubjectItem;
        }
    }

    public class SubjectItem
    {
        public string Subject { get; set; }
        public string Missing { get; set; }
        public string Total { get; set; }
        public string Percent { get; set; }
        public int PercentCalc
        {
            get
            {
                string normalized = Percent?.Replace(',', '.') ?? "0";
                double realPercent = 0;

                if (normalized.Contains("%"))
                {
                    realPercent = double.Parse(normalized.Replace("%", ""), NumberStyles.Any, CultureInfo.InvariantCulture);
                }

                if (realPercent < 20)
                {
                    return 1;
                }
                else if (realPercent >= 20 && realPercent < 30)
                {
                    return 2;
                }
                else if (realPercent >= 30)
                {
                    return 3;
                }                
                else
                {
                    return 1;
                }
            }
        }

        public Brush PercentBrush => PercentCalc switch
        {
            1 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#77DD77")), // zelená
            2 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDFD96")), // žlutá
            3 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6C64")), // červená
        };
    }
}
