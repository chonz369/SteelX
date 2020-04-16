using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;

public static class NetworkUtils
{
    public static uint FloatToUInt32(float value) { return new UIntFloat() { floatValue = value }.intValue; }
    public static float UInt32ToFloat(uint value) { return new UIntFloat() { intValue = value }.floatValue; }

    static NetworkUtils() {
        stopwatch.Start();
    }

    public static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    public static void MemCopy(byte[] src, int srcIndex, byte[] dst, int dstIndex, int count) {
        for (int i = 0; i < count; ++i)
            dst[dstIndex++] = src[srcIndex++];
    }

    public static int MemCmp(byte[] a, int aIndex, byte[] b, int bIndex, int count) {
        for (int i = 0; i < count; ++i) {
            var diff = b[bIndex++] - a[aIndex++];
            if (diff != 0)
                return diff;
        }

        return 0;
    }
    public static int MemCmp(uint[] a, int aIndex, uint[] b, int bIndex, int count) {
        for (int i = 0; i < count; ++i) {
            var diff = b[bIndex++] - a[aIndex++];
            if (diff != 0)
                return (int)diff;
        }

        return 0;
    }

    public static uint SimpleHashStreaming(uint old_hash, uint value) {
        return old_hash * 179 + value + 1;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct UIntFloat
    {
        [FieldOffset(0)]
        public float floatValue;
        [FieldOffset(0)]
        public uint intValue;
    }
}

class ByteArrayComp : IEqualityComparer<byte[]>, IComparer<byte[]>
{
    public static readonly ByteArrayComp instance = new ByteArrayComp();

    public int Compare(byte[] x, byte[] y) {
        if (x == null || y == null)
            throw new ArgumentNullException("Trying to compare array with null");
        var xl = x.Length;
        var yl = y.Length;
        if (xl != yl)
            return yl - xl;
        for (int i = 0; i < xl; i++) {
            var d = y[i] - x[i];
            if (d != 0)
                return d;
        }
        return 0;
    }

    public bool Equals(byte[] x, byte[] y) {
        return Compare(x, y) == 0;
    }

    public int GetHashCode(byte[] x) {
        if (x == null)
            throw new ArgumentNullException("Trying to get hash of null");
        var xl = x.Length;
        if (xl >= 4)
            return (int)(x[0] + (x[1] << 8) + (x[2] << 16) + (x[3] << 24));
        else
            return 0;
    }
}

public class Aggregator
{
    const int k_WindowSize = 120;

    public float previousValue;
    public FloatRollingAverage graph = new FloatRollingAverage(k_WindowSize);

    public void Update(float value) {
        graph.Update(value - previousValue);
        previousValue = value;
    }
}