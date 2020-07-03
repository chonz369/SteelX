using SteelX.Shared;

namespace SteelX.Client.Packets.Game
{
    /// <summary>
    /// Sent when the client wants to swap weapons
    /// </summary>
    public class RequestChangeWeaponset : ClientGameBasePacket
    {
        /// <summary>
        /// The weaponset they wish to switch to
        /// </summary>
        private readonly int _desiredWeaponset;

        public override Shared.PacketTypes PacketType
        {
            get
            {
                return Shared.PacketTypes.REQ_CHANGE_WEAPONSET;
            }
        }

        public RequestChangeWeaponset(byte[] data, GameSession client) : base(data, client)
        {
            _desiredWeaponset = GetInt();
        }

        /*public override string GetType()
        {
            return "REQ_CHANGE_WEAPONSET";
        }*/

        protected override void RunImpl()
        {
            GetClient().GameInstance.SwitchWeapons(Unit, _desiredWeaponset);
        }
    }
}