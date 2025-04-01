using OpenQA.Selenium.DevTools.V132.DOM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace edupageTest
{
    internal class Graph
    {
        private Dictionary<string, AttendanceRecords> _attendanceData;
        private int _semesterType;
        private List<System.Windows.Shapes.Rectangle> _rectangleList = new();
        private Canvas _graphCanvas;
        private Dictionary<string, string> _subjectShortcuts;

        private double _maxHeight = 425;
        private double _width = 100;
        private int _numBars = 3;
        private int _scale = 2;

        public Graph(Dictionary<string, AttendanceRecords> attendanceData, int semesterType, Canvas canvas, Dictionary<string, string> subjectShortcuts)
        {
            _attendanceData = attendanceData;
            _semesterType = semesterType;
            _graphCanvas = canvas;
            _subjectShortcuts = subjectShortcuts;
        }

        public void DrawMainMenuGraph()
        {
            if (_graphCanvas.ActualWidth <= 0 || _graphCanvas.ActualHeight <= 0)
            {
                // Čekání na načtení canvasu
                _graphCanvas.Loaded += (sender, e) =>
                {
                    this.DrawMainMenuGraph();
                };
                return;
            }

            DrawGraphContent();
        }
        private void DrawGraphContent()
        {
            double canvasWidth = _graphCanvas.ActualWidth;
            double canvasHeight = _graphCanvas.ActualHeight;

            // Výpočet pozic pro čáry
            double totalWidthOfBars = _width * _numBars;
            double totalSpacing = canvasWidth - totalWidthOfBars;
            double spacing = totalSpacing / (_numBars + 1);

            double line30_Y = _maxHeight - (1 + _scale * _maxHeight * 0.30);
            double line20_Y = _maxHeight - (1 + _scale * _maxHeight * 0.20);


            #region Vytvoření Čar

            // Čára na 30%
            Line line30 = new Line
            {
                X1 = 0,
                X2 = 500,
                Y1 = line30_Y,
                Y2 = line30_Y,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            // Čára na 20%
            Line line20 = new Line
            {
                X1 = 0,
                X2 = 500,
                Y1 = line20_Y,
                Y2 = line20_Y,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
            };

            TextBlock textBlock30 = new TextBlock
            {
                Text = "30%",
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            TextBlock textBlock20 = new TextBlock
            {
                Text = "20%",
                Foreground = new SolidColorBrush(Colors.Black),
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Panel.SetZIndex(line30, 3);
            Panel.SetZIndex(line20, 3);
            Panel.SetZIndex(textBlock20, 4);
            Panel.SetZIndex(textBlock30, 4);

            Canvas.SetBottom(textBlock30, (_scale * (1 + (0.3 * _maxHeight))) + 10);
            Canvas.SetBottom(textBlock20, (_scale * (1 + (0.2 * _maxHeight))) + 10);
            Canvas.SetLeft(textBlock30, 5);
            Canvas.SetLeft(textBlock20, 5);


            _graphCanvas.Children.Add(textBlock30);
            _graphCanvas.Children.Add(textBlock20);
            _graphCanvas.Children.Add(line30);
            _graphCanvas.Children.Add(line20);
            #endregion

            #region Vytvoření Default Sloupců   

            for (int i = 0; i < 3; i++)
            {
                var rect = new System.Windows.Shapes.Rectangle
                {
                    Height = _maxHeight,
                    Width = _width,
                    Fill = new SolidColorBrush(Colors.Red),
                    RadiusX = 10,
                    RadiusY = 10,
                };

                string subjectText = "";

                if (_subjectShortcuts.ContainsKey(_attendanceData.ElementAt(i).Key))
                {
                    subjectText = _subjectShortcuts[_attendanceData.ElementAt(i).Key];
                }
                else if (_subjectShortcuts.ContainsKey(_attendanceData.ElementAt(i).Key + "1"))
                {
                    subjectText = _subjectShortcuts[_attendanceData.ElementAt(i).Key + "1"];
                }
                else if (_subjectShortcuts.ContainsKey(_attendanceData.ElementAt(i).Key + "2"))
                {
                    subjectText = _subjectShortcuts[_attendanceData.ElementAt(i).Key + "2"];
                }
                else
                {
                    Console.WriteLine("Klíč nenalezen");
                }

                var textBlock = new TextBlock
                {
                    Text = subjectText,
                    Foreground = new SolidColorBrush(Colors.Black),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Panel.SetZIndex(rect, 2);
                _rectangleList.Add(rect);

                double leftPositionRect = spacing + i * (_width + spacing);

                Canvas.SetLeft(rect, leftPositionRect);
                Canvas.SetBottom(rect, 0);

                Canvas.SetLeft(textBlock,leftPositionRect + (_width / 2) - 10);
                Canvas.SetBottom(textBlock, 10);
                _graphCanvas.Children.Add(rect);
                _graphCanvas.Children.Add(textBlock);
            }
            #endregion

            if (_semesterType == 1)
            {
                for (int i = 0; i < _rectangleList.Count; i++)
                {
                    var firstSemesterPercent = (double.Parse(_attendanceData.ElementAt(i).Value.FirstSemester.Percent)) / 100;
                    double calculatedHeight = _scale * (1 + (firstSemesterPercent * _maxHeight));

                    _rectangleList[i].Height = Math.Min(calculatedHeight, _maxHeight);
                    GraphsColorChange(firstSemesterPercent, i);
                }
            }
            else
            {
                for (int i = 0; i < _rectangleList.Count; i++)
                {
                    var secondSemesterPercent = (double.Parse(_attendanceData.ElementAt(i).Value.SecondSemester.Percent)) / 100;
                    double calculatedHeight = _scale * (1 + (secondSemesterPercent * _maxHeight));

                    _rectangleList[i].Height = Math.Min(calculatedHeight, _maxHeight);
                    GraphsColorChange(secondSemesterPercent, i);
                }
            }
        }

        #region Dynamická změna barvy

        private void GraphsColorChange(double firstSemesterPercent, int i)
        {
            if (firstSemesterPercent < 20)
            {
                _rectangleList[i].Fill = new SolidColorBrush(Colors.Green);
            }
            else if (firstSemesterPercent >= 20 && firstSemesterPercent < 30)
            {
                _rectangleList[i].Fill = new SolidColorBrush(Colors.Yellow);
            }
            else
            {
                _rectangleList[i].Fill = new SolidColorBrush(Colors.Red);
            }
        }
        #endregion
    }
}
