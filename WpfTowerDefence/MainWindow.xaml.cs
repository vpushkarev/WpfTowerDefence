using System;
using System.Diagnostics;
using System.Windows;

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
            set { /*Dispatcher.Invoke(new Action(() => {*/ resultLabel.Content = value; /*}));*/ }
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
            Process.GetCurrentProcess().Close();
        }
    }
}
