using System;
namespace SpaceInvaders
{
    public static class Config
    {
        // Hằng số chung
        public const int MaxEscaped = 5; // số quái thoát tối đa
        public const int BoardWidth = 80;  // chiều rộng màn hình game
        public const int BoardHeight = 35; // chiều cao màn hình game
        public const string GameTitle = "Space Invaders UEH Edition";

        // Enum độ khó
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        // Thiết lập tốc độ sinh invader theo độ khó
        public static int GetSpawnRate(Difficulty d)
        {
            switch (d)
            {
                case Difficulty.Easy: return 90;   // chậm hơn
                case Difficulty.Medium: return 60; // mặc định
                case Difficulty.Hard: return 30;   // nhanh hơn
                default: return 60;
            }
        }
    }
}
