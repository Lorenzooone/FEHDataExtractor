using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEHDataExtractor
{
    class TempestTrial : CommonRelated
    {
        StringXor id_tag;
        StringXor image; //Speculation
        StringXor id_tag2;
        StringXor externalID; //Probably used for the name 
        private Int64Xor start;
        private Int64Xor finish;
        byte[] _unknown1; //17 elements
        // 7 bytes of padding
        Bonus_units upper_bonus_units;
        Bonus_units lower_bonus_units;
        //Something _unknown2;
        //Something _unknown3;
        byte[] _unknown4; //...? 8 elements, it's pointed to
        TT_point_rewards tTPointRewards;
        TT_rank_rewards tTRankRewards;
        Maps maps;
        UInt32Xor mapSize; //xor 2F 27 3B C3

        public TempestTrial()
        {
            Name = "Tempest Trial";
            Size = 144;
            ElemXor = new byte[]{ 0x37, 0x54, 0x19, 0xC5, 0xE6, 0xFE, 0x7C, 0x03 };

            Start = new Int64Xor(0x60, 0xf6, 0x37, 0xc5, 0x36, 0xa2, 0x0d, 0xdc);
            Finish = new Int64Xor(0xE9, 0x56, 0xBD, 0xFA, 0x2A, 0x69, 0xAD, 0xC8);
            Unknown1 = new byte[17];
            Unknown4 = new byte[8];
            MapSize = new UInt32Xor(0x2F, 0x27, 0x3B, 0xC3);
        }
        public TempestTrial(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }

        public Int64Xor Start { get => start; set => start = value; }
        public Int64Xor Finish { get => finish; set => finish = value; }
        public StringXor Id_tag { get => id_tag; set => id_tag = value; }
        public StringXor Image { get => image; set => image = value; }
        public StringXor Id_tag2 { get => id_tag2; set => id_tag2 = value; }
        public StringXor ExternalID { get => externalID; set => externalID = value; }
        public byte[] Unknown1 { get => _unknown1; set => _unknown1 = value; }
        public Bonus_units Upper_bonus_units { get => upper_bonus_units; set => upper_bonus_units = value; }
        public Bonus_units Lower_bonus_units { get => lower_bonus_units; set => lower_bonus_units = value; }
        //public Something Unknown2 { get => _unknown2; set => _unknown2 = value; }
        //public Something Unknown3 { get => _unknown3; set => _unknown3 = value; }
        public byte[] Unknown4 { get => _unknown4; set => _unknown4 = value; }
        public TT_point_rewards TTPointRewards { get => tTPointRewards; set => tTPointRewards = value; }
        public TT_rank_rewards TTRankRewards { get => tTRankRewards; set => tTRankRewards = value; }
        public Maps Maps { get => maps; set => maps = value; }
        public UInt32Xor MapSize { get => mapSize; set => mapSize = value; }

        public override void InsertIn(long a, byte[] data)
        {
            Archive.NegateIndex += 13;
            Id_tag = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
            Archive.Index++;
            Image = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, Common);
            Archive.Index++;
            Id_tag2 = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, Common);
            Archive.Index++;
            ExternalID = new StringXor(ExtractUtils.getLong(a + 24, data) + offset, data, Common);
            Archive.Index++;
            Start.XorValue(ExtractUtils.getLong(a + 32, data));
            Finish.XorValue(ExtractUtils.getLong(a + 40, data));
            Upper_bonus_units = new Bonus_units();
            Upper_bonus_units.InsertIn(Archive, ExtractUtils.getLong(a + 72, data) + offset, data);
            Archive.Index++;
            Lower_bonus_units = new Bonus_units();
            Lower_bonus_units.InsertIn(Archive, ExtractUtils.getLong(a + 80, data) + offset, data);
            Archive.Index++;
            //Something
            Archive.Index++;
            //Something2
            Archive.Index++;
            TTPointRewards = new TT_point_rewards();
            TTPointRewards.InsertIn(Archive, ExtractUtils.getLong(a + 112, data) + offset, data);
            Archive.Index++;
            TTRankRewards = new TT_rank_rewards();
            TTRankRewards.InsertIn(Archive, ExtractUtils.getLong(a + 120, data) + offset, data);
            Archive.Index++;
            MapSize.XorValue(ExtractUtils.getInt(a + 136, data));
            Maps = new Maps((int)MapSize.Value);
            Archive.Index++;
            Maps.InsertIn(Archive, ExtractUtils.getLong(a + 128, data) + offset, data);
        }

        public override string ToString()
        {
            string text = "ID: " + Id_tag + Environment.NewLine;
            text += "Image: " + Image + Environment.NewLine;
            text += "ID 2: " + Id_tag2 + Environment.NewLine;
            text += getStuff(ExternalID, "Name: ");
            text += "External ID: " + ExternalID + Environment.NewLine;
            text += "Start time: " + (Start.Value < 0 ? "Not available" : DateTimeOffset.FromUnixTimeSeconds(Start.Value).DateTime.ToLocalTime().ToString()) + Environment.NewLine;
            text += "End time: " + (Finish.Value < 0 ? "Not available" : DateTimeOffset.FromUnixTimeSeconds(Finish.Value).DateTime.ToLocalTime().ToString()) + Environment.NewLine;
            text += "Upper " + Upper_bonus_units;
            text += "Lower " + Lower_bonus_units;
            text += "----------------------------------------------------------------------" + Environment.NewLine;
            text += TTPointRewards.ToString() + TTRankRewards + Maps;
            return text;
        }
    }

    public class Maps:ExtractionBase
    {
        Single_entry[] map;

        public Single_entry[] Map { get => map; set => map = value; }

        public Maps(int size)
        {
            Size = size;
        }

        public override void InsertIn(long a, byte[] data)
        {
            Map = new Single_entry[Size];
            for(int i = 0; i < Map.Length; i++)
            {
                Map[i] = new Single_entry();
                Map[i].InsertIn(Archive, ExtractUtils.getLong(a + (i * 8), data) + offset, data);
                Archive.Index++;
            }
        }

        public override string ToString()
        {
            string text = "";
            for (int i = 0; i < Size; i++)
                text += "Map Collection " + (i + 1).ToString() + ":" + Environment.NewLine + Map[i];
            return text;
        }
    }

    public class Single_entry : CommonRelated
    {
        StringXor id;
        Single_map[/* possible_maps_collections */] possible_maps;
        StringXor boss; //points to pointer to string
        UInt32Xor possible_maps_collections;
        byte[] _unknown1; //24 elements
        //4 bytes of padding

        public StringXor Id { get => id; set => id = value; }
        public Single_map[] Possible_maps { get => possible_maps; set => possible_maps = value; }
        public StringXor Boss { get => boss; set => boss = value; }
        public UInt32Xor Possible_maps_collections { get => possible_maps_collections; set => possible_maps_collections = value; }
        public byte[] Unknown1 { get => _unknown1; set => _unknown1 = value; }

        public Single_entry()
        {
            Possible_maps_collections = new UInt32Xor(0xCB, 0x72, 0x2B, 0x1D);
        }

        public override void InsertIn(long a, byte[] data)
        {
            Id = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
            Archive.Index++;
            Boss = new StringXor(ExtractUtils.getLong(ExtractUtils.getLong(a + 16, data) + offset, data) + offset, data, Common);
            Archive.Index += 2;
            Possible_maps_collections.XorValue(ExtractUtils.getInt(a + 24, data));
            long pos = ExtractUtils.getLong(a + 8, data) + offset;
            Archive.Index++;
            Possible_maps = new Single_map[Possible_maps_collections.Value];
            for (int i = 0; i < Possible_maps.Length; i++)
            {
                Possible_maps[i] = new Single_map();
                Possible_maps[i].InsertIn(Archive, pos + (i * Possible_maps[i].Size), data);
            }
        }

        public override string ToString()
        {
            string text = "Map Collection ID: " + Id + Environment.NewLine;
            text += "Boss: " + getHeroName(Boss.Value) + Environment.NewLine;
            text += "Possible Maps:" + Environment.NewLine;
            for (int i = 0; i < Possible_maps.Length; i++)
                text += Possible_maps[i];
            text += "---------------------------------------" + Environment.NewLine;
            return text;
        }
    }

    public class Single_map : CommonRelated
    {
        StringXor[/* ids_size */] ids;
        UInt32Xor ids_size; //xor 0D 15 31 20
                           //4 bytes of padding
        byte[] _unknown1; // 10 elements
        //6 bytes of padding

        public Single_map()
        {
            Size = 32;
            Ids_size = new UInt32Xor(0x0D, 0x15, 0x31, 0x20);
        }

        public Single_map(long a, byte[] data):this()
        { InsertIn(a, data); }

        public StringXor[] Ids { get => ids; set => ids = value; }
        public UInt32Xor Ids_size { get => ids_size; set => ids_size = value; }
        public byte[] Unknown1 { get => _unknown1; set => _unknown1 = value; }

        public override void InsertIn(long a, byte[] data)
        {
            Ids_size.XorValue(ExtractUtils.getInt(a + 8, data));
            long pos = ExtractUtils.getLong(a, data) + offset;
            Archive.Index++;
            Ids = new StringXor[Ids_size.Value];
            for (int i = 0; i < Ids_size.Value; i++)
            {
                Ids[i] = new StringXor(ExtractUtils.getLong(pos + (i * 8), data) + offset, data, Common);
                Archive.Index++;
            }
        }

        public override string ToString()
        {
            string text = "";
            for (int i = 0; i < Ids_size.Value; i++)
            {
                text += Ids[i] + Environment.NewLine;
            }
            return text;
        }
    }

    public class Bonus_units : CommonRelated
    {
        StringXor[] bonus_units_name;
        UInt32Xor units_size;
        UInt32Xor bonus;

        public Bonus_units()
        {
            Units_size = new UInt32Xor(0x12, 0x09, 0x47, 0xF3);
            Bonus = new UInt32Xor(0x57, 0x2D, 0xF8, 0x88);
        }

        public Bonus_units(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }
        
        public UInt32Xor Units_size { get => units_size; set => units_size = value; }
        public UInt32Xor Bonus { get => bonus; set => bonus = value; }
        public StringXor[] Bonus_units_name { get => bonus_units_name; set => bonus_units_name = value; }

        public override void InsertIn(long a, byte[] data)
        {
            Units_size.XorValue(ExtractUtils.getInt(a + 8, data));
            Bonus.XorValue(ExtractUtils.getInt(a + 12, data));
            Bonus_units_name = new StringXor[Units_size.Value];
            long pos = ExtractUtils.getLong(a, data) + offset;
            Archive.Index++;
            for (int i = 0; i < Bonus_units_name.Length; i++)
            {
                Bonus_units_name[i] = new StringXor(ExtractUtils.getLong(pos + (i * 8), data) + offset, data, Common);
                Archive.Index++;
            }
        }

        public override string ToString()
        {
            string text = "Bonus units group:" + Environment.NewLine;
            text += "Bonus: " + (Bonus.Value - 100).ToString() + "%" + Environment.NewLine;
            for (int i = 0; i < Bonus_units_name.Length; i++)
            {
                text += getHeroName(Bonus_units_name[i].Value) + Environment.NewLine;
            }
            return text;
        }
    }

 /*   public class Something : ExtractionBase
    {
        Something_else[] Stuff;
        byte[] _unknown1; //Maybe size ? 4 elements
                              //4 bytes of padding
    };

    public class Something_else : CommonRelated
    {
        StringXor Rank;
        byte[] _unknown1; //8 elements
    };
 */
    public class TT_point_rewards : ExtractionBase
    {
        Single_TT_point_reward[] rewards;
        UInt32Xor rewards_size; //xor E0 88 99 62

        public Single_TT_point_reward[] Rewards { get => rewards; set => rewards = value; }
        public UInt32Xor Rewards_size { get => rewards_size; set => rewards_size = value; }

        public TT_point_rewards()
        {
            Rewards_size = new UInt32Xor(0xE0, 0x88, 0x99, 0x62);
        }

        public TT_point_rewards(long a, byte[] data):this()
        { InsertIn(a, data); }

        public override void InsertIn(long a, byte[] data)
        {
            Rewards_size.XorValue(ExtractUtils.getInt(a + 8, data));
            long pos = ExtractUtils.getLong(a, data) + offset;
            Rewards = new Single_TT_point_reward[Rewards_size.Value];
            Archive.Index++;
            for (int i = 0; i < Rewards.Length; i++)
            {
                Rewards[i] = new Single_TT_point_reward();
                Rewards[i].InsertIn(Archive, pos + (i * Rewards[i].Size), data);
            }
        }

        public override string ToString()
        {
            string text = "Point Rewards:" + Environment.NewLine;
            for (int i = 0; i < Rewards.Length; i++)
                text += Rewards[i].ToString();
            text += "----------------------------------------------------------------------" + Environment.NewLine;
            return text;
        }
    }

    public class Single_TT_point_reward : CommonRelated
    {
        Reward_Payload reward;
        UInt32Xor payload_size; //xor EC 9D 02 5A
        StringXor reward_ID;
        UInt32Xor requiredPoints; //xor BD CB 42 F3

        public Reward_Payload Reward { get => reward; set => reward = value; }
        public UInt32Xor Payload_size { get => payload_size; set => payload_size = value; }
        public StringXor Reward_ID { get => reward_ID; set => reward_ID = value; }
        public UInt32Xor RequiredPoints { get => requiredPoints; set => requiredPoints = value; }

        public Single_TT_point_reward()
        {
            Size = 32;
            Payload_size = new UInt32Xor(0xEC, 0x9D, 0x02, 0x5A);
            RequiredPoints = new UInt32Xor(0xBD, 0xCB, 0x42, 0xF3);
        }

        public Single_TT_point_reward(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }

        public override void InsertIn(long a, byte[] data)
        {
            Payload_size.XorValue(ExtractUtils.getInt(a + 8, data));
            Reward = new Reward_Payload((int)Payload_size.Value);
            Reward.InsertIn(Archive, ExtractUtils.getLong(a, data) + offset, data);
            Archive.Index++;
            Reward_ID = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, Common);
            Archive.Index++;
            RequiredPoints.XorValue(ExtractUtils.getInt(a + 24, data));
        }

        public override string ToString()
        {
            string text = "ID: " + Reward_ID + Environment.NewLine;
            text += "Required points: " + RequiredPoints + Environment.NewLine;
            text += Reward + "-------------------------------------" + Environment.NewLine;
            return text;
        }
    }

    public class TT_rank_rewards : ExtractionBase
    {
        Single_TT_rank_reward[] rewards;
        UInt32Xor rewards_size; //xor 32 90 F9 D5

        public TT_rank_rewards()
        {
            Rewards_size = new UInt32Xor(0x32, 0x90, 0xF9, 0xD5);
        }

        public TT_rank_rewards(long a, byte[] data) : this()
        { InsertIn(a, data); }

        public Single_TT_rank_reward[] Rewards { get => rewards; set => rewards = value; }
        public UInt32Xor Rewards_size { get => rewards_size; set => rewards_size = value; }

        public override void InsertIn(long a, byte[] data)
        {
            Rewards_size.XorValue(ExtractUtils.getInt(a + 8, data));
            long pos = ExtractUtils.getLong(a, data) + offset;
            Rewards = new Single_TT_rank_reward[Rewards_size.Value];
            Archive.Index++;
            for (int i = 0; i < Rewards.Length; i++)
            {
                Rewards[i] = new Single_TT_rank_reward();
                Rewards[i].InsertIn(Archive, pos + (i * Rewards[i].Size), data);
            }
        }

        public override string ToString()
        {
            string text = "Rank Rewards:" + Environment.NewLine;
            for (int i = 0; i < Rewards.Length; i++)
                text += Rewards[i].ToString();
            text += "----------------------------------------------------------------------" + Environment.NewLine;
            return text;
        }
    }

    public class Single_TT_rank_reward : CommonRelated
    {
        Reward_Payload reward;
        UInt32Xor payload_size; //xor 78 3C B7 4D
        StringXor reward_ID;
        UInt32Xor min; //xor 3C 5A 80 C1
        UInt32Xor max; //xor 32 90 F9 D5

        public Reward_Payload Reward { get => reward; set => reward = value; }
        public UInt32Xor Payload_size { get => payload_size; set => payload_size = value; }
        public StringXor Reward_ID { get => reward_ID; set => reward_ID = value; }
        public UInt32Xor Min { get => min; set => min = value; }
        public UInt32Xor Max { get => max; set => max = value; }

        public Single_TT_rank_reward()
        {
            Size = 32;
            Payload_size = new UInt32Xor(0x78, 0x3C, 0xB7, 0x4D);
            Min = new UInt32Xor(0x3C, 0x5A, 0x80, 0xC1);
            Max = new UInt32Xor(0x32, 0x90, 0xF9, 0xD5);
        }

        public Single_TT_rank_reward(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }

        public override void InsertIn(long a, byte[] data)
        {
            Payload_size.XorValue(ExtractUtils.getInt(a + 8, data));
            Reward = new Reward_Payload((int)Payload_size.Value);
            Reward.InsertIn(Archive, ExtractUtils.getLong(a, data) + offset, data);
            Archive.Index++;
            Reward_ID = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, Common);
            Archive.Index++;
            Min.XorValue(ExtractUtils.getInt(a + 24, data));
            Max.XorValue(ExtractUtils.getInt(a + 28, data));
        }

        public override string ToString()
        {
            string text = "ID: " + Reward_ID + Environment.NewLine;
            text += "Rank: " + Min + "-" + Max + Environment.NewLine;
            text += Reward + "-------------------------------------" + Environment.NewLine;
            return text;
        }
    }

}

