# Mega Mix Mod Manager
 Basic mod creator and installer for Hatsune Miku Mega Mix
 ![UI](Preview Images/ImageMain.png)

## Features
Install mods into a sortable list that determines load order, the higher the mod the earlier it will load. mods installed should be structured with either the romfs folder as the zip root, or the romfs folder in the zip root.<br/>
**Example:**
```bash
├──romfs
    ├──rom_switch
        ├──rom
```
or 
```bash
├──rom_switch
    ├──rom
```

Create mod zip files with info and thumbnail viewable in the mod manager. the selected mod folder should have the same structure as displayed above.<br/>

File Merging, certain database files can be merged(currently only supports pv_db lite merging) so that mods that edit the same database can be used together.<br/>
merging comes in different options, Lite merging is per entry based, so it will check the entry as a whole object. Deep merging will analyze each part of an entry and merge them all together.<br/>
**Example:**<br/>
Lite merging the pv_db is check if the pv_001 in the mod folder is different than then one in the game dump, if so it will use that entry instead of the final merged pv_db.<br/>
Deep merging will check each line in the pv_db and determine whats new and what to add, so if one mod adds an extra_singer to pv_001 and changes the songinfo, the final merged pv_db will have both changes.
 

## Options
Game Dump Path should select the romfs folder of your game dump, a check box indicate if the game dump has the required files present.<br/>
The Export Path should select the romfs folder of where you want the mods to be.
Default Author will auto fill the author line in the mod creator.<br/>
Merge Options let's you select what level of merging you want for different database files.
