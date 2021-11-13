using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_performer
    {
        public string chara { get; set; }
        public bool Fixed { get; set; }
        public int pseudo_same_id { get; set; }
        public int pv_costume { get; set; }
        public string size { get; set; }
        public string type { get; set; }

        public List<pvEntry_performer> Read(StreamReader sr)
        {
            List<pvEntry_performer> list = new List<pvEntry_performer>();



            return list;
        }
    }
}
