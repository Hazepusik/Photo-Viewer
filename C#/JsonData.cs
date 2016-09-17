using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.CSharp;
using System.Linq;

namespace SDKSamples.ImageSample
{
    public static class JsonData
    {
        private static RootObject _root;

        public static RootObject Root { get { return _root ?? Upload(); } }

        public static RootObject Upload(string workingDir = null)
        {
            workingDir = workingDir ?? Environment.CurrentDirectory;
            var jsonFile = Path.Combine(workingDir, Path.Combine("SampleData", "data.json"));
            if (!File.Exists(jsonFile))
                return null;
            _root = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(jsonFile));
            return _root;
        }

    }

    public class Component
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Pdf { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public string urlSmall { get; set; }
        public string urlLarge { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Component> Components { get; set; }
    }

    public class Group
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public List<Item> Items { get; set; }

        public int Up(string filename)
        {
            var toUp = Items.IndexOf(Items.First(i => Path.GetFileName(i.urlLarge) == filename));
            if (toUp > 0)
            {
                var tmp = Items[toUp].id;
                Items[toUp].id = Items[toUp - 1].id;
                Items[toUp - 1].id = tmp;
                Items = Items.OrderBy(i => int.Parse(i.id)).ToList();
            }
            return Math.Max(0, toUp - 1);
        }

        internal int Down(string filename)
        {
            var toDown = Items.IndexOf(Items.First(i => Path.GetFileName(i.urlLarge) == filename));
            if (toDown < Items.Count - 1)
            {
                var tmp = Items[toDown].id;
                Items[toDown].id = Items[toDown + 1].id;
                Items[toDown + 1].id = tmp;
                Items = Items.OrderBy(i => int.Parse(i.id)).ToList();
            }
            return Math.Min(Items.Count - 1, toDown + 1);
        }

        public static Group NewPhoto()
        {
            return new Group
            {
                Id = "1",
                Title = "Фотогалерея",
                ImagePath = "/Assets/photo.png",
            };
        }
    }

    public class RootObject
    {
        public List<Group> Groups { get; set; }

        public Group GetPhoto()
        {
            return Groups.FirstOrDefault(g => g.Id == "1") ?? Group.NewPhoto();
        }
    }

}
