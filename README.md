# SteelX  
Unity version 2018.3.6f1 (Not Important for Backend Services)

# Project Scope

*Notes: anything that runs monobehaviour runs on unity engine and is also a frontend utility.*

## CORE
- **Mech.cs :** struct is a breakdown on mech parts used anywhere a mech needs to be passed as a value.
  - Arm
  - Leg
  - Head
  - Booster
  - Core
- **Weapon.cs:** struct for weapon loadout (Left hand slot, and right slot combined make one weapon struct).
  - Name
  - Damage
  - Range
  - Type
- **Skill.cs:** struct for skill info
  - SP
  - Damage
  - Range
  - Name
  - Type
  - Description
- **Mechanaught.cs:** class where it contains both the Mech struct, and Weapon/Skill loadouts; the durability and hp remaining on the different parts or overall mech. Also has name of build
  - WeaponLoadout (1 and 2)
    - LeftHand
    - RightHand
  - SkillLoadout (1-4)  
Can only add skills compatible with weapons selected
    - Skills
  - Mech
    - Parts
    - Color
    - Durability
    - Stats
      - HP
      - EN
      - SP
Name
- **Player.cs:** Player profile and public record
  - Active/Current/Primary
    - Mechanaught (+ Mech Name)
    - Pilot
    - Rank 
    - Clan/Guild
    - Bonuses  
Players were allowed to buy double experience and money for set amount of time
  - Money
    - Premium Credits
    - Regular Credits
    - Repair Points
  - Mastery (Experience?) Points  
When you gained enough experience points to level up… you earned a mastery point. You were allowed to assign mastery points to misc stats for extra perks
    - Kills/Deaths Ratio => KDR
    - Weapons
      - KDR with Weapon
      - Mastery Rank
    - Maps/Game Mode
    - Mech
    - Pilot
    - Perks (Bonuses Stats from Mastery Rank)
  - Inventory 
    - Available Slots (Hanger Space)
    - Items Purchased/Owned
    - Mech Builds/Sets/Loadouts


## SERVICES
- **Operator.cs**
  - Manage Mastery Points
  - Change Pilot
  - Show Available Pilots  
Pilots are only displayed as they're unlocked
- **OperatorStats.cs**
  - Automatically generated, not sure a new class is needed just to do simple math
  - Display Before/After Stat Differences
  - Calculate and Tally Stats for Combined Parts
- **Lobby.cs**
  - Display Pilot Dialog
  - Load Active Mech (Show Player Info)
  - Load Friend List
  - Load Chat Rooms
    - Guild/Clan
    - Private/Whisper
    - Friends
    - Public/Region/Server
- **Shop.cs**
  - Purchase Item
  - Load All Items 
    - Mastery Rank
    - Price
    - Durability
    - Name
    - Weight type (Series)
    - Description
- **Hanger.cs**
  - Load All Mechanaughts (Each part can only be used once per build)
    - Remaining Durability
    - Rename Mech
    - ~~Change Pilot~~  
Pilot can only be changed from operator menu
    - Change Paint Color
- **Gameplay.cs**  
By default when match begins all players can load onto map as connections are initializing. Upon all connections being established, game countdown begins and players are reset back to “spawn location” window.   
*Note: Kill Cams double as Spectator Cams (Players are allowed to join matches as spectators in available room slots)* 
  - Match Stats
    - Duration
    - Score
    - Map Objectives
  - Respawn Window
    - Load Hanger
    - Change Active Mech
    - Display Spawn locations on map
    - Display Map objectives on map
    - Display Players on map
  - Load All Players
    - Load Active Mech  
Mech Pilots could only be changed and selected before match (or before respawn)...
    - Check Durability   
if durability is zero, mech cannot be used anymore
    - Check Credits  
if durability is zero, credits are needed to purchase more durability
    - Check repair points  
Each time player respawns, repair points are used to recover damages (durability) 
    - ~~Track~~ Update Status  
Only refresh on anything that changes?...
    - Locations 
    - Key Inputs  
If player is idle for too long, they should get some sort of punishment?
    - Duration 
Active amount of time spent playing, for end game reward
    - Minimap/Radar   
was there a radar in the game?...
    - Mech HP  
    - Revive/Spawn location  
Player will continue to respawn at last selected location they previously chose
    - Track Stats  
Stats should be saved and recorded as soon as they’re triggered. Premature termination doesnt invalidate player performance. Premature termination do not grant end-game rewards.  
Players are awarded credits based on performance (overall damage dealt/kills made)...
      - Kills/Death  
With weapons for broadcast display, weapon mastery, and competitive profile tracking
      - Damage   
I think you can get mastery points for amount of damage given per weapon type...
      - Duration  
Players are rewarded experience points based on how long they played for
    - ~~Manage~~ Get Connections (Room/Server)  
      - Assign Network_Id to Player
      - Load/Join Chat (Room/Server)
  - Room Commands
    - [Vote to] Kick Player
    - Change/Assign Team
    - Suicide/Unstick Me
    - Mute Player
    - End Match (Return Everyone to Room Lobby)
    - Change Host (change room admin) 
    - Report/Flag Player
    - Invite Player [to Match]
         



ResourceManager.cs : class, manages all resources that need to be loaded in game (like prefabs
