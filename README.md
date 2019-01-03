# FEHDataExtractor

A Data Extractor for FEH files. For now it supports:

Heroes (assets/Common/SRPG/Person)

Enemies (assets/Common/SRPG/Enemy)

Generic Text search in files (assets/Common/.*)

Grand Conquest Map Settings (assets/Common/Occupation/World)

Skills (assets/Common/SRPG/Skill)

Quests (assets/Common/Mission)

Tempest Trial (assets/Common/SRPG/SequentialMap) BETA!

Messages (assets/*/Message/**/*.bin)

Weapon Classes (assets/Common/SRPG/Weapon.bin.lz)

To have translated output, use the menu option Load -> Messages and choose the folder with the .lz files of the chosen language. If all goes well, the output will be translated. The output will also be written to the specified folder and its subfolders. (Example: choosing USEN/Message as the folder will get all the translated text)

If an update that adds Weapon classes comes to the game, load the messages and then examine assets/Common/SRPG/Weapon.bin.lz with the Weapons option. This will add the new weapons in for the time being and you will be able to examine what you want without the extractor crashing. An update will come later adding them to the default weapons.

In the future it will support more things. Supports auto decompression.

Thanks to https://github.com/SciresM/FEAT and https://github.com/einstein95/dsdecmp for the base of the decompression code.