using Mega_Mix_Mod_Manager.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_disp2d
    {
        public int set_name { get; set; }
        public int target_shadow_type { get; set; }
        public string title_2d_layer { get; set; }
        public int title_end_2d_field { get; set; }
        public int title_end_2d_low_field { get; set; }
        public int title_end_3d_field { get; set; }
        public int title_start_2d_field { get; set; }
        public int title_start_2d_low_field { get; set; }
        public int title_start_3d_field { get; set; }

        public pvEntry_disp2d Read(StreamReader sr)
        {
            pvEntry_disp2d disp2d = new pvEntry_disp2d();
            string line;

            while ((line = StreamReaderLookAhead.LookAheadLine(sr)).Contains(".disp2d"))
            {
                ExecuteOP(sr, disp2d, line.Split('.')[1]);
            }

            return disp2d;
        }

        private pvEntry_disp2d ExecuteOP(StreamReader sr, pvEntry_disp2d disp2d, string operation)
        {
            Dictionary<string, Action> op = new Dictionary<string, Action>();
            op["set_name"] = () => { disp2d.set_name = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
            op["target_shadow_type"] = () => { disp2d.target_shadow_type = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
            op["title_2d_layer"] = () => { disp2d.title_2d_layer = sr.ReadLine().Split('=')[1]; };
            op["title_end_2d_field"] = () => { disp2d.title_end_2d_field = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
            op["title_end_2d_low_field"] = () => { disp2d.title_end_2d_low_field = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
            op["title_end_3d_field"] = () => { disp2d.title_end_3d_field = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
            op["title_start_2d_field"] = () => { disp2d.title_start_2d_field = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
            op["title_start_2d_low_field"] = () => { disp2d.title_start_2d_low_field = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
            op["title_start_3d_field"] = () => { disp2d.title_start_3d_field = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };

            op[operation].Invoke();
            return disp2d;
        }
    }
}
