using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SolImageGallery2_WPF
{
    /// <summary>
    /// Логика взаимодействия для InitWindow.xaml
    /// </summary>
    public partial class InitWindow : Window
    {
        public InitWindow()
        {
            InitializeComponent();
            this.ImagesDir.Text = Environment.CurrentDirectory;
        }

        private void OnImagesDirChangeClick(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = fbd.ShowDialog();
            if (Directory.Exists(fbd.SelectedPath))
                ImagesDir.Text = fbd.SelectedPath;
            else
                MessageBox.Show("Unknown dir: " + fbd.SelectedPath);
        }
        
        private void OnPresentationClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(ImagesDir.Text))
                MessageBox.Show("Unknown dir: " + ImagesDir.Text);
            else
            {
                MainWindow mainWindow = new MainWindow(ImagesDir.Text, "pres2");
                mainWindow.Show();
            }
        }
        private void OnPhotoClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(ImagesDir.Text))
                MessageBox.Show("Unknown dir: " + ImagesDir.Text);
            else
            {
                MainWindow mainWindow = new MainWindow(ImagesDir.Text, "images");
                mainWindow.Show();
            }
        }
    }
}
