using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mega_Mix_Mod_Manager.IO
{
    public static class StreamReaderLookAhead
    {
        public static string LookAheadLine(StreamReader sr)
        {
            long pos = sr.BaseStream.Position;
            string line = sr.ReadLine();

            if (line == null)
                return null;

            sr.BaseStream.Seek(pos, SeekOrigin.Begin);
            return line;
        }
    }
}
