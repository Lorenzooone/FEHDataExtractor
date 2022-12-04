using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEHDataExtractor
{
    public class SingleWeaponClass
    {
        private string name;
        private int index;
        private string color;
        private int range;
        private bool magical;
        private bool is_staff;
        private bool is_dagger;
        private bool is_breath;
        private bool is_beast;

        public SingleWeaponClass(string name, int index, string color, int range, bool magical, bool is_staff, bool is_dagger, bool is_breath, bool is_beast)
        {
            Name = name;
            Index = index;
            Color = color;
            Range = range;
            Magical = magical;
            Is_staff = is_staff;
            Is_dagger = is_dagger;
            Is_breath = is_breath;
            Is_beast = is_beast;
        }

        public override string ToString()
        {
            return name.ToString();
        }

        public string Name { get => name; set => name = value; }
        public string Color { get => color; set => color = value; }
        public int Range { get => range; set => range = value; }
        public bool Magical { get => magical; set => magical = value; }
        public bool Is_staff { get => is_staff; set => is_staff = value; }
        public bool Is_dagger { get => is_dagger; set => is_dagger = value; }
        public bool Is_breath { get => is_breath; set => is_breath = value; }
        public int Index { get => index; set => index = value; }
        public bool Is_beast { get => is_beast; set => is_beast = value; }
    }
    class WeaponClass:CommonRelated
    {

        StringXor id_tag;
        StringXor[] sprite_base;
        StringXor base_weapon;
        UInt32Xor index;
        ByteXor color;
        ByteXor range;
        ByteXor _unknown1;
        ByteXor equip_group;
        ByteXor res_damage;
        ByteXor is_staff;
        ByteXor is_dagger;
        ByteXor is_breath;
        ByteXor is_beast;
                                           // 4 bytes of padding
        public WeaponClass()
        {
            Name = "Weapon Classes";
            Size = 0x30;
            ElemXor = new byte[]{ 0x4F, 0x4C, 0x66, 0x6D, 0xEB, 0x17, 0xBA, 0xA7 };
            Sprite_base = new StringXor[2];
            Index = new UInt32Xor(0x0C, 0x41, 0xD3, 0x90);
            Color = new ByteXor(0x2C);
            Range = new ByteXor(0x8B);
            Unknown1 = new ByteXor(0xD0);
            Equip_group = new ByteXor(0xB7);
            Res_damage = new ByteXor(0x07);
            Is_staff = new ByteXor(0x78);
            Is_dagger = new ByteXor(0xD7);
            Is_breath = new ByteXor(0x11);
            Is_beast = new ByteXor(0xB0);
        }

        public WeaponClass(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }

        public StringXor Id_tag { get => id_tag; set => id_tag = value; }
        public StringXor[] Sprite_base { get => sprite_base; set => sprite_base = value; }
        public StringXor Base_weapon { get => base_weapon; set => base_weapon = value; }
        public UInt32Xor Index { get => index; set => index = value; }
        public ByteXor Color { get => color; set => color = value; }
        public ByteXor Range { get => range; set => range = value; }
        public ByteXor Unknown1 { get => _unknown1; set => _unknown1 = value; }
        public ByteXor Equip_group { get => equip_group; set => equip_group = value; }
        public ByteXor Res_damage { get => res_damage; set => res_damage = value; }
        public ByteXor Is_staff { get => is_staff; set => is_staff = value; }
        public ByteXor Is_dagger { get => is_dagger; set => is_dagger = value; }
        public ByteXor Is_breath { get => is_breath; set => is_breath = value; }
        public ByteXor Is_beast { get => is_beast; set => is_beast = value; }

        public override void InsertIn(long a, byte[] data)
        {
            Id_tag = new StringXor(ExtractUtils.getLong(a, data) + offset, data, Common);
            Archive.Index++;
            Sprite_base[0] = new StringXor(ExtractUtils.getLong(a + 8, data) + offset, data, 0);
            if(!Sprite_base[0].ToString().Equals(""))
                Archive.NegateIndex++;
            Sprite_base[1] = new StringXor(ExtractUtils.getLong(a + 16, data) + offset, data, 0);
            if (!Sprite_base[1].ToString().Equals(""))
                Archive.NegateIndex++;
            Base_weapon = new StringXor(ExtractUtils.getLong(a + 24, data) + offset, data, Common);
            Archive.Index++;
            Index.XorValue(ExtractUtils.getInt(a + 32, data));
            Color.XorValue(data[a + 36]);
            Range.XorValue(data[a + 37]);
            Unknown1.XorValue(data[a + 38]);
            Equip_group.XorValue(data[a + 39]);
            //40 : Unknown2
            Res_damage.XorValue(data[a + 41]);
            Is_staff.XorValue(data[a + 42]);
            Is_dagger.XorValue(data[a + 43]);
            Is_breath.XorValue(data[a + 44]);
            Is_beast.XorValue(data[a + 45]);
        }
        public override string ToString()
        {
            String text = Id_tag.Value;
            if (Table.Contains("M" + Id_tag.Value.Insert(3, "_H")))
            {
                text = Table["M" + Id_tag.Value.Insert(3, "_H")].ToString();
                text = text.Remove(text.IndexOf("."));
                text = text.Contains("bow") ? text.Replace("bow", "Bow") : text;
            }
            text = "Weapon class: " + text + Environment.NewLine;
            text += !Sprite_base[0].ToString().Equals("") ? "Sprite: " + Sprite_base[0] + Environment.NewLine : "";
            text += !Sprite_base[1].ToString().Equals("") ? "Sprite: " + Sprite_base[1] + Environment.NewLine : "";
            text += "Base weapon: " + getStuffExclusive(Base_weapon, "");
            text += "Index: " + Index.Value + Environment.NewLine;
            text += "Colour: " + Colours.getString((Color.Value - 1) & 3) + Environment.NewLine;
            text += "Range: " + Range.Value + Environment.NewLine;
            text += "Equipment Group: " + Equip_group.Value + Environment.NewLine;
            text += "Targets: " + (Res_damage.Value == 1 ? "Resistance" : "Defense") + Environment.NewLine;
            text += Is_staff.Value == 1 ? "Is staff" + Environment.NewLine : "";
            text += Is_dagger.Value == 1 ? "Is dagger" + Environment.NewLine : "";
            text += Is_breath.Value == 1 ? "Is breath" + Environment.NewLine : "";
            text += Is_beast.Value == 1 ? "Is beast" + Environment.NewLine : "";
            text += "--------------------------------------------" + Environment.NewLine;
            return text;
        }
    }
    public class WeaponClasses : ExtractionBase
    {
        private Int64Xor numElem;
        private WeaponClass[] things;

        public Int64Xor NumElem { get => numElem; set => numElem = value; }
        internal WeaponClass[] Things { get => things; set => things = value; }

        public WeaponClasses()
        {
            WeaponClass tmp = new WeaponClass();
            Name = tmp.Name;
            NumElem = new Int64Xor(tmp.ElemXor);
        }
        public WeaponClasses(long a, byte[] data) : this()
        {
            InsertIn(a, data);
        }
        public override void InsertIn(long a, byte[] data)
        {
            a = Archive.Ptr_list[Archive.Index];
            NumElem.XorValue(ExtractUtils.getLong(a + 8, data));
            Archive.Index++;
            Things = new WeaponClass[NumElem.Value];
            String[] Wp = new String[NumElem.Value];
            SingleWeaponClass[] alpha = new SingleWeaponClass[NumElem.Value];
            a = ExtractUtils.getLong(a, data) + offset;
            for (int i = 0; i < NumElem.Value; i++)
            {
                Things[i] = new WeaponClass();
                Things[i].InsertIn(Archive, a + (Things[i].Size * i), data);
                String text = Things[i].Id_tag.Value;
                if (Table.Contains("M" + Things[i].Id_tag.Value.Insert(3, "_H")))
                {
                    text = Table["M" + Things[i].Id_tag.Value.Insert(3, "_H")].ToString();
                    text = text.Remove(text.IndexOf("."));
                    text = text.Contains("bow") ? text.Replace("bow", "Bow") : text;
                }
                alpha[Things[i].Index.Value] = new SingleWeaponClass(text, (int)Things[i].Index.Value, Colours.getString((Things[i].Color.Value - 1) & 3), Things[i].Range.Value, Things[i].Res_damage.Value == 1, Things[i].Is_staff.Value == 1, Things[i].Is_dagger.Value == 1, Things[i].Is_breath.Value == 1, Things[i].Is_beast.Value == 1);
                Wp[Things[i].Index.Value] = alpha[Things[i].Index.Value].ToString();
            }
            WeaponNames = new StringsUpdatable(Wp);
            WeaponsData = alpha;
        }

        public override string ToString()
        {
            String text = "";
            for (int i = 0; i < NumElem.Value; i++)
                text += Things[i];
            return text;
        }
    }
}
