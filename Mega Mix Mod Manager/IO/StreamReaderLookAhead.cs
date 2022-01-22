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
        /// <summary>
        /// Reads a line of characters from the current stream and returns the data as a string, then returns to the starting point of that line of characters
        /// </summary>
        /// <returns>
        /// The next line from the input stream, or null if the end of the input stream is reached.
        /// </returns>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public static string LookAheadLine(this StreamReader sr)
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
