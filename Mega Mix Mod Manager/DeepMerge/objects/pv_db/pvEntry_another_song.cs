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
            List<pvEntry_another_song> list = new List<pvEntry_another_song>();
            string line;

            while (!(line = sr.ReadLine()).Contains("length="))
            {
                pvEntry_another_song another_Song = new pvEntry_another_song();
                another_Song.name = line.Split('=')[1];
                another_Song.name_en = sr.ReadLine().Split('=')[1];
                another_Song.song_file_name = sr.ReadLine().Split('=')[1];
                another_Song.vocal_chara_num = sr.ReadLine().Split('=')[1];
                another_Song.vocal_disp_name = sr.ReadLine().Split('=')[1];
                another_Song.vocal_disp_name_en = sr.ReadLine().Split('=')[1];
                list.Add(another_Song);
            }
            sr.ReadLine();
            return list;
        }
    }
}
