using Data.Model;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Base packet for game packets from the client
    /// </summary>
    public abstract class ClientGameBasePacket : ClientBasePacket
    {
        /// <summary>
        /// The unit for this client
        /// </summary>
        protected readonly Unit Unit;
        
        protected ClientGameBasePacket(byte[] data, GameSession client) 
            : base(data, client)
        {
            Unit = GetClient().User.CurrentUnit;
        }

        /// <summary>
        /// Ticks the unit by reading the current time stamp
        /// TODO: Should this be called during the send step?
        /// </summary>
        protected void TickUnit()
        {
            var ping = GetUInt();
            
            var delta = Unit.UpdatePing(ping);
            
            GetClient().GameInstance?.TickUnit(Unit, delta);
        }

        /// <summary>
        /// Reads the units current aim and pos values from the packet
        /// Usually located at the end of the packet
        /// </summary>
        protected void GetUnitPositionAndAim()
        {
            // TODO: Make this better
            // Just for practice mode right now
            if (Unit == null) return;
            
            // Read aim
            Unit.AimX = GetShort();
            Unit.AimY = GetShort();

            // Read position
            Unit.WorldPosition.X = GetFloat();
            Unit.WorldPosition.Y = GetFloat();
            Unit.WorldPosition.Z = GetFloat();
        }
    }
}