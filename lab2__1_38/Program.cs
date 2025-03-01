using System;
using System.Numerics;

public class LongLong
{
    public long highPart;
    public ulong lowPart;

    public LongLong(long high, ulong low)
    {
        highPart = high;
        lowPart = low;
    }
    public static LongLong operator +(LongLong a, LongLong b)
    {
        ulong newLow = a.lowPart + b.lowPart;
        long newHigh = a.highPart + b.highPart;

        if (newLow < a.lowPart) 
        {
            newHigh++;
        }

        return new LongLong(newHigh, newLow);
    }
    public static LongLong operator -(LongLong a, LongLong b)
    {
        ulong newLow;
        long newHigh = a.highPart - b.highPart;

        if (a.lowPart < b.lowPart)
        {
            newLow = ulong.MaxValue - b.lowPart + a.lowPart + 1;
            newHigh--;
        }
        else
        {
            newLow = a.lowPart - b.lowPart;
        }

        return new LongLong(newHigh, newLow);
    }
    public static bool operator ==(LongLong a, LongLong b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }
        if (a is null || b is null)
        {
            return false;
        }
        return a.highPart == b.highPart && a.lowPart == b.lowPart;
    }
    public static bool operator !=(LongLong a, LongLong b)
    {
        return !(a == b);
    }
    public static bool operator <(LongLong a, LongLong b)
    {
        if (a.highPart < b.highPart)
        {
            return true;
        }
        if (a.highPart == b.highPart)
        {
            return a.lowPart < b.lowPart;
        }
        return false;
    }
    public static bool operator >(LongLong a, LongLong b)
    {
        if (a.highPart > b.highPart)
        {
            return true;
        }
        if (a.highPart == b.highPart)
        {
            return a.lowPart > b.lowPart;
        }
        return false;
    }
    public override bool Equals(object obj)
    {
        if (obj is LongLong other)
        {
            return this == other;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(highPart, lowPart);
    }
    public BigInteger ToBigInteger()
    {
        return ((BigInteger)highPart << 64) | lowPart;
    }
    public override string ToString()
    {
        return ToBigInteger().ToString();
    }
}
class Program
{
    static void Main()
    {
        LongLong a = new LongLong(0, 11111111);
        LongLong b = new LongLong(0, 22222221);

        LongLong sum = a + b;
        LongLong diff = a - b;

        Console.WriteLine($"Sum: {sum}");
        Console.WriteLine($"Difference: {diff}");
        Console.WriteLine($"a == b: {a == b}");
        Console.WriteLine($"a < b: {a < b}");
    }
}