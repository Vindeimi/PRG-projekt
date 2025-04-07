using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace edupageTest
{
    internal class AppContext
    {
        public static List<KeyValuePair<string, GradeRecords>> Grades { get; set; }
        public static int SemesterType { get; set; }
        public static DriverInitialization Driver { get; set; }
        public static Grades GradesInt { get; set; }
        public static bool NeedsGraphInitialization { get; set; }
        public static MainSetup Setup { get; set; }
        public static Border MenuBorder { get; set; }
        public static Canvas GraphCanvas{ get; set; }
        public static Canvas GradesCanvas{ get; set; }
        public static Dictionary<string, string> SubjectShortcut { get; set; }
        public static MainWindow MainWindow { get; set; }
        public static ObservableCollection<GradeDisplayItem> GradeDisplay { get; set; }
        public static Graph Graph { get; set; }
        public static Graph AttendanceGraph { get; set; }
        public static Dictionary<string, AttendanceRecords> AttendanceData { get; set; }
        public static Attendance Attendance { get; set; }
        public static Canvas AttendanceOverlayCanvas { get; set; }
        public static Frame MainFrame { get; set; }
    }
}
