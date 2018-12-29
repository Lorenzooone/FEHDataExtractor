using System;
using System.Collections.Generic;
using System.Text;

public abstract class Xor
{
    private byte[] xorData;
    public static int offset = 0x20;

    public Xor(params byte[] A)
	{
        XorData = new byte[A.Length];
        for (int i = 0; i < A.Length; i++)
            XorData[i] = A[i];
	}

    public byte getDataXorred(byte data, int index)
    {
        return (byte)(data ^ XorData[index % XorData.Length]);
    }

    public byte[] XorData { get => xorData; set => xorData = value; }
}

public class UInt64Xor : Xor
{
    private UInt64 value;

    public UInt64Xor(params byte[] A) : base(A) { }

    public void XorValue(Int64 value)
    { Value = (UInt64)(getDataXorred((byte)(value & 255), 0) + (getDataXorred((byte)((value >> 8) & 255), 1) << 8) + (getDataXorred((byte)((value >> 16) & 255), 2) << 16) + (getDataXorred((byte)((value >> 24) & 255), 3) << 24) + ((long)getDataXorred((byte)((value >> 32) & 255), 4) << 32) + ((long)getDataXorred((byte)((value >> 40) & 255), 5) << 40) + ((long)getDataXorred((byte)((value >> 48) & 255), 6) << 48) + ((long)getDataXorred((byte)((value >> 56) & 255), 7) << 56)); }

    override public String ToString()
    {
        return Value.ToString();
    }

    public UInt64 Value { get => value; set => this.value = value; }
}

public class Int64Xor : Xor
{
    private Int64 value;

    public Int64Xor(params byte[] A) : base(A) { }
    public void XorValue(Int64 value)
    { Value = (Int64)(getDataXorred((byte)(value & 255), 0) + (getDataXorred((byte)((value >> 8) & 255), 1) << 8) + (getDataXorred((byte)((value >> 16) & 255), 2) << 16) + (getDataXorred((byte)((value >> 24) & 255), 3) << 24) + (getDataXorred((byte)((value >> 32) & 255), 4) << 32) + (getDataXorred((byte)((value >> 40) & 255), 5) << 40) + (getDataXorred((byte)((value >> 48) & 255), 6) << 48) + (getDataXorred((byte)((value >> 56) & 255), 7) << 56)); }

    override public String ToString()
    {
        return Value.ToString();
    }

    public Int64 Value { get => value; set => this.value = value; }
}

public class UInt32Xor : Xor
{
    private UInt32 value;

    public UInt32Xor(params byte[] A) : base(A) { }

    public void XorValue(Int32 value)
    { Value = (UInt32)(getDataXorred((byte)(value & 255), 0) + (getDataXorred((byte)((value >> 8) & 255), 1) << 8) + (getDataXorred((byte)((value >> 16) & 255), 2) << 16) + (getDataXorred((byte)((value >> 24) & 255), 3) << 24)); }

    override public String ToString()
    {
        return Value.ToString();
    }

    public UInt32 Value { get => value; set => this.value = value; }
}

public class Int32Xor : Xor
{
    private Int32 value;

    public Int32Xor(params byte[] A) : base(A) { }

    public void XorValue(Int32 value)
    { Value = getDataXorred((byte)(value & 255), 0) + (getDataXorred((byte)((value >> 8) & 255), 1) << 8) + (getDataXorred((byte)((value >> 16) & 255), 2) << 16) + (getDataXorred((byte)((value >> 24) & 255), 3) << 24); }

    override public String ToString()
    {
        return Value.ToString();
    }

    public Int32 Value { get => value; set => this.value =value; }
}

public class ByteXor : Xor
{
    private byte value;

    public ByteXor(params byte[] A) : base(A) { }

    public void XorValue(byte value)
    { Value = getDataXorred(value, 0); }

    override public String ToString()
    {
        return Value.ToString();
    }

    public byte Value { get => value; set => this.value = value; }
}

public class SByteXor : Xor
{
    private SByte value;

    public SByteXor(params byte[] A) : base(A) { }

    public void XorValue(byte value)
    { Value = (SByte)getDataXorred(value, 0); }

    override public String ToString()
    {
        return Value.ToString();
    }

    public SByte Value { get => value; set => this.value = value; }
}

public class UInt16Xor : Xor
{
    private UInt16 value;

    public UInt16Xor(params byte[] A) : base(A) { }

    public void XorValue(Int16 value)
    { Value = (UInt16)(getDataXorred((byte)(value & 255), 0) + (getDataXorred((byte)((value >> 8) & 255), 1) << 8)); }

    override public String ToString()
    {
        return Value.ToString();
    }

    public UInt16 Value { get => value; set => this.value = value; }
}

public class Int16Xor : Xor
{
    private Int16 value;

    public Int16Xor(params byte[] A) : base(A) { }

    public void XorValue(Int16 value)
    { Value = (Int16)(getDataXorred((byte)(value & 255), 0) + (getDataXorred((byte)((value >> 8) & 255), 1) << 8)); }

    override public String ToString()
    {
        return Value.ToString();
    }

    public Int16 Value { get => value; set => this.value = value; }
}

public class ExtractUtils
{
    public static int getInt(long a, byte[] data)
    {
        return data[a] + (data[a + 1] << 8) + (data[a + 2] << 16) + (data[a + 3] << 24);
    }

    public static short getShort(long a, byte[] data)
    {
        return (short)(data[a] + (data[a + 1] << 8));
    }

    public static long getLong(long a, byte[] data)
    {
        return (long)data[a] + (data[a + 1] << 8) + (data[a + 2] << 16) + (data[a + 3] << 24) + ((long)data[a + 4] << 32) + ((long)data[a + 5] << 40) + ((long)data[a + 6] << 48) + ((long)data[a + 7] << 56);
    }

    public static String BitmaskConvertToString(UInt32 value, String[] Names) {
        String text = "";
        int tmp = 1;
        bool start = true;
        for (int i = 0; i < Names.Length; i++)
        {
            if (((value & tmp) >> i) == 1)
            {
                if (!start)
                    text += ", " + Names[i];
                else
                    text += " " + Names[i];
                start = false;
            }
            tmp = tmp << 1;
        }
        return text;
    }
}

public class StringXor : Xor
{
    private String value;

    public StringXor(long a, byte[] data, params byte[] A) : base(A)
    {
        Value = "";
        List<byte> tmp = new List<byte>();
        if (a != offset && a < data.Length) {
            for (int i = 0; a + i < data.Length && data[a + i] != 0; i++)
                tmp.Add(getDataXorred(data[a + i], i));
            Value = Encoding.UTF8.GetString(tmp.ToArray());
        }
    }

    override public String ToString()
    {
        return Value;
    }

    public String Value { get => value; set => this.value = value; }
}