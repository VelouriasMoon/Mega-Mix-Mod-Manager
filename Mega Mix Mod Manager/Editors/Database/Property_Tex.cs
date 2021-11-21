using System.ComponentModel;
using MikuMikuLibrary.Databases;

namespace Mega_Mix_Mod_Manager.Editors.Database
{
    internal class Property_Tex
    {
        [Category("Texture")]
        [Browsable(true)]
        [ReadOnly(false)]
        public string Name { get; set; }

        [Category("Texture")]
        [Browsable(true)]
        [ReadOnly(false)]
        public uint ID { get; set; }

        public void Read(TextureInfo textureInfo)
        {
            Name = textureInfo.Name;
            ID = textureInfo.Id;
        }

        public TextureInfo Write()
        {
            TextureInfo textureInfo = new TextureInfo();
            textureInfo.Name = Name;
            textureInfo.Id = ID;
            return textureInfo;
        }
    }
}
