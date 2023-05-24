# Description:
This mod adds an RPG-like system of levels and attribute increases: - Wacky Branch 1.7.0

BIG WACKY UPDATE! - You asked for more categories, Wacky adds 2 MORE! You asked for using trophies to generate XP. Wacky added XP Orbs and Magic Potions!

Support me at https://www.buymeacoffee.com/WackyMole https://ko-fi.com/wackymole

<img src="https://wackymole.com/hosts/bmc_qr.png" width="100"/> <script type='text/javascript' src='https://storage.ko-fi.com/cdn/widget/Widget_2.js'></script><script type='text/javascript'>kofiwidget2.init('Support Me on Ko-fi', '#29abe0', 'H2H6LL5GA');kofiwidget2.draw();</script> 

![https://wackymole.com/hosts/mainimage.png](https://wackymole.com/hosts/mainimage.png)

Features:
 - Shared group XP. Outside of groups all XP awards go to the character who struck the last blow.
 - Custom mobs can be added for XP gain.
 - MMO-like friends list. -[Groups](https://valheim.thunderstore.io/package/Smoothbrain/Groups/)
 - On screen XP bar.
 - Compatible with [ItemRequiresSkillLevel](https://valheim.thunderstore.io/package/Detalhes/ItemRequiresSkillLevel/) mod. Equipment can be limited by level or attribute.
 - Compatible with [KGMarketplace mod](https://valheim.thunderstore.io/package/KGvalheim/Marketplace_And_Server_NPCs_Revamped/). Experience rewards can be added: (EpicMMO_Exp:250) Quests can be limited by level (EpicMMO_Level: text, 20)
 
 ![https://wackymole.com/hosts/2nd%20image.png](https://wackymole.com/hosts/2nd%20image.png)

<details><summary>Attributes</summary>

	Strength: Physical Damage increase, Carry Weight Increase, Stamina Regeneration%

	Agility: Attack Stamina Consumption decrease, Stamina increase, Stamina consumption (running, jumping) decreased,

	Intellect: Elemental Damage increase, Elemental Armor increase, Eitr regen increases,  Eitr Increase

	Endurance: Physical Armor increase, HP increase, Health Regeneration

</details> 

<details><summary>Friends list</summary>

MMO-like friends list. -Groups MOD Group to earn XP, download requires Group mod for each client https://valheim.thunderstore.io/package/Smoothbrain/Groups/

Click the plus button at the bottom of the friends bar. Enter the name of the character you wish to add, starting with a capital letter.
   ![https://wackymole.com/hosts/3rd%20image.png](https://wackymole.com/hosts/3rd%20image.png)
The player will receive a friend request. Once accepted, the character will appear in your friends list. Group invites can be sent from the friends list. 

# Warning: 
- If you accept a friend request while the player who sent it is not logged in with the character, you will not be added to their friends list and they will need to resend the friend request.
- You cannot send friend requests to yourself or characters you have already added. If you need to send another friend request, remove the character from the list first.
- Friend requests that have been sent, but not accepted will be removed on logout. They must be accepted while both characters are online.
</details> 

<details><summary>Creature level control</summary>

This mod assigns levels to all in-game monsters.

![https://wackymole.com/hosts/creaturecontrol.png](https://wackymole.com/hosts/creaturecontrol.png)

Mobs (names, levels, exp) from other mods are included:

Fantasy-Creatures, AirAnimals, Defaults, DoOrDieMonsters, LandAnimals, MonsterlabZ, Outsiders, SeaAnimals, Monstrum (free and paid), Krumpac Mods(free and paid), Teddy Bears, PungusSouls

Monsters that are 1 level higher than the character + MaxLevelRange will curve XP.

With defaults, starting exp req is 500 with a 1.04 multiplayer.  So first 5 levels of experience required will be: level 1 is 500, 2 is 1020, 3 is 1560, 4 is 2122, 5 is 2707

FirstLevelExperience used on each level: disabled means that the levels will not add 500 each time: level 1 is 520, 2 is 541, 3 is 562, 4 is 585, 5 is 608. The jsons will all have to be reworked if this is disabled

Below is an image of 1.04 +500 and with FirstLevelExperience disabled, so no 500 added. The difference is a lot. Also 1.08 scaling is added just to show how it gets into the millions pretty quickly. 

![https://wackymole.com/hosts/epicmmolevelcalcs.png](https://wackymole.com/hosts/epicmmolevelcalcs.png)

With Low_damage_level- Damage dealt to a higher level monster will be reduced by the difference in levels. E.g. (Character level 20/ Monster level 50 = 0.4. Damage dealt will be 0.4% of normal damage) 
damageFactor = (float)(playerLevel + LowDamageConfig)/ monsterLevel; You can configure LowDamageConfig to adjust damage scaling up or down. Damage Factor will not go above 1 or below .1f

Higher level monsters will have their names appear in red. Monsters within your range will be white.

If you are significantly higher level than a monster, your XP award will be reduced. Monsters that are significantly lower level than you will have their names appear in cyan.

All of these formulas functions can be configured in the settings file.
A file listing all monsters and their levels is located in config/EpicMMOSystem/MonsterDB_"Version".jsons

A file called Version.txt is created in the folder. It contains the mod version that was used to create it. Replace it with "NO" to stop it from overwritting on a future update.

Latest Update for Jsons config is <b> 1.7.0 </b>(Number will be updated when Jsons recieve an update)

Please note:
When upgrading the mod to a newer version, new fields in the settings file will be created automatically. You will have to manually re-edit these values if you have changed them.
If you have no custom settings in the configuration file, you should delete the file so that a fresh one can be created by the new version.

Note for other Mods: This mod uses hit.toolTier to pass the Lvl of player



</details>

<details><summary>Reset Skill Points</summary>

There are configs for setting the Reset currency, default is coins. You set the ammount per level.

There is also an Item called ResetTrophy that you can spawn or add to the builtin droplist that will allow any level reset with only 1 ResetTrophy.

The mod looks for your reset currency first and then ResetTrophies. Only consumes 1, so make this a very rare item. 

</details>

<details><summary>UI</summary>

![https://wackymole.com/hosts/CenterBar.png](https://wackymole.com/hosts/CenterBar.png)

	1HudPanelPosition: Main UI Panel Draggable, default color set by HudBackgroundCol, Type "none" to make it disappear

	HudBarScale: Scale this up or down to resize ALL MMO UI elements. - 1.0 Should cover all of your screen horizontally 

	2-5 UI elements have Position, Scale and Color: 
	 Scale (x, y, z)- z does not matter. - float
	 Color: #(6 digit Hex),  optional 7-8 Digit means alpha. #986100FF (FF -alpha of 1) or use without # red, cyan, blue, 
	 darkblue, lightblue, purple, yellow, lime, fuchsia, white, silver, grey, black, orange, brown, maroon, green, olive, navy, teal, aqua, magenta
	 set color to none, to hide element
	 
	 Can all be set to "none" to make individual elements disappear

	2ExpPanelPosition: Dragable EXP BAR	

	3StaminaPanelPosition: Dragable
	
	4HpPanelPosition: Dragable

	5EitrPanelPosition: Dragable, will disappear and reappear when you have Eitr.

	DisabledHealthIcons: This disables the red Health Icon that is normal present under vanilla health bar

	To enable ONLY EXP bar , enable OldXPBar Bar Only and restart - not dragable in this mode, this is being slowly phased out. 

	![https://wackymole.com/hosts/Attributes.png](https://wackymole.com/hosts/Attributes.png)

</details> 

<details><summary>Console commands</summary>

Admin only commands: - Should work in singleplayer now
 - To set a character's level: `epicmmosystem level [value] [name]` 
 - To reset attribute points: `epicmmosystem reset_points [name]` 
 - To recalc levels based on total experience: `epicmmosystem recalc [name]` 
 - Should work with spaces in names now or replace spaces with '&'
</details> 

<details><summary>Feedback</summary>


Wacky Git https://github.com/Wacky-Mole/WackyEpicMMOSystem

Original git - https://github.com/Single-sh/EpicMMOSystem

For questions or suggestions please join discord channel: [Odin Plus Team](https://discord.gg/odinplus)

Support me at https://www.buymeacoffee.com/WackyMole 

<img src="https://wackymole.com/hosts/bmc_qr.png" width="100"/>

Original Creator: LambaSun or my [mod branch](https://discord.com/channels/826573164371902465/977656428670111794)

</details> 
