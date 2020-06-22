using System;
using System.Drawing;
using System.Linq;
using GameServer.Managers;
using GameServer.ServerPackets;
using Console = Colorful.Console;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Sent when the client re-connects after switching to a room server
    /// </summary>
    public class ConnectSwitch : ClientBasePacket
    {
        /// <summary>
        /// The job code used to reconnect
        /// TODO: Error handling on all this
        /// </summary>
        private readonly int _jobCode;
        
        public ConnectSwitch(byte[] data, GameSession client) : base(data, client)
        {
            Console.WriteLine("Packet size: {0}",Color.Coral, Size);
            
            Console.WriteLine("Packet raw: {0}", Color.Coral,
                String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));

            _jobCode = GetInt();
            Console.WriteLine("Int?? - : {0}", _jobCode); // ??
        }

        public override string GetType()
        {
            return "CONNECT_SWITCH";
        }

        protected override void RunImpl()
        {
            var client = GetClient();
            
           // TODO: Error handling
           ServerManager.ConnectSwitch(client, _jobCode);
           
           client.SendPacket(new ConnectResult(0, client.User));
        }
    }
}