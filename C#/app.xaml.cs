using System;
using System.Windows;
using System.Windows.Data;
using System.Xml;
using System.Configuration;

namespace SDKSamples.ImageSample
{
    public partial class app : Application
    {
       
        public static object photoRes = null;
        void OnApplicationStartup(object sender, StartupEventArgs args)
        {
            var window = new InitWindow();
            window.Show();
            //photoRes = this.Resources["Photos"];
            //MainWindow mainWindow = new MainWindow(null);
            //mainWindow.Show();
            //mainWindow.Photos = (PhotoCollection)(this.Resources["Photos"] as ObjectDataProvider).Data;
            //mainWindow.Photos.Path = Environment.CurrentDirectory + "\\images";
        }
    }
}