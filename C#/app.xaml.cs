using System;
using System.Windows;
using System.Windows.Data;
using System.Xml;
using System.Configuration;

namespace SDKSamples.ImageSample
{
    public partial class app : Application
    {
        void OnApplicationStartup(object sender, StartupEventArgs args)
        {
            //InitWindow window = new InitWindow();
            //window.Show();
            MainWindow mainWindow = new MainWindow(null);
            mainWindow.Show();
            mainWindow.Photos = (PhotoCollection)(this.Resources["Photos"] as ObjectDataProvider).Data;
            mainWindow.Photos.Path = Environment.CurrentDirectory + "\\images";
        }
    }
}