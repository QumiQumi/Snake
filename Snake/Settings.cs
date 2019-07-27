using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };
    class Settings
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static int Goal { get; set; }
        public static int LvlCounter { get; set; }
        public static bool GameOver { get; set; }
        public static bool Pause { get; set; }
        public static Direction direction { get; set; }

        public Settings()
        {
            Width = 20;
            Height = 20;
            Speed = 10;
            Score = 0;
            Points = 200;
            Goal = 1000;
            LvlCounter = 1;
            GameOver = false;
            Pause = true;
            direction = Direction.Down;
        }
    }
}
