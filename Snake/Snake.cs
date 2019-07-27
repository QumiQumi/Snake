using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class SnakeBehave
    {
       /* private List<Circle> Snake = new List<Circle>();
        // змейка кушает
        private void Eat()
        {
            //Добавляем кусочек тела
            Circle circle = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            //Обновляем счет
            Settings.Score += Settings.Points;
            Form1.Form1Reference.lblScore.Text = "Score: " + Settings.Score.ToString()
                          + "\nGoal for " + Settings.LvlCounter + " level: " + Settings.Goal
                          + "\nCurrent speed is " + Settings.Speed; ;
            // DrawString(lblScore, pbCanvas.Size.Width + 10, 10);
            GenerateFood();
        }

        // смерть змейки
        private void Die()
        {
            Settings.GameOver = true;
            Form1.Form1Reference.pbCanvas.Visible = false;
            lblScore.Visible = false;
            // lblScore = "";


            lblGameOver = new Label()
            {
                Location = new Point(this.Size.Width / 20, this.Size.Height / 20),
                ForeColor = Color.Black,
                Size = new System.Drawing.Size(200, 50),
                Font = new Font(lblGameOver.Font.Name, 15,
                  lblGameOver.Font.Style)
            };
            lblGameOver.Parent = this;
            string gameOver = "Game over\nYour score is: " + Settings.Score;
            lblGameOver.Text = gameOver;
            lblGameOver.Visible = true;
            //добавить строки
            //  string[] row = new string[3] { "", lblPlayerName.Text, Settings.Score.ToString() };
        }
        // создаем рандомное число
        static int GenerateDigit(Random random, int maxPos)
        {
            return random.Next(0, maxPos);
        }

        // создаем препятствие
        private void GenerateObstacle()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Circle circle = new Circle
            {
                X = GenerateDigit(random, maxXPos),
                Y = GenerateDigit(random, maxYPos)
            };
            Obstacle.Add(circle);
        }

        // создаем еду
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            food = new Circle();
            food.X = GenerateDigit(random, maxXPos);
            food.Y = GenerateDigit(random, maxYPos);
        }

        // переход на новый лвл
        private void NewLevel()
        {

            Snake.Clear();
            GenerateObstacle();
            GenerateFood();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);
            Settings.Goal += 1000;
            Settings.Speed++;
            Settings.LvlCounter++;
            lblScore.Text = "Score: " + Settings.Score.ToString()
                          + "\nGoal for " + Settings.LvlCounter + " level: " + Settings.Goal
                          + "\nCurrent speed is " + Settings.Speed;
            //DrawString(lblScore, pbCanvas.Size.Width + 10, 10);


        }*/
    }
}

            
