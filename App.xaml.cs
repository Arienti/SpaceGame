using System.Windows;

namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>    
    public partial class App : Application
    {
        public static string? imagesLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\images\";
        public static string? audioLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Audio\";
    }
}
