using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Mega_Mix_Mod_Manager.Objects
{
    public class ModInfo
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        public void Write(string outpath)
        {
            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(this);
            File.WriteAllText($"{outpath}\\.temp\\modinfo.yaml", yaml);
        }
    }

    public class ModInfoCreator
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public Image Img { get; set; }
        public string DumpPath { get; set; }
        public string Region { get; set; }

        public ModInfo ToModInfo()
        {
            ModInfo info = new ModInfo();
            info.Name = Name;
            info.Author = Author;
            info.Description = Description;
            return info;
        }
    }
}
