using System;

namespace Helper.Socket.Bytes
{
    public static class Bytes
    {
        public static void AppendSpecifiedBytes(ref byte[] dst, byte[] src)
        {
            // Get the starting length of dst
            int i = dst.Length;
            // Resize dst so it can hold the bytes in src
            Array.Resize(ref dst, dst.Length + src.Length);
            // For each element in src
            for (int j = 0; j < src.Length; j++)
            {
                // Add the element to dst
                dst[i] = src[j];
                // Increment dst index
                i++;
            }
        }
    }
}
