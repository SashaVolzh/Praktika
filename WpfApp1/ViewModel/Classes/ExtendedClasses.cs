using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModel.Classes
{
    static class ExtendedClasses
    {
        static public bool ByteEquals(this byte[] current, byte[]? other)
        {
            if (other == null || current.Length != other.Length) return false;
            for (int i = 0; i < current.Length; i++)
            {
                if (current[i] != other[i]) return false;
            }
            return true;
        }
    }
}
