using Mega_Mix_Mod_Manager.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.DeepMerge.objects.pv_db
{
    public class pvEntry_ex_song
    {
        public string chara { get; set; }
        public List<ex_song_ex_auth> ex_auth { get; set; }
        public string file { get; set; }

        public class ex_song_ex_auth
        {
            public string name { get; set; }
            public string org_name { get; set; }
        }

        public List<pvEntry_ex_song> Read(StreamReader sr)
        {
            List<pvEntry_ex_song> list = new List<pvEntry_ex_song>();

            string line;
            while (!(line = sr.ReadLine()).Contains("ex_song.length="))
            {
                pvEntry_ex_song ex_song = new pvEntry_ex_song();
                ex_song.chara = line.Split('=')[1];
                if (StreamReaderLookAhead.LookAheadLine(sr).Contains("ex_auth"))
                {
                    List<ex_song_ex_auth> auth = new List<ex_song_ex_auth>();
                    while (!StreamReaderLookAhead.LookAheadLine(sr).Contains("ex_auth.length"))
                    {
                        ex_song_ex_auth ex_Auth = new ex_song_ex_auth();
                        ex_Auth.name = sr.ReadLine().Split('=')[1];
                        ex_Auth.org_name = sr.ReadLine().Split('=')[1];
                        auth.Add(ex_Auth);
                    }
                    sr.ReadLine();
                    ex_song.ex_auth = auth;
                }
                ex_song.file = sr.ReadLine().Split('=')[1];
            }
            sr.ReadLine();
            return list;
        }
    }
}
