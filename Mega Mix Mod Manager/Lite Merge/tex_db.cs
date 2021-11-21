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
    internal class tex_db
    {
        public static List<string> log;
        public static TextureDatabase Read(string infile)
        {
            using (FileStream fs = new FileStream(infile, FileMode.Open))
            {
                using (EndianBinaryReader ebr = new EndianBinaryReader(fs, MikuMikuLibrary.IO.Endianness.Little))
                {
                    TextureDatabase textureDatabase = new TextureDatabase();
                    textureDatabase.Read(ebr);
                    return textureDatabase;
                }
            }
        }

        private static TextureDatabase GetNewEntires(TextureDatabase BaseTex, TextureDatabase ModTex, TextureDatabase Final)
        {
            foreach (TextureInfo texture in ModTex.Textures)
            {
                TextureInfo result = BaseTex.GetTextureInfo(texture.Id);
                if (result == null)
                {
                    if (Final.GetTextureInfo(texture.Id) != null)
                    {
                        log.Append($"Dupicate Texture ID Found: {texture.Id}, skipping...");
                    }
                    else
                    {
                        Final.Textures.Add(texture);
                    }
                }
                
            }
            return Final;
        }

        public static void Merge(string BaseTex, string[] mods, string outfile)
        {
            TextureDatabase original = Read(BaseTex);
            TextureDatabase textureDatabase = new TextureDatabase();
            log = new List<string>();

            foreach (string mod in mods)
            {
                TextureDatabase modified = Read(mod);
                textureDatabase = GetNewEntires(original, modified, textureDatabase);
            }

            foreach(TextureInfo texture in textureDatabase.Textures)
            {
                original.Textures.Add(texture);
            }

            if (!Directory.Exists(Path.GetDirectoryName(outfile)))
                Directory.CreateDirectory(Path.GetDirectoryName(outfile));
            original.Save(outfile);
        }
    }
}
