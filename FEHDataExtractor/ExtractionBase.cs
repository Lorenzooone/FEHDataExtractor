using System;

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
}

public abstract class ExtractionBase
{
    public static int offset = 0x20;

    private HSDARC archive;

    public abstract void InsertIn(long a, byte[] data);
    public void InsertIn(HSDARC archive, long a, byte[] data) {
        Archive = archive;
        InsertIn(a, data);
    }

    public string Name { get; set; }
    public HSDARC Archive { get => archive; set => archive = value; }
}
public abstract class GCRelated : ExtractionBase
{
    public byte[] GC = { 0x17, 0xFC, 0xC9, 0xEA, 0x79, 0x69, 0x24, 0xBD, 0xA4, 0x54, 0x0E, 0x58, 0xBD, 0x8B, 0x36, 0xCD, 0xAF, 0xB4, 0xE2, 0x09, 0x3C, 0x1F, 0x8C, 0x9C, 0xD1, 0x48, 0x51, 0xA1, 0xFB, 0xAD, 0x48, 0x7E, 0xC3, 0x38, 0x5A, 0x41 };
}
public abstract class CommonRelated : ExtractionBase
{
    public byte[] Common = { 0x81, 0x00, 0x80, 0xA4, 0x5A, 0x16, 0x6F, 0x78, 0x57, 0x81, 0x2D, 0xF7, 0xFC, 0x66, 0x0F, 0x27, 0x75, 0x35, 0xB4, 0x34, 0x10, 0xEE, 0xA2, 0xDB, 0xCC, 0xE3, 0x35, 0x99, 0x43, 0x48, 0xD2, 0xBB, 0x93, 0xC1 };
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

    public static readonly String[] Weapons = {"Red Sword", "Blue Lance", "Green Axe", "Red Bow", "Blue Bow", "Green Bow", "Colorless Bow",  "Red Dagger", "Blue Dagger", "Green Dagger", "Colorless Dagger", "Red Tome", "Blue Tome", "Green Tome", "Colorless Staff", "Red Breath", "Blue Breath", "Green Breath", "Colorless Breath"};
    public static readonly String[] Tome_Elem = {"None", "Fire", "Thunder", "Wind", "Light", "Dark"};
    public static readonly String[] Movement = {"Infantry", "Armored", "Cavalry", "Flying"};
    public static readonly String[] Series = {"Heroes", "Shadow Dragon and the Blade of Light / Mystery of the Emblem / Shadow Dragon / New Mystery of the Emblem", "Gaiden / Echoes", "Genealogy of the Holy War", "Thracia 776", "The Binding Blade", "The Blazing Blade", "The Sacred Stones", "Path of Radiance", "Radiant Dawn", "Awakening", "Fates"};

    public CharacterRelated()
    {
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
        Hp.Value = (short)(Level1.Hp.Value + (0.39 * (Growths.Hp.Value * 1.14 + 0.005) + 0.005));
        Atk.Value = (short)(Level1.Atk.Value + (0.39 * (Growths.Atk.Value * 1.14 + 0.005) + 0.005));
        Spd.Value = (short)(Level1.Spd.Value + (0.39 * (Growths.Spd.Value * 1.14 + 0.005) + 0.005));
        Def.Value = (short)(Level1.Def.Value + (0.39 * (Growths.Def.Value * 1.14 + 0.005) + 0.005));
        Res.Value = (short)(Level1.Res.Value + (0.39 * (Growths.Res.Value * 1.14 + 0.005) + 0.005));
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

public class Legendary : ExtractionBase
{
    private Stats bonuses;
    private ByteXor element;
    private static readonly String[] LegendaryElement = {"Fire", "Water", "Wind", "Earth", "Light", "Dark", "Astra", "Anima"};

    public Legendary()
    {
        Name = "";
        Element = new ByteXor(5);
    }

    public Legendary(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    override public String ToString()
    {
        String text = "Bonus Stats: " + Bonuses;
        text += "Element: " + LegendaryElement[Element.Value] + Environment.NewLine;

        return text;
    }

    public override void InsertIn(long a, byte[] data)
    {
        if (a != offset)
        {
            Bonuses = new Stats(a, data);
            Element.XorValue(data[a + 0x10]);
        }
    }

    public Stats Bonuses { get => bonuses; set => bonuses = value; }
    public ByteXor Element { get => element; set => element = value; }
}

public class Enemy : CharacterRelated
{
    StringXor topWeapon;
    ByteXor _spawnable_Enemy;              // XOR cipher: C5
    ByteXor is_boss;                // XOR cipher: 6A

    public ByteXor Is_boss { get => is_boss; set => is_boss = value; }
    public ByteXor Spawnable_Enemy { get => _spawnable_Enemy; set => _spawnable_Enemy = value; }
    public StringXor TopWeapon { get => topWeapon; set => topWeapon = value; }

    public Enemy() : base()
    {
        Name = "Enemies";
        Id_num = new UInt32Xor(0xD4, 0x41, 0x2F, 0x42);
        Weapon_type = new ByteXor(0xE4);
        Tome_class = new ByteXor(0x81);
        Move_type = new ByteXor(0x0D);
        Spawnable_Enemy = new ByteXor(0xC5);
        Is_boss = new ByteXor(0x6A);
    }

    public Enemy(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    // 7 bytes of padding

    public override void InsertIn(long a, byte[] data)
    {
        if (Archive.Index == 0)
            Archive.Index++;
        Id_tag = new StringXor(ExtractUtils.getLong(Archive.Ptr_list[Archive.Index++], data) + offset, data, Common);
        Roman = new StringXor(ExtractUtils.getLong(Archive.Ptr_list[Archive.Index++], data) + offset, data, Common);
        if (Roman.Value.Equals("NONE"))
        {
            return;
        }
        Face_name = new StringXor(ExtractUtils.getLong(Archive.Ptr_list[Archive.Index++], data) + offset, data, Common);
        a = Archive.Ptr_list[Archive.Index++];
        Face_name2 = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
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
        String text = "Internal Identifier: " + Id_tag + Environment.NewLine;
        text += "Romanized Identifier: " + Roman + Environment.NewLine;
        if (Roman.Value.Equals("NONE"))
        {
            return text + "-----------------------------------------------------------------------------------------------" + Environment.NewLine;
        }
        text += "Face Folder: " + Face_name + Environment.NewLine;
        text += "Face Folder no. 2: " + Face_name2 + Environment.NewLine;
        if (!TopWeapon.Value.Equals(""))
            text += "Default Weapon: " + TopWeapon + Environment.NewLine;
        text += "Timestamp: ";
        text += Timestamp.Value < 0 ? "Not available" + Environment.NewLine : DateTimeOffset.FromUnixTimeSeconds(Timestamp.Value).DateTime.ToLocalTime() + Environment.NewLine;
        text += "ID: " + Id_num + Environment.NewLine;
        text += "Weapon: " + Weapons[Weapon_type.Value] + Environment.NewLine;
        text += "Tome Element: " + Tome_Elem[Tome_class.Value] + Environment.NewLine;
        text += "Movement Type: " + Movement[Move_type.Value] + Environment.NewLine;
        text += Spawnable_Enemy.Value == 0 ? "Randomly spawnable enemy" + Environment.NewLine : "Not randomly spawnable enemy" + Environment.NewLine;
        text += Is_boss.Value == 0 ? "Normal enemy" + Environment.NewLine : "Special enemy" + Environment.NewLine;
        text += "5 Stars Level 1 Stats: " + Base_stats;
        Stats tmp = new Stats(Base_stats, Growth_rates);
        text += "5 Stars Level 40 Stats: " + tmp;
        text += "Growth Rates: " + Growth_rates;
        text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;

        return text;
    }

}

public class Hero : CharacterRelated
{
    public static readonly String[] PrintSkills = {"Default Weapon: ", "Default Assist: ", "Default Special: ", "Unknown: ", "Unknown: ", "Unknown: ", "Unlocked Weapon: ", "Unlocked Assist: ", "Unlocked Special: ", "Passive A: ", "Passive B: ", "Passive C: ", "Unknown: ", "Unknown: "};

    Legendary legendary;
    UInt32Xor sort_value;
    ByteXor series;
    ByteXor regular_hero;
    ByteXor permanent_hero;
    ByteXor base_vector_id;
    ByteXor refresher;
    ByteXor _unknown2;
    // 7 bytes of padding
    Stats max_stats;
    StringXor[,] skills;

    public Hero():base()
    {
        Name = "Heroes";
        Id_num = new UInt32Xor(0x18, 0x4E, 0x6E, 0x5F);
        Sort_value = new UInt32Xor(0x9B, 0x34, 0x80, 0x2A);
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

    public Hero(long a, byte[] data) : this() {
        InsertIn(a, data);
    }

    public override void InsertIn(long a, byte[] data)
    {
        if (Archive.Index == 0)
            Archive.Index++;
        Id_tag = new StringXor(ExtractUtils.getLong(Archive.Ptr_list[Archive.Index++], data) + offset, data, Common);
        Roman = new StringXor(ExtractUtils.getLong(Archive.Ptr_list[Archive.Index++], data) + offset, data, Common);
        if (Roman.Value.Equals("NONE"))
        {
            return;
        }
        Face_name = new StringXor(ExtractUtils.getLong(Archive.Ptr_list[Archive.Index++], data) + offset, data, Common);
        a = Archive.Ptr_list[Archive.Index++];
        Face_name2 = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
        Legendary = new Legendary(ExtractUtils.getLong(a + 8, data) + offset, data);
        if (Legendary.Bonuses != null)
            Archive.Index++;
        Timestamp.XorValue((ExtractUtils.getLong(a + 16, data)));
        Id_num.XorValue((ExtractUtils.getInt(a + 24, data)));
        Sort_value.XorValue((ExtractUtils.getInt(a + 28, data)));
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
        Max_stats = new Stats(a + 80, data);
        for (int i = 0; i < Skills.Length / PrintSkills.Length; i++)
            for (int j = 0; j < PrintSkills.Length; j++)
            {
                Skills[i, j] = new StringXor(ExtractUtils.getLong((j * 8) + (i * PrintSkills.Length * 8) + a + 96, data) + offset, data, Common);
                if (!Skills[i, j].ToString().Equals(""))
                {
                    Archive.Index++;
                }
            }
    }

    public override string ToString()
    {
        String text = "Internal Identifier: " + Id_tag + Environment.NewLine;
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
        text += "Weapon: " + Weapons[Weapon_type.Value] + Environment.NewLine;
        text += "Tome Element: " + Tome_Elem[Tome_class.Value] + Environment.NewLine;
        text += "Movement Type: " + Movement[Move_type.Value] + Environment.NewLine;
        text += "Series: " + Series[Series1.Value] + Environment.NewLine;
        text += Regular_hero.Value == 0 ? "Not randomly spawnable hero" + Environment.NewLine : "Randomly spawnable hero" + Environment.NewLine;
        text += Permanent_hero.Value == 0 ? "Can be sent home and merged" + Environment.NewLine : "Cannot be sent home or merged" + Environment.NewLine;
        text += "BVID: " + Base_vector_id + Environment.NewLine;
        text += Refresher.Value == 0 ? "Cannot learn Sing/Dance" + Environment.NewLine : "Can learn Sing/Dance" + Environment.NewLine;
        text += "5 Stars Level 1 Stats: " + Base_stats;
        Stats tmp = new Stats(Base_stats, Growth_rates);
        text += "5 Stars Level 40 Stats: " + tmp;
        text += "Growth Rates: " + Growth_rates;
        text += "Enemy Stats: " + Max_stats;
        for (int i = 0; i < Skills.Length / PrintSkills.Length; i++)
        {
            text += (i + 1) + " Star";
            text += i == 0 ? "" : "s";
            text += " Rarity Skills -------------------------------------------------------------------------" + Environment.NewLine;
            for (int j = 0; j < PrintSkills.Length; j++)
                text += PrintSkills[j] + Skills[i, j] + Environment.NewLine;
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
    public Stats Max_stats { get => max_stats; set => max_stats = value; }
    public StringXor[,] Skills { get => skills; set => skills = value; }

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
            text += "Neighbour " + i + ": " + Neighbours[i] + Environment.NewLine;
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
        long address = Archive.Ptr_list[Archive.Index++];
        Image = new StringXor(ExtractUtils.getLong(address, data) + offset, data, GC);
        Unknown1.XorValue((ExtractUtils.getLong(address + 8, data)));
        AreaCount.XorValue((ExtractUtils.getLong(address + 16, data)));
        Areas = new GCArea[AreaCount.Value];
        long[] pointers = new long[AreaCount.Value];
        long pos = ExtractUtils.getLong(Archive.Ptr_list[Archive.Index++], data);
        for (long i = 0; i < pointers.Length; i++)
        {
            pointers[i] = Archive.Ptr_list[Archive.Index++];
        }
        for (long i = 0; i < pointers.Length; i++)
        {
            Areas[i] = new GCArea(ExtractUtils.getLong(pointers[i], data) + offset, data, Archive);
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

public class GenericText : CommonRelated
{
    private StringXor[] elements;
    public GenericText()
    {
        Name = "Generic Text";
    }

    public GenericText(long a, byte[] data) : this()
    {
        InsertIn(a, data);
    }

    public StringXor[] Elements { get => elements; set => elements = value; }

    public override void InsertIn(long a, byte[] data)
    {
        Elements = new StringXor[Archive.Ptr_list_length];
        for (long i = 0; i < Elements.Length; i++)
        {
            Elements[i] = new StringXor(ExtractUtils.getLong(Archive.Ptr_list[Archive.Index++], data) + offset, data, Common);
        }
    }

    public override String ToString()
    {
        String text = "";
        for (int i = 0; i < Elements.Length; i++)
        {
            text += Elements[i] + Environment.NewLine;
        }

        return text;
    }

}