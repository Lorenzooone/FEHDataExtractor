using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FEHDataExtractor
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            initializeWeapons();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(new GCWorld(), new BaseExtractArchive<SinglePerson>(),
                new BaseExtractArchive<SingleEnemy>(), new GenericText("", CommonRelated.Common),
                new BaseExtractArchive<SingleSkill>(), new BaseExtractArchive<Quest_group>(),
                new Decompress(), new BaseExtractArchive<TempestTrial>(),
                new Messages(), new WeaponClasses(),
                new BaseExtractArchiveDirect<Forging_Bonds>(),
                new BaseExtractArchiveInteger<SingleSubscription>(),
                new BaseExtractArchive<SingleArenaPerson>(),
                new BaseExtractArchive<SingleCaptainSkill>(),
                new BaseExtractArchive<SingleSkillAccessory>(),
                new BaseExtractArchive<SingleWeaponRefine>()
                ));
        }

        public static void initializeWeapons()
        {
            SingleWeaponClass[] a = new SingleWeaponClass[24];
            a[0] = new SingleWeaponClass("Sword", 0, "Red", 1, false, false, false, false, false);
            a[1] = new SingleWeaponClass("Lance", 1, "Blue", 1, false, false, false, false, false);
            a[2] = new SingleWeaponClass("Axe", 2, "Green", 1, false, false, false, false, false);
            for (int i = 0; i < 4; i++)
            {
                a[i + 3] = new SingleWeaponClass(ExtractionBase.Colours.getString(i) + " Bow", 3 + i, ExtractionBase.Colours.getString(i), 2, false, false, false, false, false);
            }
            for (int i = 0; i < 4; i++)
            {
                a[i + 7] = new SingleWeaponClass(ExtractionBase.Colours.getString(i) + " Dagger", 7 + i, ExtractionBase.Colours.getString(i), 2, false, false, true, false, false);
            }
            for (int i = 0; i < 4; i++)
            {
                a[i + 11] = new SingleWeaponClass(ExtractionBase.Colours.getString(i) + " Tome", 11 + i, ExtractionBase.Colours.getString(i), 2, true, false, false, false, false);
            }
            a[15] = new SingleWeaponClass("Staff", 14, "Colorless", 2, true, true, false, false, false);
            for (int i = 0; i < 4; i++)
            {
                a[i + 16] = new SingleWeaponClass(ExtractionBase.Colours.getString(i) + " Breath", 15 + i, ExtractionBase.Colours.getString(i), 1, true, false, false, true, false);
            }
            for (int i = 0; i < 4; i++)
            {
                a[i + 20] = new SingleWeaponClass(ExtractionBase.Colours.getString(i) + " Beast", 19 + i, ExtractionBase.Colours.getString(i), 1, false, false, false, false, true);
            }
            ExtractionBase.WeaponsData = a;
        }
    }
}
