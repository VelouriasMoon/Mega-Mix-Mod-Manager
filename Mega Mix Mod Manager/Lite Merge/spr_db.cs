using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuLibrary.Databases;
using MikuMikuLibrary.IO.Common;

namespace Mega_Mix_Mod_Manager.Lite_Merge
{
    internal class spr_db
    {
        public static List<string> log;
        public static SpriteDatabase Read(string infile)
        {
            using (FileStream fs = new FileStream(infile, FileMode.Open))
            {
                using (EndianBinaryReader ebr = new EndianBinaryReader(fs, MikuMikuLibrary.IO.Endianness.Little))
                {
                    SpriteDatabase spriteDatabase = new SpriteDatabase();
                    spriteDatabase.Read(ebr);
                    return spriteDatabase;
                }
            }
        }

        private static SpriteDatabase GetNewEntires(SpriteDatabase BaseSpr, SpriteDatabase ModSpr, SpriteDatabase Final)
        {
            foreach (SpriteSetInfo spriteSetInfo in ModSpr.SpriteSets)
            {
                if (BaseSpr.GetSpriteSetInfo(spriteSetInfo.Id) == null)
                {
                    if (Final.GetSpriteSetInfo(spriteSetInfo.Id) != null)
                    {
                        log.Append($"Dupicate Texture ID Found: {spriteSetInfo.Id}, skipping...");
                    }
                    else
                    {
                        Final.SpriteSets.Add(spriteSetInfo);
                    }
                }
            }

            return Final;
        }

        public static void Merge(string BaseObj, string[] mods, string outfile)
        {
            SpriteDatabase original = Read(BaseObj);
            SpriteDatabase spriteDatabase = new SpriteDatabase();
            log = new List<string>();

            foreach (string mod in mods)
            {
                SpriteDatabase modified = Read(mod);
                spriteDatabase = GetNewEntires(original, modified, spriteDatabase);
            }

            foreach (SpriteSetInfo spriteSet in spriteDatabase.SpriteSets)
            {
                original.SpriteSets.Add(spriteSet);
            }

            if (!Directory.Exists(Path.GetDirectoryName(outfile)))
                Directory.CreateDirectory(Path.GetDirectoryName(outfile));
            original.Save(outfile);
        }
    }
}
