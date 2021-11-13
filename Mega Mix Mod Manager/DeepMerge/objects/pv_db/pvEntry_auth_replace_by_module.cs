using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_auth_replace_by_module
    {
        public int id { get; set; }
        public int module_id { get; set; }
        public string name { get; set; }
        public string org_name { get; set; }

        public List<pvEntry_auth_replace_by_module> Read(StreamReader sr)
        {
            List<pvEntry_auth_replace_by_module> list = new List<pvEntry_auth_replace_by_module>();
            string line;

            while (!(line = sr.ReadLine()).Contains(".length="))
            {
                pvEntry_auth_replace_by_module item = new pvEntry_auth_replace_by_module();
                item.id = Convert.ToInt32(line.Split('=')[1]);
                item.module_id = Convert.ToInt32(sr.ReadLine().Split('=')[1]);
                item.name = sr.ReadLine().Split('=')[1];
                item.org_name = sr.ReadLine().Split('=')[1];
                list.Add(item);
            }
            sr.ReadLine();
            return list;
        }
    }
}
