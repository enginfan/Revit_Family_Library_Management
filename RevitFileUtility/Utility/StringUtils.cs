using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleKill.RevitFileUtility.Utility
{
    internal static class StringUtils
    {
        internal static string TakeNumbers(string source)
        {
            var characters = source.ToCharArray();
            var numbers = characters.Where(character => character >= '0' && character <= '9');
            return string.Join("", numbers);
        }
    }
}
