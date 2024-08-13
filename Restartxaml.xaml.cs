using System;
using System.Diagnostics;
using System.Windows;

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
            if (Environment.ProcessPath != null)
            {
                Process.Start(Environment.ProcessPath);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
