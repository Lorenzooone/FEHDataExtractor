using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEHDataExtractor
{
    public class QuestUnitMatch : CommonRelated
    {
        StringXor hero_id;
        Int32Xor color;
        Int32Xor wep_type;
        Int32Xor mov_type;
        Int16Xor lv;
        UInt16Xor _unknown1;
        UInt16Xor blessing;
        UInt16Xor blessed;

        public QuestUnitMatch()
        {
            Size = 32;
            Color = new Int32Xor(0x65, 0xAD, 0x72, 0x4D);
            Wep_type = new Int32Xor(0xAC, 0x5B, 0x43, 0x66);
            Mov_type = new Int32Xor(0x6F, 0x43, 0x29, 0xE6);
            Lv = new Int16Xor(0x3F, 0x4F);
            Unknown1 = new UInt16Xor(0xDE, 0xC8);
            Blessing = new UInt16Xor(0xB4, 0x7D);
            Blessed = new UInt16Xor(0xB2, 0x76);
        }

        public QuestUnitMatch(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }

        public StringXor Hero_id { get => hero_id; set => hero_id = value; }
        public Int32Xor Color { get => color; set => color = value; }
        public Int32Xor Wep_type { get => wep_type; set => wep_type = value; }
        public Int32Xor Mov_type { get => mov_type; set => mov_type = value; }
        public Int16Xor Lv { get => lv; set => lv = value; }
        public UInt16Xor Unknown1 { get => _unknown1; set => _unknown1 = value; }
        public UInt16Xor Blessing { get => blessing; set => blessing = value; }
        public UInt16Xor Blessed { get => blessed; set => blessed = value; }

        public override string ToString()
        {
            string text = "";
            if (!Hero_id.Value.Equals(""))
                text += Hero_id + ", ";
            text += Color.Value != -1 ? "Color: " + Colours[Color.Value] + ", " : "";
            text += Wep_type.Value != -1 ? "Weapon: " + Weapons[Wep_type.Value] + ", " : "";
            text += Mov_type.Value != -1 ? "Movement: " + Movement[Mov_type.Value] + ", " : "";
            text += Lv.Value != -1 ? "At least level: " + Lv + ", " : "";
            text += Blessing.Value != 0 ? "Legendary/Blessed: " + LegendaryElement[Blessing.Value - 1] + ", " : "";
            text += Blessed.Value != 0 ? "Blessed: " + LegendaryElement[Blessed.Value - 1] + ", " : "";
            if (!text.Equals(""))
                text = text.Remove(text.Length - 2);
            return text;
        }


        public override void InsertIn(long a, byte[] data)
        {
            Hero_id = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
            if (!Hero_id.Value.Equals(""))
                Archive.Index++;
            Color.XorValue(ExtractUtils.getInt(a + 8, data));
            Wep_type.XorValue(ExtractUtils.getInt(a + 12, data));
            Mov_type.XorValue(ExtractUtils.getInt(a + 16, data));
            Lv.XorValue(ExtractUtils.getShort(a + 20, data));
            Unknown1.XorValue(ExtractUtils.getShort(a + 22, data));
            Blessing.XorValue(ExtractUtils.getShort(a + 24, data));
            Blessed.XorValue(ExtractUtils.getShort(a + 26, data));
        }
    }

    public class Quest_definition : CommonRelated
    {
        StringXor quest_id;
        StringXor common_id;
        UInt32Xor times;                    // XOR cipher: 0D 7A F1 2C
        UInt32Xor trigger;                  // XOR cipher: DF 48 33 E3
        StringXor map_group;
        UInt32Xor game_mode;                // XOR cipher: BA C0 0C 1C
        Int32Xor difficulty;                // XOR cipher: BB 84 05 F3
        UInt32Xor _unknown1;                // XOR cipher: 35 39 26 54
        Int32Xor survive;                   // XOR cipher: DE 41 B1 C2
        UInt64Xor _unknown2;                // XOR cipher: 5B 00 00 00 00 00 00 00
        UInt32Xor game_mode2;               // XOR cipher: A9 19 8E 41
                                           // 4 bytes of padding
        StringXor map_id;
        QuestUnitMatch unit_reqs;
        QuestUnitMatch foe_reqs;
        Reward_Payload reward;
        Int32Xor payload_size;             // XOR cipher: 62 D6 5A 74

        public static readonly string[] TriggerType = { "", "On foe defeat", "On scenario clear", "On Arena Assault clear", "On Tap Battle floor clear", "On Tap Battle boss clear" };
        public static readonly string[] GameModeType = { "", "Normal Map", "", "Special Map", "", "Training Tower", "Arena Duel", "Voting Gauntlet", "Tempest Trials", "", "", "Arena Assault", "Tap Battle", "", "Grand Conquests", "", "", "Aether Raids" };
        public static readonly string[] Difficulties = { "", "Hard", "Lunatic", "Infernal", "", "", "Intermediate", "Advanced", "", "", "", "consecutive battles to win" };


        public Quest_definition()
        {
            Size = 88;
            Times = new UInt32Xor(0x0D, 0x7A, 0xF1, 0x2C);
            Trigger = new UInt32Xor(0xDF, 0x48, 0x33, 0xE3);
            Game_mode = new UInt32Xor(0xBA, 0xC0, 0x0C, 0x1C);
            Difficulty = new Int32Xor(0xBB, 0x84, 0x05, 0xF3);
            Unknown1 = new UInt32Xor(0x35, 0x39, 0x26, 0x54);
            Survive = new Int32Xor(0xDE, 0x41, 0xB1, 0xC2);
            Unknown2 = new UInt64Xor(0x5B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00);
            Game_mode2 = new UInt32Xor(0xA9, 0x19, 0x8E, 0x41);
            Unit_reqs = new QuestUnitMatch();
            Foe_reqs = new QuestUnitMatch();
            Payload_size = new Int32Xor(0x62, 0xD6, 0x5A, 0x74);
            Size += Unit_reqs.Size + Foe_reqs.Size;
        }

        public Quest_definition(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }

        public override string ToString()
        {
            string text = "ID: " + Quest_id + Environment.NewLine;
            if (!Common_id.Value.Equals(""))
                text += "Shared ID: " + Common_id + Environment.NewLine;
            text += "Has to be done: " + Times + (Times.Value == 1 ? " Time":" Times") + Environment.NewLine;
            text += TriggerType[Trigger.Value] + (Trigger.Value == 0 ? "" : Environment.NewLine);
            text += Map_group.Value.Equals("") ? "" : "Map Group: " + Map_group + Environment.NewLine;
            text += Game_mode.Value == 0 ? "" : "Game Mode: " + GameModeType[Game_mode.Value] + Environment.NewLine;
            text += Difficulty.Value == -1 ? "" : "Difficulty: " + (Game_mode.Value <= 3 ? Difficulties[Difficulty.Value] : (Game_mode.Value == 6 ? Difficulties[Difficulty.Value + 5] : (Difficulty.Value == 1 ? "1 battle to win" : Difficulty.Value + " " + Difficulties[11]))) + Environment.NewLine;
            text += Survive.Value == -1 ? "" : Survive + " units need to survive" + Environment.NewLine;
            text += Map_id.Value.Equals("")? "" : "Map ID: " + Map_id + Environment.NewLine;
            text += "Unit requirements: " + Unit_reqs + Environment.NewLine;
            text += "Enemy requirements: " + Foe_reqs + Environment.NewLine;
            text += Reward;
            return text;
        }

        public override void InsertIn(long a, byte[] data)
        {
            Quest_id = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
            Archive.Index++;
            Common_id = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
            if (!Common_id.Value.Equals(""))
                Archive.Index++;
            Times.XorValue(ExtractUtils.getInt(a + 16, data));
            Trigger.XorValue(ExtractUtils.getInt(a + 20, data));
            Map_group = new StringXor(ExtractUtils.getLong(a + 24, data) + offset, data, Common);
            if (!Map_group.Value.Equals(""))
                Archive.Index++;
            Game_mode.XorValue(ExtractUtils.getInt(a + 32, data));
            Difficulty.XorValue(ExtractUtils.getInt(a + 36, data));
            Unknown1.XorValue(ExtractUtils.getInt(a + 40, data));
            Survive.XorValue(ExtractUtils.getInt(a + 44, data));
            Unknown2.XorValue(ExtractUtils.getLong(a + 48, data));
            Game_mode2.XorValue(ExtractUtils.getInt(a + 56, data));
            Map_id = new StringXor(ExtractUtils.getLong(a + 64, data) + offset, data, Common);
            if (!Map_id.Value.Equals(""))
                Archive.Index++;
            Unit_reqs.InsertIn(Archive, a + 72, data);
            Foe_reqs.InsertIn(Archive, a + 72 + Unit_reqs.Size, data);
            a += Unit_reqs.Size + Foe_reqs.Size + 72;
            Payload_size.XorValue(ExtractUtils.getInt(a + 8, data));
            Reward = new Reward_Payload(Payload_size.Value);
            Archive.Index++;
            Reward.InsertIn(Archive, ExtractUtils.getLong(a, data) + offset, data);
        }

        public StringXor Quest_id { get => quest_id; set => quest_id = value; }
        public StringXor Common_id { get => common_id; set => common_id = value; }
        public UInt32Xor Times { get => times; set => times = value; }
        public UInt32Xor Trigger { get => trigger; set => trigger = value; }
        public StringXor Map_group { get => map_group; set => map_group = value; }
        public UInt32Xor Game_mode { get => game_mode; set => game_mode = value; }
        public Int32Xor Difficulty { get => difficulty; set => difficulty = value; }
        public UInt32Xor Unknown1 { get => _unknown1; set => _unknown1 = value; }
        public Int32Xor Survive { get => survive; set => survive = value; }
        public UInt64Xor Unknown2 { get => _unknown2; set => _unknown2 = value; }
        public UInt32Xor Game_mode2 { get => game_mode2; set => game_mode2 = value; }
        public StringXor Map_id { get => map_id; set => map_id = value; }
        public QuestUnitMatch Unit_reqs { get => unit_reqs; set => unit_reqs = value; }
        public QuestUnitMatch Foe_reqs { get => foe_reqs; set => foe_reqs = value; }
        public Reward_Payload Reward { get => reward; set => reward = value; }
        public Int32Xor Payload_size { get => payload_size; set => payload_size = value; }
    }
    public class Quest_list : CommonRelated
    {
        StringXor difficulty;
        Quest_definition[] quests;
        UInt32Xor quest_count;

        public StringXor Difficulty { get => difficulty; set => difficulty = value; }
        public Quest_definition[] Quests { get => quests; set => quests = value; }
        public UInt32Xor Quest_count { get => quest_count; set => quest_count = value; }

        public Quest_list()
        {
            Size = 24;
            Quest_count = new UInt32Xor(0xBF, 0x5A, 0xB7, 0xE7);
        }

        public Quest_list(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }

        public override void InsertIn(long a, byte[] data)
        {
            Difficulty = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
            if(!Difficulty.Value.Equals(""))
                Archive.Index++;
            Quest_count.XorValue(ExtractUtils.getInt(a + 16, data));
            Quests = new Quest_definition[Quest_count.Value];
            a = ExtractUtils.getLong(a + 8, data) + offset;
            Archive.Index++;
            for (int i = 0; i < Quests.Length; i++)
            {
                Quests[i] = new Quest_definition();
                Quests[i].InsertIn(Archive, a + (i * Quests[i].Size), data);
            }
        }

        public override string ToString()
        {
            String text = Difficulty.Value.Equals("")? "" : "Difficulty: " + Difficulty;
            for(int i = 0; i < Quests.Length; i++)
            {
                text += "Quest " + (i + 1) + ":" + Environment.NewLine + Quests[i];
                text += i == Quests.Length - 1 ? "" : "--------------------------------------------" + Environment.NewLine;
            }
            return text;
        }
    }
    public class Quest_group : CommonRelated
    {
        StringXor id_tag;
        StringXor group;
        Quest_list[] lists;
        Int64Xor start;
        Int64Xor finish;
        Byte[] _unknown1;
        UInt32Xor difficulties;
        UInt32Xor sort_id;
        int _unknown2;

        public Quest_group()
        {
            Size = 80;
            Start = new Int64Xor(0x60, 0xF6, 0x37, 0xC5, 0x36, 0xA2, 0x0D, 0xDC);
            Finish = new Int64Xor(0xE9, 0x56, 0xBD, 0xFA, 0x2A, 0x69, 0xAD, 0xC8);
            Unknown1 = new byte[24];
            Difficulties = new UInt32Xor(0x54, 0x4E, 0xB2, 0xEE);
            Sort_id = new UInt32Xor(0xFC, 0x4C, 0x39, 0x7A);
        }

        public Quest_group(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }

        public StringXor Id_tag { get => id_tag; set => id_tag = value; }
        public StringXor Group { get => group; set => group = value; }
        public Quest_list[] Lists { get => lists; set => lists = value; }
        public Int64Xor Start { get => start; set => start = value; }
        public Int64Xor Finish { get => finish; set => finish = value; }
        public byte[] Unknown1 { get => _unknown1; set => _unknown1 = value; }
        public UInt32Xor Difficulties { get => difficulties; set => difficulties = value; }
        public UInt32Xor Sort_id { get => sort_id; set => sort_id = value; }
        public int Unknown2 { get => _unknown2; set => _unknown2 = value; }

        public override void InsertIn(long a, byte[] data)
        {
            Id_tag = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
            Archive.Index++;
            Group = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
            Archive.Index++;
            Start.XorValue(ExtractUtils.getLong(a + 24, data));
            Finish.XorValue(ExtractUtils.getLong(a + 32, data));
            Difficulties.XorValue(ExtractUtils.getInt(a + 64, data));
            Sort_id.XorValue(ExtractUtils.getInt(a + 68, data));
            a = ExtractUtils.getLong(a + 16, data) + offset;
            Archive.Index++;
            Lists = new Quest_list[Difficulties.Value];
            for (int i = 0; i < Lists.Length; i++)
            {
                Lists[i] = new Quest_list();
                Lists[i].InsertIn(Archive, a + (i * Lists[i].Size), data);
            }
        }

        public override string ToString()
        {
            String text = "Quest Group ID: " + Id_tag + Environment.NewLine;
            text += "Quest Group Group: " + Group + Environment.NewLine;
            text += "Start time: " + (Start.Value < 0 ? "Not available" : DateTimeOffset.FromUnixTimeSeconds(Start.Value).DateTime.ToLocalTime().ToString()) + Environment.NewLine;
            text += "End time: " + (Finish.Value < 0 ? "Not available" : DateTimeOffset.FromUnixTimeSeconds(Finish.Value).DateTime.ToLocalTime().ToString()) + Environment.NewLine;
            text += "Sort ID: " + Sort_id + Environment.NewLine;
            for (int i = 0; i < Lists.Length; i++)
            {
                text += "Quest List " + (i + 1) + ":" + Environment.NewLine + Lists[i];
            }
            text += "-----------------------------------------------------------------------------------------------" + Environment.NewLine;
            return text;
        }
    }
    public class BaseQuest : ExtractionBase
    {
        private Int64Xor numElem;
        private Quest_group[] quests;

        public Int64Xor NumElem { get => numElem; set => numElem = value; }
        public Quest_group[] Quests { get => quests; set => quests = value; }

        public BaseQuest()
        {
            Name = "Quests";
            NumElem = new Int64Xor(0x5B, 0x54, 0x4C, 0x70, 0x0B, 0x1E, 0x4E, 0x03);
        }
        public BaseQuest(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }
        public override void InsertIn(long a, byte[] data)
        {
            a = Archive.Ptr_list[Archive.Index];
            NumElem.XorValue(ExtractUtils.getLong(a + 8, data));
            Archive.Index++;
            Quests = new Quest_group[NumElem.Value];
            a = ExtractUtils.getLong(a, data) + offset;
            for (int i = 0; i < NumElem.Value; i++)
            {
                Quests[i] = new Quest_group();
                Quests[i].InsertIn(Archive, a + (Quests[i].Size * i), data);
            }
        }

        public override string ToString()
        {
            String text = "";
            for (int i = 0; i < NumElem.Value; i++)
                text += Quests[i];
            return text;
        }
    }
}
