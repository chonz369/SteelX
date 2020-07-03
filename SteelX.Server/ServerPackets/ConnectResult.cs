using System;
using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets
{
	/// <summary>
	/// Response to a users login request
	/// </summary>
	/// 0x00 = success
	/// 0x01 = failed
	/// 0x02 = wrong info
	/// 0x08 = already logged in
	/// 0x24 = more than one player?
	/// -8 = Switch server?
	/// -6 NEW USER 
	//TODO: Move these to enum
	public class ConnectResult : ServerBasePacket
	{
		/// <summary>
		/// The user who was found
		/// </summary>
		private readonly Player _user;

		/// <summary>
		/// The result of the login request
		/// </summary>
		private readonly int _resultCode;

		public ConnectResult(int resultCode, Player user = null)
		{
			_resultCode = resultCode;
			_user = user;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.SERVER_CONNECT_RESULT;
			}
		}
		
		/*public override string GetType()
		{
			return "SERVER_CONNECT_RESULT";
		}

		public override byte GetId()
		{
			return 0x02;
		}*/

		protected override void WriteImpl()
		{           
			// Result code first
			WriteInt(_resultCode);

			// Success codes
			if (_resultCode == 0 || _resultCode == -6)
			{
				WriteUInt(_user.Id);
				WriteString("UNKNOWN"); // Unknown string. Possibly clan name?
				WriteString(_user.Username);
				WriteString(_user.Nickname);
				WriteUInt(_user.Level);
				
				for (var i = 0; i < 256; i++)
				{
					WriteByte(0); // Config data??
				}
				WriteString("myserver.address");
				WriteInt(255);
			}
			// The lines below are in the client packet def, but dont appear to be needed. Saving them just in case
			// They might be used for a server switching system that was never used
			//else
			//{
			//    // Failed, send empty packet
			//    WriteUInt(0);
			//    WriteString("X"); 
			//    WriteString("X"); 
			//    WriteString("X"); 
			//    WriteUInt(0);
			//}
			//
			//// Unknown
			//for (var i = 0; i < 256; i++)
			//{
			//    WriteByte(0); // Config data??
			//}
			//WriteString("myserver.address");
			//WriteInt(255);
		}
	}
}