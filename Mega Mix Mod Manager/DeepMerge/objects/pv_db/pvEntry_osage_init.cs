using Mega_Mix_Mod_Manager.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_osage_init
    {
        public int frame { get; set; }
        public string motion { get; set; }
        public string stage { get; set; }

        public List<pvEntry_osage_init> Read(StreamReader sr)
        {
            List<pvEntry_osage_init> list = new List<pvEntry_osage_init>();

            while (!StreamReaderLookAhead.LookAheadLine(sr).Contains(".length="))
            {
                pvEntry_osage_init osage = new pvEntry_osage_init();
                osage.frame = Convert.ToInt32(sr.ReadLine().Split('=')[1]);
                osage.motion = sr.ReadLine().Split('=')[1];
                osage.stage = sr.ReadLine().Split('=')[1];
                list.Add(osage);
            }
            sr.ReadLine();

            return list;
        }
    }
}
