using System.Collections.Generic;

namespace Oyster.Examples.ProtoBuf
{
    public class BytesComparer : IEqualityComparer<byte[]>
    {
        public static readonly IEqualityComparer<byte[]> Instance = new BytesComparer();

        public bool Equals(byte[] x, byte[] y)
        {
            if (x == y) return true;
            if (x == null || y == null || x.Length != y.Length) return false;

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i]) return false;
            }
            return true;
        }

        public int GetHashCode(byte[] x)
        {
            if (x == null) return 0;

            int result = -1623343517;
            for (int i = 0; i < x.Length; i++)
            {
                result = -1521134295 * result + x[i];
            }
            return result;
        }
    }
}
