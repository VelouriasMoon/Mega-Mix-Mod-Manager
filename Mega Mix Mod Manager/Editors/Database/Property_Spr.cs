using System.Collections.Generic;
using System.ComponentModel;
using Mega_Mix_Mod_Manager.IO;
using MikuMikuLibrary.Databases;
using System.Reflection;

namespace Mega_Mix_Mod_Manager.Editors.Database
{
    internal class Property_Spr
    {
        [Category("Sprite Set")]
        [Browsable(true)]
        [ReadOnly(false)]
        public string Name { get; set; }

        [Category("Sprite Set")]
        [Browsable(true)]
        [ReadOnly(false)]
        public uint ID { get; set; }

        [Category("Sprite Set")]
        [Browsable(true)]
        [ReadOnly(false)]
        public string FileName { get; set; }

        [Category("Sprite Set")]
        [TypeConverter(typeof(DatabaseTypeConverter))]
        [Browsable(true)]
        [ReadOnly(false)]
        public List<DatabaseObject> Sprites { get; set; } = new List<DatabaseObject>();

        [Category("Sprite Set")]
        [TypeConverter(typeof(DatabaseTypeConverter))]
        [Browsable(true)]
        [ReadOnly(false)]
        public List<DatabaseObject> Textures { get; set; } = new List<DatabaseObject>();

        public void Read(SpriteSetInfo spriteSetInfo)
        {
            Name = spriteSetInfo.Name;
            ID = spriteSetInfo.Id;
            FileName = spriteSetInfo.FileName;

            foreach (SpriteInfo spr in spriteSetInfo.Sprites)
            {
                DatabaseObject databaseObject = new DatabaseObject() { Name = spr.Name, ID = spr.Id, Index = spr.Index};
                Sprites.Add(databaseObject);
            }

            foreach (SpriteTextureInfo tex in spriteSetInfo.Textures)
            {
                DatabaseObject databaseObject = new DatabaseObject() { Name = tex.Name, ID = tex.Id, Index = tex.Index };
                Textures.Add(databaseObject);
            }
        }
    }
}
