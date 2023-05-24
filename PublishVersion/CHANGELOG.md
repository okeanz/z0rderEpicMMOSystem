<details>
  <summary><b><span style="color:aqua;font-weight:200;font-size:20px">
    ChangeLog
</span></b></summary>

| Version | Changes                                                                                                                                                                                                                                                                                                                                |
|----------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 1.7.0  | - BIG WACKY UPDATE! - Added Category Vitality and Specializing, while rearranging the categories. <br/>Added hover stats, so you can see what exactly adding points to a category does.  Red being most powerful <br/> Added 6 magic orbs that grant various levels of XP on consumption, they have 1 % chance to drop from all mobs. Guaranteed from Bosses. </br> Added Magic Potions and Magical Fermentor, so you can utilize trophies to make the mead and use the potion to get more XP! 3 Potion levels and various ways to craft mead. <br/> added attackSpeed, MiningDmg, TreeCuttingDmg and Critical attack chance and Critical Damage. 
| 1.6.7  | - Updated KG Marketplace API <br/> Updated DoororDieMob and LandAnimals jsons thanks to DeeJay <br/> Fixed a bug with FloatText XP not being correct
| 1.6.6  | - More UI adjustments, - Scaling/Position on NavBar and PointHud,
| 1.6.5  | - TeddyBears json added <br/> removeAllDropsFromNonPlayerKills is true by default (If a Tree kills a mob, you won't get drop) <br /> Added 'U Jerk, NoExpOn Red/Blue' cfg option, for unfun times/admins. 
| 1.6.4  | - Fixed 'RemoveAllDrops From NonPlayer Kills' for some configs
| 1.6.3  | - Fixed OldExp Bar <br /> close or apply, now closes window. <br /> Update Outsider json
| 1.6.2  | - Update Chinese <br/> Fixed UI so it can disappear with Ctrl-F3 <br/> Group EXP Range (GROUP MOD ONLY) - If killer didn't get xp, group won't get xp <br/> Added Player EXP Range, how far the player who killed a mob gets XP. <br/> Added the ability for tames to give their master EXP on kills <br/> Added Json for Krumpac mobs <br/> RemoveAllDrops From NonPlayer Kills config. - No more random creature drops - Not enabled by default </br> Added Config to remove alert on Left side for XP on mob death </br> Config for popup XP string
| 1.6.1  | - eXP level fix<br/> HP/stamina, XP/eitr bar elements can be changed to "none" and update/disappear realtime<br/> Update Chinese <br/> Included color exp on mob death.
| 1.6.0  | - Fixed ResetTrophy not being consumed on use.<br/> Added Chinese Translation <br/> Fixed some red errors with CLLC <br/> Included an excel file for comparison. 
| 1.5.9  | - Update for 214.2
| 1.5.8  | - Update for single char word languages - horizonal overflow <br/> MonsterDB update and added mob levels for Monstrum(beta)
| 1.5.7  | - Fix for stamina consumption for running and jumping. Was increasing instead of decreasing, credits for discovery.  Moissonneur and Kevin
| 1.5.6  | - Added German Localization <br /> Changed defaults to not take any drops away, no matter the level. - Blax complained enough - <br /> Stamina Regen is a percentage <br /> Added MOB UI string for wacky fun, aka single char word languages <br /> Update ItemManager <br /> Added ability to set "none" on color for all hud elements to make them go away, should live update. Might have relaunch to get them to come back| 1.5.5  | - Update readme to talk about "FirstLevelExperience used on each level" - Added an excel image for XP comparisons between modes - Most people should NOT disable this, makes balancing completely different.
| 1.5.4: | - Updated to allow level and reset commands for Spaced Names. <br/>Updated Jsons, Added extra text file for people who don't read readme or version changes... you know who you are.... <br/> Added abilty for EpicMMO to recalculate maxlvls on serversync updates. I still don't recommend live updating with this mod, but less bugs now. <br/> Serious discussion: It appears if you ever changed expierence values(rateExp,expForLvlMonster, etc) after players started playing, things could get wonky unless you reset them(even after game restarts). I added a TotalExp tracker, but it won't be useful unless you restart all your players back to 0. I have added another command to Terminal recalc, but it will reset players levels to 0 if not a new charc on this update.  <br/> Added MobLevelPosition and BossLevelPosition for server admins to config mob bar placement. </br> Fixed lowDamageExtraConfig, small oversight <br/> Added ResetTrophy item for people to add to droplists <br/> Added EitrIncrease to Intellegence - More OP 
| 1.5.3: | - Fixed bug in Groups exp sharing. <br/> Added MajesticChickens json
| 1.5.2: | - Added Colors and Scale to Individual UI elements.<br/> Fixed EpicLoot drop bug, made Nav Panel moveable, Eitr UI adjustments<br/> Low_damage_config for extra configurability on low damage mode
| 1.5.1: | - Added Stamina regeneration<br/>
| 1.5.0: | - Changed Config to WackyMole.EpicMMOSystem.cfg<br/> - Made all the UI elements dragable<br/> - Realtime setting of (x,y) position in config, type "none" in BackgroundColor to remove brown bar.<br/> - Added Filewatcher to Jsons<br/> - dedicated Server only<br/> - Added filewatcher to configs, Updated Group logic<br/> - Revamped Mentor mode.<br/>
| 1.4.1: | - Fix Version Check and Multiplayer Sync, moved Monster Bar again.<br/>
| 1.4.0: | - Fix for inventory to bag JC (hopefully)<br/> - Changed Configs,PLEASE DELETE OLD CONFIGS!<br/> - added removeDropMax, removeDropMax,removeBossDropMax, removeBossDropMix, curveExp, curveBossExp.<br/> - Allow for multiple Jsons to be searched<br/> - Added admin rights to singleplayer hosting<br/> - Boss drop is determined by mob.faction(), curveBossExp Exp is just the 6 main bosses. <br/> - Updated Monster.json moved to configs instead of plugin.<br/> - Added ExtraDebugmode for future issues.<br/> - Updated MonserDB_Default for mistlands,LandAnimals mod, MonsterLabZ, Outsiders, SeaAnimals, Fantasy Creatures, Air Animals, and Outsiders.<br/> - Json file in MMO folder is searched.<br/> - Added Version text to easily update in future.<br/> - Write "NO" in Ver.txt to skip future updates. Moved Monster lvl bar [] for boss and non boss<br/>
| 1.3.1: | - Dual wield and EpicMMO Thanks to KG, sponsored by Aldhari/Skaldhari<br/>
| 1.3.0: | - WackyEpicMMOSystem release, until author comes back. Code from Azumatt - Updated Chat, Group and ServerSync<br/>
| 1.2.8: | - Added a limiter for the maximum attribute value.<br/>- New view health and stamina bar (in the configuration you can return the old display where only the experience is displayed).<br/>
| 1.2.7: | - Fix version check<br/>
| 1.2.6: | - Fixed bug of different amount of experience. Added ability to add your own items or currency to reset<br/> attributes.
| 1.2.5: | - Fix damage monsters and fix error for friends list<br/>
| 1.2.4: | - Fix version check<br/>
| 1.2.3: | - Add console command and xp loss on death<br/>
| 1.2.2: | - Add button to open the quest journal (Marketplace) and profession window<br/>
| 1.2.1: | - Fix errors with EAQS<br/>
| 1.2.0: | - Add friends list feature<br/>
| 1.1.0: | - Add creature level control<br/>
| 1.0.1: | - Fix localization and append english text for config comments.<br/>
| 1.0.0: | - Release<br/>
</details> 