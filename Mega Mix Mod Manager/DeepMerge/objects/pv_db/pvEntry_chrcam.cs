using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_chrcam
    {
        public string chara { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string org_name { get; set; }

        public List<pvEntry_chrcam> Read(StreamReader sr)
        {
            List<pvEntry_chrcam> list = new List<pvEntry_chrcam>();
            string line;

            while (!(line = sr.ReadLine()).Contains(".length="))
            {
                pvEntry_chrcam item = new pvEntry_chrcam();
                item.chara = line.Split('=')[1];
                item.id = Convert.ToInt32(sr.ReadLine().Split('=')[1]);
                item.name = sr.ReadLine().Split('=')[1];
                item.org_name = sr.ReadLine().Split('=')[1];
                list.Add(item);
            }
            sr.ReadLine();
            return list;
        }
    }
}
