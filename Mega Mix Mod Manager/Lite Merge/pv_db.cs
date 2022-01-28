using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Mega_Mix_Mod_Manager.Lite_Merge
{
    internal class pv_db
    {
        public static SortedDictionary<string, List<string>> Read(string infile)
        {
            string[] lines = File.ReadAllLines(infile);
            SortedDictionary<string, List<string>> entries = new SortedDictionary<string, List<string>>();

            foreach (string line in lines)
            {
                string[] data = line.Split('.');
                if (line == string.Empty || data[0][0] == '#')
                    continue;
                
                if (!entries.ContainsKey(data[0]))
                {
                    entries.Add(data[0], new List<string>());
                }
                entries[data[0]].Add(line);
            }

            return entries;
        }

        public static void Write(SortedDictionary<string, List<string>> entries, string outpath)
        {
            List<string[]> pv_db = new List<string[]>();

            foreach (var entry in entries)
            {
                pv_db.Add(entry.Value.ToArray());
            }

            if (!Directory.Exists(Path.GetDirectoryName(outpath)))
                Directory.CreateDirectory(Path.GetDirectoryName(outpath));

            using (StreamWriter sw = new StreamWriter(outpath))
            {
                foreach (string[] data in pv_db)
                {
                    foreach (string item in data)
                    {
                        sw.WriteLine(item);
                    }
                }
            }
        }

        public static SortedDictionary<string, List<string>> GetNewEntries(string base_db, string mod_db, SortedDictionary<string, List<string>> final_db)
        {
            SortedDictionary<string, List<string>> Base = Read(base_db);
            SortedDictionary<string, List<string>> mod = Read(mod_db);

            foreach (var entry in mod)
            {
                if (!Base.ContainsKey(entry.Key))
                {
                    final_db.Add(entry.Key, entry.Value);
                }
                else
                {
                    var test = string.Concat(entry.Value.ToArray());
                    var test1 = string.Concat(Base[entry.Key].ToArray());

                    if (test != test1)
                    {
                        final_db.Remove(entry.Key);
                        final_db.Add(entry.Key, entry.Value);
                    }
                }
            }

            return final_db;
        }

        public static void MergeMods(string[] files, string Basepv, string outfile)
        {
            SortedDictionary<string, List<string>> pvdb = new SortedDictionary<string, List<string>>();
            SortedDictionary<string, List<string>> merged = pv_db.Read(Basepv);

            foreach (string file in files)
            {
                if (Path.GetFileName(file) == "pv_db.txt")
                    pvdb = GetNewEntries(Basepv, file, pvdb);
            }
            
            foreach (var entry in pvdb)
            {
                if (merged.ContainsKey(entry.Key))
                    merged.Remove(entry.Key);
                merged.Add(entry.Key, entry.Value);
            }
            Write(merged, outfile);
        }

        public static void Export(SortedDictionary<string, List<string>> infile)
        {

        }
    }
}
