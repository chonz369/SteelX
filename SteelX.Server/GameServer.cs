using System;
using System.Net;
using System.Net.Sockets;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using NetCoreServer;

namespace GameServer
{
    public class GameServer : TcpServer
    {
        public GameServer(IPAddress address, int port) : base(address, port)
        {
            using (var db = new ExteelContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
        
        protected override TcpSession CreateSession()
        {
            var session = new GameSession(this);
            return session;
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Game TCP server caught an error with code {error}");
        }
    }
}