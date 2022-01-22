using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.Serialization;
using MikuMikuLibrary.Databases;
using Mega_Mix_Mod_Manager.Objects;
using Mega_Mix_Mod_Manager.Lite_Merge;

namespace Mega_Mix_Mod_Manager.Editors.ModCreator
{
    internal class Creator
    {
        public static void MakeMod(ModInfoCreator info)
        {
            using (SaveFileDialog sdf = new SaveFileDialog())
            {
                sdf.Filter = "Supported Files(*.zip, *.MikuMod)|*.zip;*.MikuMod|Zip Files(*.zip)|*.zip|Miku Mod(*.MikuMod)|*.MikuMod";
                sdf.RestoreDirectory = true;
                sdf.FileName = info.Name + ".zip";

                if (sdf.ShowDialog() == DialogResult.OK)
                {
                    string[] files = Directory.GetFiles(info.Path, "*", SearchOption.AllDirectories);
                    string[] LoggableFiles = new string[] { "obj_db.bin", "spr_db.bin", "tex_db.bin" };
                    List<string> MergeList = new List<string>();
                    Directory.CreateDirectory($"{info.Path}\\.temp");

                    foreach (string file in files)
                    {
                        if (Path.GetExtension(sdf.FileName) == ".MikuMod" && LoggableFiles.Contains(Path.GetFileName(file)))
                        {
                            Directory.CreateDirectory($"{info.Path}\\.temp\\Log");
                            LogFile(file, info);
                        }
                        else
                        {
                            if (!Directory.Exists($"{info.Path}\\.temp\\{Path.GetDirectoryName(file.Replace(info.Path, ""))}"))
                            {
                                Directory.CreateDirectory($"{info.Path}\\.temp\\{Path.GetDirectoryName(file.Replace(info.Path, ""))}");
                            }
                            File.Copy(file, $"{info.Path}\\.temp\\{file.Replace(info.Path, "")}", true);
                        }
                    }

                    if (info.Img != null)
                        info.Img.Save($"{info.Path}\\.temp\\thumbnail.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    info.ToModInfo().Write(info.Path);

                    if (File.Exists(sdf.FileName))
                        File.Delete(sdf.FileName);
                    ZipFile.CreateFromDirectory($"{info.Path}\\.temp", sdf.FileName);
                    Directory.Delete($"{info.Path}\\.temp", true);
                }
            }
            
        }

        private static void LogFile(string infile, ModInfoCreator info)
        {
            CommonDatabase database = new CommonDatabase();
            if (Path.GetFileName(infile) == "obj_db.bin")
            {
                ObjectDatabase objdb = new ObjectDatabase();
                ObjectDatabase basedb = obj_db.Read($"{info.DumpPath}\\rom_switch\\rom\\objset\\obj_db.bin");
                ObjectDatabase moddb = obj_db.Read(infile);
                objdb = obj_db.GetNewEntires(basedb, moddb, objdb);
                database.Read(objdb);
            }
            else if (Path.GetFileName(infile) == "spr_db.bin")
            {
                SpriteDatabase spritedb = new SpriteDatabase();
                spritedb = spr_db.GetNewEntires(spr_db.Read($"{info.DumpPath}\\{info.Region}\\rom\\2d\\spr_db.bin"), spr_db.Read(infile), spritedb);
                database.Read(spritedb);
            }
            else if (Path.GetFileName(infile) == "tex_db.bin")
            {
                TextureDatabase texturedb = new TextureDatabase();
                texturedb = tex_db.GetNewEntires(tex_db.Read($"{info.DumpPath}\\rom_switch\\rom\\objset\\tex_db.bin"), tex_db.Read(infile), texturedb);
                database.Read(texturedb);
            }
            if (database.Entries.Count > 0)
            {
                var serializer = new SerializerBuilder().Build();
                string yaml = serializer.Serialize(database);
                File.WriteAllText($"{info.Path}\\.temp\\Log\\{Enum.GetName(typeof(CommonType), database.DatabaseType)}_db.yaml", yaml);
            }
        }
    }
}
