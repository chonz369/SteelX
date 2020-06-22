using GameServer.Game;

namespace GameServer.ServerPackets.Lobby
{
    /// <summary>
    /// Sent when the user enters a game
    /// </summary>
    public class GameEntered : ServerBasePacket
    {
        /// <summary>
        /// The room the user wishes to enter
        /// </summary>
        private readonly GameInstance _room;

        private readonly GameEnteredResult _result;
        
        public GameEntered(GameEnteredResult result, GameInstance room = null)
        {
            _room = room;
            _result = result;
        }
        
        public override string GetType()
        {
            return "LOBBY_GAME_ENTERED";
        }

        public override byte GetId()
        {
            return 0x31;
        }

        protected override void WriteImpl()
        {
            // 0 = success
            // 1 = no longer available or technical problem
            // 2 = battle full
            // 3 = battle has started but not in play yet
            // 4 = time limit has expired - maybe game over?
            // 5 = Incorrect login path? Maybe old system where people joined from web?
            // 6 = No units ready due to repair issues
            // 7 = Cannot participate?
            // 8 = Something about lower class? Maybe an old balance system?
            // 9 = No rooms that match criteria? maybe quick join result?
            // 0xa = Expelled - cannot enter for X minutes
            WriteInt((int)_result); // Result
            
            WriteInt(0); // Unknown
            
            switch (_result)
            {
                case GameEnteredResult.Expelled:
                    WriteInt(60); // Expulsion time -or- sessionId?
                    break;
                case GameEnteredResult.Success:
                    WriteInt(_room.Id);
                    this.WriteRoomInfo(_room);
                    break;
                default:
                    WriteInt(0); // Unknown
                    break;
            }
                
        }
    }

    public enum GameEnteredResult
    {
        Success = 0,
        NotAvailable = 1,
        Full = 2,
        JustStarted = 3,
        JustFinished = 4,
        BadLoginPath = 5,
        NoUnitsAvailable = 6,
        CannotParticipate = 7,
        LowerClass = 8,
        NoRoomsMatch = 9,
        Expelled = 0xa
    }
}