using SpaceInvaders;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

public class Enemies
{
    public static int enemyLimit = new Random().Next(4, 6);
    public static int enemyCounts = 1;
    public int Enemysprite = 0;
    public static int speed = 1;
    public int Height = 60;
    public int Width = 50;
    public int X = new Random().Next(0, (int)Application.Current.MainWindow.Width - 50);
    public int Y = -300;
    public Rectangle enemyRec = new Rectangle();
    ImageBrush enemyImage = new ImageBrush();
    public void DrawEnemy()
    {
        Random r = new Random();

        Enemysprite = r.Next(1, 4);

        switch (Enemysprite)
        {
            case 1:
                enemyImage.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}enemyBlack1.png"));
                break;
            case 2:
                enemyImage.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}enemyBlue2.png"));
                break;
            case 3:
                enemyImage.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}enemyGreen3.png"));
                break;
            case 4:
                enemyImage.ImageSource = new BitmapImage(new Uri($"{App.imagesLocation}enemyRed4.png"));
                break;
        }
        enemyRec.Height = 50;
        enemyRec.Width = 50;
        enemyRec.Fill = enemyImage;
    }
}

