using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.Objects
{
    public class Settings
    {
        public string Game_Dump { get; set; }
        public string Export_Path { get; set; }
        public string Mods_Folder { get; set; } = ".\\Mods";
        public string Default_Author { get; set; }
        public string pv_Merge { get; set; }
        public string obj_Merge { get; set; }
        public string spr_Merge { get; set; }
        public string tex_Merge { get; set; }
        public string farc_Merge { get; set; }

    }
}
