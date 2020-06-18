using FEHDataExtractor;
using System;
using System.Collections;

//Make the program less crash-happy when things get updated
public class StringsUpdatable
{
    private String[] strs;
    public int Length;

    public StringsUpdatable(String[] strs)
    {
        this.strs = strs;
        Length = strs.Length;
    }

    public string getString(int index)
    {
        if (index >= 0 && index < strs.Length)
            return strs[index];
        else
            return "??? UNKNOWN VALUE, MUST BE UPDATED, VALUE = " + index;
    }
}

public class HSDARC
{
    public static int offset = 0x20;
    private int archive_size;
    private int ptr_list_offset;
    private int ptr_list_length;
    private int _offset1;
    private int _offset2;
    private int _unknown1;
    private byte[] data;
    private int negateIndex;
    private int index;
    private Int64 tag;
    private Int64[] ptr_list;

    public HSDARC(long a, byte[] data)
    {
        Archive_size = ExtractUtils.getInt(0, data);
        Ptr_list_offset = ExtractUtils.getInt(4, data) + offset;
        Ptr_list_length = ExtractUtils.getInt(8, data);
        Ptr_list = new long [Ptr_list_length];
        Index = 0;
        NegateIndex = 0;
        this.Data = data;
        for (int i = 0; i < Ptr_list_length; i++)
        {
            Ptr_list[i] = ExtractUtils.getLong(Ptr_list_offset + (i * 8), data) + offset;
        }
    }

    public int Archive_size { get => archive_size; set => archive_size = value; }
    public int Ptr_list_offset { get => ptr_list_offset; set => ptr_list_offset = value; }
    public int Ptr_list_length { get => ptr_list_length; set => ptr_list_length = value; }
    public int Offset1 { get => _offset1; set => _offset1 = value; }
    public int Offset2 { get => _offset2; set => _offset2 = value; }
    public int Unknown1 { get => _unknown1; set => _unknown1 = value; }
    public long Tag { get => tag; set => tag = value; }
    public long[] Ptr_list { get => ptr_list; set => ptr_list = value; }
    public byte[] Data { get => data; set => data = value; }
    public int Index { get => index; set => index = value; }
    public int NegateIndex { get => negateIndex; set => negateIndex = value; }
}

public abstract class ExtractionBase
{
    public static readonly int offset = 0x20;
    private static Hashtable table = new Hashtable();
    public static StringsUpdatable WeaponNames = new StringsUpdatable(new string[]{ "Sword", "Lance", "Axe", "Red Bow", "Blue Bow", "Green Bow", "Colourless Bow", "Red Dagger", "Blue Dagger", "Green Dagger", "Colourless Dagger", "Red Tome", "Blue Tome", "Green Tome", "Colourless Tome", "Staff", "Red Breath", "Blue Breath", "Green Breath", "Colourless Breath", "Red Beast", "Blue Beast", "Green Beast", "Colourless Beast" });
    public static SingleWeaponClass[] WeaponsData;
    public static readonly StringsUpdatable Tome_Elem = new StringsUpdatable(new string[] { "None", "Fire", "Thunder", "Wind", "Light", "Dark", "Stone" });
    public static readonly StringsUpdatable Movement = new StringsUpdatable(new string[] { "Infantry", "Armored", "Cavalry", "Flying" });
    public static readonly StringsUpdatable Series = new StringsUpdatable(new string[] { "Heroes", "Shadow Dragon and the Blade of Light / Mystery of the Emblem / Shadow Dragon / New Mystery of the Emblem", "Gaiden / Echoes", "Genealogy of the Holy War", "Thracia 776", "The Binding Blade", "The Blazing Blade", "The Sacred Stones", "Path of Radiance", "Radiant Dawn", "Awakening", "Fates", "Three Houses" });
    public static readonly StringsUpdatable BadgeColor = new StringsUpdatable(new string[] { "Scarlet", "Azure", "Verdant", "Trasparent" });
    public static readonly StringsUpdatable ShardColor = new StringsUpdatable(new string[] { "Universal", "Scarlet", "Azure", "Verdant", "Trasparent" });
    public static readonly StringsUpdatable SkillCategory = new StringsUpdatable(new string[] { "Weapon", "Assist", "Special", "Passive A", "Passive B", "Passive C", "Sacred Seal", "Refined Weapon Skill Effect", "Beast Effect" });
    public static readonly StringsUpdatable Ranks = new StringsUpdatable(new string[] { "C", "B", "A", "S" });
    public static readonly StringsUpdatable LegendaryElement = new StringsUpdatable(new string[] { "Fire", "Water", "Wind", "Earth", "Light", "Dark", "Astra", "Anima" });
    public static readonly StringsUpdatable Colours = new StringsUpdatable(new string[] { "Red", "Blue", "Green", "Colorless" });
    private byte[] elemXor;
    private int size = 0;


    private HSDARC archive;

    public abstract void InsertIn(long a, byte[] data);
    public void InsertIn(HSDARC archive, long a, byte[] data) {
        Archive = archive;
        InsertIn(a, data);
    }

    public string getHeroName(string str)
    {
        return (Table.Contains("M" + str) ? (Table["M" + str] + (Table.Contains("M" + str.Insert(3, "_HONOR")) ? " - " + Table["M" + str.Insert(3, "_HONOR")] : "")) : str);
    }

    public string getStuff(StringXor Str, string otherstring)
    {
        return Table.Contains("M" + Str.Value) ? otherstring + Table["M" + Str.Value] + Environment.NewLine : "";
    }

    public string getStuffExclusive(StringXor Str, string otherstring)
    {
        return Table.Contains("M" + Str.Value) ? otherstring + Table["M" + Str.Value] + Environment.NewLine : otherstring + Str + Environment.NewLine;
    }

    public string Name { get; set; }
    public HSDARC Archive { get => archive; set => archive = value; }
    public int Size { get => size; set => size = value; }
    public byte[] ElemXor { get => elemXor; set => elemXor = value; }
    public static Hashtable Table { get => table; set => table = value; }
}

public abstract class LoginRelated: ExtractionBase
{
    public static readonly byte[] Login = { 0xCD, 0x76, 0x95, 0x7D, 0x7A, 0xF5, 0xB3, 0x0B, 0xE6, 0xC9, 0x39, 0x72, 0xFC, 0x9E, 0xE2, 0x73, 0x3D, 0x31, 0x98, 0x23, 0xC0, 0x28, 0x2F, 0xA0, 0xE6, 0x5E, 0xB3, 0x9C, 0x6C, 0x27, 0xA9, 0xCB, 0xB7, 0x26, 0x68, 0x64 };
}

public abstract class GCRelated : ExtractionBase
{
    public static readonly byte[] GC = { 0x17, 0xFC, 0xC9, 0xEA, 0x79, 0x69, 0x24, 0xBD, 0xA4, 0x54, 0x0E, 0x58, 0xBD, 0x8B, 0x36, 0xCD, 0xAF, 0xB4, 0xE2, 0x09, 0x3C, 0x1F, 0x8C, 0x9C, 0xD1, 0x48, 0x51, 0xA1, 0xFB, 0xAD, 0x48, 0x7E, 0xC3, 0x38, 0x5A, 0x41 };
}

public abstract class FBRelated : ExtractionBase
{
    public static readonly byte[] FB = { 0x2F, 0x08, 0x66, 0xED, 0x7C, 0x98, 0x34, 0x2A, 0xE4, 0xAC, 0x41, 0xD1, 0xE5, 0x1F, 0xD2, 0x5E, 0x28, 0x32, 0x76, 0xDE, 0x87, 0x0A, 0xA7, 0xF9, 0x44, 0x28, 0x26, 0xC7, 0x25, 0x21, 0x06, 0x68, 0xE3, 0x72, 0x96, 0x3A, 0x24, 0xEA, 0xA2, 0x4F, 0xDF, 0xEB, 0x11, 0xDC, 0x50, 0x26, 0x3C, 0x78, 0xD0, 0x89, 0x04, 0xA9, 0xF7, 0x4A, 0x26, 0x28, 0xC9, 0x2B};
}

public abstract class CommonRelated : ExtractionBase
{
    public static readonly byte[] Common = { 0x81, 0x00, 0x80, 0xA4, 0x5A, 0x16, 0x6F, 0x78, 0x57, 0x81, 0x2D, 0xF7, 0xFC, 0x66, 0x0F, 0x27, 0x75, 0x35, 0xB4, 0x34, 0x10, 0xEE, 0xA2, 0xDB, 0xCC, 0xE3, 0x35, 0x99, 0x43, 0x48, 0xD2, 0xBB, 0x93, 0xC1 };
}

public abstract class CharacterRelated: CommonRelated
{
    StringXor id_tag;
    StringXor roman;
    StringXor face_name;
    StringXor face_name2;
    Int64Xor timestamp;
    UInt32Xor id_num;
    ByteXor weapon_type;
    ByteXor tome_class;
    ByteXor move_type;
    Stats base_stats;
    Stats growth_rates;

    public string getCharacterStuff()
    {
        string text = "";
        if (Table.Contains("M" + Id_tag.ToString()))
        {
            text += "Name: " + Table["M" + Id_tag.ToString()] + Environment.NewLine;
            text += Table.Contains("M" + Id_tag.Value.Insert(3, "_HONOR")) ? "Epithet: " + Table["M" + Id_tag.Value.Insert(3, "_HONOR")] + Environment.NewLine : "";
            text += Table.Contains("M" + Id_tag.Value.Insert(3, "_H")) ? "Description: " + Table["M" + Id_tag.Value.Insert(3, "_H")].ToString().Replace("\\n", " ").Replace("\\r", " ") + Environment.NewLine : "";
            text += Table.Contains("M" + Id_tag.Value.Insert(3, "_VOICE")) ? "Voice Actor: " + Table["M" + Id_tag.Value.Insert(3, "_VOICE")] + Environment.NewLine : "";
            text += Table.Contains("M" + Id_tag.Value.Insert(3, "_ILLUST")) ? "Illustrator: " + Table["M" + Id_tag.Value.Insert(3, "_ILLUST")] + Environment.NewLine : "";
        }
        return text;
    }

    private string SuperStats(Stats Base_stats, Stats Growth_rates, Stats lvl40, bool greater, string phrase)
    {
        string text = "";
        int growthInc = 5;
        if (!greater)
            growthInc = -5;
        Stats newRates = new Stats();
        newRates.Hp.Value = (short)(Growth_rates.Hp.Value + growthInc);
        newRates.Atk.Value = (short)(Growth_rates.Atk.Value + growthInc);
        newRates.Spd.Value = (short)(Growth_rates.Spd.Value + growthInc);
        newRates.Def.Value = (short)(Growth_rates.Def.Value + growthInc);
        newRates.Res.Value = (short)(Growth_rates.Res.Value + growthInc);
        int i = 1;
        if (!greater)
            i = -1;
        int val = 4;
        Stats newBaseStats = new Stats();
        newBaseStats.Hp.Value = (short)(Base_stats.Hp.Value + i);
        newBaseStats.Atk.Value = (short)(Base_stats.Atk.Value + i);
        newBaseStats.Spd.Value = (short)(Base_stats.Spd.Value + i);
        newBaseStats.Def.Value = (short)(Base_stats.Def.Value + i);
        newBaseStats.Res.Value = (short)(Base_stats.Res.Value + i);
        Stats tmpStats = new Stats(newBaseStats, newRates);
        int len = 0;
        if ((greater && tmpStats.Hp.Value - val >= lvl40.Hp.Value) || (! greater && tmpStats.Hp.Value + val <= lvl40.Hp.Value))
        {
            len++;
        }
        if ((greater && tmpStats.Atk.Value - val >= lvl40.Atk.Value) || (!greater && tmpStats.Atk.Value + val <= lvl40.Atk.Value))
        {
            len++;
        }
        if ((greater && tmpStats.Spd.Value - val >= lvl40.Spd.Value) || (!greater && tmpStats.Spd.Value + val <= lvl40.Spd.Value))
        {
            len++;
        }
        if ((greater && tmpStats.Def.Value - val >= lvl40.Def.Value) || (!greater && tmpStats.Def.Value + val <= lvl40.Def.Value))
        {
            len++;
        }
        if ((greater && tmpStats.Res.Value - val >= lvl40.Res.Value) || (!greater && tmpStats.Res.Value + val <= lvl40.Res.Value))
        {
            len++;
        }
        text += len > 0 ? len > 1 ? phrase + "s: " : phrase + ": " : "";

        if ((greater && tmpStats.Hp.Value - val >= lvl40.Hp.Value) || (!greater && tmpStats.Hp.Value + val <= lvl40.Hp.Value))
        {
            text += "Hp";
            len--;
            if (len > 0)
                text += ", ";
        }
        if ((greater && tmpStats.Atk.Value - val >= lvl40.Atk.Value) || (!greater && tmpStats.Atk.Value + val <= lvl40.Atk.Value))
        {
            text += "Atk";
            len--;
            if (len > 0)
                text += ", ";
        }
        if ((greater && tmpStats.Spd.Value - val >= lvl40.Spd.Value) || (!greater && tmpStats.Spd.Value + val <= lvl40.Spd.Value))
        {
            text += "Spd";
            len--;
            if (len > 0)
                text += ", ";
        }
        if ((greater && tmpStats.Def.Value - val >= lvl40.Def.Value) || (!greater && tmpStats.Def.Value + val <= lvl40.Def.Value))
        {
            text += "Def";
            len--;
            if (len > 0)
                text += ", ";
        }
        if ((greater && tmpStats.Res.Value - val >= lvl40.Res.Value) || (!greater && tmpStats.Res.Value + val <= lvl40.Res.Value))
        {
            text += "Res";
            len--;
            if (len > 0)
                text += ", ";
        }

        return text;

    }

    public string SuperBoonBane(Stats Base_stats, Stats Growth_rates, Stats lvl40)
    {
        return (SuperStats(Base_stats, Growth_rates, lvl40, true, "Superboon").Equals("") ? "" : SuperStats(Base_stats, Growth_rates, lvl40, true, "Superboon") + Environment.NewLine) + (SuperStats(Base_stats, Growth_rates, lvl40, false, "Superbane").Equals("") ? "" : SuperStats(Base_stats, Growth_rates, lvl40, false, "Superbane") + Environment.NewLine);
    }

    public CharacterRelated()
    {
        Size = 79;
        Timestamp = new Int64Xor(0x9B, 0x48, 0xB6, 0xE9, 0x42, 0xE7, 0xC1, 0xBD);
    }

    public StringXor Id_tag { get => id_tag; set => id_tag = value; }
    public StringXor Roman { get => roman; set => roman = value; }
    public StringXor Face_name { get => face_name; set => face_name = value; }
    public StringXor Face_name2 { get => face_name2; set => face_name2 = value; }
    public Int64Xor Timestamp { get => timestamp; set => timestamp = value; }
    public UInt32Xor Id_num { get => id_num; set => id_num = value; }
    public ByteXor Weapon_type { get => weapon_type; set => weapon_type = value; }
    public ByteXor Tome_class { get => tome_class; set => tome_class = value; }
    public ByteXor Move_type { get => move_type; set => move_type = value; }
    public Stats Base_stats { get => base_stats; set => base_stats = value; }
    public Stats Growth_rates { get => growth_rates; set => growth_rates = value; }
}

public class Stats : ExtractionBase
{
    private Int16Xor hp, atk, spd, def, res, unknown1, unknown2, unknown3;

    public Stats()
    {
        Size = 16;
        Name = "";
        Hp = new Int16Xor(0x32, 0xD6);
        Atk = new Int16Xor(0xA0, 0x14);
        Spd = new Int16Xor(0x5E, 0xA5);
        Def = new Int16Xor(0x66, 0x85);
        Res = new Int16Xor(0xE5, 0xAE);
        Unknown1 = new Int16Xor(0);
        Unknown2 = new Int16Xor(0);
        Unknown3 = new Int16Xor(0);
    }

    public Stats(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public Stats(Stats Level1, Stats Growths):this()
    {
        Hp.Value = (short)(Level1.Hp.Value + (int)(0.39f * ((int)(Growths.Hp.Value * 1.14f + 0.000005f))));
        Atk.Value = (short)(Level1.Atk.Value + (int)(0.39f * ((int)(Growths.Atk.Value * 1.14f + 0.000005f))));
        Spd.Value = (short)(Level1.Spd.Value + (int)(0.39f * ((int)(Growths.Spd.Value * 1.14f + 0.000005f))));
        Def.Value = (short)(Level1.Def.Value + (int)(0.39f * ((int)(Growths.Def.Value * 1.14f + 0.000005f))));
        Res.Value = (short)(Level1.Res.Value + (int)(0.39f * ((int)(Growths.Res.Value * 1.14f + 0.000005f))));
    }

    override public String ToString()
    {
        String text = Hp + "|" + Atk + "|" + Spd + "|" + Def + "|" + Res + Environment.NewLine;

        return text;
    }

    public override void InsertIn(long a, byte[] data)
    {
        Hp.XorValue(ExtractUtils.getShort(a, data));
        Atk.XorValue(ExtractUtils.getShort(a + 2, data));
        Spd.XorValue(ExtractUtils.getShort(a + 4, data));
        Def.XorValue(ExtractUtils.getShort(a + 6, data));
        Res.XorValue(ExtractUtils.getShort(a + 8, data));
        Unknown1.XorValue(ExtractUtils.getShort(a + 0xA, data));
        Unknown2.XorValue(ExtractUtils.getShort(a + 0xC, data));
        Unknown3.XorValue(ExtractUtils.getShort(a + 0xE, data));
    }

    public void IncrementAll()
    {
        Hp.Value++;
        Atk.Value++;
        Spd.Value++;
        Def.Value++;
        Res.Value++;
    }

    public Int16Xor Hp { get => hp; set => hp = value; }
    public Int16Xor Atk { get => atk; set => atk = value; }
    public Int16Xor Spd { get => spd; set => spd = value; }
    public Int16Xor Def { get => def; set => def = value; }
    public Int16Xor Res { get => res; set => res = value; }
    public Int16Xor Unknown1 { get => unknown1; set => unknown1 = value; }
    public Int16Xor Unknown2 { get => unknown2; set => unknown2 = value; }
    public Int16Xor Unknown3 { get => unknown3; set => unknown3 = value; }
}

public class Legendary : CommonRelated
{
    public static readonly StringsUpdatable LegKind = new StringsUpdatable(new string[] { "Legendary/Mythic", "Duo" });
    private StringXor duo_skill_id;
    private Stats bonuses;
    private ByteXor kind;
    private ByteXor element;
    private ByteXor bst;
    private ByteXor is_duel;

    public Legendary()
    {
        Size = 17; //It's a pointer, so who cares about size
        Name = "";
        Kind = new ByteXor(0x21);
        Element = new ByteXor(5);
        Bst = new ByteXor(0x0F);
        Is_duel = new ByteXor(0x80);
    }

    public Legendary(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    override public String ToString()
    {
        String text = "";
        text += Duo_skill_id.Value != "" ? getStuffExclusive(Duo_skill_id, "Duo Skill: ") : "";
        text += "Kind: " + LegKind.getString(Kind.Value - 1) + Environment.NewLine;
        text += "Bonus Stats: " + Bonuses;
        text += Element.Value != 0 ? "Element: " + LegendaryElement.getString(Element.Value - 1) + Environment.NewLine : "";
        text += Bst.Value != 0 ? "Arena BST: " + Bst.Value + Environment.NewLine : "";
        text += Is_duel.Value != 0 ? "Duel Hero" + Environment.NewLine : "";

        return text;
    }

    public override void InsertIn(long a, byte[] data)
    {
        if (a != offset)
        {
            Duo_skill_id = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
            Bonuses = new Stats(a + 8, data);
            Kind.XorValue(data[a + 0x18]);
            Element.XorValue(data[a + 0x19]);
            Bst.XorValue(data[a + 0x1A]);
            Is_duel.XorValue(data[a + 0x1B]);
        }
    }

    public Stats Bonuses { get => bonuses; set => bonuses = value; }
    public ByteXor Element { get => element; set => element = value; }
    public StringXor Duo_skill_id { get => duo_skill_id; set => duo_skill_id = value; }
    public ByteXor Bst { get => bst; set => bst = value; }
    public ByteXor Is_duel { get => is_duel; set => is_duel = value; }
    public ByteXor Kind { get => kind; set => kind = value; }
}

public class Dragonflowers : CommonRelated
{
    private Int32Xor dflowers;
    
    public Int32Xor Dflowers { get => dflowers; set => dflowers = value; }

    public Dragonflowers()
    {
        Size = 4; //It's a pointer, so who cares about size
        Name = "";
        Dflowers = new Int32Xor(0x74, 0x37, 0x01, 0xA0);
    }

    public Dragonflowers(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    override public String ToString()
    {
        String text = "";
        text += "Max Dragonflowers: " + Dflowers.Value + Environment.NewLine;
        return text;
    }

    public override void InsertIn(long a, byte[] data)
    {
        if (a != offset)
        {
            Dflowers.XorValue((ExtractUtils.getInt(a, data)));
        }
    }
}

public class SingleEnemy : CharacterRelated
{
    StringXor topWeapon;
    ByteXor _spawnable_Enemy;
    ByteXor is_boss;

    public ByteXor Is_boss { get => is_boss; set => is_boss = value; }
    public ByteXor Spawnable_Enemy { get => _spawnable_Enemy; set => _spawnable_Enemy = value; }
    public StringXor TopWeapon { get => topWeapon; set => topWeapon = value; }

    public SingleEnemy() : base()
    {
        Name = "Enemies";
        ElemXor = new byte[] { 0x5C, 0x34, 0xC5, 0x9C, 0x11, 0x95, 0xCA, 0x62 };
        Size += 17; //10 of the stuff + 7 of padding
        Id_num = new UInt32Xor(0xD4, 0x41, 0x2F, 0x42);
        Weapon_type = new ByteXor(0xE4);
        Tome_class = new ByteXor(0x81);
        Move_type = new ByteXor(0x0D);
        Spawnable_Enemy = new ByteXor(0xC5);
        Is_boss = new ByteXor(0x6A);
    }

    public SingleEnemy(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {
        Id_tag = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Archive.Index++;
        Roman = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
        Archive.Index++;
        if (Roman.Value.Equals("NONE"))
        {
            return;
        }
        Face_name = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, Common);
        Archive.Index++;
        a += 24;
        Face_name2 = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Archive.Index++;
        TopWeapon = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
        if (!TopWeapon.Value.Equals(""))
            Archive.Index++;
        Timestamp.XorValue((ExtractUtils.getLong(a + 16, data)));
        Id_num.XorValue((ExtractUtils.getInt(a + 24, data)));
        Weapon_type.XorValue(data[a + 28]);
        Tome_class.XorValue(data[a + 29]);
        Move_type.XorValue(data[a + 30]);
        Spawnable_Enemy.XorValue(data[a + 31]);
        Is_boss.XorValue(data[a + 32]);
        Base_stats = new Stats(a + 40, data);
        Base_stats.IncrementAll();
        Growth_rates = new Stats(a + 56, data);
    }


    public override string ToString()
    {
        String text = "";
        text += getCharacterStuff();
        text += "Internal Identifier: " + Id_tag + Environment.NewLine;
        text += "Romanized Identifier: " + Roman + Environment.NewLine;
        if (Roman.Value.Equals("NONE"))
        {
            return text + "-----------------------------------------------------------------------------------------------" + Environment.NewLine;
        }
        text += "Face Folder: " + Face_name + Environment.NewLine;
        text += "Face Folder no. 2: " + Face_name2 + Environment.NewLine;
        if (!TopWeapon.Value.Equals(""))
            text += getStuffExclusive(TopWeapon, "Default Weapon: ");
        text += "Timestamp: ";
        text += Timestamp.Value < 0 ? "Not available" + Environment.NewLine : DateTimeOffset.FromUnixTimeSeconds(Timestamp.Value).DateTime.ToLocalTime() + Environment.NewLine;
        text += "ID: " + Id_num + Environment.NewLine;
        text += "Weapon: " + WeaponNames.getString(Weapon_type.Value) + Environment.NewLine;
        text += "Tome Element: " + Tome_Elem.getString(Tome_class.Value) + Environment.NewLine;
        text += "Movement Type: " + Movement.getString(Move_type.Value) + Environment.NewLine;
        text += Spawnable_Enemy.Value == 0 ? "Randomly spawnable enemy" + Environment.NewLine : "Not randomly spawnable enemy" + Environment.NewLine;
        text += Is_boss.Value == 0 ? "Normal enemy" + Environment.NewLine : "Special enemy" + Environment.NewLine;
        text += "5 Stars Level 1 Stats: " + Base_stats;
        Stats tmp = new Stats(Base_stats, Growth_rates);
        text += "5 Stars Level 40 Stats: " + tmp;
        text += "Growth Rates: " + Growth_rates;
        text += "BST: " + (tmp.Hp.Value + tmp.Atk.Value + tmp.Spd.Value + tmp.Def.Value + tmp.Res.Value) + Environment.NewLine;
        text += SuperBoonBane(Base_stats, Growth_rates, tmp);
        text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;

        return text;
    }

}

public class Decompress : CharacterRelated
{

    public Decompress() : base()
    {
        Name = "Decompress";
    }

    public Decompress(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {

    }

}

public class SinglePerson : CharacterRelated
{
    public static readonly String[] PrintSkills = {"Default Weapon: ", "Default Assist: ", "Default Special: ", "Unknown: ", "Unknown: ", "Unknown: ", "Unlocked Weapon: ", "Unlocked Assist: ", "Unlocked Special: ", "Passive A: ", "Passive B: ", "Passive C: ", "Unknown: ", "Unknown: "};

    Legendary legendary;
    Dragonflowers dflowers;
    UInt32Xor sort_value;
    UInt32Xor origin;
    ByteXor series;
    ByteXor regular_hero;
    ByteXor permanent_hero;
    ByteXor base_vector_id;
    ByteXor refresher;
    ByteXor _unknown2;
    // 6 bytes of padding
//    Stats max_stats;
    StringXor[,] skills;

    public SinglePerson():base()
    {
        Name = "Heroes";
        ElemXor = new byte[] { 0xE1, 0xB9, 0x3A, 0x3C, 0x79, 0xAB, 0x51, 0xDE };
        Size += 33 + (5 * 8 * PrintSkills.Length);
        Id_num = new UInt32Xor(0x18, 0x4E, 0x6E, 0x5F);
        Sort_value = new UInt32Xor(0x9B, 0x34, 0x80, 0x2A);
        Origin = new UInt32Xor(0x08, 0xB8, 0x64, 0xE6);
        Weapon_type = new ByteXor(6);
        Tome_class = new ByteXor(0x35);
        Move_type = new ByteXor(0x2A);
        Series1 = new ByteXor(0x43);
        Regular_hero = new ByteXor(0xA1);
        Permanent_hero = new ByteXor(0xC7);
        Base_vector_id = new ByteXor(0x3D);
        Refresher = new ByteXor(0xFF);
        Unknown2 = new ByteXor(0);
        Skills = new StringXor[5, PrintSkills.Length];
    }

    public SinglePerson(long a, byte[] data) : this() {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {
        Id_tag = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Archive.Index++;
        Roman = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
        Archive.Index++;
        if (Roman.Value.Equals("NONE"))
        {
            return;
        }
        Face_name = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, Common);
        Archive.Index++;
        a += 24;
        Face_name2 = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Archive.Index++;
        Legendary = new Legendary(ExtractUtils.getLong(a + 8, data) + offset, data);
        if (Legendary.Bonuses != null)
            Archive.Index++;
        Dflowers = new Dragonflowers(ExtractUtils.getLong(a + 16, data) + offset, data);
        Archive.Index++;
        a += 8;
        Timestamp.XorValue((ExtractUtils.getLong(a + 16, data)));
        Id_num.XorValue((ExtractUtils.getInt(a + 24, data)));
        Sort_value.XorValue((ExtractUtils.getInt(a + 28, data)));
        Origin.XorValue((ExtractUtils.getInt(a + 32, data)));
        Weapon_type.XorValue(data[a + 36]);
        Tome_class.XorValue(data[a + 37]);
        Move_type.XorValue(data[a + 38]);
        Series1.XorValue(data[a + 39]);
        Regular_hero.XorValue(data[a + 40]);
        Permanent_hero.XorValue(data[a + 41]);
        Base_vector_id.XorValue(data[a + 42]);
        Refresher.XorValue(data[a + 43]);
        Unknown2.XorValue(data[a + 44]);
        Base_stats = new Stats(a + 48, data);
        Base_stats.IncrementAll();
        Growth_rates = new Stats(a + 64, data);
//        Max_stats = new Stats(a + 80, data);
        for (int i = 0; i < Skills.Length / PrintSkills.Length; i++)
            for (int j = 0; j < PrintSkills.Length; j++)
            {
                Skills[i, j] = new StringXor(ExtractUtils.getLong((j * 8) + (i * PrintSkills.Length * 8) + a + 80, data) + offset, data, Common);
                if (!Skills[i, j].ToString().Equals(""))
                {
                    Archive.Index++;
                }
            }
    }

    public override string ToString()
    {
        String text = "";
        text += getCharacterStuff();
        text += "Internal Identifier: " + Id_tag + Environment.NewLine;
        text += "Romanized Identifier: " + Roman + Environment.NewLine;
        if (Roman.Value.Equals("NONE"))
        {
            return text + "-----------------------------------------------------------------------------------------------" + Environment.NewLine;
        }
        text += "Face Folder: " + Face_name + Environment.NewLine;
        text += "Face Folder no. 2: " + Face_name2 + Environment.NewLine;
        if (Legendary.Bonuses != null)
            text += Legendary;
        text += "Timestamp: ";
        text += Timestamp.Value < 0 ? "Not available" + Environment.NewLine : DateTimeOffset.FromUnixTimeSeconds(Timestamp.Value).DateTime.ToLocalTime() + Environment.NewLine;
        text += "ID: " + Id_num + Environment.NewLine;
        text += "Sort Value: " + Sort_value + Environment.NewLine;
        text += "Origin: " + Origin.Value + Environment.NewLine;
        text += "Weapon: " + WeaponNames.getString(Weapon_type.Value) + Environment.NewLine;
        text += "Tome Element: " + Tome_Elem.getString(Tome_class.Value) + Environment.NewLine;
        text += "Movement Type: " + Movement.getString(Move_type.Value) + Environment.NewLine;
        text += "Series: " + Series.getString(Series1.Value) + Environment.NewLine;
        text += Regular_hero.Value == 0 ? "Not randomly spawnable hero" + Environment.NewLine : "Randomly spawnable hero" + Environment.NewLine;
        text += Permanent_hero.Value == 0 ? "Can be sent home and merged" + Environment.NewLine : "Cannot be sent home or merged" + Environment.NewLine;
        text += "BVID: " + Base_vector_id + Environment.NewLine;
        text += Refresher.Value == 0 ? "Cannot learn Sing/Dance" + Environment.NewLine : "Can learn Sing/Dance" + Environment.NewLine;
        text += Dflowers;
        text += "5 Stars Level 1 Stats: " + Base_stats;
        Stats tmp = new Stats(Base_stats, Growth_rates);
        text += "5 Stars Level 40 Stats: " + tmp;
        text += "Growth Rates: " + Growth_rates;
        text += "BST: " + (tmp.Hp.Value + tmp.Atk.Value + tmp.Spd.Value + tmp.Def.Value + tmp.Res.Value) + Environment.NewLine;
        text += SuperBoonBane(Base_stats, Growth_rates, tmp);
        text += "Growth Rates Total: " + (Growth_rates.Hp.Value + Growth_rates.Atk.Value + Growth_rates.Spd.Value + Growth_rates.Def.Value + Growth_rates.Res.Value) + Environment.NewLine;
//        text += "Enemy Stats: " + Max_stats;
        for (int i = 0; i < Skills.Length / PrintSkills.Length; i++)
        {
            text += (i + 1) + " Star";
            text += i == 0 ? "" : "s";
            text += " Rarity Skills -------------------------------------------------------------------------" + Environment.NewLine;
            for (int j = 0; j < PrintSkills.Length; j++)
                text += getStuffExclusive(Skills[i, j], PrintSkills[j]);
            if(i == (Skills.Length / PrintSkills.Length) -1)
                text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        return text;
    }

    public Legendary Legendary { get => legendary; set => legendary = value; }
    public UInt32Xor Sort_value { get => sort_value; set => sort_value = value; }
    public ByteXor Series1 { get => series; set => series = value; }
    public ByteXor Regular_hero { get => regular_hero; set => regular_hero = value; }
    public ByteXor Permanent_hero { get => permanent_hero; set => permanent_hero = value; }
    public ByteXor Base_vector_id { get => base_vector_id; set => base_vector_id = value; }
    public ByteXor Refresher { get => refresher; set => refresher = value; }
    public ByteXor Unknown2 { get => _unknown2; set => _unknown2 = value; }
//    public Stats Max_stats { get => max_stats; set => max_stats = value; }
    public StringXor[,] Skills { get => skills; set => skills = value; }
    public UInt32Xor Origin { get => origin; set => origin = value; }
    public Dragonflowers Dflowers { get => dflowers; set => dflowers = value; }
}

public class GCArea:GCRelated
{
    private UInt32Xor node, x, y;
    private ByteXor isBase;
    private ByteXor army;
    private UInt16Xor area_No;
    private StringXor[] areaBonuses;
    private StringXor adjacentAreaBonus, mapId;
    private UInt32Xor neighboursCount;
    private UInt32Xor[] neighbours;

    private GCArea()
    {
        Name = "";
        Node = new UInt32Xor(0, 0x4F, 0x9B, 0x4B);
        X = new UInt32Xor(0x3A, 0x6F, 0x42, 0x5E);
        Y = new UInt32Xor(0x30, 0xDB, 0x1E, 0x0F);
        IsBase = new ByteXor(0x10);
        Army = new ByteXor(1);
        Area_No = new UInt16Xor(0x83, 0xF7);
        AreaBonuses = new StringXor[2];
        NeighboursCount = new UInt32Xor(0xB0, 0xB6, 0x5D, 0x6E);
    }

    public GCArea(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public GCArea(long a, byte[] data, HSDARC archive) : this()
    {
        Archive = archive;
        InsertIn(a, data);
    }

    override public String ToString()
    {
        String text = "ID: " + Node + Environment.NewLine;
        text += "X: " + X + Environment.NewLine;
        text += "Y: " + Y + Environment.NewLine;
        text += IsBase.Value == 1 ? "A team base" + Environment.NewLine : "Not a team base" + Environment.NewLine;
        text += Army.Value == 0 ? "Red" + Environment.NewLine : Army.Value == 1 ? "Blue" + Environment.NewLine : "Green" + Environment.NewLine;
        text += "Area Number: " + Area_No + Environment.NewLine;
        text += "Bonus 1: " + AreaBonuses[0] + Environment.NewLine;
        text += "Bonus 2: " + AreaBonuses[1] + Environment.NewLine;
        text += "Bonus to Adjacent Areas: " + AdjacentAreaBonus + Environment.NewLine;
        text += "Map ID: " + MapId + Environment.NewLine;
        for (int i = 0; i < Neighbours.Length; i++)
        {
            text += "Neighbour " + (i + 1).ToString() + ": " + Neighbours[i] + Environment.NewLine;
        }

        return text;
    }

    public override void InsertIn(long a, byte[] data)
    {
        Node.XorValue(((ExtractUtils.getInt(a, data))));
        X.XorValue((ExtractUtils.getInt(a + 4, data)));
        Y.XorValue((ExtractUtils.getInt(a + 8, data)));
        IsBase.XorValue(data[a + 12]);
        Army.XorValue(data[a + 13]);
        Area_No.XorValue((ExtractUtils.getShort(a + 14, data)));
        AreaBonuses[0] = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, GC);
        AreaBonuses[1] = new StringXor(ExtractUtils.getLong(a + 24, data) + offset, data, GC);
        AdjacentAreaBonus = new StringXor(ExtractUtils.getLong(a + 32, data) + offset, data, GC);
        MapId = new StringXor(ExtractUtils.getLong(a + 40, data) + offset, data, GC);
        if (!AreaBonuses[0].ToString().Equals(""))
            Archive.Index++;
        if (!AreaBonuses[1].ToString().Equals(""))
            Archive.Index++;
        if (!AdjacentAreaBonus.ToString().Equals(""))
            Archive.Index++;
        if (!MapId.ToString().Equals(""))
            Archive.Index++;
        NeighboursCount.XorValue((ExtractUtils.getInt(a + 48, data)));
        Neighbours = new UInt32Xor[NeighboursCount.Value];
        for (int i = 0; i < NeighboursCount.Value; i++)
        {
            Neighbours[i] = new UInt32Xor(0x5E, 0x6E, 0x8E, 9);
            Neighbours[i].XorValue((ExtractUtils.getInt(a + 52 + (i * 4), data)));
        }
    }

    public UInt32Xor Node { get => node; set => node = value; }
    public UInt32Xor X { get => x; set => x = value; }
    public UInt32Xor Y { get => y; set => y = value; }
    public ByteXor IsBase { get => isBase; set => isBase = value; }
    public ByteXor Army { get => army; set => army = value; }
    public UInt16Xor Area_No { get => area_No; set => area_No = value; }
    public StringXor[] AreaBonuses { get => areaBonuses; set => areaBonuses = value; }
    public StringXor AdjacentAreaBonus { get => adjacentAreaBonus; set => adjacentAreaBonus = value; }
    public StringXor MapId { get => mapId; set => mapId = value; }
    public UInt32Xor NeighboursCount { get => neighboursCount; set => neighboursCount = value; }
    public UInt32Xor[] Neighbours { get => neighbours; set => neighbours = value; }
}

public class GCWorld : GCRelated
{
    private StringXor image;
    private UInt64Xor unknown1, areaCount;
    private GCArea[] areas;

    public GCWorld()
    {
        Name = "GC World";
        unknown1 = new UInt64Xor(0);
        areaCount = new UInt64Xor(0x4B, 0x88, 0x65, 0x71, 0x3B, 0x36, 0xF5, 0xEF);
    }

    public GCWorld(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {
        Image = new StringXor(ExtractUtils.getLong(a, data) + offset, data, GC);
        Archive.Index++;
        Unknown1.XorValue((ExtractUtils.getLong(a + 8, data)));
        AreaCount.XorValue((ExtractUtils.getLong(a + 16, data)));
        Areas = new GCArea[AreaCount.Value];
        long pos = ExtractUtils.getLong(a + 24, data) + offset;
        Archive.Index++;
        for (long i = 0; i < Areas.Length; i++)
        {
            Areas[i] = new GCArea(ExtractUtils.getLong(pos + (i * 8), data) + offset, data, Archive);
            Archive.Index++;
        }
    }

    public override String ToString()
    {
        String text = "Image: " + Image + Environment.NewLine;
        text += "Unknown 1: " + Unknown1 + Environment.NewLine + Environment.NewLine;
        for (int i = 0; i < Areas.Length; i++)
        {
            text += "Area " + i + ": " + Environment.NewLine + Areas[i] + Environment.NewLine;
        }

        return text;
    }

    public StringXor Image { get => image; set => image = value; }
    public UInt64Xor Unknown1 { get => unknown1; set => unknown1 = value; }
    public UInt64Xor AreaCount { get => areaCount; set => areaCount = value; }
    public GCArea[] Areas { get => areas; set => areas = value; }
}

public class SingleSkill : CommonRelated
{
    StringXor id_tag;
    StringXor refine_base;
    StringXor name_id;
    StringXor desc_id;
    StringXor refine_id;
    StringXor beast_effect_id;
    StringXor[] prerequisites; //2 Elements!
    StringXor next_skill;
    StringXor[] sprites; //No Xor! 4 Elements!
    Stats statistics;
    Stats class_params;
    Stats skill_params;
    Stats skill_params2;
    Stats refine_stats;
    UInt32Xor num_id;
    UInt32Xor sort_id;
    UInt32Xor icon_id;
    UInt32Xor wep_equip;
    UInt32Xor mov_equip;
    UInt32Xor sp_cost;
    ByteXor category;
    ByteXor tome_class;
    ByteXor exclusive;
    ByteXor enemy_only;
    ByteXor range;
    ByteXor might;
    SByteXor cooldown_count;
    ByteXor assist_cd;
    ByteXor healing;
    ByteXor skill_range;
    UInt16Xor score;
    SByteXor promotion_tier;
    SByteXor promotion_rarity;
    ByteXor refined;
    ByteXor refine_sort_id;
    UInt32Xor wep_effective;
    UInt32Xor mov_effective;
    UInt32Xor wep_shield;
    UInt32Xor mov_shield;
    UInt32Xor wep_weakness;
    UInt32Xor mov_weakness;
    UInt32Xor wep_adaptive;
    UInt32Xor mov_adaptive;
    UInt32Xor timing_id;
    UInt32Xor ability_id;
    UInt32Xor limit1_id;
    Int16Xor[] limit1_params;         //2 elements!
    UInt32Xor limit2_id;
    Int16Xor[] limit2_params;         //2 elements!
    UInt32Xor target_wep;
    UInt32Xor target_mov;
    StringXor passive_next;
    Int64Xor timestamp;
    ByteXor random_allowed;
    ByteXor min_lv;
    ByteXor max_lv;
    ByteXor tt_inherit_base;
    ByteXor random_mode;
    ByteXor range_shape;
    ByteXor target_either;
    // 3 bytes of padding
    //    StringXor id_tag2;
    //    StringXor next_seal;
    //    StringXor prev_seal;
    //    UInt16Xor ss_coin;
    //    UInt16Xor ss_badge_type;
    //    UInt16Xor ss_badge;
    //    UInt16Xor ss_great_badge;

    public SingleSkill()
    {
        Name = "Skills";
        ElemXor = new byte[] { 0xAD, 0xE9, 0xDE, 0x4A, 0x07, 0xC7, 0xEC, 0x7F };
        Size = 320;
        Prerequisites = new StringXor[2];
        Sprites = new StringXor[4];
        Num_id = new UInt32Xor(0x23, 0x3A, 0xA5, 0xC6);
        Sort_id = new UInt32Xor(0xAC, 0xF8, 0xDB, 0x8D);
        Icon_id = new UInt32Xor(0x73, 0x21, 0xDF, 0xC6);
        Wep_equip = new UInt32Xor(0x28, 0x98, 0xB9, 0x35);
        Mov_equip = new UInt32Xor(0xEB, 0x18, 0x28, 0xAB);
        Sp_cost = new UInt32Xor(0x69, 0xF6, 0x31, 0xC0);
        Category = new ByteXor(0xBC);
        Tome_class = new ByteXor(0xF1);
        Exclusive = new ByteXor(0xCC);
        Enemy_only = new ByteXor(0x4F);
        Range = new ByteXor(0x56);
        Might = new ByteXor(0xD2);
        Cooldown_count = new SByteXor(0x56);
        Assist_cd = new ByteXor(0xF2);
        Healing = new ByteXor(0x95);
        Skill_range = new ByteXor(9);
        Score = new UInt16Xor(0x32, 0xA2);
        Promotion_tier = new SByteXor(0xE0);
        Promotion_rarity = new SByteXor(0x75);
        Refined = new ByteXor(0x2);
        Refine_sort_id = new ByteXor(0xFC);
        Wep_effective = new UInt32Xor(0x43, 0x3D, 0xBE, 0x23);
        Mov_effective = new UInt32Xor(0xEB, 0xDA, 0x3F, 0x82);
        Wep_shield = new UInt32Xor(0x43, 0xB7, 0xBA, 0xAA);
        Mov_shield = new UInt32Xor(0x5B, 0xF2, 0xBE, 0x0E);
        Wep_weakness = new UInt32Xor(0xAF, 0x02, 0x5A, 0x00);
        Mov_weakness = new UInt32Xor(0x19, 0xB8, 0x69, 0xB2);
        Wep_adaptive = new UInt32Xor(0x29, 0x26, 0x4E, 0x49);
        Mov_adaptive = new UInt32Xor(0x2E, 0xEF, 0x6C, 0xEE);
        Timing_id = new UInt32Xor(0x48, 0x66, 0x77, 0x9C);
        Ability_id = new UInt32Xor(0x25, 0x73, 0xB0, 0x72);
        Limit1_id = new UInt32Xor(0x32, 0xB8, 0xBD, 0x0E);
        Limit1_params = new Int16Xor[2];
        Limit1_params[0] = new Int16Xor(0x90, 0xA5);
        Limit1_params[1] = new Int16Xor(0x90, 0xA5);
        Limit2_id = new UInt32Xor(0x32, 0xB8, 0xBD, 0x0E);
        Limit2_params = new Int16Xor[2];
        Limit2_params[0] = new Int16Xor(0x90, 0xA5);
        Limit2_params[1] = new Int16Xor(0x90, 0xA5);
        Target_wep = new UInt32Xor(0xD7, 0xC9, 0x9F, 0x40);
        Target_mov = new UInt32Xor(0x22, 0xD1, 0x64, 0x6C);
        Timestamp = new Int64Xor(0x51, 0x9F, 0xFE, 0x3B, 0xF9, 0x39, 0x3F, 0xED);
        Random_allowed = new ByteXor(0x10);
        Min_lv = new ByteXor(0x90);
        Max_lv = new ByteXor(0x24);
        Tt_inherit_base = new ByteXor(0x19);
        Random_mode = new ByteXor(0xBE);
        Range_shape = new ByteXor(0x5C);
        Target_either = new ByteXor(0xA7);
        /*        Ss_coin = new UInt16Xor(0x40, 0xC5);
                Ss_badge_type = new UInt16Xor(0x0F, 0xD5);
                Ss_badge = new UInt16Xor(0xEC, 0x8C);
                Ss_great_badge = new UInt16Xor(0xFF, 0xCC);
                */

    }

    public SingleSkill(long a, byte[] data) : this() {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {
        Id_tag = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Archive.Index++;
        Refine_base = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
        if (!Refine_base.Value.Equals(""))
        {
            Archive.Index++;
        }
        Name_id = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, Common);
        Archive.Index++;
        a += 24;
        Desc_id = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Archive.Index++;
        Refine_id = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
        if (!Refine_id.Value.Equals(""))
            Archive.Index++;
        a += 8;
        Beast_effect_id = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
        for (int i = 0; i < Prerequisites.Length; i++)
        {
            Prerequisites[i] = new StringXor(ExtractUtils.getLong(a + 16 + (8 * i), data) + offset, data, Common);
            if (!Prerequisites[i].Value.Equals(""))
                Archive.Index++;
        }
        Next_skill = new StringXor(ExtractUtils.getLong(a + 32, data) + offset, data, Common);
        if (!Next_skill.Value.Equals(""))
            Archive.Index++;
        a += 8;
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i] = new StringXor(ExtractUtils.getLong(a + 32 + (8 * i), data) + offset, data, 0);
            if (!Sprites[i].Value.Equals(""))
                Archive.NegateIndex++;
        }
        Statistics = new Stats(a + 64, data);
        Class_params = new Stats(a + 80, data);
        Skill_params = new Stats(a + 96, data);
        Skill_params2 = new Stats(a + 112, data);
        Refine_stats = new Stats(a + 128, data);
        a += 0x10;
        Num_id.XorValue((ExtractUtils.getInt(a + 128, data)));
        Sort_id.XorValue((ExtractUtils.getInt(a + 132, data)));
        Icon_id.XorValue((ExtractUtils.getInt(a + 136, data)));
        Wep_equip.XorValue((ExtractUtils.getInt(a + 140, data)));
        Mov_equip.XorValue((ExtractUtils.getInt(a + 144, data)));
        Sp_cost.XorValue((ExtractUtils.getInt(a + 148, data)));
        Category.XorValue(data[a + 152]);
        Tome_class.XorValue(data[a + 153]);
        Exclusive.XorValue(data[a + 154]);
        Enemy_only.XorValue(data[a + 155]);
        Range.XorValue(data[a + 156]);
        Might.XorValue(data[a + 157]);
        Cooldown_count.XorValue(data[a + 158]);
        Assist_cd.XorValue(data[a + 159]);
        Healing.XorValue(data[a + 160]);
        Skill_range.XorValue(data[a + 161]);
        Score.XorValue(ExtractUtils.getShort(a + 162, data));
        Promotion_tier.XorValue(data[a + 164]);
        Refined.XorValue(data[a + 166]);
        Refine_sort_id.XorValue(data[a + 167]);
        Wep_effective.XorValue((ExtractUtils.getInt(a + 168, data)));
        Mov_effective.XorValue((ExtractUtils.getInt(a + 172, data)));
        Wep_shield.XorValue((ExtractUtils.getInt(a + 176, data)));
        Mov_shield.XorValue((ExtractUtils.getInt(a + 180, data)));
        Wep_weakness.XorValue((ExtractUtils.getInt(a + 184, data)));
        Mov_weakness.XorValue((ExtractUtils.getInt(a + 188, data)));
        Wep_adaptive.XorValue((ExtractUtils.getInt(a + 192, data)));
        Mov_adaptive.XorValue((ExtractUtils.getInt(a + 196, data)));
        Timing_id.XorValue((ExtractUtils.getInt(a + 200, data)));
        Ability_id.XorValue((ExtractUtils.getInt(a + 204, data)));
        Limit1_id.XorValue((ExtractUtils.getInt(a + 208, data)));
        Limit1_params[0].XorValue((ExtractUtils.getShort(a + 212, data)));
        Limit1_params[1].XorValue((ExtractUtils.getShort(a + 214, data)));
        Limit2_id.XorValue((ExtractUtils.getInt(a + 216, data)));
        Limit2_params[0].XorValue((ExtractUtils.getShort(a + 220, data)));
        Limit2_params[1].XorValue((ExtractUtils.getShort(a + 222, data)));
        Target_wep.XorValue((ExtractUtils.getInt(a + 224, data)));
        Target_mov.XorValue((ExtractUtils.getInt(a + 228, data)));
        Passive_next = new StringXor(ExtractUtils.getLong(a + 232, data) + offset, data, Common);
        if (!Passive_next.Value.Equals(""))
            Archive.Index++;
        Timestamp.XorValue((ExtractUtils.getLong(a + 240, data)));
        Random_allowed.XorValue(data[a + 248]);
        Min_lv.XorValue(data[a + 249]);
        Max_lv.XorValue(data[a + 250]);
        Tt_inherit_base.XorValue(data[a + 251]);
        Random_mode.XorValue(data[a + 252]);
        Range_shape.XorValue(data[a + 256]);
        Target_either.XorValue(data[a + 257]);
        /*        Id_tag2 = new StringXor(ExtractUtils.getLong(a + 256, data) + offset, data, Common);
                if (!Id_tag2.Value.Equals(""))
                    Archive.Index++;
                Next_seal = new StringXor(ExtractUtils.getLong(a + 264, data) + offset, data, Common);
                if (!Next_seal.Value.Equals(""))
                    Archive.Index++;
                Prev_seal = new StringXor(ExtractUtils.getLong(a + 272, data) + offset, data, Common);
                if (!Prev_seal.Value.Equals(""))
                    Archive.Index++;
                Ss_coin.XorValue((ExtractUtils.getShort(a + 280, data)));
                Ss_badge_type.XorValue((ExtractUtils.getShort(a + 282, data)));
                Ss_badge.XorValue((ExtractUtils.getShort(a + 284, data)));
                Ss_great_badge.XorValue((ExtractUtils.getShort(a + 286, data)));
                */
    }

    public override string ToString()
    {
        String text = "";
        text += Table.Contains(Name_id.Value) ? "Name: " + Table[Name_id.Value] + Environment.NewLine : "";
        text += Table.Contains(Desc_id.Value) ? "Description: " + Table[Desc_id.Value].ToString().Replace("\\n", " ").Replace("\\r", " ") + Environment.NewLine : "";
        text += "Internal Identifier: " + Id_tag + Environment.NewLine;
        if (!Refine_base.Value.Equals(""))
            text += getStuff(Refine_base, "Base Weapon: ") + "Base Weapon ID: " + Refine_base + Environment.NewLine;
        text += "Name Identifier: " + Name_id + Environment.NewLine;
        text += "Description Identifier: " + Desc_id + Environment.NewLine;
        if (!Refine_id.Value.Equals(""))
            text += getStuff(Refine_id, "Refine: ") + "Refine ID: " + Refine_id + Environment.NewLine;
        if (!Beast_effect_id.Value.Equals(""))
            text += getStuff(Beast_effect_id, "Beast Effect: ") + "Beast Effect ID: " + Beast_effect_id + Environment.NewLine;
        for (int i = 0; i < Prerequisites.Length; i++)
        {
            if(!Prerequisites[i].Value.Equals(""))
                text += getStuff(Prerequisites[i], "Prerequisite Skill: ") + "Prerequisite Skill ID: " + Prerequisites[i] + Environment.NewLine;
        }
        if (!Next_skill.Value.Equals(""))
            text += getStuff(Next_skill, "Next Enemy Skill: ") + "Next Enemy Skill ID: " + Next_skill + Environment.NewLine;
        if (!Sprites[0].Value.Equals(""))
            text += "Bow Sprite: " + Sprites[0] + Environment.NewLine;
        if (!Sprites[1].Value.Equals(""))
        {
            if (Sprites[0].Value.Equals(""))
                text += "Weapon Sprite: " + Sprites[1] + Environment.NewLine;
            else
                text += "Arrow Sprite: " + Sprites[1] + Environment.NewLine;
        }
        if (!Sprites[2].Value.Equals(""))
            text += "Map Animation: " + Sprites[2] + Environment.NewLine;
        if (!Sprites[3].Value.Equals(""))
            text += "AoE Special Animation: " + Sprites[3] + Environment.NewLine;
        text += "Stat bonuses: " + Statistics;
        int tmp = 1;
        bool start = true;
        bool is_Staff = false;
        bool is_Breath = false;
        bool is_Dagger = false;
        bool is_Beast = false;
        String tmp2 = "";
        for (int i = 0; i < WeaponNames.Length; i++)
        {
            if (((Wep_equip.Value & tmp)>>i) == 1)
            {
                if (!start)
                    tmp2 += ", " + WeaponNames.getString(i);
                else
                    tmp2 += " " + WeaponNames.getString(i);
                if (Category.Value == 0)
                {
                    if (WeaponsData[i].Is_breath)
                        is_Breath = true;
                    if (WeaponsData[i].Is_staff)
                        is_Staff = true;
                    if (WeaponsData[i].Is_dagger)
                        is_Dagger = true;
                    if (WeaponsData[i].Is_beast)
                        is_Beast = true;
                } 
                start = false;
            }
            tmp = tmp << 1;
        }
        if (is_Breath && Class_params.Hp.Value == 1)
        {
                text += "Targets lowest defense on enemy with Movement target and Weapon target" + Environment.NewLine;
        }
        else if (is_Staff && (Class_params.Hp.Value == 1 || Class_params.Hp.Value == 2))
        {
            if (Class_params.Hp.Value == 1)
                text += "Damage calculated the same as other weapons";
            else
                text += "Foe cannot counterattack";
            text += Environment.NewLine;
        }
        else if (is_Dagger && Class_params.Hp.Value > 0)
        {
            text += "Inflicts ";
            String temp = "";
            if (Class_params.Atk.Value > 0)
            {
                text += "-" + Class_params.Atk + " Attack";
                temp = ",";
            }
            if (Class_params.Spd.Value > 0)
            {
                text += temp + "-" + Class_params.Spd + " Speed";
                temp = ",";
            }
            if (Class_params.Def.Value > 0)
            {
                text += temp + "-" + Class_params.Def + " Defense";
                temp = ",";
            }
            if (Class_params.Res.Value > 0)
            {
                text += temp + "-" + Class_params.Res + " Resistance";
                temp = ",";
            }
            text += "on foes within " + Class_params.Hp + " spaces of target through their next action" + Environment.NewLine;
        }
        else if (is_Beast && (Class_params.Hp.Value == 1))
        {
            text += "When transformed, grants: ";
            String temp = "";
            if (Class_params.Atk.Value > 0)
            {
                text += Class_params.Atk + " Attack";
                temp = ",";
            }
            if (Class_params.Spd.Value > 0)
            {
                text += temp + Class_params.Spd + " Speed";
                temp = ",";
            }
            if (Class_params.Def.Value > 0)
            {
                text += temp + Class_params.Def + " Defense";
                temp = ",";
            }
            if (Class_params.Res.Value > 0)
            {
                text += temp + Class_params.Res + " Resistance";
                temp = ",";
            }
            text += Environment.NewLine;
        }
        else
            text += "Class dependant Parameters: " + Class_params;
        text += "Skill Parameters: " + Skill_params;
        text += "Skill Parameters 2: " + Skill_params2;
        text += "Refine Stats: " + Refine_stats;
        text += "ID: " + Num_id + Environment.NewLine;
        text += "Sort: " + Sort_id + Environment.NewLine;
        text += "Icon ID: " + Icon_id + Environment.NewLine;
        text += "Can be equipped by:" + tmp2 + Environment.NewLine;
        text += "Can be equipped by:" + ExtractUtils.BitmaskConvertToString(Mov_equip.Value, Movement) + Environment.NewLine;
        text += "Sp cost: " + Sp_cost + Environment.NewLine;
        text += "Category: " + SkillCategory.getString(Category.Value) + Environment.NewLine;
        text += "Tome Element: " + Tome_Elem.getString(Tome_class.Value) + Environment.NewLine;
        text += Exclusive.Value == 1 ? "Exclusive skill" + Environment.NewLine : "Inheritable skill" + Environment.NewLine;
        text += Enemy_only.Value == 1 ? "Enemy exclusive" + Environment.NewLine : "Not enemy exclusive" + Environment.NewLine;
        text += Range.Value == 0 ? "Can't have range" + Environment.NewLine : "Range: " + Range + Environment.NewLine;
        text += Might.Value == 0 ? "Can't have might" + Environment.NewLine : "Might: " + Might + Environment.NewLine;
        text += "Cooldown change: " + Cooldown_count + Environment.NewLine;
        text += Assist_cd.Value == 1 ? "Assist grants -1 cooldown after use" + Environment.NewLine : "";
        text += Healing.Value == 1 ? "Healing assist" + Environment.NewLine : "";
        text += "Effect Range: " + Skill_range + Environment.NewLine;
        text += "Score: " + Score + Environment.NewLine;
        text += "Possible promotions: " + Promotion_tier + Environment.NewLine;
        text += Promotion_rarity.Value == 0 ? "" : "Promote if rarity is above " + Promotion_rarity + Environment.NewLine;
        text += Refined.Value == 1 ? "Refined" + Environment.NewLine : "";
        text += Refine_sort_id.Value == 0 ? "" : "Refine Sort:" + Refine_sort_id + Environment.NewLine;
        text += Wep_effective.Value == 0 ? "" : "Weapon effective against:" + ExtractUtils.BitmaskConvertToString(Wep_effective.Value, WeaponNames) + Environment.NewLine;
        text += Mov_effective.Value == 0 ? "" : "Movement effective against:" + ExtractUtils.BitmaskConvertToString(Mov_effective.Value, Movement) + Environment.NewLine;
        text += Wep_shield.Value == 0 ? "" : "Weapon shield against:" + ExtractUtils.BitmaskConvertToString(Wep_shield.Value, WeaponNames) + Environment.NewLine;
        text += Mov_shield.Value == 0 ? "" : "Movement shield against:" + ExtractUtils.BitmaskConvertToString(Mov_shield.Value, Movement) + Environment.NewLine;
        text += Wep_weakness.Value == 0 ? "" : "Weapon weakness against:" + ExtractUtils.BitmaskConvertToString(Wep_weakness.Value, WeaponNames) + Environment.NewLine;
        text += Mov_weakness.Value == 0 ? "" : "Movement weakness against:" + ExtractUtils.BitmaskConvertToString(Mov_weakness.Value, Movement) + Environment.NewLine;
        text += Wep_adaptive.Value == 0 ? "" : "Weapon adaptive against:" + ExtractUtils.BitmaskConvertToString(Wep_adaptive.Value, WeaponNames) + Environment.NewLine;
        text += Mov_adaptive.Value == 0 ? "" : "Movement adaptive against:" + ExtractUtils.BitmaskConvertToString(Mov_adaptive.Value, Movement) + Environment.NewLine;
        text += "Timing ID: " + Timing_id + Environment.NewLine;
        text += "Ability ID: " + Ability_id + Environment.NewLine;
        text += "Limit 1 ID: " + Limit1_id + Environment.NewLine;
        for (int i = 0; i < Limit1_params.Length; i++)
        {
            if (Limit1_id.Value != 0 && !Limit1_params[i].Value.Equals(""))
                text += "Limit 1 Parameter " + (i + 1) + ": " + Limit1_params[i] + Environment.NewLine;
        }
        text += "Limit 2 ID: " + Limit2_id + Environment.NewLine;
        for (int i = 0; i < Limit2_params.Length; i++)
        {
            if (Limit2_id.Value != 0 && !Limit2_params[i].Value.Equals(""))
                text += "Limit 2 Parameter " + (i + 1) + ": " + Limit2_params[i] + Environment.NewLine;
        }
        text += Target_wep.Value == 0 ? "" : "Weapon target:" + ExtractUtils.BitmaskConvertToString(Target_wep.Value, WeaponNames) + Environment.NewLine;
        text += Target_mov.Value == 0 ? "" : "Movement target:" + ExtractUtils.BitmaskConvertToString(Target_mov.Value, Movement) + Environment.NewLine;
        if(!Passive_next.Value.Equals(""))
            text += getStuff(Passive_next, "Next Enemy Passive: ") + "Next Enemy Passive ID: " + Passive_next + Environment.NewLine;
        text += "Timestamp: ";
        text += Timestamp.Value < 0 ? "Not available" + Environment.NewLine : DateTimeOffset.FromUnixTimeSeconds(Timestamp.Value).DateTime.ToLocalTime() + Environment.NewLine;
        text += (Random_allowed.Value == 0 ? "The skill cannot be used by random units" : "The skill may be equipped by random units") + Environment.NewLine;
        text += Min_lv.Value == 0 ? "" : "Minimum Enemy Level: " + Min_lv + Environment.NewLine;
        text += Max_lv.Value == 0 ? "" : "Maximum Enemy Level: " + Max_lv + Environment.NewLine;
        text += (Tt_inherit_base.Value == 0 ? "The skill will not be considered for 10th Stratum of Training Tower" : "The skill will be considered for 10th Stratum of Training Tower if equipped in the base version of the map") + Environment.NewLine;
        text += (Random_allowed.Value == 0 && Random_mode.Value == 0) ? "" : ((Random_mode.Value == 0 ? "The skill cannot be used by random units" : Random_mode.Value == 1 ? "The skill may be equipped by any random unit" : "The skill may be equipped by random units who normally own it") + Environment.NewLine);
        text += "Range shape: " + Range_shape.Value + Environment.NewLine;
        text += (Target_either.Value == 1) ? ("Targets both enemies and allies" + Environment.NewLine) : "";
        /*        if (!Next_seal.Value.Equals(""))
                    text += getStuff(Next_seal, "Next Seal: ") + "Next Seal ID: " + Next_seal + Environment.NewLine;
                if (!Prev_seal.Value.Equals(""))
                    text += getStuff(Prev_seal, "Previous Seal: ") + "Previous Seal ID: " + Prev_seal + Environment.NewLine;
                text += Ss_coin.Value == 0 ? "" : "Sacred Seal required Coins: " + Ss_coin + Environment.NewLine;
                text += Ss_coin.Value == 0 ? "" : "Sacred Seal required Badge type: " + BadgeColor[Ss_badge_type.Value] + Environment.NewLine;
                text += Ss_coin.Value == 0 ? "" : "Sacred Seal required Badges: " + Ss_badge + Environment.NewLine;
                text += Ss_coin.Value == 0 ? "" : "Sacred Seal required Great Badges: " + Ss_great_badge + Environment.NewLine;
        */
        text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;

        return text;
    }

    public StringXor Id_tag { get => id_tag; set => id_tag = value; }
    public StringXor Refine_base { get => refine_base; set => refine_base = value; }
    public StringXor Name_id { get => name_id; set => name_id = value; }
    public StringXor Desc_id { get => desc_id; set => desc_id = value; }
    public StringXor Refine_id { get => refine_id; set => refine_id = value; }
    public StringXor[] Prerequisites { get => prerequisites; set => prerequisites = value; }
    public StringXor Next_skill { get => next_skill; set => next_skill = value; }
    public StringXor[] Sprites { get => sprites; set => sprites = value; }
    public Stats Statistics { get => statistics; set => statistics = value; }
    public Stats Class_params { get => class_params; set => class_params = value; }
    public Stats Skill_params { get => skill_params; set => skill_params = value; }
    public Stats Refine_stats { get => refine_stats; set => refine_stats = value; }
    public UInt32Xor Num_id { get => num_id; set => num_id = value; }
    public UInt32Xor Sort_id { get => sort_id; set => sort_id = value; }
    public UInt32Xor Icon_id { get => icon_id; set => icon_id = value; }
    public UInt32Xor Wep_equip { get => wep_equip; set => wep_equip = value; }
    public UInt32Xor Mov_equip { get => mov_equip; set => mov_equip = value; }
    public UInt32Xor Sp_cost { get => sp_cost; set => sp_cost = value; }
    public ByteXor Category { get => category; set => category = value; }
    public ByteXor Tome_class { get => tome_class; set => tome_class = value; }
    public ByteXor Exclusive { get => exclusive; set => exclusive = value; }
    public ByteXor Enemy_only { get => enemy_only; set => enemy_only = value; }
    public ByteXor Range { get => range; set => range = value; }
    public ByteXor Might { get => might; set => might = value; }
    public SByteXor Cooldown_count { get => cooldown_count; set => cooldown_count = value; }
    public ByteXor Assist_cd { get => assist_cd; set => assist_cd = value; }
    public ByteXor Healing { get => healing; set => healing = value; }
    public ByteXor Skill_range { get => skill_range; set => skill_range = value; }
    public UInt16Xor Score { get => score; set => score = value; }
    public SByteXor Promotion_tier { get => promotion_tier; set => promotion_tier = value; }
    public SByteXor Promotion_rarity { get => promotion_rarity; set => promotion_rarity = value; }
    public ByteXor Refined { get => refined; set => refined = value; }
    public ByteXor Refine_sort_id { get => refine_sort_id; set => refine_sort_id = value; }
    public UInt32Xor Wep_effective { get => wep_effective; set => wep_effective = value; }
    public UInt32Xor Mov_effective { get => mov_effective; set => mov_effective = value; }
    public UInt32Xor Wep_shield { get => wep_shield; set => wep_shield = value; }
    public UInt32Xor Mov_shield { get => mov_shield; set => mov_shield = value; }
    public UInt32Xor Wep_weakness { get => wep_weakness; set => wep_weakness = value; }
    public UInt32Xor Mov_weakness { get => mov_weakness; set => mov_weakness = value; }
    public UInt32Xor Wep_adaptive { get => wep_adaptive; set => wep_adaptive = value; }
    public UInt32Xor Mov_adaptive { get => mov_adaptive; set => mov_adaptive = value; }
    public UInt32Xor Timing_id { get => timing_id; set => timing_id = value; }
    public UInt32Xor Ability_id { get => ability_id; set => ability_id = value; }
    public UInt32Xor Limit1_id { get => limit1_id; set => limit1_id = value; }
    public Int16Xor[] Limit1_params { get => limit1_params; set => limit1_params = value; }
    public UInt32Xor Limit2_id { get => limit2_id; set => limit2_id = value; }
    public Int16Xor[] Limit2_params { get => limit2_params; set => limit2_params = value; }
    public UInt32Xor Target_wep { get => target_wep; set => target_wep = value; }
    public UInt32Xor Target_mov { get => target_mov; set => target_mov = value; }
    public StringXor Passive_next { get => passive_next; set => passive_next = value; }
    public Int64Xor Timestamp { get => timestamp; set => timestamp = value; }
    public ByteXor Min_lv { get => min_lv; set => min_lv = value; }
    public ByteXor Max_lv { get => max_lv; set => max_lv = value; }
//    public StringXor Id_tag2 { get => id_tag2; set => id_tag2 = value; }
//    public StringXor Next_seal { get => next_seal; set => next_seal = value; }
//    public StringXor Prev_seal { get => prev_seal; set => prev_seal = value; }
//    public UInt16Xor Ss_coin { get => ss_coin; set => ss_coin = value; }
//    public UInt16Xor Ss_badge_type { get => ss_badge_type; set => ss_badge_type = value; }
//    public UInt16Xor Ss_badge { get => ss_badge; set => ss_badge = value; }
//    public UInt16Xor Ss_great_badge { get => ss_great_badge; set => ss_great_badge = value; }
    public StringXor Beast_effect_id { get => beast_effect_id; set => beast_effect_id = value; }
    public ByteXor Random_allowed { get => random_allowed; set => random_allowed = value; }
    public ByteXor Tt_inherit_base { get => tt_inherit_base; set => tt_inherit_base = value; }
    public ByteXor Random_mode { get => random_mode; set => random_mode = value; }
    public ByteXor Range_shape { get => range_shape; set => range_shape = value; }
    public ByteXor Target_either { get => target_either; set => target_either = value; }
    public Stats Skill_params2 { get => skill_params2; set => skill_params2 = value; }
}

public class GenericText : ExtractionBase
{
    private StringXor[] elements;
    private byte[] XorArr;
    public GenericText(string name, byte[] xor)
    {
        Name = "Generic Text" + (name.Equals("") ? "" : " " + name);
        XorArr = xor;
    }

    public string getVals(string i)
    {
        string text = "";
        text = getHeroName(i);
        if (text.Equals(i) && i.Length > 1)
        {
            text = Table.Contains("MID_SCF_" + i.Remove(i.Length - 1)) ? Table["MID_SCF_" + i.Remove(i.Length - 1)].ToString() : i;
        }
        return text;
    }

    public GenericText(string name, byte[] xor, long a, byte[] data) : this(name, xor)
    {
        InsertIn(a, data);
    }

    public StringXor[] Elements { get => elements; set => elements = value; }

    public override void InsertIn(long a, byte[] data)
    {
        Elements = new StringXor[Archive.Ptr_list_length];
        for (long i = 0; i < Elements.Length; i++)
        {
            Elements[i] = new StringXor(ExtractUtils.getLong(Archive.Ptr_list[Archive.Index++], data) + offset, data, XorArr);
        }
    }

    public override String ToString()
    {
        String text = "";
        for (int i = 0; i < Elements.Length; i++)
        {
            text += getVals(Elements[i].Value) + Environment.NewLine;
        }

        return text;
    }

}

public class Messages : ExtractionBase
{
    private string[,] translatedMessages;
    private long numElem;
    public static readonly byte[] MSG = { 0x6F, 0xB0, 0x8F, 0xD6, 0xEF, 0x6A, 0x5A, 0xEB, 0xC6, 0x76, 0xF6, 0xE5, 0x56, 0x9D, 0xB8, 0x08, 0xE0, 0xBD, 0x93, 0xBA, 0x05, 0xCC, 0x26, 0x56, 0x65, 0x1E, 0xF8, 0x2B, 0xF9, 0xA1, 0x7E, 0x41, 0x18, 0x21, 0xA4, 0x94, 0x25, 0x08, 0xB8, 0x38, 0x2B, 0x98, 0x53, 0x76, 0xC6, 0x2E, 0x73, 0x5D, 0x74, 0xCB, 0x02, 0xE8, 0x98, 0xAB, 0xD0, 0x36, 0xE5, 0x37 };

    public Messages()
    {
        Name = "Messages";
        Size = 0x10;
    }
    public Messages(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public string[,] TranslatedMessages { get => translatedMessages; set => translatedMessages = value; }
    public long NumElem { get => numElem; set => numElem = value; }

    public override void InsertIn(long a, byte[] data)
    {
        NumElem = ExtractUtils.getLong(a, data);
        TranslatedMessages = new string[NumElem, 2];
        for (int i = 0; i < NumElem; i++)
        {
            TranslatedMessages[i, 0] = new StringXor(ExtractUtils.getLong(a + 8 + (i * Size), data) + offset, data, MSG).ToString();
            Archive.Index++;
            TranslatedMessages[i, 1] = new StringXor(ExtractUtils.getLong(a + 0x10 + (i * Size), data) + offset, data, MSG).ToString();
            Archive.Index++;
            if (!Table.Contains(TranslatedMessages[i, 0]))
                Table.Add(TranslatedMessages[i, 0], TranslatedMessages[i, 1]);
        }
    }

    public override string ToString()
    {
        String text = "";
        for (int i = 0; i < NumElem; i++)
            text += TranslatedMessages[i,0] + ": " + TranslatedMessages[i,1] + Environment.NewLine;
        return text;
    }
}

public class BaseExtractArchive<T> : ExtractionBase where T : ExtractionBase, new()
{
    private Int64Xor numElem;
    private T[] things;

    public Int64Xor NumElem { get => numElem; set => numElem = value; }
    internal T[] Things { get => things; set => things = value; }

    public BaseExtractArchive()
    {
        T tmp = new T();
        Name = tmp.Name;
        NumElem = new Int64Xor(tmp.ElemXor);
    }
    public BaseExtractArchive(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }
    public override void InsertIn(long a, byte[] data)
    {
        a = Archive.Ptr_list[Archive.Index];
        NumElem.XorValue(ExtractUtils.getLong(a + 8, data));
        Archive.Index++;
        Things = new T[NumElem.Value];
        a = ExtractUtils.getLong(a, data) + offset;
        for (int i = 0; i < NumElem.Value; i++)
        {
            Things[i] = new T();
            Things[i].InsertIn(Archive, a + (Things[i].Size * i), data);
        }
        Archive.Index = Archive.Ptr_list_length;
    }

    public override string ToString()
    {
        String text = "";
        for (int i = 0; i < NumElem.Value; i++)
            text += Things[i];
        return text;
    }
}

public class BaseExtractArchiveDirect<T> : ExtractionBase where T : ExtractionBase, new()
{
    private Int64 numElem;
    private T[] things;

    public Int64 NumElem { get => numElem; set => numElem = value; }
    internal T[] Things { get => things; set => things = value; }

    public BaseExtractArchiveDirect()
    {
        T tmp = new T();
        Name = tmp.Name;
        NumElem = new Int64();
    }
    public BaseExtractArchiveDirect(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }
    public override void InsertIn(long a, byte[] data)
    {
        a = Archive.Ptr_list[Archive.Index];
        NumElem = ExtractUtils.getLong(offset, data);
        Archive.Index++;
        Things = new T[NumElem];
        for (int i = 0; i < NumElem; i++)
        {
            Int64 b = ExtractUtils.getLong(a + (i * 8), data) + offset;
            Things[i] = new T();
            Things[i].InsertIn(Archive, b, data);
        }
        Archive.Index = Archive.Ptr_list_length;
    }

    public override string ToString()
    {
        String text = "";
        for (int i = 0; i < NumElem; i++)
            text += Things[i] + (i == NumElem - 1 ? "" : "\n");
        return text;
    }
}