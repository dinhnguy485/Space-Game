using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//May 15th 2024
//Tri Nguyen_ISC3U
//Space Game
namespace Space_Game
{
    public partial class MainForm : Form
    {
        Random randGen = new Random();
        Rectangle player1 = new Rectangle(200, 375, 20, 20);
        Rectangle player2 = new Rectangle(470, 375, 20, 20);
        Rectangle time = new Rectangle(345, 0, 10, 700);

        List<Rectangle> planetListRight = new List<Rectangle>();
        List<Rectangle> planetListLeft = new List<Rectangle>();
        List<int> planetSpeed = new List<int>();
        List<int> planetSize = new List<int>();


        int player1Score = 0;
        int player2Score = 0;
        int playerSpeed = 4;
        int planetSpeeds = 4;
        int timeSpeed = 1;
        int randValue;
        int randX;

        bool upPressed = false;
        bool downPressed = false;
        bool wPressed = false;
        bool sPressed = false;



        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
        Image Allies = Properties.Resources.US;
        Image Axis = Properties.Resources.German;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upPressed = false;
                    break;
                case Keys.Down:
                    downPressed = false;
                    break;

                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upPressed = true;
                    break;
                case Keys.Down:
                    downPressed = true;
                    break;

                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
            }
        }

        private void playerMovement()
        {
            if (wPressed == true && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
            }
            if (sPressed == true && player1.Y < this.Height - player1.Height )
            {
                player1.Y += playerSpeed;
            }

            if (upPressed == true && player2.Y > 0)
            {
                player2.Y = player2.Y - playerSpeed;
            }

            if (downPressed == true && player2.Y < this.Height - player2.Height )
            {
                player2.Y = player2.Y + playerSpeed;
            }
        }

        private void HandleIntersection()
        {
            for (int i = 0; i < planetListRight.Count; i++)
            {
                if (player1.IntersectsWith(planetListRight[i]))
                {
                    player1.Y = 375;
                }
                else if (player1.Y <= 0)
                {
                    player1.Y = 375;
                    player1Score++;
                }

                if (player2.IntersectsWith(planetListRight[i]))
                {
                    player2.Y = 375;
                }
                else if (player2.Y <= 0)
                {
                    player2.Y = 375;
                    player2Score++;
                }
            }

            for (int i = 0; i < planetListLeft.Count; i++)
            {
                if (player1.IntersectsWith(planetListLeft[i]))
                {
                    player1.Y = 375;
                }
                else if (player1.Y <= 0)
                {
                    player1.Y = 375;
                    player1Score++;
                }

                if (player2.IntersectsWith(planetListLeft[i]))
                {
                    player2.Y = 375;
                }
                else if (player2.Y <= 0)
                {
                    player2.Y = 375;
                    player2Score++;
                }
            }
        }

        private void PlanetMovement()
        {
            for (int i = 0; i < planetListRight.Count; i++)
            {

                int x = planetListRight[i].X + planetSpeeds;
                planetListRight[i] = new Rectangle(x, planetListRight[i].Y, 30, 5);
            }

            for (int i = 0; i < planetListLeft.Count; i++)
            {

                int x = planetListLeft[i].X - planetSpeeds;
                planetListLeft[i] = new Rectangle(x, planetListLeft[i].Y, 30, 5);
            }
        }

        private void HandleTimer()
        {
            time.Y += timeSpeed;
            if (time.Y >= this.Height)
            {
                gameTimer.Stop();
            }
        }

        private void PlanetSpawning()
        {
            randValue = randGen.Next(0, 100);
            if (randValue < 20)
            {
                if (randValue < 10)
                {
                    randX = randGen.Next(50, this.Height - 100);
                    Rectangle planet = new Rectangle(-10, randX, 30, 5);
                    planetListRight.Add(planet);
                }
                else if (randValue > 10)
                {
                    randX = randGen.Next(50, this.Height - 100);
                    Rectangle planet = new Rectangle(700, randX, 30, 5);
                    planetListLeft.Add(planet);
                }
            }
        }

        private void HandlePlanetRemoval()
        {
            for (int i = 0;i< planetListRight.Count; i++)
            {
                if (planetListRight[i].X >= this.Width)
                {
                    planetListRight.RemoveAt(i);
                }
            }

            for (int i = 0; i< planetListLeft.Count; i++)
            {
                if (planetListLeft[i].X < 0)
                {
                    planetListLeft.RemoveAt(i);
                }
            }
        }



        private void gameTimer_Tick(object sender, EventArgs e)
        {
            playerMovement();
            HandleIntersection();
            PlanetMovement();
            PlanetSpawning();
            HandlePlanetRemoval();
            Refresh();
        }
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            player1Label.Text = $"Allies: {player1Score}";
            player2Label.Text = $"{player2Score}: Axis";
            e.Graphics.DrawImage(Allies, player1);
            e.Graphics.DrawImage(Axis, player2);
            for(int i = 0; i< planetListRight.Count; i++)
            {
                e.Graphics.FillRectangle (whiteBrush, planetListRight[i]);
            }

            for (int i = 0; i < planetListLeft.Count; i++)
            {
                e.Graphics.FillRectangle(whiteBrush, planetListLeft[i]);
            }
            e.Graphics.FillRectangle(whiteBrush, time);
        }

        private void gameTImer2_Tick(object sender, EventArgs e)
        {
            HandleTimer();
        }
    }
}
