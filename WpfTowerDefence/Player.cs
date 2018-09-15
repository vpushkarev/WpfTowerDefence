using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace WpfTowerDefence
{
    public class Player
    {
        DispatcherTimer timer;
        private double _money = 10;
        public delegate void MethodContainer(double money);
        public event MethodContainer onCount;

        public double Money
        {
            get { return _money; }
            set { _money = value; onCount?.Invoke(Money); }
        }

        public void timerPlayerStart()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(AddingMoney);
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer.Start();
        }

        void AddingMoney(object sender, EventArgs e)
        {
            Money += 0.5;
            Debug.WriteLine("Money{0}", Money);
        }

        internal void timerPlayerStop()
        {
            timer.Stop();
        }

    }
}




