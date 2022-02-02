using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.Objects
{
    public class VersionMap
    {
        public enum Region
        {
            JPN,
            ENG,
        }

        public Dictionary<string, string> Version { get; set; }

        public VersionMap(Region region)
        {
            Version = new Dictionary<string, string>();
            if (region == Region.JPN)
            {
                Version.Add("1.0.3", "FAE8AA778E4CE612CE4E7655739BB94FF9D2CB11");
                Version.Add("1.0.4", "E20FE72B6DFDB1966CE5C3627218834A7CE326C1");
                Version.Add("1.0.5", "F51A4F345A39903C79BB28CA64C074C1EB19C77F");
                Version.Add("1.0.6", "3BD96B7C174A9AD189F33984F707596A7FCBFCF3");
                Version.Add("1.0.7", "E0C41878F636AD4F362B74FB0C77B95A22CE7364");
            }
            else if (region == Region.ENG)
            {
                Version.Add("1.0.1", "2FF03FC088CBA7A7710F18D8A34CAAD98ECEFA31");
                Version.Add("1.0.2", "3BD96B7C174A9AD189F33984F707596A7FCBFCF3");
                Version.Add("1.0.4", "E0C41878F636AD4F362B74FB0C77B95A22CE7364");
            }
        }

        public static Region GetRegion(string name)
        {
            if (name == "rom_switch" || name == "rom_switch_cn" || name == "rom_switch_tw")
                return Region.JPN;
            if (name == "rom_switch_en")
                return Region.ENG;
            else
                return Region.ENG;
        }
    }
}
