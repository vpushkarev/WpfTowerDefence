using System;
using System.Diagnostics;
using System.Threading;

namespace WpfTowerDefence
{
    public class Player
    {
        Timer timer;

        // Деньги изменяются из потока пользовательского интерфейса, как показано в методе AddTower, 
        //  и из таймера.Кажется, сначала он должен быть помечен как "volatile", а объект 
        //игрока должен быть заблокирован при выполнении изменений денег?
        private volatile double _money = 10;
        public event Action<double> OnCount;

        public double Money
        {
            get
            {
                return _money;
            }
        }

        public void TimerPlayerStart()
        {
            timer = new Timer(AddMoney, null, 0, 1000);
        }

        public void AddMoney(object sender)
        {
            _money += 0.5;
            OnCount?.Invoke(_money);
            Debug.WriteLine("Money{0}", _money);
        }

        internal void TimerPlayerStop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

    }
}




