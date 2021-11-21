using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuLibrary.Archives;
using MikuMikuLibrary.IO;
using MikuMikuLibrary.IO.Common;

namespace Mega_Mix_Mod_Manager.Lite_Merge
{
    internal class farc
    {
        private static FarcArchive Read(string infile)
        {
            using (FileStream fs = new FileStream(infile, FileMode.Open))
            {
                using (EndianBinaryReader ebr = new EndianBinaryReader(fs, MikuMikuLibrary.IO.Endianness.Big))
                {
                    FarcArchive farcArchive = new FarcArchive();
                    farcArchive.Read(ebr);
                    return farcArchive;
                }
            }
        }

        public static void Merge(string dumpPath, string ModPath, string outPath)
        {
            string[] modFiles = Directory.GetFiles(ModPath, "*.farc", SearchOption.AllDirectories);
            string[] vanillaFiles = Directory.GetFiles(dumpPath, "*.farc", SearchOption.AllDirectories);

            //file loop to check and merge each file in the mods folder
            foreach (string file in modFiles)
            {
                MemoryStream ms = new MemoryStream();
                FarcArchive Final = new FarcArchive();
                //check if the game dump contains the modded file
                if (vanillaFiles.Contains( vanillaFiles.LastOrDefault(x => x.Contains(file.Substring(file.LastIndexOf("rom")))) ))
                {
                    string VanillaFile = vanillaFiles.LastOrDefault(x => x.Contains(file.Substring(file.LastIndexOf("rom"))));
                    if (VanillaFile.Contains("2d"))
                        continue;

                    FileStream fs = File.OpenRead(VanillaFile);
                    Final = BinaryFile.Load<FarcArchive>(fs);

                    using (var Stream = File.OpenRead(file))
                    {
                        FarcArchive ModFarc = BinaryFile.Load<FarcArchive>(Stream);

                        //loop through the mod file's contents to find what to add to vanilla farc
                        foreach (string entry in ModFarc)
                        {
                            using (var source = ModFarc.Open(entry, EntryStreamMode.MemoryStream))
                            {
                                Final.Add(entry, source, false, ConflictPolicy.Replace);
                            }

                        }
                    }
                }
                else
                {
                    Final = Read(file);
                }

                if (!Directory.Exists(Path.GetDirectoryName($"{outPath}\\rom_switch\\{file.Substring(file.LastIndexOf("rom"))}")))
                    Directory.CreateDirectory(Path.GetDirectoryName($"{outPath}\\rom_switch\\{file.Substring(file.LastIndexOf("rom"))}"));
                Final.Save($"{outPath}\\rom_switch\\{file.Substring(file.LastIndexOf("rom"))}");
            }
        }
    }
}
