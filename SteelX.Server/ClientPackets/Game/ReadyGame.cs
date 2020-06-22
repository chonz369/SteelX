using System;
using System.Drawing;
using System.Linq;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when a user has sucessfully loaded into the game and is ready to play
    /// </summary>
    public class ReadyGame : ClientBasePacket
    {
        public ReadyGame(byte[] data, GameSession client) : base(data, client)
        {
            Console.WriteLine("Packet size: {0}",Color.Coral, Size);
            
            Console.WriteLine("Packet raw: {0}", Color.Coral,
                String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));

            Console.WriteLine("Int?? - : {0}", GetInt()); // ??
        }

        public override string GetType()
        {
            return "GAME_READY_GAME";
        }

        protected override void RunImpl()
        {
            var client = GetClient();

            client.GameInstance.GameReady(client);
        }
    }
}