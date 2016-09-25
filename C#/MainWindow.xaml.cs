using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
//using System.Windows.Shapes;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;

namespace SDKSamples.ImageSample
{
    public sealed partial class MainWindow : Window
    {
        public PhotoCollection Photos;
        private string _path;

        public MainWindow(string path)
        {
            InitializeComponent();
            _path = path;
        }

        private void OnPhotoClick(object sender, RoutedEventArgs e)
        {
            PhotoView pvWindow = new PhotoView();
            pvWindow.SelectedPhoto = (Photo)PhotosListBox.SelectedItem;
            pvWindow.Show();
        }

        private void editPhoto(object sender, RoutedEventArgs e)
        {
            PhotoView pvWindow = new PhotoView();
            pvWindow.SelectedPhoto = (Photo)PhotosListBox.SelectedItem;
            pvWindow.Show();
        }

        private void OnImagesDirChangeClick(object sender, RoutedEventArgs e)
        {
            this.Photos.DirectoryPath = ImagesDir.Text;
        }

        private void OnUpClick(object sender, RoutedEventArgs e)
        {
            var photo = (Photo)PhotosListBox.SelectedItem;
            if (photo != null)
            {
                var current = JsonData.Root.GetPhoto().Up(Path.GetFileName(photo.Source));
                Photos.Update();
                this.PhotosListBox.SelectedIndex = current;
            }
        }

        private void OnDownClick(object sender, RoutedEventArgs e)
        {
            var photo = (Photo)PhotosListBox.SelectedItem;
            if (photo != null)
            {
                var current = JsonData.Root.GetPhoto().Down(Path.GetFileName(photo.Source));
                Photos.Update();
                this.PhotosListBox.SelectedIndex = current;
            }
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            var photo = (Photo)PhotosListBox.SelectedItem;
            if (photo != null)
            {
                var current = JsonData.Root.GetPhoto().Delete(Path.GetFileName(photo.Source));
                Photos.Update();
                this.PhotosListBox.SelectedIndex = current;
            }
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            this.Photos.DirectoryPath = ImagesDir.Text;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            this.Photos.DirectoryPath = ImagesDir.Text;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var photo = JsonData.Root.Groups.First(g => g.Id == "1");
            Photos = (PhotoCollection)(app.Current.Resources["Photos"] as ObjectDataProvider).Data;
            Photos.DirectoryPath = Environment.CurrentDirectory + "\\images";
            if (photo.Items.Count > 0)
                ImagesDir.Text = Environment.CurrentDirectory+Path.GetDirectoryName(photo.Items[0].urlSmall);

        }
    }
}