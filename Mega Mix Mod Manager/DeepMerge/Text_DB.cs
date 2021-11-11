using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega_Mix_Mod_Manager.DeepMerge.objects;

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

                for (int i = 0; i < output[key].Count; i++)
                {
                    string type = output[key][i].Split('.')[1];

                    if (type == "another_song")
                    {
                        entry.another_song = new List<pvEntry_another_song>();

                        while (!output[key][i].Contains("another_song.length"))
                        {
                            pvEntry_another_song another_song = new pvEntry_another_song();
                            another_song.name = output[key][i++].Split('=')[1];
                            another_song.name_en = output[key][i++].Split('=')[1];
                            another_song.vocal_chara_num = output[key][i++].Split('=')[1];
                            another_song.vocal_disp_name = output[key][i++].Split('=')[1];
                            another_song.vocal_disp_name_en = output[key][i++].Split('=')[1];
                            entry.another_song.Add(another_song);
                        }
                    }
                    else if (type == "auth_replace_by_module")
                    {
                        entry.auth_replace_by_module = new List<pvEntry_auth_replace_by_module>();

                        while (!output[key][i].Contains("auth_replace_by_module.length"))
                        {
                            pvEntry_auth_replace_by_module auth_replace_by_module = new pvEntry_auth_replace_by_module();
                            auth_replace_by_module.id = Convert.ToInt32(output[key][i++].Split('=')[1]);
                            auth_replace_by_module.module_id = Convert.ToInt32(output[key][i++].Split('=')[1]);
                            auth_replace_by_module.name = output[key][i++].Split('=')[1];
                            auth_replace_by_module.org_name = output[key][i++].Split('=')[1];
                            entry.auth_replace_by_module.Add(auth_replace_by_module);
                        }
                    }
                    else if (type == "bpm")
                        entry.bpm = Convert.ToInt32(output[key][i].Split('=')[1]);
                    else if (type == "chainslide_failure_name")
                        entry.chainslide_failure_name = output[key][i++].Split('=')[1];
                    else if (type == "chainslide_first_name")
                        entry.chainslide_first_name = output[key][i++].Split('=')[1];
                    else if (type == "chainslide_sub_name")
                        entry.chainslide_sub_name = output[key][i++].Split('=')[1];
                    else if (type == "chainslide_success_name")
                        entry.chainslide_success_name = output[key][i++].Split('=')[1];
                    else if (type == "chrcam")
                    {
                        entry.chrcam = new List<pvEntry_chrcam>();

                        while (!output[key][i].Contains("chrcam.length"))
                        {
                            pvEntry_chrcam chrcam = new pvEntry_chrcam();
                            chrcam.chara = output[key][i++].Split('=')[1];
                            chrcam.id = Convert.ToInt32(output[key][i++].Split('=')[1]);
                            chrcam.name = output[key][i++].Split('=')[1];
                            chrcam.org_name = output[key][i++].Split('=')[1];
                            entry.chrcam.Add(chrcam);
                        }
                    }
                    else if (type == "chreff")
                    {

                    }
                }

                pv_db.Add(key, entry);
            }

            return pv_db;
        }
    }

    
}
