using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout_Game
{
    public partial class FormBreakout : Form
    {

        bool goLeft, goRight, isGameOver;
        int score, ballx, bally, playerSpeed;
        Random rnd = new Random();
        PictureBox[] blockArray;
        List<PictureBox> extraBalls = new List<PictureBox>();
        private PictureBox changeWidthPictureBox; // PictureBox für Changewidth-Player
        private bool pictureBoxAdded = false; // Variable zur Verfolgung des Hinzufügens
        private PictureBox addBallsPictureBox; // PictureBox für Changewidth-Player
        private bool pictureBoxBallAdded = false; // Variable zur Verfolgung des Hinzufügens

        public FormBreakout()
        {
            InitializeComponent();
            //setupGame();
            PlaceBlocks();
            


        }
        private void ResetBall()
        {
            picBall.Location = new Point(189 , 260);
        }
        private void setupGame()
        {
            
            score = 0;
            ballx = 5;
            bally = 5;
            playerSpeed = 18;
            lblScore.Text = "Score: " + score;
            changeWidthPictureBox = null;
            pictureBoxAdded = false;
            addBallsPictureBox = null;
            pictureBoxBallAdded = false;

            picPlayer.Width = 90;
            picPlayer.Location = new Point(345, 448);

            ResetBall();

            GameTimer.Start();
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "Blocks")
                {
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));                    
                }
                // Überprüfe, ob die Hintergrundfarbe schwarz ist (RGB-Wert 0,0,0)
                if (x.BackColor.ToArgb() == Color.FromArgb(0, 0, 0).ToArgb() && (string)x.Tag == "Blocks")
                {
                    // Ändere die Hintergrundfarbe zu weiß (RGB-Wert 255, 255, 255)
                    x.BackColor = Color.FromArgb(255, 255, 255);
                }
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            lblScore.Text = "Score: " + score;
            if (goLeft == true && picPlayer.Left > 5)
            {
                picPlayer.Left -= playerSpeed;
            }
            if (goRight == true && picPlayer.Left < 524)
            {
                picPlayer.Left += playerSpeed;
            }

            //picBall.Left += ballx;
            //picBall.Top += bally;

            //if (picBall.Left < 0 || picBall.Left > 605)
            //{
            //    ballx = -ballx;
            //}
            //if (picBall.Top < 0)
            //{
            //    bally = -bally;
            //}

            //if (picBall.Bounds.IntersectsWith(picPlayer.Bounds))
            //{
            //   // picBall.Top = 463;
            //    bally = rnd.Next(5, 10) * -1;
            //    if (ballx < 0)
            //    {
            //        ballx = rnd.Next(5, 10) * -1;
            //    }
            //    else
            //    {
            //        ballx = rnd.Next(5, 10);
            //    }
            //}
            // Bewege den originalen Ball
            MoveBall(picBall);

            //Bewege die zusätzlichen Bälle und überprüfe Kollisionen
            foreach (PictureBox extraBall in extraBalls)
            {
                MoveBall(extraBall);
            }
            //// Move the extra balls and check for collisions
            //for (int i = 0; i < extraBalls.Count; i++)
            //{

            //    CheckCollisions(extraBalls[i]); // Check collisions for each extra ball
            //}

            CheckCollisions(picBall);
            //foreach (Control x in this.Controls)
            //{
            //    if (x is PictureBox && (string)x.Tag == "Blocks")
            //    {
            //        if (picBall.Bounds.IntersectsWith(x.Bounds))
            //        {
            //            score += 1;
            //            if (score == 3)
            //            {
            //                AddExtraBalls(3);
            //            }
            //           // bally = -bally;
            //            this.Controls.Remove(x);
            //        }
            //    }
            //}



            FunktionAddChangeWidthPictureBox();
            FunktionAddBallsPicturbox();

            if (score == 72)
            {
                gameOver("You Win!! Press Enter to Play Again");
            }
            if (picBall.Top > 500)
            {
                gameOver("You Lose!! Press Enter to try again");
            }

        }   

        private void FormBreakout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
        }
        private void gameOver(string message)
        {
            isGameOver = true;
            GameTimer.Stop();
            lblScore.Text = "Score: " + score + " " + message;
            Controls.Remove(changeWidthPictureBox); // Changewidth-Bild entfernen
            Controls.Remove(addBallsPictureBox); // AddBall-Bild entfernen

        }

        private void FormBreakout_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeBlocks();
                PlaceBlocks();
            }
        }
        private void PlaceBlocks()
        {
            blockArray = new PictureBox[72];
            int a = 0;
            int top = 50;
            int left = 10;
            for (int i = 0; i < blockArray.Length; i++)
            {
                blockArray[i] = new PictureBox();
                blockArray[i].Height = 10;
                blockArray[i].Width = 45;
                blockArray[i].Tag = "Blocks";
                blockArray[i].BackColor = Color.White;
                if (a == 12)
                {
                    top = top + 20;
                    left = 10;
                    a = 0;
                }
                if (a < 12)
                {
                    a++;
                    blockArray[i].Left = left;
                    blockArray[i].Top = top;
                    this.Controls.Add(blockArray[i]);
                    left = left + 51;
                }
            }
            setupGame();
        }
        private void removeBlocks()
        {
            foreach (PictureBox x in blockArray)
            {
                this.Controls.Remove(x);
            }
        }
        private void AddExtraBalls(int count)
        {

            for (int i = 0; i < count; i++)
            {
                PictureBox newBall = new PictureBox();
                newBall.Size = new Size(12, 14);
                newBall.BackColor = Color.Red;
                // Vertikal gestapelt mit 20 Pixel Abstand
                //  newBall.Location = new Point(picBall.Location.X, picBall.Location.Y + i * 20);
                this.Controls.Add(newBall);
                extraBalls.Add(newBall);

            }
        }
            
        private void CheckCollisions(PictureBox ball)
        {
            // Check collisions with the blocks for the specified ball
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "Blocks")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 1;
                        this.Controls.Remove(x);
                    }
                }
            }
        }
     
        private void MoveBall(PictureBox ball)
        {
            ball.Left += ballx;
            ball.Top += bally;

            // Überprüfe Kollisionen mit den Blöcken
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "Blocks")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 1;
                        bally = -bally;
                        this.Controls.Remove(x);
                    }
                }
            }
            if (ball.Bounds.IntersectsWith(picPlayer.Bounds))
            {
               
                bally = rnd.Next(5, 10) * -1;
                if (ballx < 0)
                {
                    ballx = rnd.Next(5, 10) * -1;
                }
                else
                {
                    ballx = rnd.Next(5, 10);
                }
            }

            // Überprüfe, ob der Ball den Rahmen berührt
            if (ball.Left < 0 || ball.Right > ClientSize.Width)
            {
                ballx = -ballx;
            }
            if (ball.Top < 0 || ball.Bottom > ClientSize.Height)
            {
                gameOver( "sie haben verlorren");
            }
        }
        private void ChangePlayerWidth()
        {
           
                picPlayer.Width = 150;
            
        }

        private void CreateChangeWidthPictureBox()
        {
            // Überprüfen, ob die PictureBox bereits hinzugefügt wurde
            if (!pictureBoxAdded)
            {
                // Changewidth-Bild erstellen und konfigurieren
                changeWidthPictureBox = new PictureBox
                {
                    Width = 40,
                    Height = 40,
                    BackColor = Color.Transparent,
                    // Bild aus Ressourcen setzen
                    Image = Properties.Resources.settings 
                };
                // Changewidth-PictureBox zur Form hinzufügen

                Controls.Add(changeWidthPictureBox); 
                // Zufällige X-Position setzen
                changeWidthPictureBox.Location = new Point(new Random().Next(0, ClientSize.Width - changeWidthPictureBox.Width), 0);
                // Status aktualisieren
                pictureBoxAdded = true; 
            }
        }
        private void FunktionAddChangeWidthPictureBox()
        {
            if (score == 1 && changeWidthPictureBox == null) // Wenn Score 2 ist und Changewidth-Bild nicht erstellt wurde
            {
                CreateChangeWidthPictureBox(); // Changewidth-PictureBox erstellen
            }

            if (changeWidthPictureBox != null) // Wenn Changewidth-Bild existiert
            {
                changeWidthPictureBox.Top += 5; // Changewidth-Bild nach unten bewegen

                if (changeWidthPictureBox.Bounds.IntersectsWith(picPlayer.Bounds)) // Kollision mit Spieler-PictureBox prüfen
                {
                    Controls.Remove(changeWidthPictureBox); // Changewidth-Bild entfernen
                    changeWidthPictureBox.Dispose(); // Ressourcen freigeben
                    changeWidthPictureBox = null;
                    ChangePlayerWidth(); // Spieler-PictureBox-Breite erhöhen
                }
            }
        }
        private void CreateAddBalls()
        {
            // Überprüfen, ob die PictureBox bereits hinzugefügt wurde
            if (!pictureBoxBallAdded)
            {
                // Changewidth-Bild erstellen und konfigurieren
                addBallsPictureBox = new PictureBox
                {
                    Width = 40,
                    Height = 40,
                    BackColor = Color.Red,
                    // Bild aus Ressourcen setzen
                    Image = Properties.Resources.shoot3
                };
                // Changewidth-PictureBox zur Form hinzufügen

                Controls.Add(addBallsPictureBox);
                // Zufällige X-Position setzen
                addBallsPictureBox.Location = new Point(new Random().Next(0, ClientSize.Width - addBallsPictureBox.Width), 0);
                // Status aktualisieren
                pictureBoxBallAdded= true;
            }
        }

        private void FunktionAddBallsPicturbox()
        {
            if (score == 2 && addBallsPictureBox == null) // Wenn Score 2 ist und Changewidth-Bild nicht erstellt wurde
            {
                CreateAddBalls(); // Changewidth-PictureBox erstellen
            }

            if (addBallsPictureBox != null) // Wenn Changewidth-Bild existiert
            {
                addBallsPictureBox.Top += 5; // Changewidth-Bild nach unten bewegen

                if (addBallsPictureBox.Bounds.IntersectsWith(picPlayer.Bounds)) // Kollision mit Spieler-PictureBox prüfen
                {
                    Controls.Remove(addBallsPictureBox); // Changewidth-Bild entfernen
                    addBallsPictureBox.Dispose(); // Ressourcen freigeben
                    addBallsPictureBox = null;
                    AddExtraBalls(3); // Spieler-PictureBox-Breite erhöhen
                }
            }
        }
    }




    
}
