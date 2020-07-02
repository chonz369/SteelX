using System;
using System.Drawing;
using System.Linq;
using GameServer.ServerPackets.Game;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when the user first spawns. Not sure exactly how it works
    /// </summary>
    public class SelectBase : ClientBasePacket
    {
        public SelectBase(byte[] data, GameSession client) : base(data, client)
        {
            Console.WriteLine("Packet size: {0}",Color.Coral, Size);
            
            Console.WriteLine("Packet raw: {0}", Color.Coral,
                String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));

            Console.WriteLine("Int?? - : {0}", GetInt()); // ??
        }

        public override string GetType()
        {
            return "GAME_SELECT_BASE";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(new BaseSelected());
        }
    }
}