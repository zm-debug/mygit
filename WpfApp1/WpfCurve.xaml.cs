using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;


namespace WpfApp1
{
    /// <summary>
    /// WpfCurve.xaml 的交互逻辑
    /// </summary>
    public partial class WpfCurve : Window
    {
        private ObservableDataSource<Point> dataSource = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource1 = new ObservableDataSource<Point>();
        private PerformanceCounter performanceCounter = new PerformanceCounter();
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private int currentSecond = 0;
        bool wendu = false;//标志是否滚屏
        public WpfCurve()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (wendu)
            {
                wendu = false;
            }
            else
            {
                wendu = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            plotter.AddLineGraph(dataSource, Colors.Red, 2, "百分比");
            plotter.AddLineGraph(dataSource1, Colors.RoyalBlue, 2, "sin");
            plotter.LegendVisible = true;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(0.1);
            dispatcherTimer.Tick += timer_Tick;
            dispatcherTimer.IsEnabled = true;
            plotter.AxisGrid.Visibility = Visibility.Hidden;           
            plotter.Viewport.FitToView();

        }
        int xaxis = 0;
        int yaxis = 0;
        int group = 100;//默认组距

        Queue q = new Queue();

        private void timer_Tick(object sender, EventArgs e)
        {
            performanceCounter.CategoryName = "Processor";
            performanceCounter.CounterName = "% Processor Time";
            performanceCounter.InstanceName = "_Total";
            double x = currentSecond;
            double y = performanceCounter.NextValue();
            double y1 = Math.Sin(currentSecond * 0.2)*20;
            Point point = new Point(x, y);
            Point point1 = new Point(x,y1);
            dataSource.AppendAsync(base.Dispatcher, point);
            dataSource1.AppendAsync(base.Dispatcher, point1);
            if (wendu)
            {
                if (q.Count < group)
                {
                    q.Enqueue((int)y);//入队
                    yaxis = 0;
                    foreach (int c in q)
                        if (c > yaxis)
                            yaxis = c;
                }
                else
                {
                    q.Dequeue();//出队
                    q.Enqueue((int)y);//入队
                    yaxis = 0;
                    foreach (int c in q)
                        if (c > yaxis)
                            yaxis = c;
                }

                if (currentSecond - group > 0)
                    xaxis = currentSecond - group;
                else
                    xaxis = 0;

                //Debug.Write("{0}\n", yaxis.ToString());
                plotter.Viewport.Visible = new System.Windows.Rect(xaxis, -1*yaxis, group, yaxis*2);//主要注意这里一行
                
            }
            currentSecond++;
        }

    }
}
