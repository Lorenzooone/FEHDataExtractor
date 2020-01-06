using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEHDataExtractor
{
    public class Forging_Bonds : FBRelated
    {
        private static StringsUpdatable hearts = new StringsUpdatable(new string[]{ "Green   ", "Blue    ", "Red     ", "Yellow  ", "Unknown ", "Unknown2" });

        private StringXor id_tag;
        private StringXor reference;
        private StringXor image;
        private StringXor id_tag2;
        private StringXor icon;
        private StringXor[] characters; //"characterSize" elements, a pointer to the strings
        private UInt32Xor[] numbers; //"numberSize" numbers...? Xor: 0xAC, 0xEF, 0xDB, 0x88
        private Something[] things; //3 things... Maybe related to supports with everyone...?
        private Multipliers multipliersValues;
        private UInt32Xor[] multipliersCharacters; //multiplierSize elements, Xor: 0x8C, 0xCD, 0x08, 0x5B
        private UInt32Xor[,] things2; //Matrix of 2 x 10 elements. Purpose unknown. First element's Xor seems to be 0xB1 0x84 0x16 0x5F | Scheme is 4 3 3
        private UInt32Xor[] things3; //4 elements with each 4 bytes of padding after them.
        private StringXor[] bonusAccessories; //8 elements.
        private PointReward[] points;
        private DailyReward[] dailies;
        private StringXor[,] assets; //Matrix of 3 x 4 elements, related to the supports with everyone.
        private Int64Xor start;
        private Int64Xor finish;
        private ByteXor[] unknown; //17 bytes + 7 of padding
        private UInt32Xor characterSize; //Xor: 0x66, 0x2D, 0x6E, 0xBA
        private UInt32Xor numberSize; //Xor: 0x41, 0xA2, 0xA8, 0xEC
        private UInt32Xor unknown2;
        private UInt32Xor unknown3;
        private UInt32Xor multiplierSize; //Xor: 0x91, 0x44, 0x97, 0x52
        private UInt32Xor unknown4;
        private UInt32Xor unknown5;
        private UInt32Xor bonusAccessoriesNum; //Xor: 0x34, 0x66, 0x23, 0x54
        private UInt32Xor pointsNum; //Xor: 0x62, 0xF6, 0xA1, 0x7A
        private UInt32Xor dailiesNum; //Xor: 0xF4, 0x9D, 0x5F, 0xDD
        private ByteXor[] unknown6; //24 elements
        
        public StringXor Id_tag { get => id_tag; set => id_tag = value; }
        public StringXor Reference { get => reference; set => reference = value; }
        public StringXor Image { get => image; set => image = value; }
        public StringXor Id_tag2 { get => id_tag2; set => id_tag2 = value; }
        public StringXor Icon { get => icon; set => icon = value; }
        public StringXor[] Characters { get => characters; set => characters = value; }
        public UInt32Xor[] Numbers { get => numbers; set => numbers = value; }
        public Something[] Things { get => things; set => things = value; }
        public Multipliers MultipliersValues { get => multipliersValues; set => multipliersValues = value; }
        public UInt32Xor[] MultipliersCharacters { get => multipliersCharacters; set => multipliersCharacters = value; }
        public UInt32Xor[,] Things2 { get => things2; set => things2 = value; }
        public UInt32Xor[] Things3 { get => things3; set => things3 = value; }
        public StringXor[] BonusAccessories { get => bonusAccessories; set => bonusAccessories = value; }
        public PointReward[] Points { get => points; set => points = value; }
        public DailyReward[] Dailies { get => dailies; set => dailies = value; }
        public StringXor[,] Assets { get => assets; set => assets = value; }
        public Int64Xor Start { get => start; set => start = value; }
        public Int64Xor Finish { get => finish; set => finish = value; }
        public ByteXor[] Unknown { get => unknown; set => unknown = value; }
        public UInt32Xor CharacterSize { get => characterSize; set => characterSize = value; }
        public UInt32Xor NumberSize { get => numberSize; set => numberSize = value; }
        public UInt32Xor Unknown2 { get => unknown2; set => unknown2 = value; }
        public UInt32Xor Unknown3 { get => unknown3; set => unknown3 = value; }
        public UInt32Xor MultiplierSize { get => multiplierSize; set => multiplierSize = value; }
        public UInt32Xor Unknown4 { get => unknown4; set => unknown4 = value; }
        public UInt32Xor Unknown5 { get => unknown5; set => unknown5 = value; }
        public UInt32Xor BonusAccessoriesNum { get => bonusAccessoriesNum; set => bonusAccessoriesNum = value; }
        public UInt32Xor PointsNum { get => pointsNum; set => pointsNum = value; }
        public UInt32Xor DailiesNum { get => dailiesNum; set => dailiesNum = value; }
        public ByteXor[] Unknown6 { get => unknown6; set => unknown6 = value; }

        public Forging_Bonds()
        {
            Name = "Forging Bonds";
            Things = new Something[3];
            MultipliersValues = new Multipliers();
            Assets = new StringXor[3, 4];
            Start = new Int64Xor(0x60, 0xF6, 0x37, 0xC5, 0x36, 0xA2, 0x0D, 0xDC);
            Finish = new Int64Xor(0xE9, 0x56, 0xBD, 0xFA, 0x2A, 0x69, 0xAD, 0xC8);
            CharacterSize = new UInt32Xor(0x66, 0x2D, 0x6E, 0xBA);
            NumberSize = new UInt32Xor(0x41, 0xA2, 0xA8, 0xEC);
            MultiplierSize = new UInt32Xor(0x91, 0x44, 0x97, 0x52);
            BonusAccessoriesNum = new UInt32Xor(0x34, 0x66, 0x23, 0x54);
            PointsNum = new UInt32Xor(0xC2, 0xF6, 0xA1, 0x7A);
            DailiesNum = new UInt32Xor(0xF4, 0x9D, 0x5F, 0xDD);
        }

        public override void InsertIn(long a, byte[] data)
        {
            Id_tag = new StringXor(ExtractUtils.getLong(a, data) + offset, data, FB);
            Reference = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, FB);
            Image = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, FB);
            Id_tag2 = new StringXor(ExtractUtils.getLong(a + 24, data) + offset, data, FB);
            Icon = new StringXor(ExtractUtils.getLong(a + 32, data) + offset, data, FB);
            MultipliersValues.InsertIn(ExtractUtils.getLong(a + 64, data) + offset, data);
            for (int i = 0; i < 3; i++)
            {
                long pos = ExtractUtils.getLong(a + 120 + (i * 8), data) + offset;
                for (int j = 0; j < 4; j++)
                    Assets[i, j] = new StringXor(ExtractUtils.getLong(pos + (j * 8), data) + offset, data, FB);
            }
            Start.XorValue(ExtractUtils.getLong(a + 144, data));
            Finish.XorValue(ExtractUtils.getLong(a + 152, data));
            CharacterSize.XorValue(ExtractUtils.getInt(a + 184, data));
            NumberSize.XorValue(ExtractUtils.getInt(a + 188, data));
            MultiplierSize.XorValue(ExtractUtils.getInt(a + 200, data));
            bonusAccessoriesNum.XorValue(ExtractUtils.getInt(a + 212, data));
            PointsNum.XorValue(ExtractUtils.getInt(a + 216, data));
            DailiesNum.XorValue(ExtractUtils.getInt(a + 220, data));
            Characters = new StringXor[CharacterSize.Value];
            long position = ExtractUtils.getLong(a + 40, data) + offset;
            for (int i = 0; i < CharacterSize.Value; i++)
            {
                Characters[i] = new StringXor(ExtractUtils.getLong(position + (i * 8), data) + offset, data, FB);
            }
            MultipliersCharacters = new UInt32Xor[MultiplierSize.Value];
            position = ExtractUtils.getLong(a + 72, data) + offset;
            for (int i = 0; i < MultiplierSize.Value; i++)
            {
                MultipliersCharacters[i] = new UInt32Xor(0x8C, 0xCD, 0x08, 0x5B);
                MultipliersCharacters[i].XorValue(ExtractUtils.getInt(position + (i * 8), data));
            }
            BonusAccessories = new StringXor[BonusAccessoriesNum.Value];
            position = ExtractUtils.getLong(a + 96, data) + offset;
            for (int i = 0; i < BonusAccessoriesNum.Value; i++)
            {
                BonusAccessories[i] = new StringXor(ExtractUtils.getLong(position + (i * 8), data) + offset, data, FB);
            }
            Points = new PointReward[PointsNum.Value * CharacterSize.Value];
            position = ExtractUtils.getLong(a + 104, data) + offset;
            for (int i = 0; i < PointsNum.Value * CharacterSize.Value; i++)
            {
                Points[i] = new PointReward();
                Points[i].InsertIn(position + (i * Points[i].Size), data);
            }
            Dailies = new DailyReward[DailiesNum.Value];
            position = ExtractUtils.getLong(a + 112, data) + offset;
            for (int i = 0; i < DailiesNum.Value; i++)
            {
                Dailies[i] = new DailyReward();
                Dailies[i].InsertIn(position + (i * Dailies[i].Size), data);
            }
            Archive.Index = Archive.Ptr_list_length;
        }

        public override string ToString()
        {
            String text = "";
            text += "ID: " + Id_tag + Environment.NewLine;
            text += "ID2: " + Id_tag2 + Environment.NewLine;
            text += "Reference: " + Reference + Environment.NewLine;
            text += "Image: " + Image + Environment.NewLine;
            text += "Icon: " + Icon + Environment.NewLine;
            text += "Start: " + (Start.Value < 0 ? "Not available" : DateTimeOffset.FromUnixTimeSeconds((long)Start.Value).DateTime.ToLocalTime().ToString()) + Environment.NewLine;
            text += "Finish: " + (Finish.Value < 0 ? "Not available" : DateTimeOffset.FromUnixTimeSeconds((long)Finish.Value).DateTime.ToLocalTime().ToString()) + Environment.NewLine;
            text += "Characters:" + Environment.NewLine;
            for (int i = 0; i < Characters.Length; i++)
                text += getHeroName(Characters[i].Value) + Environment.NewLine;
            text += "Bonus Accessories:" + Environment.NewLine;
            for (int i = 0; i < BonusAccessories.Length; i++)
                text += getStuffExclusive(BonusAccessories[i], "");
            text += "Assets:" + Environment.NewLine;
            for (int i = 0; i < 3; i++)
                text += Assets[i, 0] + " | " + Assets[i, 1] + " | " + Assets[i, 2] + " | " + Assets[i, 3] + Environment.NewLine;
            text += "Point Rewards:" + Environment.NewLine;
            for(int i = 0; i < CharacterSize.Value; i++)
            {
                text += getHeroName(Characters[i].Value) + ":" + Environment.NewLine;
                for (int j = 0; j < Points.Length; j++)
                    if (Points[j].Character.Value.Equals(Characters[i].Value))
                        text += Points[j];
                text += "---------------------------------------------------------------------------" + Environment.NewLine;
            }
            text += "Daily Rewards:" + Environment.NewLine;
            for (int i = 0; i < Dailies.Length; i++)
                text += Dailies[i];
            text += MultipliersValues + Environment.NewLine; //This is to separate content
            int days = DateTimeOffset.FromUnixTimeSeconds(Finish.Value).DateTime.Subtract(DateTimeOffset.FromUnixTimeSeconds(Start.Value).DateTime).Days;
            text += "Days    ";
            int hour = DateTimeOffset.FromUnixTimeSeconds(Start.Value).DateTime.ToLocalTime().Hour;
            for (int i = 0; i < MultiplierSize.Value / days; i++)
            {
                int tmp = (hour + (i * (24 / ((int)MultiplierSize.Value / days)))) % 24;
                text += (tmp < 10 ? "0" : "") + tmp + ":00        ";
            }
            text += Environment.NewLine;
            for (int i = 0; i < days; i++)
            {
                text += "Day " + (i + 1) + (i < 9? " ":"") + ": ";
                for (int j = 0; j < MultiplierSize.Value / days; j++)
                    text += string.Format(new System.Globalization.CultureInfo("en-US"), "{0:N1}", get_multiplier((uint)((i * ((int)(MultiplierSize.Value / days))) + j), MultipliersValues.Probs, MultipliersValues.Mults) / 100.0) + "x " + hearts.getString((int)MultipliersCharacters[(i* (MultiplierSize.Value/days)) + j].Value);
                text += Environment.NewLine;
            }
            text += "---------------------------------------------------------------------------" + Environment.NewLine;
            return text;
        }

        //Following code comes from HertzDevil's work with a bit of adjustements. Credits to him.
        void initRandBuf(UInt32[] v0, UInt32 v1)
        {

            UInt32 state = v1;
            for (int i = 0; i < 4; ++i)
                v0[i] = state = 0x6C078965 * (state ^ (state >> 30)) + (uint)i;
        }

        Int32 randMagicCycle(UInt32[] v0, Int32 v1, Int32 v2)
        {
            UInt32 t = v0[0];
            UInt32 s = v0[3];
            t ^= t << 11;
            t ^= t >> 8;
            t ^= s;
            t ^= s >> 19;
            v0[0] = v0[1];
            v0[1] = v0[2];
            v0[2] = v0[3];
            v0[3] = t;
            return (Int32)(Math.Min(v1, v2) + (t & 0x7FFFFFFF) % (Math.Abs(v1 - v2) + 1));
        }

        Int32 get_multiplier(UInt32 period, Int32Xor[] probs, Int32Xor[] mults)
        {
            UInt32[] randbuf = { 0, 0, 0, 0 };
            initRandBuf(randbuf, period + (uint)Start.Value);
            Int32 x = randMagicCycle(randbuf, 1, 100);
            for (int i = 0; i < probs.Length; ++i)
            {
                x -= probs[i].Value;
                if (x <= 0)
                    return mults[i].Value;
            }
            return 0;
        }
    }

    public class Something : ExtractionBase
    {
        private ByteXor[] stuff; //16 bytes

        public ByteXor[] Stuff { get => stuff; set => stuff = value; }

        public override void InsertIn(long a, byte[] data)
        {
            throw new NotImplementedException();
        }
    }

    public class Multipliers : ExtractionBase
    {
        private ByteXor[] unknown; //8 elements
        private UInt32Xor numElem; // Xor: 0xDF, 0x15, 0x78, 0x37
        //4 bytes of padding
        private Int32Xor[] mults; //Pointer to a 2 x numElem matrix. Values and multiplier. //Xor: 0x8E, 0x31, 0x83, 0x2E and 0x7C, 0x02, 0x22, 0x51
        private Int32Xor[] probs;

        public UInt32Xor NumElem { get => numElem; set => numElem = value; }
        public Int32Xor[] Mults { get => mults; set => mults = value; }
        public ByteXor[] Unknown { get => unknown; set => unknown = value; }
        public Int32Xor[] Probs { get => probs; set => probs = value; }

        public Multipliers()
        {
            NumElem = new UInt32Xor(0xDF, 0x15, 0x78, 0x37);
        }

        public Multipliers(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }

        public override void InsertIn(long a, byte[] data)
        {
            NumElem.XorValue(ExtractUtils.getInt(a + 8, data));
            Mults = new Int32Xor[NumElem.Value];
            Probs = new Int32Xor[NumElem.Value];
            long pos = ExtractUtils.getLong(a + 16, data) + offset;
            for (int i = 0; i < NumElem.Value; i++)
            {
                
                Mults[i] = new Int32Xor(0x7C, 0x02, 0x22, 0x51);
                Mults[i].XorValue(ExtractUtils.getInt(pos + 4 + (i * 8), data));
                Probs[i] = new Int32Xor(0x8E, 0x31, 0x83, 0x2E);
                Probs[i].XorValue(ExtractUtils.getInt(pos + (i * 8), data));
            }
        }

        public override string ToString()
        {
            string text = "Multipliers:" + Environment.NewLine;
            for (int i = 0; i < NumElem.Value; i++)
            {
                text += (Mults[i].Value / 100.0) + "x: " + Probs[i] + "%" + Environment.NewLine;
            }
            return text;
        }
    }

    public class PointReward : FBRelated
    {
        private StringXor character; //Pointer to the actual pointer
        private UInt32Xor points; //Xor: 0x86, 0x79, 0xCB, 0x37
        private UInt32Xor rewardSize; //Xor: 0xD7, 0xB0, 0x9E, 0x3B
        private Reward_Payload reward;
        private StringXor id_tag;
        private StringXor id_tag2;

        public StringXor Character { get => character; set => character = value; }
        public UInt32Xor Points { get => points; set => points = value; }
        public UInt32Xor RewardSize { get => rewardSize; set => rewardSize = value; }
        public Reward_Payload Reward { get => reward; set => reward = value; }
        public StringXor Id_tag { get => id_tag; set => id_tag = value; }
        public StringXor Id_tag2 { get => id_tag2; set => id_tag2 = value; }

        public PointReward()
        {
            Size = 40;
            Points = new UInt32Xor(0x86, 0x79, 0xCB, 0x37);
            RewardSize = new UInt32Xor(0xD7, 0xB0, 0x9E, 0x3B);
        }

        public override void InsertIn(long a, byte[] data)
        {
            Character = new StringXor(ExtractUtils.getLong(ExtractUtils.getLong(a, data) + offset, data) + offset, data, FB);
            Points.XorValue(ExtractUtils.getInt(a + 8, data));
            RewardSize.XorValue(ExtractUtils.getInt(a + 12, data));
            Reward = new Reward_Payload((int)RewardSize.Value);
            Reward.InsertIn(ExtractUtils.getLong(a + 16, data) + offset, data);
            Id_tag = new StringXor(ExtractUtils.getLong(a + 24, data) + offset, data, FB);
            Id_tag2 = new StringXor(ExtractUtils.getLong(a + 32, data) + offset, data, FB);
        }

        public override string ToString()
        {
            string text = "ID: " + (!Id_tag.Value.Equals("") ? Id_tag.Value : Id_tag2.Value) + Environment.NewLine;
            text += "Required points: " + Points + Environment.NewLine;
            text += Reward + "-------------------------------------" + Environment.NewLine;
            return text;
        }
    }

    public class DailyReward : FBRelated
    {
        private Int64Xor start;
        private Int64Xor finish;
        private UInt32Xor rewardSize;  //Xor: 0x93, 0x65, 0x23, 0x68
        private UInt32Xor index; //Xor: 0x7C, 0x47, 0xF0, 0xA3
        private Reward_Payload reward;
        private StringXor id_tag;
        private StringXor id_tag2;

        public Int64Xor Start { get => start; set => start = value; }
        public Int64Xor Finish { get => finish; set => finish = value; }
        public UInt32Xor Index { get => index; set => index = value; }
        public Reward_Payload Reward { get => reward; set => reward = value; }
        public StringXor Id_tag { get => id_tag; set => id_tag = value; }
        public StringXor Id_tag2 { get => id_tag2; set => id_tag2 = value; }
        public UInt32Xor RewardSize { get => rewardSize; set => rewardSize = value; }

        public DailyReward()
        {
            Size = 48;
            Start = new Int64Xor(0x93, 0xC2, 0x58, 0xEA, 0x77, 0x37, 0x85, 0x35);
            Finish = new Int64Xor(0xEF, 0x20, 0xB5, 0x47, 0xC4, 0xC5, 0x6E, 0x5A);
            RewardSize = new UInt32Xor(0x93, 0x65, 0x23, 0x68);
            Index = new UInt32Xor(0x7C, 0x47, 0xF0, 0xA3);
        }

        public override void InsertIn(long a, byte[] data)
        {
            Start.XorValue(ExtractUtils.getLong(a, data));
            Finish.XorValue(ExtractUtils.getLong(a + 8, data));
            RewardSize.XorValue(ExtractUtils.getInt(a + 16, data));
            Index.XorValue(ExtractUtils.getInt(a + 20, data));
            Reward = new Reward_Payload((int)RewardSize.Value);
            Reward.InsertIn(ExtractUtils.getLong(a + 24, data) + offset, data);
            Id_tag = new StringXor(ExtractUtils.getLong(a + 32, data) + offset, data, FB);
            Id_tag2 = new StringXor(ExtractUtils.getLong(a + 40, data) + offset, data, FB);
        }

        public override string ToString()
        {
            string text = "ID: " + (!Id_tag.Value.Equals("") ? Id_tag.Value : Id_tag2.Value) + Environment.NewLine;
            text += "Start: " + (Start.Value <= 0 ? "Not available" : DateTimeOffset.FromUnixTimeSeconds((long)Start.Value).DateTime.ToLocalTime().ToString()) + Environment.NewLine;
            text += "Finish: " + (Finish.Value <= 0 ? "Not available" : DateTimeOffset.FromUnixTimeSeconds((long)Finish.Value).DateTime.ToLocalTime().ToString()) + Environment.NewLine;
            text += "Number: " + Index + Environment.NewLine;
            text += Reward + "-------------------------------------" + Environment.NewLine;
            return text;
        }
    }

}
