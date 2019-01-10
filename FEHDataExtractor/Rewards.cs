using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEHDataExtractor
{
    public class SingleReward : ExtractionBase
    {
        private Reward_Type thing;

        public Reward_Type Thing { get => thing; set => thing = value; }

        public override void InsertIn(long a, byte[] data)
        {
            byte Kind = data[a];
            switch (Kind)
            {
                case 0:
                case 2:
                case 3:
                case 4:
                case 5:
                case 0xD:
                case 0x10:
                case 0x11:
                case 0x12:
                case 0x13:
                case 0x15:
                case 0x19:
                case 0x1A:
                    Thing = new SingleCountDependant();
                    break;
                case 0x1C:
                    Thing = new Throne();
                    break;
                case 0x1D:
                case 0x1B:
                    Thing = new CountStr();
                    break;
                case 1:
                    Thing = new Hero();
                    break;
                case 6:
                    Thing = new Crystal();
                    break;
                case 0xC:
                    Thing = new Badge();
                    break;
                case 0xE:
                case 0x16:
                    Thing = new StringDependant();
                    break;
                case 0xF:
                    Thing = new ArenaAssaultItem();
                    break;
                case 0x14:
                    Thing = new Blessing();
                    break;
                case 0x17:
                    Thing = new Conversation();
                    break;
                    
                default:
                    Thing = new Unknown();
                    break;
            }
            Thing.InsertIn(a, data);
        }

        public override string ToString()
        {
            return Thing.ToString() + Environment.NewLine;
        }
    }

    public abstract class Reward_Type : ExtractionBase
    {
        byte kind;

        public static readonly String[] Thing = { "Orb", "Hero", "Hero Feather", "Stamina Potion", "Dueling Crest", "Light's Blessing", "Crystal", "", "", "", "", "", "Badge", "Battle Flag", "Sacred Seal", "Arena Assault Item", "Sacred Coin", "Refining Stone", "Divine Dew", "Arena Medal", "Blessing", "Conquest Lance", "Accessory", "Conversation", "", "Arena Crown", "Heroic Grail", "Aether Stone", "Throne", "Summoning Ticket" };

        public override void InsertIn(long a, byte[] data)
        {
            Kind = data[a];
            Size = 1;
        }

        public byte Kind { get => kind; set => kind = value; }
    }

    public class StringDependant : Reward_Type
    {
        private byte length;
        private string rew;

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            Length = data[a + 1];
            Rew = ExtractUtils.GetStringSize(a + 2, data, Length);
            Size += 1 + Length;
        }

        public override string ToString()
        {
            String text = Thing[Kind] + ": " + (Table.Contains("M" + Rew) ? Table["M" + Rew] : Rew);
            return text;
        }

        public byte Length { get => length; set => length = value; }
        public string Rew { get => rew; set => rew = value; }
    }
    public class Unknown : Reward_Type
    {
        private short theoricalCount;

        public short TheoricalCount { get => theoricalCount; set => theoricalCount = value; }

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            TheoricalCount = ExtractUtils.getShort(a + 1, data);
            Size = int.MaxValue;
        }

        public override string ToString()
        {
            String text = "Unknown reward! Kind = " + Kind.ToString("X") + "! Theorical count = " + theoricalCount.ToString();
            return text;
        }
        
    }

    public class Hero : StringDependant
    {
        private short rarity;

        public short Rarity { get => rarity; set => rarity = value; }

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            Rarity = ExtractUtils.getShort(a + 2 + Length, data);
            Size += 2;
        }

        public override string ToString()
        {
            String text = Thing[Kind] + ": " + getHeroName(Rew) + " " + Rarity + " Star";
            text += Rarity == 1 ? "" : "s";
            return text;
        }
    }

    public class Conversation : Reward_Type
    {
        private int length;
        private string rew;
        private int support;

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            Support = data[a + 1];
            Length = data[a + 2];
            Rew = ExtractUtils.GetStringSize(a + 3, data, Length);
            Size += 2 + Length;
        }

        public override string ToString()
        {
            String text = Ranks[Support] + " " + Thing[Kind] + " with " + getHeroName(Rew);
            return text;
        }

        public int Length { get => length; set => length = value; }
        public string Rew { get => rew; set => rew = value; }
        public int Support { get => support; set => support = value; }
    }

    public class SingleCountDependant : Reward_Type
    {
        private short count;

        public short Count { get => count; set => count = value; }

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            Count = ExtractUtils.getShort(a + 1, data);
            Size += 2;
        }

        public override string ToString()
        {
            String text = Count + " " + Thing[Kind];
            text += Count > 1 ? "s" : "";
            text += Kind == 5 ? " (Revival Item)" : "";
            return text;
        }
    }

    public class CountStr : SingleCountDependant
    {
        private byte length;
        private string rew;

        public byte Length { get => length; set => length = value; }
        public string Rew { get => rew; set => rew = value; }

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            Length = data[a + 3];
            Rew = ExtractUtils.GetStringSize(a + 4, data, Length);
            Size += 1 + Length;
        }

        public override string ToString()
        {
            String text = Count + " " + Thing[Kind] + " " + (Table.Contains("M" + Rew) ? Table["M" + Rew] : Rew);
            return text;
        }
    }

    public class Blessing : SingleCountDependant
    {
        private byte element;

        public byte Element { get => element; set => element = value; }

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            Element = data[a + 3];
            Size += 1;
        }

        public override string ToString()
        {
            String text = Count + " " + LegendaryElement[Element - 1] + " " + Thing[Kind];
            text += Count > 1 ? "s" : "";
            return text;
        }
    }

    public class Throne : SingleCountDependant
    {
        private byte thronetype;
        public static string[] Thrones = { "Gold", "Silver", "Bronze" };

        public byte Thronetype { get => thronetype; set => thronetype = value; }

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            Thronetype = data[a + 3];
            Size += 1;
        }

        public override string ToString()
        {
            String text = Count + " " + Thrones[Thronetype] + " " + Thing[Kind];
            text += Count > 1 ? "s" : "";
            return text;
        }
    }

    public class ArenaAssaultItem : SingleCountDependant
    {
        private byte item;
        public readonly string[] ArenaAssaultItems = { "Elixir", "Fortifying Horn", "Special Blade", "Infantry Boots", "Naga's Tear", "Dancer's Veil", "Lightning Charm", "Panic Charm", "Fear Charm", "Pressure Charm" };

        public byte Item { get => item; set => item = value; }

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            Item = data[a + 3];
            Size += 1;
        }

        public override string ToString()
        {
            String text = Count + " " + ArenaAssaultItems[Item];
            text += Count > 1 ? "s" : "";
            return text;
        }
    }

    public class Crystal : SingleCountDependant
    {
        private byte is_Crystal;
        private byte color;

        public byte Is_Crystal { get => is_Crystal; set => is_Crystal = value; }
        public byte Color { get => color; set => color = value; }

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            Is_Crystal = data[a + 3];
            Color = data[a + 4];
            Size += 2;
        }

        public override string ToString()
        {
            String text = Count + " " + ShardColor[Color] + " ";
            text += Is_Crystal == 1 ? Thing[Kind] : "Shard";
            text += Count > 1 ? "s" : "";
            return text;
        }
    }

    public class Badge : SingleCountDependant
    {
        private byte is_Great;
        private byte color;

        public byte Is_Great { get => is_Great; set => is_Great = value; }
        public byte Color { get => color; set => color = value; }

        public override void InsertIn(long a, byte[] data)
        {
            base.InsertIn(a, data);
            Is_Great = data[a + 3];
            Color = data[a + 4];
            Size += 2;
        }

        public override string ToString()
        {
            String text = Count + " " + BadgeColor[Color] + " ";
            text += Is_Great == 0 ? Thing[Kind] : "Great " + Thing[Kind];
            text += Count > 1 ? "s" : "";
            return text;
        }
    }
    public class Reward_Payload : ExtractionBase
    {
        public static readonly long Magic = 0x160707001B9AD871;
        private byte[] iV;
        private Reward_Definition rewards;
        public static readonly byte[] AES128Key = { 0x4b, 0x0d, 0xb4, 0x88, 0x61, 0x7c, 0x60, 0xa1, 0x2b, 0x09, 0x40, 0xe9, 0xed, 0x92, 0xa6, 0x8f };
        
        public byte[] IV { get => iV; set => iV = value; }
        public Reward_Definition Rewards { get => rewards; set => rewards = value; }

        public Reward_Payload(int size)
        {
            Size = size;
        }

        public override void InsertIn(long a, byte[] data)
        {
            if (ExtractUtils.getLong(a + Size - 0x18, data) != Magic)
                return;
            IV = new byte[16];
            for (int i = 0; i < 16; i++)
                IV[i] = data[a + Size - 0x10 + i];
            byte[] buf = new byte[Size - 0x18];
            for (int i = 0; i < buf.Length; i++)
                buf[i] = data[a + i];
            buf = Aes128CounterMode.DoThing(buf, IV, AES128Key);
            Rewards = new Reward_Definition();
            Rewards.InsertIn(0, buf);
        }
        public override string ToString()
        {
            return Rewards.ToString();
        }
    }
    public class Reward_Definition : ExtractionBase
    {
        private byte reward_Count;
        private SingleReward[] rewards;
        private byte[] checksum; //32 Elements, an int in little endian
        private byte[] _unknown1;
        public static readonly byte[] ChecksumPrivateKey = {0x4a, 0xb5, 0x6f, 0xfc, 0xfc, 0xad, 0xf8, 0x28, 0x90, 0x92, 0x3c, 0x64, 0x4a, 0x2f, 0xa2, 0x7b, 0x22, 0xdc, 0x04, 0xf7, 0xbf, 0xef, 0xa4, 0xd1, 0xa1, 0x0e, 0xfc, 0xd2, 0x7a, 0xc3, 0x5e, 0x2a, 0xba, 0x87, 0xcd, 0x0f, 0xb8, 0xcc, 0xcb, 0xf6, 0xb8, 0xe1, 0x4f, 0xdb, 0xde, 0xc9, 0xcd, 0xda, 0xb5, 0xf0, 0x81, 0x36, 0x27, 0xbb, 0x23, 0xb3, 0x77, 0x45, 0x71, 0x48, 0x3c, 0x83, 0x6b, 0xe2};

        public byte Reward_Count { get => reward_Count; set => reward_Count = value; }
        public SingleReward[] Rewards { get => rewards; set => rewards = value; }
        public byte[] Checksum { get => checksum; set => checksum = value; }
        public byte[] Unknown1 { get => _unknown1; set => _unknown1 = value; }

        public override void InsertIn(long a, byte[] data)
        {
            Reward_Count = data[a];
            int TotalSize = 0;
            Rewards = new SingleReward[Reward_Count];
            for (int i = 0; i < Reward_Count; i++)
            {
                Rewards[i] = new SingleReward();
                Rewards[i].InsertIn(a + 1 + TotalSize, data);
                if (Rewards[i].Thing.Size == int.MaxValue)
                {
                    TotalSize = int.MaxValue;
                    i = Reward_Count;
                }
                else
                    TotalSize += Rewards[i].Thing.Size;
            }
            if (TotalSize != int.MaxValue)
            {
                Checksum = new byte[32];
                for (int i = 0; i < 32; i++)
                    Checksum[i] = data[a + 1 + TotalSize + i];
                string signatureHashHex = HACSHA256.HACSHA(ChecksumPrivateKey, data, a, 1 + TotalSize);
                if (signatureHashHex.Equals(System.Text.Encoding.UTF8.GetString(Checksum)))
                    return;
            }
        }

        public override string ToString()
        {
            String text = "Reward";
            text += Rewards.Length > 1 ? "s" : "";
            text += ":" + Environment.NewLine;
            for (int i = 0; i < Rewards.Length; i++)
                text += Rewards[i];
            return text;
        }
    }
}
