using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for Restartxaml.xaml
    /// </summary>
    public partial class Restartxaml : Window
    {
        public Restartxaml()
        {
            InitializeComponent();
            MyText.Text = "Spaceship is destroyed";
            Score.Content = "Score:   " + MainWindow.score;
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Process.Start(Environment.ProcessPath);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
