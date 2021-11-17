using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuLibrary.Archives;
using MikuMikuLibrary.IO.Common;

namespace Mega_Mix_Mod_Manager.Lite_Merge
{
    internal class farc
    {
        public static FarcArchive Read(string infile)
        {
            using (FileStream fs = new FileStream(infile, FileMode.Open))
            {
                using (EndianBinaryReader ebr = new EndianBinaryReader(fs, MikuMikuLibrary.IO.Endianness.Big))
                {
                    FarcArchive farcArchive = new FarcArchive();
                    farcArchive.Read(ebr);
                    return farcArchive;
                }
            }
        }
    }
}
