using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuLibrary.Archives;
using MikuMikuLibrary.IO;
using MikuMikuLibrary.IO.Common;
using Mega_Mix_Mod_Manager.Objects;
using YamlDotNet.Serialization;

namespace Mega_Mix_Mod_Manager.Lite_Merge
{
    internal class farc
    {
        public static void Merge(string dumpPath, string[] modFiles, string outPath, ModList.Region region = ModList.Region.rom_switch_en)
        {
            //string[] modFiles = Directory.GetFiles(ModPath, "*.farc", SearchOption.AllDirectories);
            string[] vanillaFiles = Directory.GetFiles(dumpPath, "*.farc", SearchOption.AllDirectories);

            //file loop to check and merge each file in the mods folder
            foreach (string file in modFiles)
            {
                //check if the game dump contains the modded file
                string VanillaFile = vanillaFiles.LastOrDefault(x => x.Contains(file.Substring(file.LastIndexOf("rom"))));
                string export = $"{outPath}\\rom_switch\\{file.Substring(file.LastIndexOf("rom"))}";
                if (export.Contains("2d") && !export.Contains(Enum.GetName(typeof(ModList.Region), region)))
                    export = export.Replace("rom_switch", Enum.GetName(typeof(ModList.Region), region));
                if (File.Exists(export))
                    VanillaFile = export;
                Dictionary<string, byte[]> ModFarcFiles = new Dictionary<string, byte[]>();
                Dictionary<string, byte[]> VanillaFarcFiles = new Dictionary<string, byte[]>();
                bool compressed = true;
                int alignment = 16;

                //Extract Mod Files from farc into memory
                using (FileStream fs = File.OpenRead(file))
                {
                    var Farc = BinaryFile.Load<FarcArchive>(fs);
                    compressed = Farc.IsCompressed;
                    alignment = Farc.Alignment;
                    foreach (string filename in Farc)
                    {
                        using (MemoryStream destination = new MemoryStream())
                        {
                            using (var source = Farc.Open(filename, EntryStreamMode.OriginalStream))
                            {
                                source.CopyTo(destination);
                                ModFarcFiles.Add(filename, destination.ToArray());
                                source.Close();
                            }
                            destination.Close();
                        }
                    }
                    fs.Close();
                    Farc.Dispose();
                }

                //If game dump contains an farc with the same name read that for merging
                if (vanillaFiles.Contains(VanillaFile))
                {
                    if (VanillaFile.Contains("2d") && !export.Contains(Enum.GetName(typeof(ModList.Region), region)))
                        export = export.Replace("rom_switch", Enum.GetName(typeof(ModList.Region), region));

                    //Extract Vanilla Files from farc into memory
                    using (FileStream fs = File.OpenRead(VanillaFile))
                    {
                        var Farc = BinaryFile.Load<FarcArchive>(fs);
                        foreach (string filename in Farc)
                        {
                            using (MemoryStream destination = new MemoryStream())
                            {
                                using (var source = Farc.Open(filename, EntryStreamMode.OriginalStream))
                                {
                                    source.CopyTo(destination);
                                    VanillaFarcFiles.Add(filename, destination.ToArray());
                                    source.Close();
                                }
                                destination.Close();
                            }
                        }
                        fs.Close();
                        Farc.Dispose();
                    }
                }

                foreach (var entry in ModFarcFiles)
                {
                    if (VanillaFarcFiles.ContainsKey(entry.Key) && entry.Value != VanillaFarcFiles[entry.Key])
                        VanillaFarcFiles[entry.Key] = entry.Value;
                    if (!VanillaFarcFiles.ContainsKey(entry.Key))
                        VanillaFarcFiles.Add(entry.Key, entry.Value);
                }

                if (Directory.Exists(export))
                    Directory.Delete(export, true);

                var ExportFarc = new FarcArchive { IsCompressed = compressed, Alignment = alignment };
                Directory.CreateDirectory(export);
                foreach (var entry in VanillaFarcFiles)
                {
                    File.WriteAllBytes($"{export}\\{entry.Key}", entry.Value);
                }
                FarcLog farcLog = new FarcLog();
                farcLog.Alignment = alignment;
                farcLog.Compressed = compressed;

                var serializer = new SerializerBuilder().Build();
                string yaml = serializer.Serialize(farcLog);
                File.WriteAllText($"{export}\\FarcLog.yaml", yaml);
            }
        }

        public static void PackFarc(string inpath, string basepath, string outpath, bool Compressed = false, int Alignment = 0)
        {
            string[] files = Directory.GetFiles(inpath, "*", SearchOption.AllDirectories);
            
            //Check if Folder has a yaml log
            string yaml = files.FirstOrDefault(x => x.Contains("FarcLog.yaml"));
            if (yaml != null)
            {
                yaml = File.ReadAllText(yaml);
                var deserializer = new DeserializerBuilder().Build();
                FarcLog farcLog = deserializer.Deserialize<FarcLog>(yaml);
                Compressed = farcLog.Compressed;
                Alignment = farcLog.Alignment;
            }

            var Farc = new FarcArchive { IsCompressed = Compressed, Alignment = Alignment };
            foreach (string file in files)
            {
                if (file.Contains("FarcLog.yaml"))
                    continue;

                Farc.Add(Path.GetFileName(file), file);
            }
            Farc.Save(inpath.Replace(basepath, outpath));
            Farc.Dispose();
        }
    }
}
