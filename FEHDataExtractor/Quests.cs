using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEHDataExtractor
{
    public class QuestUnitMatch : ExtractionBase
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
            String text = "";
            return text;
        }

        public override void InsertIn(long a, byte[] data)
        {
            throw new NotImplementedException();
        }
        // 4 bytes of padding
    }
}
