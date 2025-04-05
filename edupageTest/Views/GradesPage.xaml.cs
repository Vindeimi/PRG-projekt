using OpenQA.Selenium.BiDi.Modules.Browser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace edupageTest
{
    /// <summary>
    /// Interakční logika pro GradesWindow.xaml
    /// </summary>
    public partial class GradesPage : Page
    {
        public ObservableCollection<SubjectItem> SubjectItem { get; } = new ObservableCollection<SubjectItem>();
        private List<KeyValuePair<string, GradeRecords>> _grades;

        public GradesPage()
        {
            InitializeComponent();
            _grades = AppContext.Grades;

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
            foreach (var grade in _grades)
            {
                if (grade.Key.Equals("chování", StringComparison.OrdinalIgnoreCase) ||
                    grade.Key.Equals("chovani", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string subject;
                if (AppContext.SubjectShortcut != null && AppContext.SubjectShortcut.TryGetValue(grade.Key, out subject))
                {
                    Console.WriteLine(subject); // normální předmět
                }
                else if (AppContext.SubjectShortcut != null && AppContext.SubjectShortcut.TryGetValue(grade.Key + "1", out subject))
                {
                    Console.WriteLine(subject); // předmět s 1
                }
                else if (AppContext.SubjectShortcut != null && AppContext.SubjectShortcut.TryGetValue(grade.Key + "2", out subject))
                {
                    Console.WriteLine(subject); // předmět s 2
                }
                else
                {
                    subject = grade.Key;
                }

                var grades = grade.Value.Grades;
                var diameter = grade.Value.Diameter;

                SubjectItem.Add(new SubjectItem
                {
                    Subject = subject,
                    Grades = string.Join("; ", grades.Select(g => g.Key)),
                    Diameter = diameter
                });
            }
            return SubjectItem;
        }
    }

    public class SubjectItem
    {
        public string Subject { get; set; }
        public string Grades { get; set; }
        public string Diameter { get; set; }
    }
}
