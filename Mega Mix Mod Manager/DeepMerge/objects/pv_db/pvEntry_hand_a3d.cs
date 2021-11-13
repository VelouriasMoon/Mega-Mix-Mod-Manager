using Mega_Mix_Mod_Manager.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_hand_a3d
    {
        public List<string> _1P { get; set; }

        public pvEntry_hand_a3d Read(StreamReader sr)
        {
            pvEntry_hand_a3d a3d = new pvEntry_hand_a3d();
            List<string> list = new List<string>();

            while (!StreamReaderLookAhead.LookAheadLine(sr).Contains(".length="))
            {
                string name = sr.ReadLine().Split('=')[1];
                list.Add(name);
            }
            sr.ReadLine();
            a3d._1P = list;
            return a3d;
        }
    }
}
