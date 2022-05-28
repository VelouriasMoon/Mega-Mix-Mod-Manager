using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.Enums
{
    public enum Region
    {
        rom_switch,
        rom_switch_en,
        rom_switch_cn,
        rom_switch_tw,
        rom_steam,
        rom_steam_dlc,
        rom_steam_region_dlc
    }

    public enum MergeOptions
    {
        Both,
        Install,
        Export,
        None
    }
}
