using SteelX.Shared;

namespace SteelX.Client.Packets.Game
{
    /// <summary>
    /// Sent when the client wants to swap weapons
    /// </summary>
	[System.Serializable]
    public class ChangeWeaponsetDto
    {
        /// <summary>
        /// The weaponset they wish to switch to
        /// </summary>
        public int _desiredWeaponset { get; private set; }
    }
}