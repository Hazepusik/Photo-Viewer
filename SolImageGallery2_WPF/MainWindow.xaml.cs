using Microsoft.Win32;
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
using System.Windows.Navigation;

namespace SolImageGallery2_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string _wDir = null;
        private static string _affix = null;

        public MainWindow(string path, string affix)
        {
            InitializeComponent();
            _wDir = path;
            _affix = affix;
            this.Loaded += (s, o) =>
            {
                JsonData.Upload(_wDir, _affix);
                this.UpdatePhotos();
            };
        }

        private void UpdatePhotos()
        {
            var photo = JsonData.GetCurrentGroup();
            List <ImageEntity> ListImageEntityObj = JsonData.GetAllImageData();

            if (ListImageEntityObj != null)
            {
                if (ListImageEntityObj.Count > 0)
                {
                    lbImageGallery.DataContext = ListImageEntityObj;
                }
            }
        }

        private void lbImageGallery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OnUpClick(object sender, RoutedEventArgs e)
        {
            var photo = (ImageEntity)lbImageGallery.SelectedItem;
            if (photo != null)
            {
                var current = JsonData.GetCurrentGroup().Up(Path.GetFileName(photo.ImagePath));
                this.UpdatePhotos();
                this.lbImageGallery.SelectedIndex = current;
            }
        }

        private void OnDownClick(object sender, RoutedEventArgs e)
        {
            var photo = (ImageEntity)lbImageGallery.SelectedItem;
            if (photo != null)
            {
                var current = JsonData.GetCurrentGroup().Down(Path.GetFileName(photo.ImagePath));
                this.UpdatePhotos();
                this.lbImageGallery.SelectedIndex = current;
            }
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            var photo = (ImageEntity)lbImageGallery.SelectedItem;
            if (photo != null)
            {
                var current = JsonData.GetCurrentGroup().Delete(Path.GetFileName(photo.ImagePath));
                this.UpdatePhotos();
                this.lbImageGallery.SelectedIndex = current;
            }
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg";
            if (op.ShowDialog() == true)
            {
                //var f = Path.Combine(this.Photos.DirectoryPath, Path.GetFileName(op.FileName));
                var f = Path.Combine(JsonData.WorkingDir, Path.GetFileName(op.FileName));
                if (!File.Exists(f))
                {
                    File.Copy(op.FileName, f);
                    var current = JsonData.GetCurrentGroup().Add(f);
                    this.UpdatePhotos();
                    this.lbImageGallery.SelectedIndex = current;
                }
                else
                    MessageBox.Show(f + " already exists");
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            JsonData.Save();
        }
    }
}
