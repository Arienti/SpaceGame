using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpaceInvaders.business
{
    public class PowerUp
    {
        public int x = 0;
        public int y = -30;
        public int height = 30;
        public int width = 30;
        public static int Shieldcount = new Random().Next(3000, 4500);
        public static int PowerUpTime = new Random().Next(2800, 4200);
        public int speed = 3;
        public bool shieldAppear = false;
        public bool bulletAppear = false;
        public Rectangle shieldRec = new Rectangle();
        public Rectangle PowerUpBullets = new Rectangle();
        public ImageBrush shieldImage = new ImageBrush();
        public ImageBrush BulletsIcon = new ImageBrush();
        public void AddPowerUpIcons()
        {
            Random r = new Random();

            shieldImage.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}shield_silver.png"));
            shieldRec.Height = height;
            shieldRec.Width = width;
            shieldRec.Tag = "shield";
            shieldRec.StrokeThickness = 1;
            shieldRec.Stroke = new SolidColorBrush(Colors.Azure);
            shieldRec.Fill = shieldImage;
            x = r.Next(0, (int)Application.Current.MainWindow.Width - width);
            Canvas.SetLeft(shieldRec, x);
            Canvas.SetTop(shieldRec, y);

            BulletsIcon.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}bolt_bronze.png"));
            PowerUpBullets.Height = height;
            PowerUpBullets.Width = width;
            PowerUpBullets.Tag = "BulletsIcon";
            PowerUpBullets.Stroke = new SolidColorBrush(Colors.White);
            PowerUpBullets.StrokeThickness = 3;
            PowerUpBullets.Fill = BulletsIcon;

            Canvas.SetLeft(PowerUpBullets, x);
            Canvas.SetTop(PowerUpBullets, y);
        }
    }
}
