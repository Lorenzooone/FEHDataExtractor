# FEHDataExtractor

A Data Extractor for FEH files.

Features
=====

The Extractor gives the following options to obtain data from files:

* __Heroes__ (assets/Common/SRPG/Person)

* __Enemies__ (assets/Common/SRPG/Enemy)

* __Generic Text__ search in files (assets/Common/.*)

* __Grand Conquest Map Settings__ (assets/Common/Occupation/World)

* __Skills__ (assets/Common/SRPG/Skill)

* __Quests__ (assets/Common/Mission)

* __Tempest Trials__ (assets/Common/SRPG/SequentialMap) __BETA!__

* __Messages__ (assets/*/Message/**/*.bin)

* __Weapon Classes__ (assets/Common/SRPG/Weapon.bin.lz)

* __Forging Bonds__ (assets/Common/Portrait) __BETA!__

To have translated output, use the menu option Load -> Messages and choose the folder with the .lz files of the chosen language. If all goes well, the output will be translated. The output will also be written to the specified folder and its subfolders. (Example: choosing USEN/Message as the folder will get all the translated text)

If an update that adds Weapon classes comes to the game, load the messages and then examine assets/Common/SRPG/Weapon.bin.lz with the Weapon Classes option. This will add the new weapons in for the time being and you will be able to examine what you want without the extractor crashing. An update will come later adding them to the default weapons.

In the future it will support more things. Supports auto decompression.

Credits
=====

*Thanks to https://github.com/SciresM/FEAT and https://github.com/einstein95/dsdecmp for the base of the decompression code.
*Thanks to https://github.com/HertzDevil for RE documentation and structures.
