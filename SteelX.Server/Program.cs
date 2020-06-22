using System;
using System.Drawing;
using System.Linq;
using System.Net;
using Data.Model;
using Console = Colorful.Console;
using GameServer.Config;
using GameServer.Managers;
using GameServer.ServerPackets;
using GameServer.ServerPackets.Game;
using Microsoft.EntityFrameworkCore;

namespace GameServer
{
    class Program
    {
        private static GameSession _temp;
      
        static void Main(string[] args)
        {
            // TODO: Move to config file
            var port = 15152;
            
            // TODO: Version from assembly?
            Console.WriteAscii("Exteel.Net", Color.DarkBlue);
            Console.WriteLine("Private Server v0.0.1");
            
            Console.WriteLine();
            
            Console.WriteLine("Reading shop data...");
            
            ShopDataReader.ReadGoods();
            
            Console.WriteLine("Done!");
            
            Console.WriteLine("Reading poo data...");
            
            PooReader.ReadPoo();
            
            Console.WriteLine("Done!");
            
            Console.WriteLine("Server Port is {0}", port);
            
            // Create the server
            var server = new GameServer(IPAddress.Any, port);
            
            // TODO: Move this to some sort of instance server?
            var sessionServer = new GameServer(IPAddress.Any, 7777);
            
            Console.WriteLine("Server starting...");

            server.Start();
            sessionServer.Start();
            
            Console.WriteLine("Done!");
            
            Console.WriteLine("Press Enter to stop the server or '!' to restart the server...");

            // Perform text input
            for (;;)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                // Restart the server
                if (line == "!")
                {
                    Console.Write("Server restarting...");
                    server.Restart();
                    Console.WriteLine("Done!");
                    continue;
                }

                // Create fake session
                if (line[0] == 'j')
                {
                    var data = line.Split(" ");

                    // Parse guid from string
                    //var client = (GameSession)sessionServer.Sessions[Guid.Parse(data[1])];
                    _temp = (GameSession) sessionServer.Sessions.Values.First();
                    
                    // Sign in
                    dummySignIn(_temp);
                    
                    // Join to first room
                    var room = RoomManager.GetRooms().First();

                    room.TryEnterGame(_temp);

                }
                
                // Ready fake session
                if (line[0] == 'r')
                {
                    var data = line.Split(" ");
                    
                    // Join to first room
                    var room = RoomManager.GetRooms().First();

                    room.GameReady(_temp);

                }
                
                // Respawn fake session
                if (line[0] == 's')
                {
                    var data = line.Split(" ");
                    
                    // Join to first room
                    var room = RoomManager.GetRooms().First();

                    room.SpawnUnit(_temp.User.CurrentUnit, _temp.User);

                }
                
                if (line[0] == 'b')
                {
                    var data = line.Split(" ");
                    
                    // Join to first room
                    AttackStart.RESULT = Convert.ToInt32(data[1]);

                }
                
                // Multicast admin message to all sessions
                // TODO: Handle admin commands
            }

            // Stop the server
            Console.Write("Server stopping...");
            server.Stop();
            sessionServer.Stop();
            Console.WriteLine("Done!");
        }
        
        /// <summary>
        /// TEMP - JUST FOR TESTING
        /// </summary>
        /// <param name="client"></param>
        private static void dummySignIn(GameSession client) {
            using (var db = new ExteelContext())
            {               
                // Find user
                var user = db.Users.SingleOrDefault(u => u.Username == "puga");
                
                // Load user inventory
                user.Inventory = db.Inventories
                    .Include(i => i.Parts)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.Head)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.Chest)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.Arms)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.Legs)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.Backpack)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.WeaponSet1Left)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.WeaponSet1Right)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.WeaponSet2Left)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.WeaponSet2Right)
                    
                    .SingleOrDefault(i => i.Id == user.InventoryId);
                
                
                // Load users stats
                db
                    .Entry(user)
                    .Collection(u => u.Stats)
                    .Load();
                
                // Assign to client
                client.User = user;
            }
        }
    }
}