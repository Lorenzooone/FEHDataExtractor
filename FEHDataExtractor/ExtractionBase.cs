using FEHDataExtractor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;


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
        Ptr_list = new long[Ptr_list_length];
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
    private static Hashtable tablejp = new Hashtable();
    private static Hashtable tablekr = new Hashtable();
    public static StringsUpdatable WeaponNames = new StringsUpdatable(new string[] { "Sword", "Lance", "Axe", "Red Bow", "Blue Bow", "Green Bow", "Colourless Bow", "Red Dagger", "Blue Dagger", "Green Dagger", "Colourless Dagger", "Red Tome", "Blue Tome", "Green Tome", "Colourless Tome", "Staff", "Red Breath", "Blue Breath", "Green Breath", "Colourless Breath", "Red Beast", "Blue Beast", "Green Beast", "Colourless Beast" });
    public static StringsUpdatable WeaponNames_kor = new StringsUpdatable(new string[] { "검", "창", "도끼", "적활", "청활", "녹활", "무색활", "적암기", "청암기", "녹암기", "무색암기", "적마도서", "청마도서", "녹마도서", "무색마도서", "지팡이", "적룡", "청룡", "녹룡", "무색용", "적색짐승", "청색짐승", "녹색짐승", "무색짐승" });
    public static SingleWeaponClass[] WeaponsData;
    public static readonly StringsUpdatable Tome_Elem = new StringsUpdatable(new string[] { "None", "Fire", "Thunder", "Wind", "Light", "Dark", "Stone" });
    public static readonly StringsUpdatable Tome_Elem_kor = new StringsUpdatable(new string[] { "없음", "화", "뇌", "풍", "광", "암", "석" });
    public static readonly StringsUpdatable Movement = new StringsUpdatable(new string[] { "Infantry", "Armored", "Cavalry", "Flying" });
    public static readonly StringsUpdatable Movement_kor = new StringsUpdatable(new string[] { "보병", "중장", "기마", "비병" });
    public static readonly StringsUpdatable Series = new StringsUpdatable(new string[] { "Heroes", "Shadow Dragon and the Blade of Light / Mystery of the Emblem / Shadow Dragon / New Mystery of the Emblem", "Gaiden / Echoes", "Genealogy of the Holy War", "Thracia 776", "The Binding Blade", "The Blazing Blade", "The Sacred Stones", "Path of Radiance", "Radiant Dawn", "Awakening", "Fates", "Three Houses", "Tokyo Mirage Session", "Engage" });
    public static readonly StringsUpdatable Series_kor = new StringsUpdatable(new string[] { "히어로즈", "암흑룡/문장/신암흑룡/신문장", "에코즈", "성전의 계보", "트라키아 776", "봉인의 검", "열화의 검", "성마의 광석", "창염의 궤적", "새벽의 여신", "각성", "If", "풍화설월", "환영이문록", "인게이지" });
    public static readonly StringsUpdatable BadgeColor = new StringsUpdatable(new string[] { "Scarlet", "Azure", "Verdant", "Trasparent" });
    public static readonly StringsUpdatable ShardColor = new StringsUpdatable(new string[] { "Universal", "Scarlet", "Azure", "Verdant", "Trasparent" });
    public static readonly StringsUpdatable SkillCategory = new StringsUpdatable(new string[] { "Weapon", "Assist", "Special", "Passive A", "Passive B", "Passive C", "Sacred Seal", "Refined Weapon Skill Effect", "Beast Effect" });
    public static readonly StringsUpdatable SkillCategory_kor = new StringsUpdatable(new string[] { "무기", "보조기", "오의", "A스킬", "B스킬", "C스킬", "성인", "전용연성 효과", "짐승무기 효과/비익쌍계 버프효과" });
    public static readonly StringsUpdatable Ranks = new StringsUpdatable(new string[] { "C", "B", "A", "S" });
    public static readonly StringsUpdatable LegendaryElement = new StringsUpdatable(new string[] { "Fire", "Water", "Wind", "Earth", "Light", "Dark", "Astra", "Anima" });
    public static readonly StringsUpdatable LegendaryElement_kor = new StringsUpdatable(new string[] { "화", "수", "풍", "지", "광", "암", "천", "리" });
    public static readonly StringsUpdatable Colours = new StringsUpdatable(new string[] { "Red", "Blue", "Green", "Colorless" });
    public static readonly StringsUpdatable Colours_kor = new StringsUpdatable(new string[] { "적", "청", "녹", "무" });
    private byte[] elemXor;
    private int size = 0;

    private HSDARC archive;

    public abstract void InsertIn(long a, byte[] data);
    public virtual void InsertInJp(long a, byte[] data)
    {
        InsertIn(a, data);
        System.Diagnostics.Debug.WriteLine("재정의 안됨!");
    }
    public virtual void InsertInKr()
    {
    }
    public void InsertIn(HSDARC archive, long a, byte[] data)
    {
        Archive = archive;
        InsertIn(a, data);
    }
    public void InsertInJp(HSDARC archive, long a, byte[] data)
    {
        Archive = archive;
        InsertInJp(a, data);
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

    public virtual string ToString_json()
    {
        return ToString();
    }


    public virtual byte[] ToByte()
    {
        return new byte[] { 0x00 };
    }


    public string Name { get; set; }
    public HSDARC Archive { get => archive; set => archive = value; }
    public int Size { get => size; set => size = value; }
    public byte[] ElemXor { get => elemXor; set => elemXor = value; }
    public static Hashtable Table { get => table; set => table = value; }
    public static Hashtable TableJP { get => tablejp; set => tablejp = value; }
    public static Hashtable TableKR { get => tablekr; set => tablekr = value; }
}

public abstract class LoginRelated : ExtractionBase
{
    public static readonly byte[] Login = { 0xCD, 0x76, 0x95, 0x7D, 0x7A, 0xF5, 0xB3, 0x0B, 0xE6, 0xC9, 0x39, 0x72, 0xFC, 0x9E, 0xE2, 0x73, 0x3D, 0x31, 0x98, 0x23, 0xC0, 0x28, 0x2F, 0xA0, 0xE6, 0x5E, 0xB3, 0x9C, 0x6C, 0x27, 0xA9, 0xCB, 0xB7, 0x26, 0x68, 0x64 };
}

public abstract class GCRelated : ExtractionBase
{
    public static readonly byte[] GC = { 0x17, 0xFC, 0xC9, 0xEA, 0x79, 0x69, 0x24, 0xBD, 0xA4, 0x54, 0x0E, 0x58, 0xBD, 0x8B, 0x36, 0xCD, 0xAF, 0xB4, 0xE2, 0x09, 0x3C, 0x1F, 0x8C, 0x9C, 0xD1, 0x48, 0x51, 0xA1, 0xFB, 0xAD, 0x48, 0x7E, 0xC3, 0x38, 0x5A, 0x41 };
}

public abstract class FBRelated : ExtractionBase
{
    public static readonly byte[] FB = { 0x2F, 0x08, 0x66, 0xED, 0x7C, 0x98, 0x34, 0x2A, 0xE4, 0xAC, 0x41, 0xD1, 0xE5, 0x1F, 0xD2, 0x5E, 0x28, 0x32, 0x76, 0xDE, 0x87, 0x0A, 0xA7, 0xF9, 0x44, 0x28, 0x26, 0xC7, 0x25, 0x21, 0x06, 0x68, 0xE3, 0x72, 0x96, 0x3A, 0x24, 0xEA, 0xA2, 0x4F, 0xDF, 0xEB, 0x11, 0xDC, 0x50, 0x26, 0x3C, 0x78, 0xD0, 0x89, 0x04, 0xA9, 0xF7, 0x4A, 0x26, 0x28, 0xC9, 0x2B };
}

//id XorKey
public abstract class CommonRelated : ExtractionBase
{
    public static readonly byte[] Common = { 0x81, 0x00, 0x80, 0xA4, 0x5A, 0x16, 0x6F, 0x78, 0x57, 0x81, 0x2D, 0xF7, 0xFC, 0x66, 0x0F, 0x27, 0x75, 0x35, 0xB4, 0x34, 0x10, 0xEE, 0xA2, 0xDB, 0xCC, 0xE3, 0x35, 0x99, 0x43, 0x48, 0xD2, 0xBB, 0x93, 0xC1 };
}

public abstract class CharacterRelated : CommonRelated
{
    StringXor id_tag;
    StringXor roman;
    StringXor face_name;
    StringXor face_name2;
    Int64Xor timestamp;
    UInt32Xor id_num;
    UInt32Xor version_book_num;
    ByteXor version_chapter_num;
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

    public string getCharacterStuff_json()
    {
        string text = "";
        if (Table.Contains("M" + Id_tag.ToString()))
        {

            text = "\"id_tag\": \"" + Id_tag.ToString() + "\",";
            
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
        if ((greater && tmpStats.Hp.Value - val >= lvl40.Hp.Value) || (!greater && tmpStats.Hp.Value + val <= lvl40.Hp.Value))
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
        text += len > 0 ? len > 1 && phrase.IndexOf("u") >= 0 ? phrase + "s: " : phrase + ": " : "";

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

    public string SuperBoonBane_kor(Stats Base_stats, Stats Growth_rates, Stats lvl40)
    {
        return (SuperStats(Base_stats, Growth_rates, lvl40, true, "Superboon").Equals("") ? "" : SuperStats(Base_stats, Growth_rates, lvl40, true, "슈퍼분") + Environment.NewLine) + (SuperStats(Base_stats, Growth_rates, lvl40, false, "Superbane").Equals("") ? "" : SuperStats(Base_stats, Growth_rates, lvl40, false, "슈퍼배인") + Environment.NewLine);
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
    public UInt32Xor Version_Book_num { get => version_book_num; set => version_book_num = value; }
    public ByteXor Version_Chapter_num { get => version_chapter_num; set => version_chapter_num = value; }
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

    public Stats(Stats Level1, Stats Growths) : this()
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

    public override string ToString_json()
    {
        String text = "{ ";

        text += "\"hp\":" + Hp + ",";
        text += "\"atk\":" + Atk + ",";
        text += "\"spd\":" + Spd + ",";
        text += "\"def\":" + Def + ",";
        text += "\"res\":" + Res + "}";

        return text;
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
    public static readonly StringsUpdatable LegKind = new StringsUpdatable(new string[] { "Legendary/Mythic", "Duo", "Harmonized Duo", "Ascendant", "Rearmed" });
    public static readonly StringsUpdatable LegKind_kor = new StringsUpdatable(new string[] { "전승/신계", "비익", "쌍계", "개화", "마기" });
    private StringXor duo_skill_id;
    private Stats bonuses;
    private ByteXor kind;
    private ByteXor element;
    private ByteXor bst;
    private ByteXor is_duel;
    private ByteXor is_extraslot;

    public Legendary()
    {
        Size = 17; //It's a pointer, so who cares about size
        Name = "";
        Kind = new ByteXor(0x21);
        Element = new ByteXor(5);
        Bst = new ByteXor(0x0F);
        Is_duel = new ByteXor(0x80);
        Is_Extraslot = new ByteXor(0x24);
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

    override public String ToString_json()
    {
        String text = "";
        text += "{\"duo_skill_id\":\"" + Duo_skill_id + "\",";

        text += "\"bonus_effect\": ";
        text += Bonuses.ToString_json();
        text += ",";

        text += "\"kind\":" + Kind + ",";
        text += "\"element\":" + Element + ",";
        text += "\"bst\":" + Bst + ",";
        text += "\"pair_up\":" + (Is_duel.Value != 0 ? "true" : "false") + ",";
        text += "\"ae_extra\":" + (Is_Extraslot.Value != 0 ? "true" : "false");

        text += "}";

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
    public ByteXor Is_Extraslot { get => is_extraslot; set => is_extraslot = value; }
}

public class Dragonflowers : CommonRelated
{
    private Int32Xor dflowers;
    private Int32Xor[] dflowerCostList;

    public Int32Xor Dflowers { get => dflowers; set => dflowers = value; }
    public Int32Xor[] DFlowerCostList { get => dflowerCostList; set => dflowerCostList = value; }

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
        text += "Required Dragonflowers : [";
        foreach (Int32Xor cost in DFlowerCostList)
        {
            text += cost.Value + ", ";
        }
        text = text.Substring(0, text.Length - 2);
        text += "]" + Environment.NewLine;
        return text;
    }

    public override string ToString_json()
    {
        String text = "";
        text += "{\"max_count\":" + Dflowers.Value + ",";

        text += "\"costs\": [";
        foreach (Int32Xor cost in DFlowerCostList)
        {
            text += cost.Value + ",";
        }
        text = text.Substring(0, text.Length - 1);
        text += "]}";

        return text;
    }

    public override void InsertIn(long a, byte[] data)
    {
        if (a != offset)
        {
            Dflowers.XorValue((ExtractUtils.getInt(a, data)));
            a += 16;
            DFlowerCostList = new Int32Xor[Dflowers.Value];
            for (int i = 0; i < Dflowers.Value; i++)
            {
                DFlowerCostList[i] = new Int32Xor(0x7B, 0x6A, 0x5C, 0x71);
                DFlowerCostList[i].XorValue(ExtractUtils.getInt(a + i * 4, data));
            }
        }
    }
}

public class SingleEnemy : CharacterRelated
{
    StringXor topWeapon;
    StringXor uniqueAssist;
    StringXor uniqueAssist2;
    StringXor uniqueSpecial;
    ByteXor _spawnable_Enemy;
    ByteXor is_boss;
    ByteXor refresher;
    ByteXor is_enemy;
    ByteXor is_npc;
    ByteXor byte_29;
    ByteXor byte_30;
    ByteXor byte_31;


    public ByteXor Is_boss { get => is_boss; set => is_boss = value; }
    public ByteXor Spawnable_Enemy { get => _spawnable_Enemy; set => _spawnable_Enemy = value; }
    public StringXor TopWeapon { get => topWeapon; set => topWeapon = value; }
    public StringXor UniqueAssist { get => uniqueAssist; set => uniqueAssist = value; }
    public StringXor UniqueAssist2 { get => uniqueAssist2; set => uniqueAssist2 = value; }
    public StringXor UniqueSpecial { get => uniqueSpecial; set => uniqueSpecial = value; }
    public ByteXor Refresher { get => refresher; set => refresher = value; }
    public ByteXor Is_enemy { get => is_enemy; set => is_enemy = value; }
    public ByteXor Is_npc { get => is_npc; set => is_npc = value; }
    public ByteXor Byte_29 { get => byte_29; set => byte_29 = value; }
    public ByteXor Byte_30 { get => byte_30; set => byte_30 = value; }
    public ByteXor Byte_31 { get => byte_31; set => byte_31 = value; }

    public SingleEnemy() : base()
    {
        Name = "Enemies";
        ElemXor = new byte[] { 0x5C, 0x34, 0xC5, 0x9C, 0x11, 0x95, 0xCA, 0x62 };
        Size = 120; //10 of the stuff + 7 of padding
        Id_num = new UInt32Xor(0xD4, 0x41, 0x2F, 0x42);
        Weapon_type = new ByteXor(0xE4);
        Tome_class = new ByteXor(0x81);
        Move_type = new ByteXor(0x0D);
        Spawnable_Enemy = new ByteXor(0xC4); //1 -> 가능, 0-> 불가능
        Is_boss = new ByteXor(0x6A);//-0->아님, 1-> 맞음
        Refresher = new ByteXor(0x2A);
        Is_enemy = new ByteXor(0X13);
        Is_npc = new ByteXor(0XF7);
        Byte_29 = new ByteXor(0X4F);
        Byte_30 = new ByteXor(0X15);
        Byte_31 = new ByteXor(0X6D);
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
        /*
        if (Roman.Value.Equals("NONE"))
        {
            return;
        }*/
        Face_name = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, Common);
        Archive.Index++;
        a += 24;
        Face_name2 = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Archive.Index++;
        TopWeapon = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
        if (!TopWeapon.Value.Equals(""))
            Archive.Index++;

        UniqueAssist = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, Common);
        if (!UniqueAssist.Value.Equals(""))
            Archive.Index++;

        UniqueAssist2 = new StringXor(ExtractUtils.getLong(a + 24, data) + offset, data, Common);
        if (!UniqueAssist2.Value.Equals(""))
            Archive.Index++;

        UniqueSpecial = new StringXor(ExtractUtils.getLong(a + 32, data) + offset, data, Common);
        if (!UniqueSpecial.Value.Equals(""))
            Archive.Index++;

        a += 24;
        Timestamp.XorValue((ExtractUtils.getLong(a + 16, data)));
        Id_num.XorValue((ExtractUtils.getInt(a + 24, data)));
        //28~31 새로운거 생김

        Is_npc.XorValue(data[a + 28]);
        Byte_29.XorValue(data[a + 29]);
        Byte_30.XorValue(data[a + 30]);
        Byte_31.XorValue(data[a + 31]);

        Weapon_type.XorValue(data[a + 32]);
        Tome_class.XorValue(data[a + 33]);
        Move_type.XorValue(data[a + 34]);
        Spawnable_Enemy.XorValue(data[a + 35]);
        Is_boss.XorValue(data[a + 36]);
        Refresher.XorValue(data[a + 37]);
        Is_enemy.XorValue(data[a + 38]);
        Base_stats = new Stats(a + 40, data);
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
        text += Is_npc.Value == 0 ? "Is_npc : false" + Environment.NewLine : "Is_npc : true" + Environment.NewLine;
        text += "Weapon: " + WeaponNames.getString(Weapon_type.Value) + Environment.NewLine;
        text += "Tome Element: " + Tome_Elem.getString(Tome_class.Value) + Environment.NewLine;
        text += "Movement Type: " + Movement.getString(Move_type.Value) + Environment.NewLine;
        text += Spawnable_Enemy.Value == 0 ? "Not randomly spawnable enemy" + Environment.NewLine : "Randomly spawnable enemy" + Environment.NewLine;
        text += Is_boss.Value == 0 ? "Normal enemy" + Environment.NewLine : "Special enemy" + Environment.NewLine;
        text += Refresher.Value == 0 ? "": "Refresher" + Environment.NewLine;
        text += Is_enemy.Value == 0 ? "Is_enemy : false" + Environment.NewLine : "Is_enemy : true" + Environment.NewLine;
        text += "5 Stars Level 1 Stats: " + Base_stats;
        Stats tmp = new Stats(Base_stats, Growth_rates);
        text += "5 Stars Level 40 Stats: " + tmp;
        text += "Growth Rates: " + Growth_rates;
        text += "BST: " + (tmp.Hp.Value + tmp.Atk.Value + tmp.Spd.Value + tmp.Def.Value + tmp.Res.Value) + Environment.NewLine;
        text += SuperBoonBane(Base_stats, Growth_rates, tmp);
        text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;

        return text;
    }

    public override string ToString_json()
    {
        String text = "{";

        text += "\"id_tag\":\"" + Id_tag + "\",";
        text += "\"roman\":\"" + Roman + "\",";
        text += "\"face_name\":" + (Roman.Value.Equals("NONE") ? "null" : "\"" + Face_name + "\"") + ",";
        text += "\"face_name2\":" + (Roman.Value.Equals("NONE") ? "null" : "\"" + Face_name2 + "\"") + ",";
        text += "\"top_weapon\":" + (TopWeapon.Value.Equals("") ? "null" : "\"" + TopWeapon + "\"") + ",";
        text += "\"unique_assist1\":" + (UniqueAssist.Value.Equals("") ? "null" : "\"" + UniqueAssist + "\"") + ",";
        text += "\"unique_assist2\":" + (UniqueAssist2.Value.Equals("") ? "null" : "\"" + UniqueAssist2 + "\"") + ",";
        text += "\"unique_special\":" + (UniqueSpecial.Value.Equals("") ? "null" : "\"" + UniqueSpecial + "\"") + ",";

        text += "\"timestamp\":" + (Timestamp.Value < 0 ? "null" : Timestamp.Value.ToString()) + ",";

        text += "\"id_num\":" + Id_num + ",";
        text += "\"is_npc\":" + (Is_npc.Value == 1 ? "true" : "false") + ","; ;
        text += "\"weapon_type\":" + Weapon_type + ",";
        text += "\"tome_class\":" + Tome_class + ",";
        text += "\"move_type\":" + Move_type + ",";

        text += "\"random_allowed\":" + (Spawnable_Enemy.Value == 1 ? "true" : "false") + ","; ;
        text += "\"is_boss\":" + (Is_boss.Value == 1 ? "true" : "false") + ","; ;
        text += "\"refresher\":" + (Refresher.Value == 1 ? "true" : "false") + ","; ;
        text += "\"is_enemy\":" + (Is_enemy.Value == 1 ? "true" : "false") + ","; ;

        text += "\"base_stats\": {";
        text += "\"hp\":" + Base_stats.Hp + ",";
        text += "\"atk\":" + Base_stats.Atk + ",";
        text += "\"spd\":" + Base_stats.Spd + ",";
        text += "\"def\":" + Base_stats.Def + ",";
        text += "\"res\":" + Base_stats.Res + "";
        text += "},";

        text += "\"growth_rates\": {";
        text += "\"hp\":" + Growth_rates.Hp + ",";
        text += "\"atk\":" + Growth_rates.Atk + ",";
        text += "\"spd\":" + Growth_rates.Spd + ",";
        text += "\"def\":" + Growth_rates.Def + ",";
        text += "\"res\":" + Growth_rates.Res + "";
        text += "}";

        text += "},";

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

    public override string ToString_json()
    {
        return ToString();
    }

}

public class SinglePerson : CharacterRelated
{
    public static readonly String[] PrintSkills = { "Default Weapon: ", "Default Assist: ", "Default Special: ", "Unknown: ", "Unknown: ", "Unknown: ", "Unlocked Weapon: ", "Unlocked Assist: ", "Unlocked Special: ", "Passive A: ", "Passive B: ", "Passive C: ", "Additional 1: ", "Additional 2: " };
    public static readonly String[] PrintSkills_kor = { "기본 무기", "기본 보조기", "기본 오의", "Unknown", "Unknown", "Unknown", "무기", "보조기", "오의", "A스킬", "B스킬", "C스킬", "추가스킬 1: ", "추가스킬 2: " };


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

    public SinglePerson() : base()
    {
        Name = "Heroes";
        ElemXor = new byte[] { 0xE1, 0xB9, 0x3A, 0x3C, 0x79, 0xAB, 0x51, 0xDE };
        Size += 41 + (5 * 8 * PrintSkills.Length);//33
        Id_num = new UInt32Xor(0x18, 0x4E, 0x6E, 0x5F);
        Version_Book_num = new UInt32Xor(0x3C, 0x3A, 0x19, 0x2E);
        Version_Chapter_num = new ByteXor(0xC8);
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

    public SinglePerson(long a, byte[] data) : this()
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
        Legendary = new Legendary(ExtractUtils.getLong(a + 8, data) + offset, data);
        if (Legendary.Bonuses != null)
            Archive.Index++;
        Dflowers = new Dragonflowers(ExtractUtils.getLong(a + 16, data) + offset, data);
        Archive.Index++;
        Timestamp.XorValue((ExtractUtils.getLong(a + 24, data)));
        Id_num.XorValue((ExtractUtils.getInt(a + 32, data)));
        Version_Chapter_num.XorValue(data[a + 36]);
        Version_Book_num.XorValue((ExtractUtils.getInt(a + 36, data)));//38 39 -> 19 2E 잇는데 의미 아직모름
        a += 16;
        Sort_value.XorValue((ExtractUtils.getInt(a + 24, data)));
        Origin.XorValue((ExtractUtils.getInt(a + 28, data)));
        Weapon_type.XorValue(data[a + 32]);
        Tome_class.XorValue(data[a + 33]);
        Move_type.XorValue(data[a + 34]);
        Series1.XorValue(data[a + 35]);
        Regular_hero.XorValue(data[a + 36]);
        Permanent_hero.XorValue(data[a + 37]);
        Base_vector_id.XorValue(data[a + 38]);
        Refresher.XorValue(data[a + 39]);
        Unknown2.XorValue(data[a + 40]);
        Base_stats = new Stats(a + 48, data);
        Base_stats.IncrementAll();
        Growth_rates = new Stats(a + 64, data);

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
        text += "Book : " + Version_Book_num.Value + " Chapter : " + (Version_Chapter_num.Value >= 12 ? "0" : Version_Chapter_num.Value.ToString()) + Environment.NewLine;
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

        for (int i = 0; i < Skills.Length / PrintSkills.Length; i++)
        {
            text += (i + 1) + " Star";
            text += i == 0 ? "" : "s";
            text += " Rarity Skills -------------------------------------------------------------------------" + Environment.NewLine;
            for (int j = 0; j < PrintSkills.Length; j++)
                text += getStuffExclusive(Skills[i, j], PrintSkills[j]);
            if (i == (Skills.Length / PrintSkills.Length) - 1)
                text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;
        }

        return text;
    }

    public override String ToString_json()
    {
        String text = "{";
        if (Roman.Value.Equals("NONE"))
        {
            return "";
        }
        text += "\"id_tag\":\"" + Id_tag+ "\",";
        text += "\"roman\":\"" + Roman + "\",";

        text += "\"face_name\":\"" + Face_name + "\",";
        text += "\"face_name2\":\"" + Face_name2 + "\",";
        if (Legendary.Bonuses != null)
        {
            text += "\"legendary\": ";
            text += Legendary.ToString_json();
            text += ",";
        }
        else
        {
            text += "\"legendary\": null,";
        }
        text += "\"dragonflowers\": ";
        text += Dflowers.ToString_json();
        text += ",";

        text += "\"timestamp\":" + (Timestamp.Value < 0 ? "null" : Timestamp.Value.ToString()) + ",";
        text += "\"id_num\":" + Id_num + ",";
        text += "\"version_num\":" + Version_Book_num.Value + ",";
        text += "\"sort_value\":" + Sort_value + ",";
        text += "\"origins\":" + Origin + ",";
        text += "\"weapon_type\":" + Weapon_type + ",";
        text += "\"tome_class\":" + Tome_class + ",";
        text += "\"move_type\":" + Move_type + ",";
        text += "\"series\":" + Series1 + ",";

        text += "\"random_pool\":" + Regular_hero + ",";
        text += "\"permanent_hero\":" + (Permanent_hero.Value != 0 ? "true" : "false") + ",";
        text += "\"base_vactor_id\":" + Base_vector_id + ",";
        text += "\"refresher\":" + (Refresher.Value != 0 ? "true" : "false") + ",";

        text += "\"base_stats\": ";
        text += Base_stats.ToString_json();
        text += ",";

        text += "\"growth_rates\": ";
        text += Growth_rates.ToString_json();
        text += ",";

        text += "\"skills\":[";

        for (int i = 0; i < Skills.Length / PrintSkills.Length; i++)
        {
            text += "[";

            for (int j = 0; j < PrintSkills.Length; j++)
            {
                if(!Skills[i, j].Value.Equals(""))
                {
                    text += "\""+Skills[i, j]+"\"";
                }
                else
                {
                    text += "null";
                }
                
                if(j != PrintSkills.Length -1)
                    text += ",";
            }
                
            text += "]";
            if (i != 4)
                text += ",";
        }

        text += "]},";
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

public class GCArea : GCRelated
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

    public override string ToString_json()
    {
        return ToString();
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

    public override string ToString_json()
    {
        return ToString();
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
    Stats new_stat_1;
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
    UInt32Xor wep_got_weakness;
    UInt32Xor mov_got_weakness;
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
    UInt32Xor limit3_id;
    Int16Xor[] limit3_params;         //2 elements!
    ByteXor range_shape;
    ByteXor target_either;
    ByteXor distant_counter;
    ByteXor canto_range;
    ByteXor pathfinder_range;
    ByteXor arcane_weapon;
    ByteXor unknown_byte_1;
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
        Size = 352;
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
        Wep_got_weakness = new UInt32Xor(0xCD, 0x9E, 0x7F, 0x64);
        Mov_got_weakness = new UInt32Xor(0x76, 0x41, 0x06, 0xB7);
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
        Limit3_id = new UInt32Xor(0x32, 0xB8, 0xBD, 0x0E);
        Limit3_params = new Int16Xor[2];
        Limit3_params[0] = new Int16Xor(0x90, 0xA5);
        Limit3_params[1] = new Int16Xor(0x90, 0xA5);
        Range_shape = new ByteXor(0x5C);
        Target_either = new ByteXor(0xA7);
        Distant_counter = new ByteXor(0xDB);
        Canto_range = new ByteXor(0x41);
        Pathfinder_range = new ByteXor(0xBE);
        Arcane_weapon = new ByteXor(0xAA);
        Unknown_Byte_1 = new ByteXor(0x01);
        /*        Ss_coin = new UInt16Xor(0x40, 0xC5);
                Ss_badge_type = new UInt16Xor(0x0F, 0xD5);
                Ss_badge = new UInt16Xor(0xEC, 0x8C);
                Ss_great_badge = new UInt16Xor(0xFF, 0xCC);
                */

    }

    public SingleSkill(long a, byte[] data) : this()
    {
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
        New_stat_1 = new Stats(a + 128, data);
        Refine_stats = new Stats(a + 144, data);
        a += 0x20;
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
        Wep_got_weakness.XorValue((ExtractUtils.getInt(a + 192, data)));
        Mov_got_weakness.XorValue((ExtractUtils.getInt(a + 196, data)));
        a += 8;
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
        Limit3_id.XorValue((ExtractUtils.getInt(a + 256, data)));
        Limit3_params[0].XorValue((ExtractUtils.getShort(a + 260, data)));
        Limit3_params[1].XorValue((ExtractUtils.getShort(a + 262, data)));
        Range_shape.XorValue(data[a + 264]);
        Target_either.XorValue(data[a + 265]);
        Distant_counter.XorValue(data[a + 266]);
        Canto_range.XorValue(data[a + 267]);
        Pathfinder_range.XorValue(data[a + 268]);
        Arcane_weapon.XorValue(data[a + 269]);
        Unknown_Byte_1.XorValue(data[a + 270]);
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
            if (!Prerequisites[i].Value.Equals(""))
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
            if (((Wep_equip.Value & tmp) >> i) == 1)
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
        text += "Something new Stats?: " + New_stat_1;
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
        text += Wep_got_weakness.Value == 0 ? "" : "New Unknown Value 1:" + Wep_got_weakness + Environment.NewLine;
        text += Mov_got_weakness.Value == 0 ? "" : "New Unknown Value 2:" + Mov_got_weakness + Environment.NewLine;
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
        if (!Passive_next.Value.Equals(""))
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
        text += "Distant counter: " + Distant_counter.Value + Environment.NewLine;
        text += "Canto Range: " + Canto_range.Value + Environment.NewLine;
        text += "Canto turn: " + Limit3_params[0] + "~" + Limit3_params[1] + Environment.NewLine;
        if (Pathfinder_range.Value != 0)
            text += "Pathfinder Range: " + Pathfinder_range.Value + Environment.NewLine;

        text += Arcane_weapon.Value == 0 ? "" : "Arcane Weapon";
        text += Unknown_Byte_1.Value == 0 ? "" : "Unknown Byte Value : " + Unknown_Byte_1.Value + Environment.NewLine;
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

    public override string ToString_json()
    {
        String text = "{";

        text += "\"id_tag\":\"" + Id_tag + "\",";
        if (Refine_base.Value.Equals(""))
        {
            text += "\"refine_base\":null,";
        }
        else
        {
            text += "\"refine_base\":\"" + Refine_base + "\",";
        }
        text += "\"name_id\":\"" + Name_id + "\",";
        text += "\"desc_id\":\"" + Desc_id + "\",";

        if(Refine_id.Value.Equals("")) { text += "\"refine_id\":null,"; }
        else { text += "\"refine_id\":\"" + Refine_id + "\","; }

        if (Beast_effect_id.Value.Equals("")) { text += "\"beast_effect_id\":null,"; }
        else { text += "\"beast_effect_id\":\"" + Beast_effect_id + "\","; }

        text += "\"prerequisites\":[";
        for (int i = 0; i < Prerequisites.Length; i++)
        {
            if (Prerequisites[i].Value.Equals("")) { text += "null"; }
            else { text += "\"" + Prerequisites[i] + "\"";}
            if(i < Prerequisites.Length - 1) { text += ","; }
        }
        text += "],";

        if (Next_skill.Value.Equals("")) { text += "\"next_skill\":null,"; }
        else { text += "\"next_skill\":\"" + Next_skill + "\","; }

        text += "\"sprites\":[";
        for (int i = 0; i < 4; i++)
        {
            if (Sprites[i].Value.Equals("")) { text += "null"; }
            else { text += "\"" + Sprites[i] + "\""; }
            if (i < 3) { text += ","; }
        }
        text += "],";

        text += "\"stats\":";
        text += Statistics.ToString_json();
        text += ",";

        text += "\"class_params\": ";
        text += Class_params.ToString_json();
        text += ",";

        text += "\"combat_buffs\": ";
        text += Skill_params.ToString_json();
        text += ",";


        text += "\"skill_params\": ";
        text += Skill_params2.ToString_json();
        text += ",";

        text += "\"skill_params2\": ";
        text += New_stat_1.ToString_json();
        text += ",";

        text += "\"refine_stats\": ";
        text += Refine_stats.ToString_json();
        text += ",";

        text += "\"id_num\":" + Num_id + ",";
        text += "\"sort_id\":" + Sort_id + ",";
        text += "\"icon_id\":" + Icon_id + ",";
        text += "\"wep_equip\":" + Wep_equip + ",";
        text += "\"mov_equip\":" + Mov_equip + ",";
        text += "\"sp_cost\":" + Sp_cost + ",";
        text += "\"category\":" + Category + ",";
        text += "\"tome_class\":" + Tome_class + ",";
        text += "\"exclusive\":" + (Exclusive.Value == 1? "true":"false") + ",";
        text += "\"enemy_only\":" + (Enemy_only.Value == 1 ? "true" : "false") + ",";
        text += "\"range\":" + Range + ",";
        text += "\"might\":" + Might + ",";
        text += "\"cooldown_count\":" + Cooldown_count + ",";
        text += "\"assist_cd\":" + (Assist_cd.Value == 1 ? "true" : "false") + ","; ;
        text += "\"healing\":" + (Healing.Value == 1 ? "true" : "false") + ","; ;
        text += "\"skill_range\":" + Skill_range + ",";
        text += "\"score\":" + Score + ",";
        text += "\"promotion_tier\":" + Promotion_tier + ",";
        text += "\"promotion_rarity\":" + Promotion_rarity + ",";
        text += "\"refined\":" + (Refined.Value == 1 ? "true" : "false") + ","; ;
        text += "\"refine_sort_id\":" + refine_sort_id + ",";
        text += "\"wep_effective\":" + Wep_effective + ",";
        text += "\"mov_effective\":" + Mov_effective + ",";
        text += "\"wep_shield\":" + Wep_shield + ",";
        text += "\"mov_shield\":" + Mov_shield + ",";
        text += "\"wep_eff_weakness\":" + Wep_got_weakness + ",";
        text += "\"mov_eff_weakness\":" + Mov_got_weakness + ",";
        text += "\"wep_weakness\":" + Wep_weakness + ",";
        text += "\"mov_weakness\":" + Mov_weakness + ",";
        text += "\"wep_adaptive\":" + Wep_adaptive + ",";
        text += "\"mov_adaptive\":" + Mov_adaptive + ",";
        text += "\"timing_id\":" + Timing_id + ",";
        text += "\"ability_id\":" + Ability_id + ",";

        text += "\"limit1_id\":" + Limit1_id + ",";
        text += "\"limit1_params\":[";
        for (int i = 0; i < Limit1_params.Length; i++)
        {
            if (Limit1_params[i].Value.Equals("")) { text += "null"; }
            else { text += Limit1_params[i]; }
            if (i < Limit1_params.Length - 1) { text += ","; }
        }
        text += "],";
        text += "\"limit2_id\":" + Limit2_id + ",";
        text += "\"limit2_params\":[";
        for (int i = 0; i < Limit2_params.Length; i++)
        {
            if(Limit2_params[i].Value.Equals("")) { text += "null"; }
            else { text +=  Limit2_params[i]; }
            if (i < Limit2_params.Length - 1) { text += ","; }
        }
        text += "],";

        text += "\"target_wep\":" + Target_wep + ",";
        text += "\"target_mov\":" + Target_mov + ",";
        text += "\"passive_next\":" + (Passive_next.Value.Equals("")?"null": "\"" + Passive_next + "\"") + ",";
        text += "\"timestamp\":" + (Timestamp.Value < 0 ? "null" : Timestamp.Value.ToString()) + ",";
        text += "\"random_allowed\":" + (Random_allowed.Value == 1 ? "true" : "false") + ","; ;
        text += "\"min_lv\":" + Min_lv + ",";
        text += "\"max_lv\":" + Max_lv + ",";
        text += "\"tt_inherit_base\":" + Tt_inherit_base + ",";
        text += "\"random_mode\":" + Random_mode + ",";

        text += "\"limit3_id\":" + Limit3_id + ",";
        text += "\"limit3_params\":[";
        for (int i = 0; i < Limit3_params.Length; i++)
        {
            if (Limit3_params[i].Value.Equals("")) { text += "null"; }
            else { text += Limit3_params[i]; }
            if (i < Limit3_params.Length - 1) { text += ","; }
        }
        text += "],";

        text += "\"range_shape\":" + Range_shape + ",";
        text += "\"target_either\":" + (Target_either.Value == 1 ? "true" : "false") + ","; ;
        text += "\"distant_counter\":" + (Distant_counter.Value == 1 ? "true" : "false") + ","; ;
        text += "\"canto_range\":" + Canto_range + ",";
        text += "\"pathfinder_range\":" + Pathfinder_range + ",";
        text += "\"arcane_weapon\":" + (Arcane_weapon.Value == 1 ? "true" : "false") + ""; ;

        text += "},";
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
    public Stats New_stat_1 { get => new_stat_1; set => new_stat_1 = value; }
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
    public UInt32Xor Wep_got_weakness { get => wep_got_weakness; set => wep_got_weakness = value; }
    public UInt32Xor Mov_got_weakness { get => mov_got_weakness; set => mov_got_weakness = value; }
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
    public UInt32Xor Limit3_id { get => limit3_id; set => limit3_id = value; }
    public Int16Xor[] Limit3_params { get => limit3_params; set => limit3_params = value; }
    public ByteXor Range_shape { get => range_shape; set => range_shape = value; }
    public ByteXor Target_either { get => target_either; set => target_either = value; }
    public ByteXor Distant_counter { get => distant_counter; set => distant_counter = value; }
    public ByteXor Canto_range { get => canto_range; set => canto_range = value; }
    public ByteXor Pathfinder_range { get => pathfinder_range; set => pathfinder_range = value; }
    public Stats Skill_params2 { get => skill_params2; set => skill_params2 = value; }
    public ByteXor Arcane_weapon { get => arcane_weapon; set => arcane_weapon = value; }
    public ByteXor Unknown_Byte_1 { get => unknown_byte_1; set => unknown_byte_1 = value; }
}

public class SingleSubscription : CommonRelated
{
    Int64Xor id_tag;
    Int64Xor avail_start;
    Int64Xor avail_finish;
    StringXor hero_id;

    public SingleSubscription()
    {
        Name = "Subscription";
        ElemXor = new byte[] { 0x54, 0x5C, 0xE5, 0x17 };
        Size = 32;
        Id_tag = new Int64Xor(0xD1, 0xD2, 0xDC, 0xCF, 0xB0, 0x8C, 0xC9, 0xBF);
        Avail_Start = new Int64Xor(0x77, 0x0F, 0xA9, 0xB4, 0xB0, 0xEB, 0xF9, 0x91);
        Avail_Finish = new Int64Xor(0x7F, 0xE8, 0xEC, 0xB9, 0xC8, 0x9C, 0xDC, 0x75);
    }

    public SingleSubscription(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {
        Id_tag.XorValue(ExtractUtils.getLong(a, data));
        Avail_Start.XorValue(ExtractUtils.getLong(a + 8, data));
        Avail_Finish.XorValue(ExtractUtils.getLong(a + 16, data));
        Hero_id = new StringXor(ExtractUtils.getLong(a + 24, data) + offset, data, Common);
        Archive.Index++;
    }

    public override string ToString()
    {
        String text = "";
        text += "Id: " + Id_tag.Value + Environment.NewLine;
        text += "Avail_Start: " + DateTimeOffset.FromUnixTimeSeconds(Avail_Start.Value).DateTime.ToLocalTime() + Environment.NewLine;
        text += "Avail_Finish: " + DateTimeOffset.FromUnixTimeSeconds(Avail_Finish.Value).DateTime.ToLocalTime() + Environment.NewLine;

        text += Table.Contains(Hero_id.Value) ? "Id: " + Table[Hero_id.Value] + Environment.NewLine : "Id: " + Hero_id.Value + Environment.NewLine;

        text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;

        return text;
    }

    public override string ToString_json()
    {
        return ToString();
    }

    public Int64Xor Id_tag { get => id_tag; set => id_tag = value; }
    public Int64Xor Avail_Start { get => avail_start; set => avail_start = value; }
    public Int64Xor Avail_Finish { get => avail_finish; set => avail_finish = value; }
    public StringXor Hero_id { get => hero_id; set => hero_id = value; }

}

public class SingleArenaPerson : CommonRelated
{
    Int64Xor avail_start;
    Int64Xor avail_finish;
    Int64Xor avail_sec;
    Int64Xor cycle_sec;
    StringXor hero_id;

    public SingleArenaPerson()
    {
        Name = "ArenaPerson";
        ElemXor = new byte[] { 0x21, 0x7E, 0x84, 0xF8, 0x7B, 0x75, 0x2E, 0x26 };//데이터 개수 세는거
        Size = 48;
        Avail_Start = new Int64Xor(0x60, 0xF6, 0x37, 0xC5, 0x36, 0xA2, 0x0D, 0xDC);
        Avail_Finish = new Int64Xor(0xE9, 0x56, 0xBD, 0xFA, 0x2A, 0x69, 0xAD, 0xC8);
        Avail_Sec = new Int64Xor(0x1F, 0x59, 0xF7, 0xBE, 0xBF, 0x0F, 0xEE, 0x8C); //-1?
        Cycle_Sec = new Int64Xor(0xDE, 0xA8, 0xE4, 0x2A, 0x36, 0x81, 0xDD, 0x94); //-1?
    }

    public SingleArenaPerson(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {
        Hero_id = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Avail_Start.XorValue(ExtractUtils.getLong(a + 8, data));
        Avail_Finish.XorValue(ExtractUtils.getLong(a + 16, data));
        Avail_Sec.XorValue(ExtractUtils.getLong(a + 24, data));
        Cycle_Sec.XorValue(ExtractUtils.getLong(a + 32, data));
        Archive.Index++;
    }

    public override string ToString()
    {
        String text = "";
        text += "Avail_Start: " + DateTimeOffset.FromUnixTimeSeconds(Avail_Start.Value).DateTime.ToLocalTime() + Environment.NewLine;
        text += "Avail_Finish: " + DateTimeOffset.FromUnixTimeSeconds(Avail_Finish.Value).DateTime.ToLocalTime() + Environment.NewLine;


        text += Table.Contains(Hero_id.Value) ? "Id: " + Table[Hero_id.Value] + Environment.NewLine : "Id: " + Hero_id.Value + Environment.NewLine;

        text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;

        return text;
    }

    public override string ToString_json()
    {
        return ToString();
    }


    public Int64Xor Avail_Start { get => avail_start; set => avail_start = value; }
    public Int64Xor Avail_Finish { get => avail_finish; set => avail_finish = value; }
    public StringXor Hero_id { get => hero_id; set => hero_id = value; }
    public Int64Xor Avail_Sec { get => avail_sec; set => avail_sec = value; }
    public Int64Xor Cycle_Sec { get => cycle_sec; set => cycle_sec = value; }

}

public class SingleCaptainSkill : CommonRelated
{
    StringXor skill_id;
    Stats params1;
    Int16Xor param1_1;
    Int16Xor param1_2;
    Int16Xor param1_3;
    Int16Xor param1_4;
    Int16Xor param1_5;
    Int16Xor param1_6;
    Stats params2;
    Int16Xor param2_1;
    Int16Xor param2_2;
    Int16Xor param2_3;
    Int16Xor param2_4;
    Int16Xor param2_5;
    Int16Xor param2_6;

    Int32Xor param3_1;
    Int32Xor param3_2;
    Int32Xor param3_3;

    public SingleCaptainSkill()
    {
        Name = "Captain Skill";
        ElemXor = new byte[] { 0x56, 0xEC, 0xBC, 0x6E, 0xED, 0x37, 0x70, 0xB7 };//데이터 개수 세는거
        Size = 88;
        Param1_1 = new Int16Xor(0x0E, 0xF4);
        Param1_2 = new Int16Xor(0x4B, 0x2A);
        Param1_3 = new Int16Xor(0x79, 0xCC);
        Param1_4 = new Int16Xor(0xE0, 0xA6);
        Param1_5 = new Int16Xor(0x2B, 0x66);
        Param1_6 = new Int16Xor(0x27, 0xCB);

        Param2_1 = new Int16Xor(0x0E, 0xF4);
        Param2_2 = new Int16Xor(0x4B, 0x2A);
        Param2_3 = new Int16Xor(0x79, 0xCC);
        Param2_4 = new Int16Xor(0xE0, 0xA6);
        Param2_5 = new Int16Xor(0x2B, 0x66);
        Param2_6 = new Int16Xor(0x27, 0xCB);

        Param3_1 = new Int32Xor(0x40, 0x7C, 0xDC, 0x54);
        Param3_2 = new Int32Xor(0xBF, 0xB4, 0xD1, 0xC3);
        Param3_3 = new Int32Xor(0x22, 0x49, 0x34, 0x7B);

    }

    public SingleCaptainSkill(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {
        Skill_id = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Params1 = new Stats(a + 8, data);
        Param1_1.XorValue(ExtractUtils.getShort(a + 24, data));
        Param1_2.XorValue(ExtractUtils.getShort(a + 26, data));
        Param1_3.XorValue(ExtractUtils.getShort(a + 28, data));
        Param1_4.XorValue(ExtractUtils.getShort(a + 30, data));
        Param1_5.XorValue(ExtractUtils.getShort(a + 32, data));
        Param1_6.XorValue(ExtractUtils.getShort(a + 34, data));

        Params2 = new Stats(a + 40, data);
        param2_1.XorValue(ExtractUtils.getShort(a + 56, data));
        param2_2.XorValue(ExtractUtils.getShort(a + 58, data));
        param2_3.XorValue(ExtractUtils.getShort(a + 60, data));
        param2_4.XorValue(ExtractUtils.getShort(a + 62, data));
        param2_5.XorValue(ExtractUtils.getShort(a + 64, data));
        param2_6.XorValue(ExtractUtils.getShort(a + 66, data));

        Param3_1.XorValue(ExtractUtils.getInt(a + 72, data));
        Param3_2.XorValue(ExtractUtils.getInt(a + 76, data));
        Param3_3.XorValue(ExtractUtils.getInt(a + 80, data));

        Archive.Index++;
    }

    public override string ToString()
    {
        String text = "";

        text += "Skill Id : " + Skill_id.Value + Environment.NewLine;

        text += "Stats 1 : " + Params1 + Environment.NewLine;
        text += "Values 1 : " + Param1_1 + " " + Param1_2 + " " + Param1_3 + " " + Param1_4 + " " + Param1_5 + " " + Param1_6 + " " + Environment.NewLine;

        text += "Stats 2 : " + Params2 + Environment.NewLine;
        text += "Values 2 : " + Param2_1 + " " + Param2_2 + " " + Param2_3 + " " + Param2_4 + " " + Param2_5 + " " + Param2_6 + " " + Environment.NewLine;

        text += "Values 3 : " + Param3_1 + " " + Param3_2 + " " + Param3_3 + " " + Environment.NewLine;

        text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;

        return text;
    }

    public override string ToString_json()
    {
        String text = "";

        //MID_REALTIME_PVP_SKILL_一騎当千
        //MID_REALTIME_PVP_SKILL_H_一騎当千

        text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;

        return text;
    }


    public StringXor Skill_id { get => skill_id; set => skill_id = value; }
    public Stats Params1 { get => params1; set => params1 = value; }
    public Int16Xor Param1_1 { get => param1_1; set => param1_1 = value; }
    public Int16Xor Param1_2 { get => param1_2; set => param1_2 = value; }
    public Int16Xor Param1_3 { get => param1_3; set => param1_3 = value; }
    public Int16Xor Param1_4 { get => param1_4; set => param1_4 = value; }
    public Int16Xor Param1_5 { get => param1_5; set => param1_5 = value; }
    public Int16Xor Param1_6 { get => param1_6; set => param1_6 = value; }

    public Stats Params2 { get => params2; set => params2 = value; }
    public Int16Xor Param2_1 { get => param2_1; set => param2_1 = value; }
    public Int16Xor Param2_2 { get => param2_2; set => param2_2 = value; }
    public Int16Xor Param2_3 { get => param2_3; set => param2_3 = value; }
    public Int16Xor Param2_4 { get => param2_4; set => param2_4 = value; }
    public Int16Xor Param2_5 { get => param2_5; set => param2_5 = value; }
    public Int16Xor Param2_6 { get => param2_6; set => param2_6 = value; }

    public Int32Xor Param3_1 { get => param3_1; set => param3_1 = value; }
    public Int32Xor Param3_2 { get => param3_2; set => param3_2 = value; }
    public Int32Xor Param3_3 { get => param3_3; set => param3_3 = value; }

}
public class SingleWeaponRefine : CommonRelated
{

    StringXor originalSid;
    StringXor refinedSid;

    UInt16Xor resType1;
    UInt16Xor count1;

    UInt16Xor resType2;
    UInt16Xor count2;


    UInt16Xor resType3;
    UInt16Xor count3;






    public SingleWeaponRefine()
    {
        Name = "Weapon Refine";
        ElemXor = new byte[] { 0x73, 0xFD, 0x2C, 0x43, 0x00, 0x2C, 0x16, 0x45 };//데이터 개수 세는거
        Size = 32;

        ResType1 = new UInt16Xor(0x9C, 0x43);
        Count1 = new UInt16Xor(0x44, 0x74);

        ResType2 = new UInt16Xor(0x9C, 0x43);
        Count2 = new UInt16Xor(0x44, 0x74);

        ResType3 = new UInt16Xor(0x9C, 0x43);
        Count3 = new UInt16Xor(0x44, 0x74);


    }

    public SingleWeaponRefine(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {
        OriginSid = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Archive.Index++;
        RefinedSid = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
        Archive.Index++;

        ResType1.XorValue(ExtractUtils.getShort(a + 16, data));
        Count1.XorValue(ExtractUtils.getShort(a + 18, data));
        ResType2.XorValue(ExtractUtils.getShort(a + 20, data));
        Count2.XorValue(ExtractUtils.getShort(a + 22, data));
        ResType3.XorValue(ExtractUtils.getShort(a + 24, data));
        Count3.XorValue(ExtractUtils.getShort(a + 26, data));
    }

    public override string ToString()
    {
        String text = "";


        text += "Origin Skill Id : " + OriginSid.Value + Environment.NewLine;
        text += "Refined Skill Id : " + RefinedSid.Value + Environment.NewLine;

        text += "Use 1 : res_type : " + ResType1.Value + ",  count : " + Count1.Value + Environment.NewLine;
        text += "Use 2 : res_type : " + ResType2.Value + ",  count : " + Count2.Value + Environment.NewLine;
        text += "Give : res_type : " + ResType3.Value + ",  count : " + Count3.Value + Environment.NewLine;

        text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;

        return text;
    }

    public override string ToString_json()
    {
        String text = "{";
        text += "\"orig\":\"" + OriginSid + "\",";
        text += "\"refined\":\"" + RefinedSid + "\",";

        text += "\"use\":[{";
        text += "\"res_type\":" + ResType1 + ",";
        text += "\"count\":" + Count1;
        text += "},{";
        text += "\"res_type\":" + ResType2 + ",";
        text += "\"count\":" + Count2;
        text += "}],";

        text += "\"give\":{";
        text += "\"res_type\":" + ResType3 + ",";
        text += "\"count\":" + Count3;
        text += "}";

        text += "},";
        return text;
    }


    public StringXor OriginSid { get => originalSid; set => originalSid = value; }
    public StringXor RefinedSid { get => refinedSid; set => refinedSid = value; }

    public UInt16Xor ResType1 { get => resType1; set => resType1 = value; }
    public UInt16Xor Count1 { get => count1; set => count1 = value; }

    public UInt16Xor ResType2 { get => resType2; set => resType2 = value; }
    public UInt16Xor Count2 { get => count2; set => count2 = value; }

    public UInt16Xor ResType3 { get => resType3; set => resType3 = value; }
    public UInt16Xor Count3 { get => count3; set => count3 = value; }
}

public class SingleSkillAccessory : CommonRelated
{

    StringXor idTag;
    StringXor nextIdTag;
    StringXor prevIdTag;

    UInt16Xor ssCoin;
    UInt16Xor ssBadgeType;
    UInt16Xor ssBadge;
    UInt16Xor ssGreatBadge;








    public SingleSkillAccessory()
    {
        Name = "Sacred Seal";
        ElemXor = new byte[] { 0xD6, 0xD6, 0xA9, 0x46, 0x47, 0x3E, 0x6B, 0x16 };//데이터 개수 세는거
        Size = 32;

        SsCoin = new UInt16Xor(0x40, 0xC5);
        SsBadgeType = new UInt16Xor(0x0F, 0xD5);
        SsBadge = new UInt16Xor(0xEC, 0x8C);
        SsGreatBadge = new UInt16Xor(0xFF, 0xCC);


    }

    public SingleSkillAccessory(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {
        idTag = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Archive.Index++;
        nextIdTag = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
        Archive.Index++;
        prevIdTag = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, Common);
        Archive.Index++;

        SsCoin.XorValue(ExtractUtils.getShort(a + 24, data));
        SsBadgeType.XorValue(ExtractUtils.getShort(a + 26, data));
        SsBadge.XorValue(ExtractUtils.getShort(a + 28, data));
        SsGreatBadge.XorValue(ExtractUtils.getShort(a + 30, data));

    }

    public override string ToString()
    {
        String text = "";


        text += "Skill Id : " + idTag.Value + Environment.NewLine;
        if (!nextIdTag.Value.Equals(""))
            text += "Next Skill Id : " + nextIdTag.Value + Environment.NewLine;
        if (!prevIdTag.Value.Equals(""))
            text += "Prev Skill Id : " + prevIdTag.Value + Environment.NewLine;
        text += "Required Coin : " + SsCoin.Value + Environment.NewLine;
        text += "Required Badge Type : " + BadgeColor.getString(SsBadgeType.Value) + Environment.NewLine;
        text += "Required Normal Badge : " + SsBadge.Value + Environment.NewLine;
        text += "Required Great Badge : " + SsGreatBadge.Value + Environment.NewLine;



        text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;

        return text;
    }

    public override string ToString_json()
    {
        String text = "{";

        text += "\"id_tag\":\"" + IdTag + "\",";
        if (NextIdTag.Value.Equals(""))
        {
            text += "\"next_seal\":null,";
        }
        else
        {
            text += "\"next_seal\":\"" + NextIdTag + "\",";
        }
        if (PrevIdTag.Value.Equals(""))
        {
            text += "\"prev_seal\":null,";
        }
        else
        {
            text += "\"prev_seal\":\"" + PrevIdTag + "\",";
        }

        text += "\"ss_coin\":" + SsCoin + ",";
        text += "\"ss_badge_type\":" + SsBadgeType + ",";
        text += "\"ss_badge\":" + SsBadge + ",";
        text += "\"ss_great_badge\":" + SsGreatBadge ;
        text += "},";

        return text;
    }

    public StringXor IdTag { get => idTag; set => idTag = value; }
    public StringXor NextIdTag { get => nextIdTag; set => nextIdTag = value; }
    public StringXor PrevIdTag { get => prevIdTag; set => prevIdTag = value; }

    public UInt16Xor SsCoin { get => ssCoin; set => ssCoin = value; }
    public UInt16Xor SsBadgeType { get => ssBadgeType; set => ssBadgeType = value; }
    public UInt16Xor SsBadge { get => ssBadge; set => ssBadge = value; }
    public UInt16Xor SsGreatBadge { get => ssGreatBadge; set => ssGreatBadge = value; }


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

    public override string ToString_json()
    {
        return ToString() + base.ToString();
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
    public List<byte> Byte_output = new List<byte>();
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

    public override void InsertInJp(long a, byte[] data)
    {
        NumElem = ExtractUtils.getLong(a, data);
        TranslatedMessages = new string[NumElem, 2];
        for (int i = 0; i < NumElem; i++)
        {
            TranslatedMessages[i, 0] = new StringXor(ExtractUtils.getLong(a + 8 + (i * Size), data) + offset, data, MSG).ToString();
            Archive.Index++;
            TranslatedMessages[i, 1] = new StringXor(ExtractUtils.getLong(a + 0x10 + (i * Size), data) + offset, data, MSG).ToString();
            Archive.Index++;
            if (!TableJP.Contains(TranslatedMessages[i, 0]))
            {
                TableJP.Add(TranslatedMessages[i, 0], TranslatedMessages[i, 1]);
                System.Diagnostics.Debug.WriteLine(TranslatedMessages[i, 0] + TranslatedMessages[i, 1] + TableJP[TranslatedMessages[i, 0]]);
            }
        }
    }

    public override void InsertInKr()
    {

    }

    public override string ToString()
    {
        String text = "";
        for (int i = 0; i < NumElem; i++)
            text += TranslatedMessages[i, 0] + ": " + TranslatedMessages[i, 1] + Environment.NewLine;
        return text;
    }

    public override string ToString_json()
    {
        String text = "";
        for (int i = 0; i < NumElem; i++)
        {
            text += TranslatedMessages[i, 0] + TranslatedMessages[i, 1];
        }
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

    public override string ToString_json()
    {
        String text = "";
        for (int i = 0; i < NumElem.Value; i++)
            text += Things[i].ToString_json();
        return text;
    }

}


public class BaseExtractArchiveInteger<T> : ExtractionBase where T : ExtractionBase, new()
{
    private Int32Xor numElem;
    private T[] things;

    public Int32Xor NumElem { get => numElem; set => numElem = value; }
    internal T[] Things { get => things; set => things = value; }

    public BaseExtractArchiveInteger()
    {
        T tmp = new T();
        Name = tmp.Name;
        NumElem = new Int32Xor(tmp.ElemXor);
    }
    public BaseExtractArchiveInteger(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }
    public override void InsertIn(long a, byte[] data)
    {
        a = Archive.Ptr_list[Archive.Index];
        NumElem.XorValue(ExtractUtils.getInt(a + 8, data));
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

    public override string ToString_json()
    {
        String text = "";
        for (int i = 0; i < NumElem.Value; i++)
            text += Things[i].ToString_json();
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

    public override string ToString_json()
    {
        return ToString() + base.ToString();
    }

}