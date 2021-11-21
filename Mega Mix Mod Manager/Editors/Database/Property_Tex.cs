using System.ComponentModel;
using MikuMikuLibrary.Databases;
using Mega_Mix_Mod_Manager.Objects;

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

        public void Read(CommonSet textureInfo)
        {
            Name = textureInfo.Name;
            ID = textureInfo.Id;
        }

        public CommonSet Write()
        {
            CommonSet textureInfo = new CommonSet();
            textureInfo.Name = Name;
            textureInfo.Id = ID;
            return textureInfo;
        }
    }
}
