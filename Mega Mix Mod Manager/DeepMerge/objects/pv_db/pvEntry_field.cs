using Mega_Mix_Mod_Manager.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_field
    {
        //This entry is more extensive in the pv_field, but for the pv_db a basic one should suffice
        public string ex_stage { get; set; }
        public string stage { get; set; }

        public List<pvEntry_field> Read(StreamReader sr)
        {
            List<pvEntry_field> list = new List<pvEntry_field>();

            while (!StreamReaderLookAhead.LookAheadLine(sr).Contains(".length="))
            {
                pvEntry_field field = new pvEntry_field();
                field.stage = sr.ReadLine().Split('=')[1];
                field.ex_stage = sr.ReadLine().Split('=')[1];
                list.Add(field);
            }
            sr.ReadLine();

            return list;
        }
    }
}
