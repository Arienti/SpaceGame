using SpaceInvaders.business;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Enemies> enemyList = new List<Enemies>();
        List<Bullets> bulletsList = new List<Bullets>();
        List<Rectangle> rectangleList = new List<Rectangle>();
        DispatcherTimer timer = new DispatcherTimer();
        bool L;
        bool R;
        int speed = 8;
        int damage = 50;

        private int shielddamage = 0;
        bool shieldON;
        bool activated = false;
        public static int score = 0;

        int AlienShipTime = new Random().Next(4500, 5500);
        bool AlienShipMoveUp = false;
        double AlienStep = 0;

        bool gameover = false;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
            Space.Focus();
            Space.Children.Remove(ShieldRec);

            ImageBrush.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}ufoRed.png"));

            ImageBrush background = new ImageBrush();
            background.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}Backgroud.png"));
            background.TileMode = TileMode.Tile;
            background.Viewport = new Rect(0, 0, 1, 1);
            background.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            Space.Background = background;

            ImageBrush stars = new ImageBrush();
            stars.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}star2.png"));
            Random randomStars = new Random();
            var starsCount = randomStars.Next(15, 25);
            for (int i = 0; i < starsCount; i++)
            {
                Rectangle star = new Rectangle()
                {
                    Height = 20,
                    Width = 10,
                    Fill = stars,
                    Tag = "stars"
                };
                rectangleList.Add(star);
                Canvas.SetLeft(star, randomStars.Next(0, (int)this.Width));
                Canvas.SetTop(star, randomStars.Next((int)this.Height) / 1.3);
                Space.Children.Add(star);
            }
            ImageBrush image = new ImageBrush();
            image.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}SpaceShip.png"));
            Ship.Fill = image;
            Space.Children.Remove(AlienShips);
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            foreach (var star in rectangleList)
            {
                if ((string)star.Tag == "stars")
                {
                    Canvas.SetTop(star, Canvas.GetTop(star) + 0.5);

                    if (Canvas.GetTop(star) > Canvas.GetTop(Ship) + Ship.Height * 2)
                    {
                        Canvas.SetLeft(star, new Random().Next(0, (int)this.Width));
                        Canvas.SetTop(star, 0 - star.Height);
                    }
                }
            }
            AlienShipTime--;
            if (AlienShipTime == 0)
            {
                Canvas.SetTop(AlienShips, new Random().Next(80, 200));
                Canvas.SetLeft(AlienShips, Application.Current.MainWindow.Width + AlienShips.Width);
                Space.Children.Add(AlienShips);
                AlienShipTime = new Random().Next(4500, 5500);
            }
            if (!AlienShipMoveUp)
            {
                AlienStep += 0.1;
                if (AlienStep >= 2.5)
                {
                    AlienShipMoveUp = true;
                }
            }
            else
            {
                AlienStep -= 0.1;
                if (AlienStep <= -2.5)
                {
                    AlienShipMoveUp = false;
                }
            }
            Canvas.SetTop(AlienShips, Canvas.GetTop(AlienShips) + AlienStep);
            Canvas.SetLeft(AlienShips, Canvas.GetLeft(AlienShips) - 1);
            if (Canvas.GetLeft(AlienShips) < 0 - AlienShips.Width)
            {
                Space.Children.Remove(AlienShips);
            }
            Damage.Content = "Damage: " + damage;
            Score.Content = "Score:  " + score;
            if (damage <= 0)
            {
                Restartxaml restartxaml = new Restartxaml();
                restartxaml.Owner = this;
                restartxaml.Show();
                timer.Stop();
                gameover = true;
            }
            Enemies.enemyCounts--;

            if (Enemies.enemyCounts < Enemies.enemyLimit)
            {
                MoveEnemy();
                Enemies.enemyCounts = Enemies.enemyLimit;
            }
            MoveBullets();

            if (L && Canvas.GetLeft(Ship) > 0)
            {
                Canvas.SetLeft(Ship, Canvas.GetLeft(Ship) - speed);
                foreach (var shield in rectangleList)
                {
                    if ((string)shield.Tag == "shieldactivated")
                    {
                        Canvas.SetLeft(shield, Canvas.GetLeft(shield) - speed);
                    }
                }
            }
            if (R && Canvas.GetLeft(Ship) + Ship.Width * 1.25 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(Ship, Canvas.GetLeft(Ship) + speed);
                foreach (var shield in rectangleList)
                {
                    if ((string)shield.Tag == "shieldactivated")
                    {
                        Canvas.SetLeft(shield, Canvas.GetLeft(shield) + speed);
                    }
                }
            }
            PowerUpIcons();
            MovePowerUpIcons();
        }
        private void Space_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                L = true;
            }
            if (e.Key == Key.Right)
            {
                R = true;
            }
        }

        private void Space_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                L = false;
            }
            if (e.Key == Key.Right)
            {
                R = false;
            }
            if (e.Key == Key.Space)
            {
                SoundPlayer hit = new SoundPlayer($"{App.audioLocation}laserhit.wav");
                hit.Load();
                hit.Play();
                AddBullets();
            }
        }

        public void AddEnemy()
        {
            Enemies enemies = new Enemies();

            enemyList.Add(enemies);
            enemies.DrawEnemy();
            Canvas.SetLeft(enemies.enemyRec, enemies.X);
            Canvas.SetTop(enemies.enemyRec, enemies.Y);
            Space.Children.Add(enemies.enemyRec);
        }
        public void MoveEnemy()
        {
            if (enemyList.Count < Enemies.enemyLimit)
            {
                AddEnemy();
            }
            for (int i = 0; i < enemyList.Count; i++)
            {
                Canvas.SetTop(enemyList[i].enemyRec, Canvas.GetTop(enemyList[i].enemyRec) + Enemies.speed);
                if ((score >= 200) && (score < 600))
                {
                    Enemies.enemyLimit = new Random().Next(6, 8);
                }
                if ((score >= 600) && (score < 1000))
                {
                    Enemies.speed = (int)1.5;
                    Canvas.SetTop(enemyList[i].enemyRec, Canvas.GetTop(enemyList[i].enemyRec) + Enemies.speed);
                }
                if ((score >= 1000) && (score < 1600))
                {
                    Enemies.speed = 3;
                    Enemies.enemyLimit = new Random().Next(8, 12);
                }
                if ((score >= 1600) && (score < 4000))
                {
                    Enemies.enemyLimit = new Random().Next(12, 15);
                }
                if (score > 4000)
                {
                    Enemies.enemyLimit = new Random().Next(15, 20);
                }
                Rect enemyRec = new Rect(Canvas.GetLeft(enemyList[i].enemyRec),
                Canvas.GetTop(enemyList[i].enemyRec), enemyList[i].Width, enemyList[i].Height);

                Rect ship = new Rect(Canvas.GetLeft(Ship), Canvas.GetTop(Ship), Ship.Width, Ship.Height);
                if (enemyRec.IntersectsWith(ship))
                {
                    Space.Children.Remove(enemyList[i].enemyRec);
                    enemyList.Remove(enemyList[i]);
                    damage -= 10;
                }
                if (Canvas.GetTop(enemyList[i].enemyRec) > Canvas.GetTop(Ship) + Ship.Height)
                {
                    damage -= 5;
                    Space.Children.Remove(enemyList[i].enemyRec);
                    enemyList.Remove(enemyList[i]);
                }
                foreach (var shields in rectangleList)
                {
                    if ((string)shields.Tag == "shieldactivated")
                    {
                        ImageBrush shieldImage = new ImageBrush();

                        Rect shield = new Rect(Canvas.GetLeft(shields), Canvas.GetTop(shields),
                        shields.Width, shields.Height);

                        if (enemyRec.IntersectsWith(shield))
                        {
                            shielddamage--;
                            Space.Children.Remove(enemyList[i].enemyRec);
                            enemyList.Remove(enemyList[i]);
                        }
                        if (shielddamage <= 5)
                        {
                            shieldImage.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}shield3.png"));
                            shields.Fill = shieldImage;
                        }
                        if (shielddamage <= 3)
                        {
                            shieldImage.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}shield2.png"));
                            shields.Fill = shieldImage;
                        }
                        if (shielddamage == 1)
                        {
                            shieldImage.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}shield1.png"));
                            shields.Fill = shieldImage;
                        }
                        if (shielddamage <= 0)
                        {
                            activated = false;
                            rectangleList.Remove(shields);
                            Space.Children.Remove(shields);
                            break;
                        }
                        break;
                    }
                }

                for (int j = 0; j < bulletsList.Count; j++)
                {
                    Rect bulletRec = new Rect(Canvas.GetLeft(bulletsList[j].bulletRectangle),
                    Canvas.GetTop(bulletsList[j].bulletRectangle), bulletsList[j].Width, bulletsList[j].Height);

                    if (enemyRec.IntersectsWith(bulletRec))
                    {
                        score += 10;
                        Space.Children.Remove(bulletsList[j].bulletRectangle);
                        bulletsList.Remove(bulletsList[j]);
                        Space.Children.Remove(enemyList[i].enemyRec);
                        enemyList.Remove(enemyList[i]);
                        break;
                    }
                }
            }
        }

        public void AddBullets()
        {
            Bullets bullets = new Bullets();
            if (!gameover)
            {
                if (Bullets.bulletLevel == 0)
                {
                    var X = Canvas.GetLeft(Ship) + Ship.Width / 2 - 3;
                    var Y = Canvas.GetTop(Ship) - 5;
                    bullets.X = X;
                    bullets.Y = Y;
                    bulletsList.Add(bullets);
                    bullets.Draw();
                    Canvas.SetLeft(bullets.bulletRectangle, bullets.X);
                    Canvas.SetTop(bullets.bulletRectangle, bullets.Y);
                    Space.Children.Add(bullets.bulletRectangle);
                }
                if (Bullets.bulletLevel == 1)
                {
                    var X = Canvas.GetLeft(Ship) + Ship.Width - 2;
                    var Y = Canvas.GetTop(Ship) - 5;
                    bullets.X = X;
                    bullets.Y = Y;
                    bulletsList.Add(bullets);
                    bullets.Draw();

                    Canvas.SetLeft(bullets.bulletRectangle, bullets.X);
                    Canvas.SetTop(bullets.bulletRectangle, bullets.Y);
                    Space.Children.Add(bullets.bulletRectangle);
                    Bullets bullet = new Bullets();
                    var X1 = Canvas.GetLeft(Ship) + Ship.Width - Ship.Width;
                    var Y1 = Canvas.GetTop(Ship) - 5;
                    bullet.X = X1;
                    bullet.Y = Y1;
                    bullet.Draw();

                    Canvas.SetLeft(bullet.bulletRectangle, bullet.X);
                    Canvas.SetTop(bullet.bulletRectangle, bullet.Y);
                    Space.Children.Add(bullet.bulletRectangle);
                    bulletsList.Add(bullet);
                }
                if (Bullets.bulletLevel == 2)
                {
                    var X = Canvas.GetLeft(Ship) + Ship.Width + 20;
                    var Y = Canvas.GetTop(Ship) - 5;
                    bullets.X = X;
                    bullets.Y = Y;
                    bulletsList.Add(bullets);
                    bullets.Draw();
                    Canvas.SetLeft(bullets.bulletRectangle, bullets.X);
                    Canvas.SetTop(bullets.bulletRectangle, bullets.Y);
                    Space.Children.Add(bullets.bulletRectangle);

                    Bullets bullet = new Bullets();
                    var X1 = Canvas.GetLeft(Ship) + Ship.Width / 2 - 3;
                    var Y1 = Canvas.GetTop(Ship) - 5;
                    bullet.X = X1;
                    bullet.Y = Y1;
                    bullet.Draw();
                    Canvas.SetLeft(bullet.bulletRectangle, bullet.X);
                    Canvas.SetTop(bullet.bulletRectangle, bullet.Y);
                    Space.Children.Add(bullet.bulletRectangle);
                    bulletsList.Add(bullet);

                    Bullets bulletss = new Bullets();
                    var X2 = Canvas.GetLeft(Ship) - 25;
                    var Y2 = Canvas.GetTop(Ship) - 5;
                    bulletss.X = X2;
                    bulletss.Y = Y2;
                    bulletss.Draw();

                    Canvas.SetLeft(bulletss.bulletRectangle, bulletss.X);
                    Canvas.SetTop(bulletss.bulletRectangle, bulletss.Y);
                    Space.Children.Add(bulletss.bulletRectangle);
                    bulletsList.Add(bulletss);
                }
            }
        }
        public void MoveBullets()
        {
            Bullets bullets = new Bullets();
            for (int i = 0; i < bulletsList.Count; i++)
            {
                bulletsList[i].Y -= bullets.speed;
                Canvas.SetTop(bulletsList[i].bulletRectangle, Canvas.GetTop(bulletsList[i].bulletRectangle) - bullets.speed);
                if (bulletsList[i].Y < -10)
                {
                    Space.Children.Remove(bulletsList[i].bulletRectangle);
                    bulletsList.Remove(bulletsList[i]);
                }
            }
        }
        public void PowerUpIcons()
        {
            PowerUp powerUp = new PowerUp();

            if (PowerUp.Shieldcount == 0)
            {
                powerUp.shieldAppear = true;
                rectangleList.Add(powerUp.shieldRec);
                powerUp.AddPowerUpIcons();
                Space.Children.Add(powerUp.shieldRec);

                PowerUp.Shieldcount = new Random().Next(3000, 4500);
            }
            else
            {
                powerUp.shieldAppear = false;
            }
            PowerUp.Shieldcount--;

            if (PowerUp.PowerUpTime == 0)
            {
                powerUp.bulletAppear = true;
                rectangleList.Add(powerUp.PowerUpBullets);
                powerUp.AddPowerUpIcons();
                Space.Children.Add(powerUp.PowerUpBullets);
                PowerUp.PowerUpTime = new Random().Next(2800, 4200);
            }
            else
            {
                powerUp.bulletAppear = false;
            }
            PowerUp.PowerUpTime--;
        }
        public void MovePowerUpIcons()
        {
            PowerUp power = new PowerUp();
            foreach (var i in rectangleList)
            {
                if ((string)i.Tag == "shield")
                {
                    Canvas.SetTop(i, Canvas.GetTop(i) + power.speed);
                    Rect Shield = new Rect(Canvas.GetLeft(i), Canvas.GetTop(i), i.Height, i.Width);
                    Rect ship = new(Canvas.GetLeft(Ship), Canvas.GetTop(Ship), Ship.Height, Ship.Width);

                    if (Canvas.GetTop(i) > Canvas.GetTop(Ship) + Ship.Height)
                    {
                        rectangleList.Remove(i);
                        Space.Children.Remove(i);
                        break;
                    }
                    if (Shield.IntersectsWith(ship))
                    {
                        shieldON = true;
                        shielddamage = 5;
                        ShieldActivated();
                        rectangleList.Remove(i);
                        Space.Children.Remove(i);
                        break;
                    }
                }
                if ((string)i.Tag == "BulletsIcon")
                {
                    Canvas.SetTop(i, Canvas.GetTop(i) + power.speed);
                    Rect powerUp = new Rect(Canvas.GetLeft(i), Canvas.GetTop(i), i.Height, i.Width);
                    Rect ship = new(Canvas.GetLeft(Ship), Canvas.GetTop(Ship), Ship.Height, Ship.Width);

                    if (Canvas.GetTop(i) > Canvas.GetTop(Ship) + Ship.Height)
                    {
                        rectangleList.Remove(i);
                        Space.Children.Remove(i);
                        break;
                    }
                    if (powerUp.IntersectsWith(ship))
                    {
                        Bullets.bulletLevel++;
                        rectangleList.Remove(i);
                        Space.Children.Remove(i);
                        if (Bullets.bulletLevel > 2)
                        {
                            Bullets.bulletLevel = 2;
                        }
                        break;
                    }
                }
            }
        }
        public void ShieldActivated()
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}shield3.png"));
            Rectangle shieldActivated = new Rectangle()
            {
                Width = Ship.Width * 2,
                Height = Ship.Height * 2,
                Tag = "shieldactivated",
                Fill = imageBrush
            };
            if ((shieldON) && (!activated))
            {
                activated = true;
                rectangleList.Add(shieldActivated);
                Canvas.SetLeft(shieldActivated, Canvas.GetLeft(Ship) - Ship.Width / 2);
                Canvas.SetTop(shieldActivated, Canvas.GetTop(Ship) - Ship.Height / 2);
                Space.Children.Add(shieldActivated);
            }
        }
    }
}

