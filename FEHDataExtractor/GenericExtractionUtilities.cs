using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

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
    { Value = (UInt64)(getDataXorred((byte)(value & 255), 0) + ((ulong)getDataXorred((byte)((value >> 8) & 255), 1) << 8) + ((ulong)getDataXorred((byte)((value >> 16) & 255), 2) << 16) + ((ulong)getDataXorred((byte)((value >> 24) & 255), 3) << 24) + ((ulong)getDataXorred((byte)((value >> 32) & 255), 4) << 32) + ((ulong)getDataXorred((byte)((value >> 40) & 255), 5) << 40) + ((ulong)getDataXorred((byte)((value >> 48) & 255), 6) << 48) + ((ulong)getDataXorred((byte)((value >> 56) & 255), 7) << 56)); }

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
    { Value = (Int64)(getDataXorred((byte)(value & 255), 0) + ((long)getDataXorred((byte)((value >> 8) & 255), 1) << 8) + ((long)getDataXorred((byte)((value >> 16) & 255), 2) << 16) + ((long)getDataXorred((byte)((value >> 24) & 255), 3) << 24) + ((long)getDataXorred((byte)((value >> 32) & 255), 4) << 32) + ((long)getDataXorred((byte)((value >> 40) & 255), 5) << 40) + ((long)getDataXorred((byte)((value >> 48) & 255), 6) << 48) + ((long)getDataXorred((byte)((value >> 56) & 255), 7) << 56)); }

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
    { Value = (UInt32)(getDataXorred((byte)(value & 255), 0) + (getDataXorred((byte)((value >> 8) & 255), 1) << 8) + (getDataXorred((byte)((value >> 16) & 255), 2) << 16) + ((UInt32)getDataXorred((byte)((value >> 24) & 255), 3) << 24)); }

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

public class LoadMessages
{
    private static Messages tmp = new Messages();
    public static readonly int offset = 0x20;

    public static void openFolder(string path)
    {
        if (Directory.Exists(path))
        {
            foreach (string p in (new DirectoryInfo(path)).GetFiles().Select(f => f.FullName))
                openFolder(p);
            foreach (string p in (new DirectoryInfo(path)).GetDirectories().Select(f => f.FullName))
                openFolder(p);
        }
        else if (File.Exists(path) && Path.GetExtension(path).ToLower().Equals(".lz"))
        {
            byte[] data = Decompression.Open(path);
            if (data != null)
            {
                HSDARC a = new HSDARC(0, data);
                while (a.Ptr_list_length - a.NegateIndex > a.Index)
                {
                    tmp.InsertIn(a, offset, data);
                }
                string path2 = path.Remove(path.Length - 6, 6) + "txt";
                string output = tmp.ToString();
                File.WriteAllText(path2, output);
            }
        }
    }
}

public class LoadHeroes
{
    private static BaseExtractArchive<SinglePerson> tmp = new BaseExtractArchive<SinglePerson>();
    public static readonly int offset = 0x20;

    public static String convStr(String str)
    {
        str = str.ToLower();
        str = str.Replace("\'", "");
        str = str.Replace(" ", "-");
        str = str.Replace("à", "a");
        str = str.Replace("á", "a");
        str = str.Replace("é", "e");
        str = str.Replace("è", "e");
        str = str.Replace("ì", "i");
        str = str.Replace("í", "i");
        str = str.Replace("ò", "o");
        str = str.Replace("ó", "o");
        str = str.Replace("ö", "o");
        str = str.Replace("ù", "u");
        str = str.Replace("ú", "u");
        str = str.Replace("ý", "y");
        str = str.Replace("+", "-plus-");
        str = str.Replace(",", "");
        str = str.Replace("ð", "");
        str = str.Replace("/", "-");
        str = str.Replace("\r", "");
        str = str.Replace("\n", "");
        str = str.Replace("\"", "");
        str = str.Replace(".", "");
        str = str.Replace("(", "");
        str = str.Replace(")", "");
        str = str.Replace("!", "");
        str = str.Replace("--", "-");
        if (str.LastIndexOf('-') == str.Length - 1)
            str = str.Substring(0, str.Length - 1);
        return str;
    }

    public static void exploreWep(JObject hero, String key, int point1, int point2, SinglePerson person)
    {
        List<String> vals = new List<string>();
        if (hero[key] != null)
        {
            JArray arr = (JArray)hero[key];
            vals.AddRange(arr.ToObject<string[]>());
        }
        for (int i = 4; i >= 0; i--)
        {
            if (person.Skills[i, point2] != null && person.Skills[i, point2].ToString() != "")
            {
                string val = convStr(person.getStuffExclusive(person.Skills[i, point2], ""));
                if (!vals.Contains(val))
                {
                    vals.Add(val);
                }
            }
            if (person.Skills[i, point1] != null && person.Skills[i, point1].ToString() != "")
            {
                string val = convStr(person.getStuffExclusive(person.Skills[i, point1], ""));
                if (val .Equals ("falchion") && person.HeroName .Equals ("Alm"))
                    val = "falchion-2";
                if (val .Equals( "falchion" ) && (person.HeroName .Equals ("Chrom") || person.HeroName.Equals("Lucina")))
                    val = "falchion-1";
                if (val .Equals ("missiletainn") && (person.HeroName .Equals ("Owain")))
                    val = "missiletainn-sword";
                if (!vals.Contains(val))
                {
                    vals.Add(val);
                }
            }
        }
        if(vals.Count > 0)
            hero[key] = JArray.FromObject(vals.ToArray());
    }

    public static void exploreSkill(JObject hero, String key, int point1, SinglePerson person)
    {
        List<String> vals = new List<string>();
        if (hero[key] != null)
        {
            JArray arr = (JArray)hero[key];
            vals.AddRange(arr.ToObject<string[]>());
        }
        for (int i = 4; i >= 0; i--)
        {
            if (person.Skills[i, point1] != null && person.Skills[i, point1].ToString() != "")
            {
                String val = convStr(person.getStuffExclusive(person.Skills[i, point1], ""));
                if (!vals.Contains(val))
                {
                    vals.Add(val);
                }
            }
        }
        if (vals.Count > 0)
            hero[key] = JArray.FromObject(vals.ToArray());
    }

    public static void insertStats(JObject hero, String key1, string key2, Stats person)
    {
        hero[key1][key2]["hp"] = person.Hp.Value;
        hero[key1][key2]["atk"] = person.Atk.Value;
        hero[key1][key2]["spd"] = person.Spd.Value;
        hero[key1][key2]["def"] = person.Def.Value;
        hero[key1][key2]["res"] = person.Res.Value;
    }

    public static void openFolder(string path, string kageroPath, string topPath)
    {
        string baseHeroPath = kageroPath + Path.DirectorySeparatorChar + "public" + Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar + "heroes" + Path.DirectorySeparatorChar;
        string imagesBasePath = kageroPath + Path.DirectorySeparatorChar + "images";
        if (Directory.Exists(path))
        {
            foreach (string p in (new DirectoryInfo(path)).GetFiles().Select(f => f.FullName))
                openFolder(p, kageroPath, topPath);
            foreach (string p in (new DirectoryInfo(path)).GetDirectories().Select(f => f.FullName))
                openFolder(p, kageroPath, topPath);
        }
        else if (File.Exists(path) && Path.GetExtension(path).ToLower().Equals(".lz"))
        {
            byte[] data = Decompression.Open(path);
            if (data != null)
            {
                HSDARC a = new HSDARC(0, data);
                while (a.Ptr_list_length - a.NegateIndex > a.Index)
                {
                    tmp.InsertIn(a, offset, data);
                    for (int i = 0; i < tmp.Things.Length; i++)
                    {
                        if (tmp.Things[i].Roman.Value != "NONE")
                        {
                            tmp.Things[i].getCharacterStuff();
                            String heroId = tmp.Things[i].HeroName + ", " + tmp.Things[i].Epithet;
                            String heroPath = baseHeroPath + convStr(heroId) + ".json";
                            JObject hero = new JObject();
                            if (File.Exists(heroPath))
                                hero = JObject.Parse(File.ReadAllText(heroPath));
                            hero["display"] = heroId;
                            hero["name"] = tmp.Things[i].HeroName;
                            hero["title"] = tmp.Things[i].Epithet;
                            hero["id"] = convStr(heroId);
                            hero["description"] = tmp.Things[i].Description;
                            hero["voice_actor"] = tmp.Things[i].Voice;
                            hero["illustrator"] = tmp.Things[i].Illust;
                            if (tmp.Things[i].Legendary.Bonuses != null)
                                hero["legendary"] = true;
                            hero["color"] = ExtractionBase.WeaponsData[tmp.Things[i].Weapon_type.Value].Color;
                            hero["weapon_type"] = ExtractionBase.WeaponsData[tmp.Things[i].Weapon_type.Value].Name;
                            hero["link"] = "https://kagerochart.com/hero/" + convStr(heroId);
                            hero["move_type"] = ExtractionBase.Movement[tmp.Things[i].Move_type.Value];
                            exploreWep(hero, "weapon", 0, 6, tmp.Things[i]);
                            if(tmp.Things[i].HeroName.Equals("Arthur"))
                                exploreWep(hero, "assist", 2, 8, tmp.Things[i]);
                            else if(! tmp.Things[i].HeroName.Equals("Jeorge"))
                            exploreWep(hero, "assist", 1, 7, tmp.Things[i]);
                            if(tmp.Things[i].HeroName.Equals("Jeorge"))
                                exploreWep(hero, "special", 1, 7, tmp.Things[i]);
                            else if (!tmp.Things[i].HeroName.Equals("Arthur"))
                                exploreWep(hero, "special", 2, 8, tmp.Things[i]);
                            if (tmp.Things[i].HeroName.Equals("Abel"))
                                exploreSkill(hero, "passive_a", 11, tmp.Things[i]);
                            else if (tmp.Things[i].HeroName.Equals("Ogma"))
                                exploreSkill(hero, "passive_a", 10, tmp.Things[i]);
                            else if(!tmp.Things[i].HeroName.Equals("Gwendolyn"))
                                exploreSkill(hero, "passive_a", 9, tmp.Things[i]);
                            if (tmp.Things[i].HeroName.Equals("Gwendolyn"))
                                exploreSkill(hero, "passive_b", 9, tmp.Things[i]);
                            else if(!tmp.Things[i].HeroName.Equals("Ogma"))
                                exploreSkill(hero, "passive_b", 10, tmp.Things[i]);
                            if (!tmp.Things[i].HeroName.Equals("Abel"))
                                exploreSkill(hero, "passive_c", 11, tmp.Things[i]);
                            hero["base_stat"] = new JObject();
                            hero["base_stat"]["star_5"] = new JObject();
                            hero["base_stat"]["growth"] = new JObject();
                            insertStats(hero, "base_stat", "star_5", tmp.Things[i].Base_stats);
                            insertStats(hero, "base_stat", "growth", tmp.Things[i].Growth_rates);
                            hero["base_stat"]["growth"]["total"] = (tmp.Things[i].Growth_rates.Hp.Value + tmp.Things[i].Growth_rates.Atk.Value + tmp.Things[i].Growth_rates.Spd.Value + tmp.Things[i].Growth_rates.Def.Value + tmp.Things[i].Growth_rates.Res.Value);
                            hero["base_stat"]["growth_type"] = tmp.Things[i].Base_vector_id.Value;
                            hero["base_stat"]["dragonflower"] = tmp.Things[i].Dragonflowers.Value;
                            File.WriteAllText(heroPath, hero.ToString());
                            Directory.CreateDirectory(imagesBasePath);
                            string heroImgPath = topPath + Path.DirectorySeparatorChar + "Common" + Path.DirectorySeparatorChar + "Face" + Path.DirectorySeparatorChar + tmp.Things[i].Face_name.Value;
                            DirectoryCopyMethod.DirectoryCopy(heroImgPath, imagesBasePath + Path.DirectorySeparatorChar + convStr(heroId), false);

                        }
                    }
                }
            }
        }
    }
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
        return (long)data[a] + ((long)data[a + 1] << 8) + ((long)data[a + 2] << 16) + ((long)data[a + 3] << 24) + ((long)data[a + 4] << 32) + ((long)data[a + 5] << 40) + ((long)data[a + 6] << 48) + ((long)data[a + 7] << 56);
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

    public static String GetStringSize(long a, byte[] data, long size)
    {
        String Value = "";
        Byte[] tmp = new Byte[size];
        for (int i = 0; i < size; i++)
            tmp[i] = (data[a + i]);
        Value = Encoding.UTF8.GetString(tmp).Replace("\n", "\\n").Replace("\r", "\\r");
        return Value;
    }
}

public class StringXor : Xor
{
    private String value;

    public StringXor(long a, byte[] data, params byte[] A) : base(A)
    {
        Value = "";
        List<byte> tmp = new List<byte>();
        if (a != offset && a < data.Length)
        {
            for (int i = 0; a + i < data.Length && data[a + i] != 0; i++)
            {
                if (data[a + i] != XorData[i % XorData.Length])
                    tmp.Add(getDataXorred(data[a + i], i));
                else
                    tmp.Add(data[a + i]);
            }
            Value = Encoding.UTF8.GetString(tmp.ToArray()).Replace("\n", "\\n").Replace("\r", "\\r");
        }
    }

    override public String ToString()
    {
        return Value;
    }

    public String Value { get => value; set => this.value = value; }
}

class DirectoryCopyMethod
{
    public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }
}