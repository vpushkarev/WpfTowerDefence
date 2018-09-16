using System;
using System.Diagnostics;
using System.Threading;

namespace WpfTowerDefence
{
    public class Player
    {
        Timer timer;

        public event Action<double> OnCount;
        private static object _lock = new object();

        public double Money { get; private set; } = 10;

        public void TimerPlayerStart()
        {
            timer = new Timer(AddMoney, 0.5, 0, 1000);
        }

        public void AddMoney(object sender)
        {
            lock (_lock)
            {
                if (sender is IConvertible)
                {
                    Money += ((IConvertible)sender).ToDouble(null);
                }
            }
            OnCount?.Invoke(Money);
            Debug.WriteLine("Money{0}", Money);
            
        }

        internal void TimerPlayerStop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

    }
}




