using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuLibrary.Databases;
using MikuMikuLibrary.IO.Common;
using Mega_Mix_Mod_Manager.IO;
using Mega_Mix_Mod_Manager.Objects;
using YamlDotNet.Serialization;

namespace Mega_Mix_Mod_Manager.Lite_Merge
{
    public class obj_db
    {
        public static ObjectDatabase Read(string infile)
        {
            using (FileStream fs = new FileStream(infile, FileMode.Open))
            {
                using (EndianBinaryReader ebr = new EndianBinaryReader(fs, MikuMikuLibrary.IO.Endianness.Little))
                {
                    ObjectDatabase objectDatabase = new ObjectDatabase();
                    objectDatabase.Read(ebr);
                    return objectDatabase;
                }
            }
        }

        public static ObjectDatabase GetNewEntires(ObjectDatabase BaseObj, ObjectDatabase ModObj, ObjectDatabase Final)
        {
            foreach (ObjectSetInfo objset in ModObj.ObjectSets)
            {
                List<ObjectSetInfo> results = BaseObj.ObjectSets.Where(x => x.Id == objset.Id).ToList();
                if (results.Count > 0)
                {
                    foreach (ObjectSetInfo objectSet in results)
                    {
                        if (!objectSet.Compare(objset))
                        {
                            Final.ObjectSets.Add(objset);
                        }
                    }
                }
                else
                {
                    Final.ObjectSets.Add(objset);
                }
            }
            return Final;
        }

        private static uint GetNextID(ObjectDatabase objectDatabase)
        {
            uint id = 0;
            foreach (ObjectSetInfo objectSet in objectDatabase.ObjectSets)
            {
                if (objectSet.Id >= id)
                    id = objectSet.Id;
            }
            return id + 1;
        }

        public static void Merge(string BaseObj, string[] mods, string outfile)
        {
            ObjectDatabase original = Read(BaseObj);
            ObjectDatabase objectDatabase = new ObjectDatabase();

            foreach (string mod in mods)
            {
                ObjectDatabase modified;
                if (Path.GetExtension(mod) == ".yaml")
                {
                    var deserializer = new DeserializerBuilder().Build();
                    CommonDatabase commonDatabase = deserializer.Deserialize<CommonDatabase>(File.ReadAllText(mod));
                    modified = commonDatabase.Write<ObjectDatabase>();
                }
                else
                    modified = Read(mod);
                    
                objectDatabase = GetNewEntires(original, modified, objectDatabase);
            }
            uint id = GetNextID(original);

            foreach (ObjectSetInfo obj in objectDatabase.ObjectSets)
            {
                obj.Id = id++;
                original.ObjectSets.Add(obj);
            }
            if (!Directory.Exists(Path.GetDirectoryName(outfile)))
                Directory.CreateDirectory(Path.GetDirectoryName(outfile));
            original.Save(outfile);
        }
    }
}
