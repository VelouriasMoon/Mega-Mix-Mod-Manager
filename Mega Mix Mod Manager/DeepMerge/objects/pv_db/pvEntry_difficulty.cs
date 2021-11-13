using Mega_Mix_Mod_Manager.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_difficulty
    {
        public int original { get; set; }
        public int slide { get; set; }
        public List<difficulty> easy { get; set; }
        public List<difficulty> encore { get; set; }
        public List<difficulty> extreme { get; set; }
        public List<difficulty> hard { get; set; }
        public List<difficulty> normal { get; set; }

        public class difficulty
        {
            public int att_extra { get; set; }
            public int att_original { get; set; }
            public int att_slide { get; set; }
            public int edition { get; set; }
            public string level { get; set; }
            public int level_sort_index { get; set; }
            public string script_file_name { get; set; }
            public string script_format { get; set; }
            public string version { get; set; }

            public List<difficulty> Read(StreamReader sr)
            {
                List<difficulty> list = new List<difficulty>();
                int index = Convert.ToInt32(StreamReaderLookAhead.LookAheadLine(sr).Split('.')[3]);
                string line;

                while (!(line = StreamReaderLookAhead.LookAheadLine(sr)).Contains("length="))
                {
                    difficulty dif = new difficulty();
                    while (Convert.ToInt32(line.Split('.')[3]) == index)
                    {
                        if (line.Contains("attribute"))
                            dif = executeOP(sr, dif, $"{line.Split('.')[1]}.{line.Split('.')[2]}");
                        else
                            dif = executeOP(sr, dif, line.Split('.')[1]);
                        line = StreamReaderLookAhead.LookAheadLine(sr);
                    }
                    list.Add(dif);
                }

                return list;
            }

            private difficulty executeOP(StreamReader sr, difficulty dif, string operation)
            {
                difficulty difnew = dif;
                Dictionary<string, Action> op = new Dictionary<string, Action>();
                op["attribute.extra"] = () => { difnew.att_extra = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
                op["attribute.original"] = () => { difnew.att_original = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
                op["attribute.slide"] = () => { difnew.att_slide = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
                op["edition"] = () => { difnew.edition = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
                op["level"] = () => { difnew.level = sr.ReadLine().Split('=')[1]; };
                op["level_sort_index"] = () => { difnew.level_sort_index = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
                op["script_file_name"] = () => { difnew.script_file_name = sr.ReadLine().Split('=')[1]; };
                op["script_format"] = () => { difnew.script_format = sr.ReadLine().Split('=')[1]; };
                op["version"] = () => { difnew.version = sr.ReadLine().Split('=')[1]; };

                op[operation].Invoke();
                return difnew;
            }
        }

        public pvEntry_difficulty Read(StreamReader sr)
        {
            pvEntry_difficulty difficulty = new pvEntry_difficulty();
            string line;
            while ((line = StreamReaderLookAhead.LookAheadLine(sr)).Contains(".difficulty"))
            {
                if (line.Contains("attribute"))
                    difficulty = executeOP(sr, difficulty, $"{line.Split('.')[1]}.{line.Split('.')[2]}");
                else
                    difficulty = executeOP(sr, difficulty, line.Split('.')[1]);
            }
            return difficulty;
        }

        private pvEntry_difficulty executeOP(StreamReader sr, pvEntry_difficulty difficulty, string operation)
        {
            pvEntry_difficulty difficultynew = difficulty;
            Dictionary<string, Action> op = new Dictionary<string, Action>();
            op["attribute.original"] = () => { difficultynew.original = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
            op["attribute.slide"] = () => { difficultynew.slide = Convert.ToInt32(sr.ReadLine().Split('=')[1]); };
            op["easy"] = () => { difficultynew.easy = new difficulty().Read(sr); };
            op["encore"] = () => { difficultynew.encore = new difficulty().Read(sr); };
            op["extreme"] = () => { difficultynew.extreme = new difficulty().Read(sr); };
            op["hard"] = () => { difficultynew.hard = new difficulty().Read(sr); };
            op["normal"] = () => { difficultynew.normal = new difficulty().Read(sr); };

            op[operation].Invoke();
            return difficultynew;
        }
    }
}
