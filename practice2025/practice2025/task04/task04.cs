using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace task04
{
    public class task04
    {
        public interface ISpaceship
        {
            void MoveForward();      // Движение вперед
            void Rotate(int angle);  // Поворот на угол (градусы)
            void Fire();             // Выстрел ракетой
            int Speed { get; }       // Скорость корабля
            int FirePower { get; } // Мощность выстрела
            int CurrentAngle { get; } // текущий угол поворота 
        }

        public class Cruiser: ISpaceship
        {
            public int Speed { get; } = 50;
            public int FirePower { get; } = 100;
            public int CurrentAngle { get; set; }
            public void MoveForward()
            {
                Console.WriteLine($"Скорость крейсера по прямой = {Speed}");
            }
            public void Rotate(int angle)
            {
                CurrentAngle = Math.Min(angle, 40); 
            }
            public void Fire()
            {
                Console.WriteLine($"Мощность выстрела крейсера = {FirePower}");
            }
        }

        public class Fighter : ISpaceship
        {
            public int Speed { get; } = 100;
            public int FirePower { get; } = 20;
            public int CurrentAngle { get; set; }
            public void MoveForward()
            {
                Console.WriteLine($"Скорость истребителя = {Speed}");
            }
            public void Rotate(int angle)
            {
                CurrentAngle = Math.Min(angle, 90); 
            }
            public void Fire()
            {
                Console.WriteLine($"Мощность выстрела истребителя = {FirePower}");
            }
        }

    }
}
