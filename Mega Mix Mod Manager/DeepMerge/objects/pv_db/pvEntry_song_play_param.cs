using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_song_play_param
    {
        public List<beat> bpm { get; set; }
        public int max_frame { get; set; }
        public List<beat> rhythm { get; set; }

        public class beat
        {
            public int bar { get; set; }
            public int data { get; set; }

        }
    }
}
