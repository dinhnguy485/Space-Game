using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
//May 15th 2024
//Tri Nguyen_ISC3U
//Space Game
namespace Space_Game
{
    public partial class MainForm : Form
    {

        Random randGen = new Random();
        SoundPlayer collision = new SoundPlayer(Properties.Resources.Collision);

        //Players and time.
        Rectangle player1 = new Rectangle(150, 375, 40, 35);
        Rectangle player2 = new Rectangle(500, 375, 40, 35);
        Rectangle time = new Rectangle(345, 0, 10, 700);

        //players, obstacles settings.
        List<Rectangle> planetListRight = new List<Rectangle>();
        List<Rectangle> planetListLeft = new List<Rectangle>();
        List<int> planetSpeedRight = new List<int>();
        List<int> planetSpeedLeft = new List<int>();
        List<int> planetSizeRight = new List<int>();
        List<int> planetSizeLeft = new List<int>();

        //Score, Speed variables.
        int player1Score = 0;
        int player2Score = 0;
        int playerSpeed = 4;
        int timeSpeed = 1;
        int randValue;
        int randX;

        //player movement boolean.
        bool upPressed = false;
        bool downPressed = false;
        bool wPressed = false;
        bool sPressed = false;


        //brushes and images.
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        List<Image> Plane1 = new List<Image> ( new Image[] { Properties.Resources.Plane1, Properties.Resources.plane1Flip });
        List<Image> Plane2 = new List<Image>(new Image[] { Properties.Resources.plane2, Properties.Resources.plane2Flip });
        Image Allies = Properties.Resources.Plane1;
        Image Axis = Properties.Resources.plane2;
        Image bulletRight = Properties.Resources.bulletRight;
        Image bulletLeft = Properties.Resources.bulletLeft;
        

        public MainForm()
        {
            InitializeComponent();
        }



        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            //Check the keys status.
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
            //check the key status when it is pressed.
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

                //check players input for the first screen.
                case Keys.Escape:
                    if (gameTimer.Enabled == false && gameTimer2.Enabled == false)
                    {
                        Application.Exit();
                    }
                    break;


                //when Space is pressed, reset every thing in the game and run it
                case Keys.Space:
                    if (gameTimer.Enabled == false && gameTimer2.Enabled == false)
                    {
                        InitializeGame();
                    }
                    break;
            }
        }

        //a method to reset every game variables when it's called.
        private void InitializeGame()
        {
            titleLabel.Text = " ";
            subtitleLabel.Text = " ";
            planetListLeft.Clear();
            planetListRight.Clear();
            planetSizeLeft.Clear();
            planetSizeRight.Clear();
            planetSpeedLeft.Clear();
            planetSpeedRight.Clear();
            time.Y = 0;
            player1Score = 0;
            player2Score = 0;
            player1 = new Rectangle(150, 375, 40, 35);
            player2 = new Rectangle(500, 375, 40, 35);
            gameTimer.Enabled = true;
            gameTimer2.Enabled = true;
        }

        //a method to move players
        private void playerMovement()
        {
            if (wPressed == true && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
                Allies = Plane1[0];
            }
            if (sPressed == true && player1.Y < this.Height - player1.Height )
            {
                player1.Y += playerSpeed;
                Allies = Plane1[1];
            }

            if (upPressed == true && player2.Y > 0)
            {
                player2.Y = player2.Y - playerSpeed;
                Axis = Plane2[0];

            }

            if (downPressed == true && player2.Y < this.Height - player2.Height )
            {
                player2.Y = player2.Y + playerSpeed;
                Axis = Plane2[1];
            }
        }


        //a method to check for intersection with the bullets and the goal line
        //remove the bullets lists whenever it collides.
        private void HandleIntersection()
        {
            for (int i = 0; i < planetListRight.Count; i++)
            {
                if (player1.IntersectsWith(planetListRight[i]))
                {
                    player1.Y = 375;
                    collision.Play();
                    planetListRight.RemoveAt(i);
                    planetSpeedRight.RemoveAt(i);
                    planetSizeRight.RemoveAt(i);
                }
                else if (player1.Y <= 0)
                {
                    player1.Y = 375;
                    player1Score++;
                }

            }

            for (int i = 0; i < planetListRight.Count; i++)
            {
                if (player2.IntersectsWith(planetListRight[i]))
                {
                    player2.Y = 375;
                    collision.Play();
                    planetListRight.RemoveAt(i);
                    planetSpeedRight.RemoveAt(i);
                    planetSizeRight.RemoveAt(i);
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
                    collision.Play();
                    planetListLeft.RemoveAt(i);
                    planetSpeedLeft.RemoveAt(i);
                    planetSizeLeft.RemoveAt(i);
                }
                else if (player1.Y <= 0)
                {
                    player1.Y = 375;
                    player1Score++;
                }

                if (player2.IntersectsWith(planetListLeft[i]))
                {
                    player2.Y = 375;
                    collision.Play();
                    planetListLeft.RemoveAt(i);
                    planetSpeedLeft.RemoveAt(i);
                    planetSizeLeft.RemoveAt(i);

                }
                else if (player2.Y <= 0)
                {
                    player2.Y = 375;
                    player2Score++;
                }
            }
        }

        //to move the bullets in random speed
        private void PlanetMovement()
        {
            for (int i = 0; i < planetListRight.Count; i++)
            {

                int x = planetListRight[i].X + planetSpeedRight[i];
                planetListRight[i] = new Rectangle(x, planetListRight[i].Y, planetSizeRight[i], 20);
            }

            for (int i = 0; i < planetListLeft.Count; i++)
            {

                int x = planetListLeft[i].X + planetSpeedLeft[i];
                planetListLeft[i] = new Rectangle(x, planetListLeft[i].Y, planetSizeLeft[i], 20);
            }
        }

        //a method to check the timer
        private void HandleTimer()
        {
            time.Y += timeSpeed;
            if (time.Y >= this.Height + 1)
            {
                gameTimer.Stop();
                gameTimer2.Stop();
                Refresh();
            }

        }


        // a method to generate random bullets, speeds, size
        private void PlanetSpawning()
        {
            randValue = randGen.Next(0, 100);
            if (randValue < 15)
            {
                if (randValue < 7)
                {
                    randX = randGen.Next(50, this.Height - 100);
                    Rectangle planet = new Rectangle(-10, randX, 30, 20);
                    planetListRight.Add(planet);
                    planetSpeedRight.Add(randGen.Next(5, 15));
                    planetSizeRight.Add(randGen.Next(15, 30));
                }
                else if (randValue > 7)
                {
                    randX = randGen.Next(50, this.Height - 100);
                    Rectangle planet = new Rectangle(700, randX, 30, 20);
                    planetListLeft.Add(planet);
                    planetSpeedLeft.Add(randGen.Next(-15, -5));
                    planetSizeLeft.Add(randGen.Next(15, 30));
                }
            }
        }

        // a method to remove bullets when it hits the sides
        private void HandlePlanetRemoval()
        {
            for (int i = 0;i< planetListRight.Count; i++)
            {
                if (planetListRight[i].X > this.Width)
                {
                    planetListRight.RemoveAt(i);
                    planetSpeedRight.RemoveAt(i);
                    planetSizeRight.RemoveAt(i);
                }
            }

            for (int i = 0; i< planetListLeft.Count; i++)
            {
                if (planetListLeft[i].X < -20)
                {
                    planetListLeft.RemoveAt(i);
                    planetSpeedLeft.RemoveAt(i);
                    planetSizeLeft.RemoveAt(i);
                }
            }
        }

        //A game that runs the game.
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
            //paint the welcome screen
            if(gameTimer.Enabled == false && gameTimer2.Enabled == false && time.Y ==0)
            {
                titleLabel.Text = "WW2 GAME";
                subtitleLabel.Text = "Press Space to start, ESC to escape";
            }

            // run the game when timer is true
            else if (gameTimer.Enabled == true && gameTimer2.Enabled == true)
            {
                player1Label.Text = $"Allies: {player1Score}";
                player2Label.Text = $"{player2Score}: Axis";
                e.Graphics.DrawImage(Allies, player1);
                e.Graphics.DrawImage(Axis, player2);
                for (int i = 0; i < planetListRight.Count; i++)
                {
                    e.Graphics.DrawImage(bulletRight, planetListRight[i]);
                }

                for (int i = 0; i < planetListLeft.Count; i++)
                {
                    e.Graphics.DrawImage(bulletLeft, planetListLeft[i]);
                }

                e.Graphics.FillRectangle(whiteBrush, time);
            }

            //runs the end screen when the games end.
            else
            {
                if (player1Score == player2Score)
                {
                    titleLabel.Text = "It's a tie";
                    subtitleLabel.Text = $"You both score {player1Score} points.";
                }
                else if (player1Score > player2Score)
                {
                    titleLabel.Text = "The Allies win";
                    subtitleLabel.Text = $"The Allies score {player1Score} points ";
                    subtitleLabel.Text += $"\nThe Axis score {player2Score} points";
                }
                else
                {
                    titleLabel.Text = "The Axis win";
                    subtitleLabel.Text = $"The Allies score {player1Score} points ";
                    subtitleLabel.Text += $"\nThe Axis score {player2Score} points";
                }
            }

        }

        //check for the timer.
        private void gameTImer2_Tick(object sender, EventArgs e)
        {
            HandleTimer();
            //Refresh();
        }
    }
}
