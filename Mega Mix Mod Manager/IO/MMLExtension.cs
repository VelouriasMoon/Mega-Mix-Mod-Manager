using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuLibrary.Databases;

namespace Mega_Mix_Mod_Manager.IO
{
    internal static class ObjectInfoAddon
    {
        public static bool Compare(this ObjectInfo obj, ObjectInfo objectInfo)
        {
            return obj.Name == objectInfo.Name && obj.Id == objectInfo.Id;
        }
    }

    internal static class ObjectSetInfoAddon
    {
        public static bool Compare(this ObjectSetInfo obj, ObjectSetInfo objectSet)
        {
            bool objects = true;
            for (int i = 0; i < obj.Objects.Count; i++)
            {
                if (!obj.Objects[i].Equals(objectSet.Objects[i]))
                    objects = false;
            }
            return obj.Name == objectSet.Name &&
                obj.Id == objectSet.Id &&
                obj.FileName == objectSet.FileName &&
                obj.TextureFileName == objectSet.TextureFileName &&
                obj.ArchiveFileName == objectSet.ArchiveFileName && objects;
        }
    }

    internal static class SpriteDatabaseAddon
    {
        public static SpriteSetInfo GetSpriteSetInfo(this SpriteDatabase spr, string spriteSetName) =>
            spr.SpriteSets.FirstOrDefault(x => x.Name.Equals(spriteSetName, StringComparison.OrdinalIgnoreCase));

        public static SpriteSetInfo GetSpriteSetInfo(this SpriteDatabase spr, uint spriteSetID) =>
            spr.SpriteSets.FirstOrDefault(x => x.Id.Equals(spriteSetID));
    }
}
