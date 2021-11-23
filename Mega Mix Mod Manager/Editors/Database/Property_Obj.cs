using System.Collections.Generic;
using System.ComponentModel;
using Mega_Mix_Mod_Manager.IO;
using MikuMikuLibrary.Databases;
using System.Reflection;
using Mega_Mix_Mod_Manager.Objects;

namespace Mega_Mix_Mod_Manager.Editors.Database
{
    internal class Property_Obj
    {
        [Category("Set Info")]
        [Browsable(true)]
        [ReadOnly(false)]
        public string Name { get; set; }

        [Category("Set Info")]
        [Browsable(true)]
        [ReadOnly(false)]
        public uint ID { get; set; }

        [Category("Set Info")]
        [Browsable(true)]
        [ReadOnly(false)]
        public string FileName { get; set; }

        [Category("Set Info")]
        [DisplayName("Texture FileName")]
        [Browsable(true)]
        [ReadOnly(false)]
        public string TextureFileName { get; set; }

        [Category("Set Info")]
        [DisplayName("Archive FileName")]
        [Browsable(true)]
        [ReadOnly(false)]
        public string ArchiveFileName { get; set; }

        [Category("Set Info")]
        [TypeConverter(typeof(DatabaseTypeConverter))]
        [Browsable(true)]
        [ReadOnly(false)]
        public List<DatabaseObject> Objects { get; set; } = new List<DatabaseObject>();

        public void Read(CommonSet objectSetInfo)
        {
            Name = objectSetInfo.Name;
            ID = objectSetInfo.Id;
            FileName = objectSetInfo.FileName;
            TextureFileName = objectSetInfo.TextureFileName;
            ArchiveFileName = objectSetInfo.ArchiveFileName;

            Objects = new List<DatabaseObject>();
            foreach (CommonEntry obj in objectSetInfo.Entries)
            {
                DatabaseObject databaseObject = new DatabaseObject() { Name = obj.Name, ID = obj.Id };
                PropertyDescriptor propDescr = TypeDescriptor.GetProperties(databaseObject.GetType())["Index"];
                BrowsableAttribute Attribute = (BrowsableAttribute)propDescr.Attributes[typeof(BrowsableAttribute)];
                FieldInfo fieldToChange = Attribute.GetType().GetField("browsable", BindingFlags.NonPublic | BindingFlags.Instance);
                fieldToChange.SetValue(Attribute, false);
                Objects.Add(databaseObject);
            }
        }

        public CommonSet Write()
        {
            CommonSet objectSetInfo = new CommonSet();
            objectSetInfo.Name = Name;
            objectSetInfo.Id = ID;
            objectSetInfo.FileName = FileName;
            objectSetInfo.TextureFileName = TextureFileName;
            objectSetInfo.ArchiveFileName = ArchiveFileName;

            foreach (DatabaseObject obj in Objects)
            {
                CommonEntry objectInfo = new CommonEntry() { Name = obj.Name, Id = obj.ID };
                objectSetInfo.Entries.Add(objectInfo);

            }
            return objectSetInfo;
        }

        public ObjectSetInfo Export()
        {
            ObjectSetInfo objectSetInfo = new ObjectSetInfo();
            objectSetInfo.Name = Name;
            objectSetInfo.Id = ID;
            objectSetInfo.FileName = FileName;
            objectSetInfo.TextureFileName = TextureFileName;
            objectSetInfo.ArchiveFileName = ArchiveFileName;
            foreach (DatabaseObject obj in Objects)
            {
                ObjectInfo objectInfo = new ObjectInfo() { Name = obj.Name, Id = obj.ID };
                objectSetInfo.Objects.Add(objectInfo);

            }
            return objectSetInfo;
        }
    }
}
