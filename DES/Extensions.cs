using System;
using System.Collections;
using System.Linq;

namespace DES {
    public static class ArrayExtensions {
        public static T[] Rotate<T>(this T[] array, int k) {
            if (k == 0)
                return array;
            k %= array.Length;
            if (k < 0)
                k = array.Length + k;
            Array.Reverse(array);
            Array.Reverse(array, 0, k);
            Array.Reverse(array, k, array.Length - k);

            return array;
        }
    }

    public static class BitArrayExtensions {
        public static string ToBinaryString(this BitArray bitArray) {
            var byteArray = new byte[8];
            bitArray.CopyTo(byteArray, 0);
            return string.Join("", byteArray.Reverse().Select(b => Convert.ToString(b, 2).PadLeft(8, '0')))[(64 - bitArray.Length)..];
        }
    }
}
