using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega_Mix_Mod_Manager.Lite_Merge;
using MikuMikuLibrary.Databases;

namespace Mega_Mix_Mod_Manager.Objects
{
    public enum CommonType
    {
        obj,
        tex,
        spr
    }

    public class CommonEntry
    {
        public string Name { get; set; }
        public uint Id { get; set; }
        public ushort Index { get; set; }
    }

    public class CommonSet
    {
        public string Name { get; set; }
        public uint Id { get; set; }
        public string FileName { get; set; }
        public string TextureFileName { get; set; }
        public string ArchiveFileName { get; set; }

        public List<CommonEntry> Entries { get; set; }
        public List<CommonEntry> Entries_2 { get; set; }

        public CommonEntry GetCommonEntry(string entryName) =>
            Entries.FirstOrDefault(e => e.Name.Equals(entryName, StringComparison.OrdinalIgnoreCase));
        public CommonEntry GetCommonEntry(uint entryId) =>
            Entries.FirstOrDefault(e => e.Id.Equals(entryId));

        public CommonEntry GetCommonEntry_2(string entryName) =>
            Entries_2.FirstOrDefault(e => e.Name.Equals(entryName, StringComparison.OrdinalIgnoreCase));
        public CommonEntry GetCommonEntry_2(uint entryId) =>
            Entries_2.FirstOrDefault(e => e.Id.Equals(entryId));
    }

    public class CommonDatabase
    {
        public CommonType DatabaseType { get; set; }
        public List<CommonSet> Entries { get; set; }

        public CommonDatabase()
        {
            Entries = new List<CommonSet>();
        }

        public CommonSet GetCommonSet(string entryName) =>
            Entries.FirstOrDefault(x => x.Name.Equals(entryName, StringComparison.OrdinalIgnoreCase));
        public CommonSet GetCommonSet(uint entryId) =>
            Entries.FirstOrDefault(x => x.Id.Equals(entryId));

        public void Read(ObjectDatabase objDatabase)
        {
            Entries.Clear();
            DatabaseType = CommonType.obj;
            foreach (var set in objDatabase.ObjectSets)
            {
                CommonSet commonSet = new CommonSet();
                commonSet.Name = set.Name;
                commonSet.Id = set.Id;
                commonSet.FileName = set.FileName;
                commonSet.TextureFileName = set.TextureFileName;
                commonSet.ArchiveFileName = set.ArchiveFileName;
                commonSet.Entries = new List<CommonEntry>();
                foreach (var entry in set.Objects)
                {
                    CommonEntry commonEntry = new CommonEntry();
                    commonEntry.Name = entry.Name;
                    commonEntry.Id = entry.Id;
                    commonSet.Entries.Add(commonEntry);
                }
                Entries.Add(commonSet);
            }
        }
        public void Read(TextureDatabase textureDatabase)
        {
            Entries.Clear();
            DatabaseType = CommonType.tex;
            foreach (var set in textureDatabase.Textures)
            {
                CommonSet commonSet = new CommonSet();
                commonSet.Name = set.Name;
                commonSet.Id = set.Id;
                Entries.Add(commonSet);
            }
        }
        public void Read(SpriteDatabase spriteDatabase)
        {
            Entries.Clear();
            DatabaseType = CommonType.spr;
            foreach (var set in spriteDatabase.SpriteSets)
            {
                CommonSet commonSet = new CommonSet();
                commonSet.Name = set.Name;
                commonSet.Id = set.Id;
                commonSet.FileName = set.FileName;
                commonSet.Entries = new List<CommonEntry>();
                commonSet.Entries_2 = new List<CommonEntry>();
                foreach (var entry in set.Sprites)
                {
                    CommonEntry commonEntry = new CommonEntry();
                    commonEntry.Name = entry.Name;
                    commonEntry.Id = entry.Id;
                    commonEntry.Index = entry.Index;
                    commonSet.Entries.Add(commonEntry);
                }
                foreach (var entry in set.Textures)
                {
                    CommonEntry commonEntry = new CommonEntry();
                    commonEntry.Name = entry.Name;
                    commonEntry.Id = entry.Id;
                    commonEntry.Index = entry.Index;
                    commonSet.Entries_2.Add(commonEntry);
                }
                Entries.Add(commonSet);
            }
        }

        public dynamic Write<T>()
        {
            if (typeof(T) == typeof(ObjectDatabase))
            {
                ObjectDatabase db = new ObjectDatabase();
                foreach (var set in Entries)
                {
                    ObjectSetInfo setInfo = new ObjectSetInfo();
                    setInfo.Name = set.Name;
                    setInfo.Id = set.Id;
                    setInfo.FileName = set.FileName;
                    setInfo.TextureFileName = set.TextureFileName;
                    setInfo.ArchiveFileName = set.ArchiveFileName;
                    foreach (var entry in set.Entries)
                    {
                        ObjectInfo objectInfo = new ObjectInfo();
                        objectInfo.Name = entry.Name;
                        objectInfo.Id = entry.Id;
                        setInfo.Objects.Add(objectInfo);
                    }
                    db.ObjectSets.Add(setInfo);
                }
                return db;
            }
            if (typeof(T) == typeof(TextureDatabase))
            {
                TextureDatabase db = new TextureDatabase();
                foreach (var set in Entries)
                {
                    TextureInfo setInfo = new TextureInfo();
                    setInfo.Name = set.Name;
                    setInfo.Id = set.Id;
                    db.Textures.Add(setInfo);
                }
                return db;
            }
            if (typeof (T) == typeof(SpriteDatabase))
            {
                SpriteDatabase db = new SpriteDatabase();
                foreach (var set in Entries)
                {
                    SpriteSetInfo setInfo = new SpriteSetInfo();
                    setInfo.Name = set.Name;
                    setInfo.Id = set.Id;
                    setInfo.FileName = set.FileName;
                    foreach(var entry in set.Entries)
                    {
                        SpriteInfo spriteInfo = new SpriteInfo();
                        spriteInfo.Name = entry.Name;
                        spriteInfo.Id = entry.Id;
                        spriteInfo.Index = entry.Index;
                        setInfo.Sprites.Add(spriteInfo);
                    }
                    foreach (var entry in set.Entries_2)
                    {
                        SpriteTextureInfo textureInfo = new SpriteTextureInfo();
                        textureInfo.Name = entry.Name;
                        textureInfo.Id = entry.Id;
                        textureInfo.Index = entry.Index;
                        setInfo.Textures.Add(textureInfo);
                    }
                    db.SpriteSets.Add(setInfo);
                }
                return db;
            }
            else
                return null;
        }


        public void Save(string outpath)
        {
            switch (DatabaseType)
            {
                case CommonType.obj:
                    ObjectDatabase obj = Write<ObjectDatabase>();
                    obj.Save(outpath);
                    break;
                case CommonType.tex:
                    TextureDatabase tex = Write<TextureDatabase>();
                    tex.Save(outpath);
                    break;
                case CommonType.spr:
                    SpriteDatabase spr = Write<SpriteDatabase>();
                    spr.Save(outpath);
                    break;
            }
        }
    }
}
