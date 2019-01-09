# FEHDataExtractor

A Data Extractor for FEH files.

Features
=====

The Extractor gives the following options to obtain data from files:

* __Heroes__ (assets/Common/SRPG/Person)
* __Enemies__ (assets/Common/SRPG/Enemy)
* __Generic Text__ search in files (assets/Common/.*)
* __GC World__ (assets/Common/Occupation/World) or the Grand Conquest Map Settings
* __Skills__ (assets/Common/SRPG/Skill)
* __Quests__ (assets/Common/Mission)
* __Tempest Trials__ (assets/Common/SRPG/SequentialMap) __BETA!__
* __Messages__ (assets/*/Message/**/*.bin)
* __Weapon Classes__ (assets/Common/SRPG/Weapon.bin.lz)
* __Forging Bonds__ (assets/Common/Portrait) __BETA!__


In the future it will support more things. Supports auto decompression.

Usage
=====

## Loading Localization (First thing you should do)

To have translated output, use the top menu option Load -> Messages and choose the folder with the .lz files of the chosen language. If all goes well, the output will be translated. The output will also be written to the specified folder and its subfolders. (Example: choosing USEN/Message as the folder will get all the US English translated text)

## Reading data - Heroes, Enemies, GC World, Skills, Quests, Tempest Trials, Weapon Classes and Forging Bonds -

To read all the above data, use the top menu option File -> Open and choose the files you want to examine in the folders suggested above. Choose the corresponding option in the dropdown box and click Extract. 

## Generic Text and its uses

Generic Text tries to read all strings from a file with the Common Xor.

Here's what you can get with it:

* __Arena Bonus Units__ ( Examine files in the /assets/Common/Delivery/ArenaPerson folder)
* __Aether Raids Bonus Units and Structures__  (Examine files in the /assets/Common/SkyCastle/BattleData folder)
* Pieces of text from other files

In general, Generic Text can be pretty useful when you don't have a way to decompile the things yet.

To get this data, use the top menu option File -> Open and choose the files you want to examine. Choose the Generic Text option in the dropdown box and click Extract.

## In case of Weapon Classes update

If an update that adds Weapon classes comes to the game, load the messages and then examine assets/Common/SRPG/Weapon.bin.lz with the Weapon Classes option. This will add the new weapons in for the time being and you will be able to examine what you want without the extractor crashing. An update will come later adding them to the default weapons.

Credits
=====

* Thanks to https://github.com/SciresM/FEAT and https://github.com/einstein95/dsdecmp for the base of the decompression code.
* Thanks to https://github.com/HertzDevil for RE documentation, methods and data structures.
