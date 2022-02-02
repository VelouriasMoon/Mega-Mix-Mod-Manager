using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Mix_Mod_Manager.Objects
{
    public class PatchList
    {
        public List<Patch> Patches {  get; set;} = new List<Patch>();

        public class Patch
        {
            public string Name { get; set; }
            public string hash { get; set; }
            public bool Enabled { get; set; } = true;
            public string Code { get; set; }
        }

        public Patch GetPatchByName(string name) =>
            Patches.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public Patch GetPatchByHash(string hash) =>
            Patches.FirstOrDefault(p => p.hash.Equals(hash, StringComparison.OrdinalIgnoreCase));
    }
}
