using Data.Model;

namespace GameServer.ServerPackets.Bridge
{
    /// <summary>
    /// Sends the users stats and info to the client
    /// </summary>
    public class AvatarInfo : ServerBasePacket
    {
        /// <summary>
        /// The user whos data to send
        /// </summary>
        private readonly ExteelUser _user;
        
        public AvatarInfo(ExteelUser user)
        {
            _user = user;
        }
        
        public override string GetType()
        {
            return "BRIDGE_SEND_AVATAR_INFO";
        }

        public override byte GetId()
        {
            return 0x8f;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Unknown - Id?
            WriteString("StringAA"); // Unknown
            
            // Write clan info if they are in one
            // TODO: Not sure how to handle empty strings
            WriteString(_user.Clan == null ? " " : _user.Clan.Name);
            
            WriteUInt(_user.Coins);
            WriteString(_user.Callsign);
            
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
            WriteUInt(_user.Level);
            WriteInt(0); // Unknown
            WriteUInt(_user.Rank);
            
            WriteUInt(_user.Experience);
            WriteUInt(_user.Credits);
            WriteInt(1); // Unknown
            WriteInt(2); // Unknown
            WriteInt(3); // Unknown
            
            WriteInt(4); // Unknown
            WriteInt(5); // Unknown
            WriteInt(6); // Unknown
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
            
            WriteInt(9); // Unknown
            WriteInt(10); // Unknown
            WriteInt(11); // Unknown
            WriteInt(12); // Unknown
            WriteInt(13); // Unknown
            
            // Unknown - Int[64] Array
            for (var i = 0; i < 64; i++)
            {
                WriteInt(0);
            }
            
            // Unknown - FArray<Int> 
            WriteInt(0); // Size
            
            // Unknown - Int[6] Array
            for (var i = 0; i < 6; i++)
            {
                WriteInt(0);
            }
            
            this.WritePilotInfo(_user.PilotInfo);
            
            // Footer info - See function above for offset issue
            WriteInt(0); // Unknown
            WriteInt(10); // Rank?
            WriteInt(120); // Interval?
            WriteInt(1000); // Credits awarded
        }
    }
}