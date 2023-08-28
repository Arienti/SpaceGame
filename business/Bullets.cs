using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpaceInvaders.business
{
    public class Bullets
    {
        public double X = 0;
        public double Y = 0;
        public int Height = 30;
        public int Width = 6;
        public int speed = 20;
        public static int bulletLevel = 0;
        public Rectangle bulletRectangle = new Rectangle();
        public void Draw()
        {
            ImageBrush bulletImage = new ImageBrush();
            bulletImage.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}laserBlue01.png"));
            bulletRectangle.Height = Height;
            bulletRectangle.Width = Width;
            bulletRectangle.Fill = bulletImage;
        }
    }
}
