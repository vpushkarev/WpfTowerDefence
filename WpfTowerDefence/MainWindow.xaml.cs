using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTowerDefence
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow main;
        public Game GameWindow;
        public string Result
        {
            get { return resultLabel.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { resultLabel.Content = value; })); }
        }

        public MainWindow()
        {
            InitializeComponent();
            main = this;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            Game gameWindow = new Game(this);
            gameWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void restartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
