using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using GameServer.ServerPackets.Game;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Called when the user wishes to respawn
    /// </summary>
    public class RequestRegain : ClientBasePacket
    {
        private readonly uint _unitId;
        
        public RequestRegain(byte[] data, GameSession client) : base(data, client)
        {
            Console.WriteLine("Packet size: {0}",Color.Coral, Size);
            
            Console.WriteLine("Packet raw: {0}", Color.Coral,
                String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));

            _unitId = GetUInt();
            
            Console.WriteLine("UnitId - : {0}", _unitId); // ??
            Console.WriteLine("baseId?? - : {0}", GetInt()); // ??
        }

        public override string GetType()
        {
            return "REQ_REGAIN";
        }

        protected override void RunImpl()
        {
            var client = GetClient();
            var unit = client.User.CurrentUnit;
            
            // Send success
            client.SendPacket(new RegainResult(unit));
            
            // Send unit info
            client.GameInstance.SpawnUnit(unit, client.User);
        }
    }
}