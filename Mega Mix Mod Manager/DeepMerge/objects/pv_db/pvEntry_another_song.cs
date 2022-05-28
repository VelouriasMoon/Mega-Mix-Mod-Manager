using Mega_Mix_Mod_Manager.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_another_song
    {
        public string name { get; set; }
        public string name_en { get; set; }
        public string song_file_name { get; set; }
        public string vocal_chara_num { get; set; }
        public string vocal_disp_name { get; set; }
        public string vocal_disp_name_en { get; set; }

        public List<pvEntry_another_song> Read(StreamReader sr)
        {
            //Setup intial data
            List<pvEntry_another_song> list = new List<pvEntry_another_song>();
            string line;
            pvEntry_another_song another_Song = new pvEntry_another_song();

            //Create operation dictionary to read dynamically sized entries
            Dictionary<string, Action> op = new Dictionary<string, Action>();
            op["name"] = () => { another_Song.name = sr.ReadLine().Split('=')[1]; };
            op["name_en"] = () => { another_Song.name_en = sr.ReadLine().Split('=')[1]; };
            op["song_file_name"] = () => { another_Song.song_file_name = sr.ReadLine().Split('=')[1]; };
            op["vocal_chara_num"] = () => { another_Song.vocal_chara_num = sr.ReadLine().Split('=')[1]; };
            op["vocal_disp_name"] = () => { another_Song.vocal_disp_name = sr.ReadLine().Split('=')[1]; };
            op["vocal_disp_name_en"] = () => { another_Song.vocal_disp_name_en = sr.ReadLine().Split('=')[1]; };

            //loop until reaching the length line which is at the end
            while (!(line = sr.LookAheadLine()).Contains("length="))
            {
                //split line into parts for easy handling
                string[] parts = line.Split('=')[0].Split('.');

                //check entry number
                if (list.Count == Convert.ToInt32(parts[2]))
                {
                    //invoke which line to read based on type
                    op[parts[3]].Invoke();
                }
                else
                {
                    //if slot number changed store current entry and make a new one
                    list.Add(another_Song);
                    another_Song = new pvEntry_another_song();
                }
            }
            //add final working entry to list
            list.Add(another_Song);

            //read past the length line
            sr.ReadLine();
            return list;
        }
    }
}
