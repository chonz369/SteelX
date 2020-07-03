using System.Collections.Generic;

namespace SteelX.Shared
{
    public struct ClanData
    {
        /// <summary>
        /// The unique Id of this clan
        /// </summary>
        public uint Id { get; private set; }
        
        /// <summary>
        /// The name of this clan
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// List of users in this clan
        /// </summary>
        public string ClanImage { get; private set; }
    }
}