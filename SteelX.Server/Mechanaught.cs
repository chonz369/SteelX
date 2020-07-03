using SteelX.Shared;

namespace SteelX.Server
{
	public class Mechanaught : SteelX.Shared.Mechanaught
	{
		#region Variables
		/// <summary>
		/// The user who is owns this unit
		/// </summary>
		//public SteelX.Server.Player User { get; set; }

		/// <summary>
		/// The inventory this unit is part of
		/// </summary>
		public uint UserInventoryId { get; set; }

		/// <summary>
		/// The team this unit is on
		/// </summary>
		public byte Team { get; private set; }

		/// <summary>
		/// The launch order position of this unit
		/// </summary>
		public int LaunchOrder { get; set; }

		/// <summary>
		/// The name of this unit
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The head for this unit
		/// </summary>
		public Part Head { get; set; }
		public uint HeadId { get; set; }

		/// <summary>
		/// The chest for this unit
		/// </summary>
		public Part Chest { get; set; }
		public uint ChestId { get; set; }

		/// <summary>
		/// The arms for this unit
		/// </summary>
		public Part Arms { get; set; }
		public uint ArmsId { get; set; }

		/// <summary>
		/// The legs for this unit
		/// </summary>
		public Part Legs { get; set; }
		public uint LegsId { get; set; }

		/// <summary>
		/// The backpack for this unit
		/// </summary>
		public Part Backpack { get; set; }
		public uint BackpackId { get; set; }

		/// <summary>
		/// The left weapon for this units first weapon set
		/// </summary>
		public Weapon WeaponSet1Left { get; set; }
		public uint WeaponSet1LeftId { get; set; }

		/// <summary>
		/// The right weapon for this units first weapon set
		/// </summary>
		public Weapon WeaponSet1Right { get; set; }
		public uint WeaponSet1RightId { get; set; }

		/// <summary>
		/// The left weapon for this units second weapon set
		/// </summary>
		public Weapon WeaponSet2Left { get; set; }
		public uint WeaponSet2LeftId { get; set; }

		/// <summary>
		/// The right weapon for this units second weapon set
		/// </summary>
		public Weapon WeaponSet2Right { get; set; }
		public uint WeaponSet2RightId { get; set; }

		/// <summary>
		/// The position of this unit in the world
		/// </summary>
		public Vector3 WorldPosition;

		//TODO: Possibly replace X and Y with some sort of vector?

		/// <summary>
		/// The x aim of this unit
		/// </summary>
		public short AimX { get; set; }

		/// <summary>
		/// The y aim of this unit
		/// </summary>
		public short AimY { get; set; }
		#endregion

		#region Constructor
		public Mechanaught() 
		{
			Arm		= Parts.AGM001; //new Arms(Parts.AGM001);
			Leg		= Parts.LGM001; //new Legs(Parts.LGM001);
			Core	= Parts.CGM001; //new Cores(Parts.CGM001);
			Head	= Parts.HGM001; //new Heads(Parts.HGM001);
			Booster	= Parts.PGM001; //new Boosters(Parts.PGM001);
		}
		public Mechanaught(MechData mech) : this()
		{
			Mech = mech;
		}
		//public Mechanaught(Mech mech) : this()
		//{
		//	Arm = new Arms(mech.Arms);
		//	Leg = new Legs(mech.Legs);
		//	Core = new Cores(mech.Core);
		//	Head = new Heads(mech.Head);
		//	Booster = new Boosters(mech.Booster);
		//}
		public Mechanaught LoadOut(MechWeapon weap, byte set = 1)
		{
			SetWeapons(weap, set);
			return this;
		}
		public void SetWeapons(MechWeapon weap, byte loadout = 1)
		{
			Weapons[loadout, 1] = new Weapon(weap.LH);
			Weapons[loadout, 2] = new Weapon(weap.RH);
		}
		#endregion
	}
}