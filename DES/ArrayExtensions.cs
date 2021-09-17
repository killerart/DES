using System;

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
}
