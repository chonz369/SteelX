namespace SteelX.Shared
{
	//ToDo: npcSkill Number xxxxx = x[Npc Level1~9]+xx[NpcType1~99]+xx[NpcVari1~99]
	// Puppet Berserker, NpcType = 5
	public struct SkillData
	{
		/// <summary>
		/// 
		/// </summary>
		public int? CodeTemplate { get; private set; }
		/// <summary>
		/// Skill name? Aura of blessing, healthy damage, etc.?
		/// Used to check whether the skill is the same on the UI. The effects used together have the same name.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Classification of code types in the proposal (skill, passive, tunning)
		/// </summary>
		public CodeTypes CodeType { get; private set; }
		/// <summary>
		/// 0,1,2,3,4
		/// </summary>
		public int Level { get; private set; }
		/// <summary>
		/// Experience to advance to the next level
		/// </summary>
		public int UpgradeExp { get; private set; }
		/// <summary>
		/// Skill Category ::= control | overclock | hacking | computing (exclusive)
		/// </summary>
		public CodeCategorys CodeCategory { get; private set; }
		/// <summary>
		/// The equipment/slot this skill is used by
		/// </summary>
		public CodeEquipTypes CodeEquipType { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public CodeWeaponTypes? CodeWeaponType { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public bool Stackable { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int? MaxStack { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int ConsumedSP { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int ConsumedEN { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int ConsumedHP { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public float CastingTime { get; private set; }
		/// <summary>
		/// Duration in seconds before skill can be casted again
		/// </summary>
		public float CoolTime { get; private set; }
		/// <summary>
		/// Activation Target? It corresponds to oneself, enemy, and friend, and is literally a target for activation. :: self, enemy, ally (exclusive)
		/// </summary>
		public CodeActivationTargets CodeActivationTarget { get; private set; }
		/// <summary>
		/// Target range of skill.
		/// Self cast do not have a target range.
		/// </summary>
		public float? TargetDistance { get; private set; }
		/// <summary>
		/// Target cone of skill.
		/// Self cast do not have a target range.
		/// </summary>
		public float? TargetAngle { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int MaxTarget { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int ProjectileCount { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int? TargetMaxProjectile { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int? CodeEffect1 { get; private set; }
		public int? CodeEffect2 { get; private set; }
		public int? CodeEffect3 { get; private set; }
		/// <summary>
		/// Identifier Id for this skill animation
		/// </summary>
		public int AttackSkill { get; private set; }
		/// <summary>
		/// SP must be equal to or greater than this amount in order to trigger
		/// </summary>
		/// Convert string to an int
		//public string ActiveCondition { get; private set; }
		public int ActiveCondition { get; private set; }
		/// <summary>
		/// Conditions needed to disable skill from being used
		/// </summary>
		public int? InactiveCondition { get; private set; }
		/// <summary>
		/// Name of the prefab asset associated with this skill's display image
		/// </summary>
		public string Icon { get; private set; }
		/// <summary>
		/// Skill name-Localization string ID
		/// </summary>
		public string DisplayName { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public string Description { get; private set; }
	}
}