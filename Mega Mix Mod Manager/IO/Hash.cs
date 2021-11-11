using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Mega_Mix_Mod_Manager.IO
{
    internal class Hash
    {
        public static string HashFile(byte[] infile)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(infile);
                StringBuilder hashstring = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    hashstring.Append(b.ToString("X2"));
                }

                return hashstring.ToString();
            }
        }

        public static string HashString(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder hashstring = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    hashstring.Append(b.ToString("X2"));
                }

                return hashstring.ToString();
            }
        }
    }
}
