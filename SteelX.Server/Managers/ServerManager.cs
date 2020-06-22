using System;
using System.Collections.Generic;
using Data.Model;
using GameServer.Util;

namespace GameServer.Managers
{
    /// <summary>
    /// This class handles user server switches from main server to gameplay server
    /// TODO: See what server id string is used for
    /// </summary>
    public static class ServerManager
    {
        private static readonly SortedList<int, Job> Jobs = new SortedList<int, Job>();
        
        /// <summary>
        /// Creates a job for a client to join a server
        /// </summary>
        /// <param name="session"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public static int JoinServer(GameSession session, string serverId)
        {
            return Jobs.AddNext(new Job { User = session.User, ConnectedStamp = session.ConnectedStamp, ServerId = serverId});
        }

        /// <summary>
        /// Reconnects a new client using a job code
        /// Automatically reattaches the user object and updates their timestamp for pinging
        /// </summary>
        /// <param name="session"></param>
        /// <param name="jobCode"></param>
        public static void ConnectSwitch(GameSession session, int jobCode)
        {
            // Get the job
            var job = Jobs[jobCode];
            
            // Update the session with the connected stamp and user
            session.User = job.User;
            session.ConnectedStamp = job.ConnectedStamp;

            Jobs.Remove(jobCode);
        }

        private class Job
        {
            public ExteelUser User;
            public DateTime ConnectedStamp;
            public string ServerId;
        }
    }
}