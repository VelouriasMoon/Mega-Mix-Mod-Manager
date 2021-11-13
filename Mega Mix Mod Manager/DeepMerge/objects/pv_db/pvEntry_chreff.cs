using Mega_Mix_Mod_Manager.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_chreff
    {
        public List<chreff_data> data { get; set; }
        public int id { get; set; }
        public string name { get; set; }

        public class chreff_data
        {
            public string name { get; set; }
            public string type { get; set; }
        }

        public List<pvEntry_chreff> Read(StreamReader sr)
        {
            List<pvEntry_chreff> list = new List<pvEntry_chreff>();

            while (!StreamReaderLookAhead.LookAheadLine(sr).Contains(".num="))
            {
                data = new List<chreff_data>();
                while (!StreamReaderLookAhead.LookAheadLine(sr).Contains(".length="))
                {
                    chreff_data newdata = new chreff_data();
                    newdata.name = sr.ReadLine().Split('=')[1];
                    newdata.type = sr.ReadLine().Split('=')[1];
                    data.Add(newdata);
                }
                sr.ReadLine();
                pvEntry_chreff item = new pvEntry_chreff();
                item.id = Convert.ToInt32(sr.ReadLine().Split('=')[1]);
                item.name = sr.ReadLine().Split('=')[1];
                list.Add(item);
            }
            sr.ReadLine();
            return list;
        }
    }
}
