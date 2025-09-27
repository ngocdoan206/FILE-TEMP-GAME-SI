using System;

namespace SpaceInvaders
{
    public class Invader : GameObjects
    {
        public char Symbol { get; set; }

        // Tốc độ dạng double (ví dụ 0.5, 1.2, 2.0 ...)
        public double Speed { get; set; } = 1.0;

        // Bộ đệm để tích lũy phần thập phân khi di chuyển
        private double _yRemainder = 0.0;

        // Constructor: x,y là int, speed mặc định = 1.0 (tham số mặc định)
        public Invader(int x, int y, double speed = 1.0) : base(x, y)
        {
            Symbol = '#';
            Speed = speed;
        }

        // Di chuyển xuống theo Speed; hỗ trợ chuyển động fractional
        public void MoveDown()
        {
            _yRemainder += Speed;
            int move = (int)Math.Floor(_yRemainder);
            if (move > 0)
            {
                PositionY += move; // PositionY là của GameObjects (int)
                _yRemainder -= move;
            }
        }

        // Đổi ký hiệu (simple)
        public void ChangeSymbol(char newSymbol)
        {
            Symbol = newSymbol;
        }

        // Overloading: đổi ký hiệu kèm đổi màu console (ví dụ dùng khi render)
        public void ChangeSymbol(char newSymbol, ConsoleColor color)
        {
            Symbol = newSymbol;
            Console.ForegroundColor = color;
        }
    }
}