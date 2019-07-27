using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Snake
{
    public partial class Form1 : Form
    {
        public static Form1 Form1Reference { get; set; }

        private List<Circle> Snake = new List<Circle>();
        private List<Circle> Obstacle = new List<Circle>();
        private Circle food = new Circle();
        public Label lblScore = new Label();
        private Label lblGameOver = new Label();
        private Label lblName = new Label();
        private Label lblBack = new Label();
        private Label lblPlayerName = new Label();
        private Label lblScoreTable = new Label();

        private Brush fadeBrush = new SolidBrush(Color.LimeGreen);
        private Button btnStartGame = new Button();
        private Button btnScoreBoard = new Button();
        private Button btnQuit = new Button();
        private String fadeText = String.Format("{0}", "Level " + Settings.LvlCounter);
        //таблица рекордов на 10 позиций

        
       // private string lblScore = "";
        Random random = new Random();


        public Form1()
        {
            Form1Reference = this;
            InitializeComponent();
          
            //настройки по умолчанию
            new Settings();

            //размер окна постоянен
            this.FormBorderStyle = FormBorderStyle.Fixed3D;

            //Назначаем скорость игры и таймер
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
           // this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            //начать новую игру
            StartGame();
        }

        //начало игры
        private void StartGame()
        {
            gameTimer.Start();

            PlayerInfo.Name = lblPlayerName.Text;
            lblGameOver.Visible = false;
            pbCanvas.Visible = true;
            lblScore.Visible = true;
            //настройки по умолчанию
            new Settings();

            //создаем новый объект игрока
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            Obstacle.Clear();
            Circle wall = new Circle { X = 5, Y = 10 };
            Obstacle.Add(wall);
            
            lblScore.Text = "Score: " + Settings.Score.ToString()
                          + "\nGoal for " + Settings.LvlCounter + " level: " + Settings.Goal
                          + "\nCurrent speed is " + Settings.Speed;

           // DrawString(lblScore, pbCanvas.Size.Width + 10, 10);
            //рандомное появление еды
            GenerateFood();

        }
       
        // действие в тик таймера
        private void UpdateScreen(object sender, EventArgs e)
        {
            gameTimer.Interval = 1000 / Settings.Speed;

            //чекаем Game Over           
            if (Settings.GameOver)
            {
                //Start game
                GameMenu();                
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();                
            }
            pbCanvas.Invalidate();
            
        }

        // прорисовка
        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (!Settings.GameOver)
            {
                //прорисовываем элементы
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColor;
                    if (i == 0)
                        snakeColor = Brushes.Black; //голова
                    else
                        snakeColor = Brushes.Green; //тело

                    //рисуем змею
                    canvas.FillEllipse(snakeColor,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));

                    //рисуем еду
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                                      food.Y * Settings.Height,
                                      Settings.Width, Settings.Height));
                }

                for (int i = 0; i < Obstacle.Count; i++)
                {
                    //рисуем премятствия
                    Random random = new Random();
                    canvas.FillRectangle(Brushes.Gray,
                        new Rectangle(Obstacle[i].X * Settings.Width,
                                      Obstacle[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));
                }
            }
        }

        // движение игрока
        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //двигаем голову
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }
                    //Максимальные координаты (края карты)
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    //Столкновение с краями
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    //Столкновение с телом
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                           Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //столкновение с препятствием
                    for (int j = 0; j < Obstacle.Count; j++)
                    {
                        if (Snake[0].X == Obstacle[j].X &&
                            Snake[0].Y == Obstacle[j].Y)
                        {
                            Die();
                        }
                    }

                    //Столкновение с едой
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                    //новый лвл
                    if (Settings.Score >= Settings.Goal)
                    {
                        NewLevel();
                    }
                }
                //Двигаем тело
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }


        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);

            //выход в меню
            if (e.KeyCode==Keys.Escape)
            {
                GameMenu();
            }

            //ввод имени
            if (e.KeyCode == Keys.Back)
                if (lblPlayerName.Text != "")
                    lblPlayerName.Text = lblPlayerName.Text.Substring(0, lblPlayerName.Text.Length - 1);
                else
                    return;
            else
                if (lblPlayerName.Text.Length < 7)
                    lblPlayerName.Text += ((char)e.KeyCode).ToString();

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

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

            /*string scoreStr = "Score: " + Settings.Score.ToString()
                          + "\nGoal for " + Settings.LvlCounter + " level: " + Settings.Goal
                          + "\nCurrent speed is " + Settings.Speed;
            lblScore.Visible = false;*/
            lblScore.Text = "Score: " + Settings.Score.ToString()
                          + "\nGoal for " + Settings.LvlCounter + " level: " + Settings.Goal
                          + "\nCurrent speed is " + Settings.Speed; ;
            //DrawString(scoreStr, pbCanvas.Size.Width + 10, 10f);
            GenerateFood();
        }

        // смерть змейки
        private void Die()
        {
            Settings.GameOver = true;
            pbCanvas.Visible = false;
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
            //DrawString("YOU DIED", Form1.si, 15);
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


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //счет
            lblScore = new Label()
            {
                Text = "Score: " + Settings.Score.ToString(),
                Location = new Point(pbCanvas.Size.Width + 10, 10),
                ForeColor = Color.Black,
                Size = new System.Drawing.Size(200, 100),
                Font = new Font(lblScore.Font.Name, 15,
                  lblScore.Font.Style)
            };
            lblScore.Parent = this;

            lblBack = new Label()
       {
           Location = new Point(0, this.Size.Height - 100),
           Size = new System.Drawing.Size(200, 40),
           Text = "",
       };
            //имя игрока
            lblName = new Label()
            {
                Text = "Enter your name: ",
                Location = new Point(this.Size.Width / 3, this.Size.Height - 150),
                ForeColor = Color.Black,
                Size = new System.Drawing.Size(150, 30),
                Font = new Font(lblName.Font.Name, 13,
                  lblName.Font.Style)
            };
            lblName.Parent = this;
            lblName.Select();
            lblScoreTable = new Label()
            {
                Text = "",
                Location = new Point(this.Size.Width / 3, 50),
                ForeColor = Color.Black,
                Size = new System.Drawing.Size(300, 500),
                Font = new Font(lblName.Font.Name, 13,
                  lblName.Font.Style)
            };
            lblScoreTable.Parent = this;

            lblPlayerName = new Label()
            {
                Text = "Username",
                Location = new Point(this.Size.Width / 3, this.Size.Height - 100),
                ForeColor = Color.Black,
                Size = new System.Drawing.Size(150, 30),
                Font = new Font(lblPlayerName.Font.Name, 13,
                                lblPlayerName.Font.Style)
            };
            lblPlayerName.Parent = this;

            GameMenu();
            
        }
        
        // игровое меню
        private void GameMenu()
        {
            ClearForm();

            lblName.Visible = true;
            lblPlayerName.Visible = true;           
            lblBack.Parent = this;

            //кнопка начала игры
            btnStartGame = new Button()
            {
                Text = "Start Game",
                Location = new Point(this.Size.Width / 3, this.Size.Height / 3 - 20),
                ForeColor = Color.Black,
                BackColor=Color.Beige,
                FlatStyle=FlatStyle.Flat,
               
                Size = new System.Drawing.Size(200, 40),
                Font = new Font(btnStartGame.Font.Name, 15,
                  btnStartGame.Font.Style),

            }; 
            btnStartGame.FlatAppearance.BorderSize = 0;
            
            btnStartGame.Parent = this;
            
            
            btnStartGame.Click += new EventHandler(this.Start_Game);

            //кнопка таблицы рекордов
            btnScoreBoard = new Button()
            {
                Text = "Rating",
                Location = new Point(this.Size.Width / 3, this.Size.Height / 3 + 25),
                ForeColor = Color.Black,
                BackColor = Color.Azure,
                FlatStyle = FlatStyle.Flat,
                Size = new System.Drawing.Size(200, 40),
                Font = new Font(btnScoreBoard.Font.Name, 15,
                  btnScoreBoard.Font.Style)
            };
            btnScoreBoard.FlatAppearance.BorderSize = 0;
            btnScoreBoard.Parent = this;
            btnScoreBoard.Click += new EventHandler(this.Rating);

            //кнопка выхода
            btnQuit = new Button()
            {
                Text = "Quit",
                Location = new Point(this.Size.Width / 3, this.Size.Height / 3 + 70),
                ForeColor = Color.Black,
                BackColor = Color.Bisque,
                FlatStyle = FlatStyle.Flat,
                Size = new System.Drawing.Size(200, 40),
                Font = new Font(btnQuit.Font.Name, 15,
                  btnQuit.Font.Style)
            };
            btnQuit.FlatAppearance.BorderSize = 0;
            btnQuit.Parent = this;
            btnQuit.Click += new EventHandler(this.Quit);

        }

        // кнопка очистка формы и начало игры
        void Start_Game(Object sender, EventArgs e)
        {
            ClearForm();
            StartGame();
        }

        // кнопка рейтинга
        void Rating(Object sender, EventArgs e)
        {
            ClearForm();           
            lblScoreTable.Visible = true;
            lblScoreTable.Text = "";
            lblBack.Text = "Для выхода в меню нажмите ESC";
            string fileNames = "Names.txt";
            string fileScores = "Scores.txt";

            // Create a list of parts.
            List<Part> parts = new List<Part>();

            //проверяем есть ли такой файл, если его нет, то создаем
            //для файла с именами
            if (File.Exists(fileNames) != true)            
                using (StreamWriter sw = new StreamWriter(new FileStream(fileNames, FileMode.Create, FileAccess.Write)))                
                    sw.WriteLine(PlayerInfo.Name);             //пишем строчку 
            else                                          //если файл есть, то откываем его и пишем в него 
                using (StreamWriter sw = new StreamWriter(new FileStream(fileNames, FileMode.Open, FileAccess.Write)))
                {
                    (sw.BaseStream).Seek(0, SeekOrigin.End);         //идем в конец файла и пишем строку или пишем то, что хотим
                    sw.WriteLine(PlayerInfo.Name);
                }          
            //для файла со счетом    
            if (File.Exists(fileScores) != true)
                using (StreamWriter sw = new StreamWriter(new FileStream(fileScores, FileMode.Create, FileAccess.Write)))
                    sw.WriteLine(Settings.Score);             //пишем строчку
            else                                          //если файл есть, то откываем его и пишем в него 
                using (StreamWriter sw = new StreamWriter(new FileStream(fileScores, FileMode.Open, FileAccess.Write)))
                {
                    (sw.BaseStream).Seek(0, SeekOrigin.End);         //идем в конец файла и пишем строку или пишем то, что хотим
                    sw.WriteLine(Settings.Score);
                }

            //добавляем части в лист            
           StreamReader fsNames=new StreamReader(fileNames);
           StreamReader fsScores = new StreamReader(fileScores);
           for (int i = 0; i < 10;i++ )
           {
               try
               {
                   string name = fsNames.ReadLine();
                   int score = int.Parse(fsScores.ReadLine());
                   parts.Add(new Part() { PartName = name, PartScore = score });
               }
               catch (ArgumentNullException q)
               {
                   Console.WriteLine(q.Message);
               }
           }
            fsNames.Close();
            fsScores.Close();
            // Write out the parts in the list. This will call the overridden 
            // ToString method in the Part class.
            Console.WriteLine("\nBefore sort:");
            foreach (Part aPart in parts)
            {
                Console.WriteLine(aPart);
            }

            // Call Sort on the list. This will use the 
            // default comparer, which is the Compare method 
            // implemented on Part.
            parts.Sort();
            parts.Reverse();
            Console.WriteLine("\nAfter sort by part number and reverse:");
            foreach (Part aPart in parts)
            {
                Console.WriteLine(aPart);
            }
            foreach (Part aPart in parts)
            {

                lblScoreTable.Text = lblScoreTable.Text+"\n" + aPart;
            }
        }
             
        //кнопка выхода
        void Quit(Object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы точно хотите выйти?",
                            "Quit",
                            MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        //очистка формы
        private void ClearForm()
        {
            lblName.Visible = false;
            lblPlayerName.Visible = false;
            lblScoreTable.Visible=false;
            pbCanvas.Visible = false;
            lblScore.Visible = false;
           // lblScore = "";
            btnStartGame.Visible = false;
            btnScoreBoard.Visible = false;
            btnQuit.Visible = false;
            btnStartGame.Enabled = false;
            btnScoreBoard.Enabled = false;
            btnQuit.Enabled = false;
            lblBack.Text = "";

            gameTimer.Dispose();
        }

        public void DrawString(string drawString, float x, float y)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }
       // public void ClearString()
    }
}

