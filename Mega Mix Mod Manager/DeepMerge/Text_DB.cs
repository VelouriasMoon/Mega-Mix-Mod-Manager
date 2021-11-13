using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db;

namespace Mega_Mix_Mod_Manager.DeepMerge
{
    public static class Text_DB
    {
        public static void Compair_pv_db(string original_pv, string mod_pv)
        {
            SortedDictionary<string, pvEntry> oripv = Log_pv_db(original_pv);
            SortedDictionary<string, pvEntry> modpv = Log_pv_db(mod_pv);


        }

        public static SortedDictionary<string, pvEntry> Log_pv_db(string pv)
        {
            Dictionary<string, List<string>> output = new Dictionary<string, List<string>>();
            string[] lines = File.ReadAllLines(pv);

            foreach (string line in lines)
            {
                string[] data = line.Split('.');
                if (line == string.Empty || data[0][0] == '#')
                    continue;
                if (!output.ContainsKey(data[0]))
                {
                    List<string> list = new List<string>();
                    output.Add(data[0], list);
                }
                output[data[0]].Add(line);
            }

            SortedDictionary<string, pvEntry> pv_db = new SortedDictionary<string, pvEntry>();
            foreach (string key in output.Keys)
            {
                pvEntry entry = new pvEntry();

                

                pv_db.Add(key, entry);
            }

            return pv_db;
        }
    }

    
}
