using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.Objects
{
    public class ModList
    {
        public Region region { get; set; } = Region.EN;
        public List<ModList_Entry> mods { get; set; } = new List<ModList_Entry>();
        public class ModList_Entry
        {
            public string Name { get; set; }
            public string hash { get; set; }
        }

        public enum Region
        {
            JPN,
            EN
        }

        public void MoveDown(int selected_index)
        {
            ModList_Entry mod = mods[selected_index];
            mods.RemoveAt(selected_index);
            if (selected_index + 1 > mods.Count)
            {
                mods.Add(mod);
            }
            else
            {
                mods.Insert(selected_index + 1, mod);
            }
        }

        public void MoveUp(int selected_index)
        {
            ModList_Entry mod = mods[selected_index];
            mods.RemoveAt(selected_index);
            if (selected_index - 1 <= 0)
            {
                mods.Insert(selected_index, mod);
            }
            else
            {
                mods.Insert(selected_index - 1, mod);
            }
            
        }
    }
}
