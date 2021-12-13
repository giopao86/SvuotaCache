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
using EmptyCache.Lib;
using EmptyCache.Lib.Models;

namespace EmptyCache.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Service s = new Service();
        public MainWindow()
        {
            InitializeComponent();
            s.LogEvent += this.Log;
        }

        private void ManageEmptyCache(object sender, RoutedEventArgs e)
        {            
            s.Execute();
        }

        private void Log(object sender, LogEventArgs e)
        {
            this.txtLog.AppendText("\r\n----------------------\r\n");
            this.txtLog.AppendText(e.ToString());
            this.lblPerc.Content = $"{e.Percent} %";
        }
    }
}
