using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.CSharp;
using System.Linq;
using System.Windows;

namespace SolImageGallery2_WPF
{
    public enum ImgType
    {
        Unknown = 0,
        Photo = 1,
        Pres = 2,
        Pdf = 4
    }

    public static class JsonData
    {
        private static string _affix;
        private static RootObject _root;
        private static string _rootDir;
        private static ImgType imgType;

        public static RootObject Root { get { return _root ?? Upload(); } }
        
        public static string WorkingDir { get { return Path.Combine(_rootDir ?? Environment.CurrentDirectory, _affix); } }

        public static List<ImageEntity> GetAllImageData()
        {
            try
            {
                List<Item> items = GetCurrentItems();
                var toSave = new List<string>();
                var files = Directory.GetFiles(WorkingDir, "*.*").Where(s => s.EndsWith(".jpeg") || s.EndsWith(".jpg")).Select(f => new FileInfo(f));
                foreach (var item in items.OrderBy(i => i.id).ToArray())
                {
                    var file = files.FirstOrDefault(f => Path.GetFileName(item.urlLarge) == f.Name);
                    if (file != null)
                    {
                        //Add(new Photo(file.FullName));
                        toSave.Add(file.FullName);
                    }
                }
                foreach (var name in files.Where(f => !toSave.Contains(f.FullName)).Select(f => f.FullName))
                {
                    //File.Delete(name);
                }

                items = items.Where(item => toSave.Select(f => Path.GetFileName(f)).Contains(Path.GetFileName(item.urlLarge))).ToList();
                SetCurrentItems(items);
                var res = toSave.Select(f => new ImageEntity { ImageName = Path.GetFileNameWithoutExtension(f), ImagePath = f }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void SetCurrentItems(List<Item> items)
        {
            switch (imgType)
            {
                case ImgType.Photo:
                    Root.GetPhoto().Items = items;
                    break;
                case ImgType.Pres:
                    Root.GetPres().Items = items;
                    break;
                case ImgType.Pdf:
                    Root.GetPdf().Items = items;
                    break;
                default:
                    MessageBox.Show("Unknown image type");
                    break;
            }
        }

        private static List<Item> GetCurrentItems()
        {
            return GetCurrentGroup().Items;
        }

        public static Group GetCurrentGroup()
        {
            switch (imgType)
            {
                case ImgType.Photo:
                    return Root.GetPhoto();
                case ImgType.Pres:
                    return Root.GetPres();
                case ImgType.Pdf:
                    return Root.GetPdf();
                default:
                    return new Group();
            }
        }

        public static RootObject Upload(string rootDir = null, string affix = null)
        {
            _rootDir = rootDir ?? Environment.CurrentDirectory;
            _affix = affix ?? "unknown";
            switch (affix)
            {
                case "images":
                    {
                        imgType = ImgType.Photo;
                        break;
                    }
                case "pres2":
                    {
                        imgType = ImgType.Pres;
                        break;
                    }
                case "pdf":
                    {
                        imgType = ImgType.Pdf;
                        break;
                    }

                default:
                    {
                        imgType = ImgType.Unknown;
                        break;
                    }
            }
            var jsonFile = Path.Combine(_rootDir, Path.Combine("SampleData", "data.json"));
            if (!File.Exists(jsonFile))
            {
                MessageBox.Show("Can not find file "+jsonFile);
                return null;
            }
            _root = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(jsonFile));
            return _root;
        }

        internal static void Save()
        {
            var jsonFile = Path.Combine(_rootDir, Path.Combine("SampleData", "data.json"));
            var json = JsonConvert.SerializeObject(_root, Formatting.Indented);
            File.WriteAllText(jsonFile, json);
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

        internal int Delete(string filename)
        {
            var toRm = Items.IndexOf(Items.First(i => Path.GetFileName(i.urlLarge) == filename));
            for (var i = toRm + 1; i < Items.Count - 2; ++i)
            {
                Items[i].id = Items[i + 1].id;
            }
            Items.RemoveAt(toRm);
            return Math.Min(Items.Count - 1, toRm);
        }


        internal int Add(string filename)
        {
            var maxId = Items.Max(i => int.Parse(i.id));
            var url = "\\" + filename.Split('\\').Reverse().Skip(1).First() + "\\" + filename.Split('\\').Last();
            var newItem = new Item
            {
                id = (maxId + 1).ToString(),
                urlSmall = url,
                urlLarge = url
            };
            Items.Add(newItem);
            return Items.Count - 1;
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

        public static Group NewPres()
        {
            return new Group
            {
                Id = "2",
                Title = "Презентации",
                ImagePath = "/Assets/pres.png",
            };
        }

        public static Group NewPdf()
        {
            return new Group
            {
                Id = "3",
                Title = "Продукция",
                ImagePath = "/Assets/prod.png",
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

        public Group GetPres()
        {
            return Groups.FirstOrDefault(g => g.Id == "2") ?? Group.NewPres();
        }

        public Group GetPdf()
        {
            return Groups.FirstOrDefault(g => g.Id == "3") ?? Group.NewPdf();
        }
    }

}
