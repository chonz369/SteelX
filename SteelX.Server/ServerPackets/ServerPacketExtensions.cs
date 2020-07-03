//using Colorful;
//using System.Drawing;
using SteelX.Shared;
using SteelX.Server;
using SteelX.Server.Game;
//using SteelX.Server.Items;

namespace SteelX.Server.Packets
{
	/// <summary>
	/// Extensions for writing common data to packets
	/// </summary>
	public static class ServerPacketExtensions
	{
		/// <summary>
		/// Writes pilot info into a packet
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="info"></param>
		public static void WritePilotInfo(this ServerBasePacket packet, PilotInfo info)
		{
			packet.WriteInt(info.AbilityPointsAvailable);

			packet.WriteByte((byte)info.HpLevel);
			packet.WriteByte((byte)info.MoveSpeedLevel);
			packet.WriteByte((byte)info.EnLevel);
			packet.WriteByte((byte)info.ScanRangeLevel);
			packet.WriteByte((byte)info.SpLevel);
			packet.WriteByte((byte)info.AimLevel);

			packet.WriteByte(0); // Unknown
			packet.WriteByte(0); // Unknown

			packet.WriteInt(info.MeleeLevel);
			packet.WriteInt(info.RangedLevel);
			packet.WriteInt(info.SiegeLevel);
			packet.WriteInt(info.RocketLevel);

			//packet.WriteInt(0); // Unknown
			packet.WriteByte(0); // NOTE: Not sure why everything after here is offset by one, so i am just doing 3 bytes instead of 4 here
			packet.WriteByte(0);
			packet.WriteByte(0);
		}

		/// <summary>
		/// Writes a parts info into a packet
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="part"></param>
		public static void WritePartInfo(this ServerBasePacket packet, Part part)
		{
			if (part != null)
			{
				packet.WriteUInt(part.Id);
				packet.WriteUInt(part.TemplateId);

				packet.WriteUShort(part.Parameters);
				packet.WriteUShort(part.Type);

				packet.WriteByte(part.Color.R);
				packet.WriteByte(part.Color.G);
				packet.WriteByte(part.Color.B);

				packet.WriteByte(1); // Unknown
				packet.WriteByte(1); // Unknown

				packet.WriteInt(1); // Unknown

				// FArray - Unknown
				packet.WriteInt(0); // Array size

				// Array contents would go here

				packet.WriteInt(10000); // Expiry time / current durability
				packet.WriteInt(10000); // Max Durability
				packet.WriteInt(1); // If 1, durability, if 2, expired?
				packet.WriteInt(0); // Unknown
			}
			else
			{
				//Console.WriteLine("Wrote EMPTY part!", Color.Green);

				packet.WriteInt(-1);
				packet.WriteUInt(0);

				packet.WriteUShort(0);
				packet.WriteUShort(0);

				packet.WriteByte(0);
				packet.WriteByte(0);
				packet.WriteByte(0);

				packet.WriteByte(0); // Unknown
				packet.WriteByte(0); // Unknown

				packet.WriteInt(0); // Unknown

				// FArray - Unknown
				packet.WriteInt(0); // Array size

				// Array contents would go here

				packet.WriteInt(0); // Unknown
				packet.WriteInt(0); // Unknown
				packet.WriteInt(0); // If 1, durability, if 2, expired?
				packet.WriteInt(0); // Unknown
			}
		}

		/// <summary>
		/// Writes room info onto a packet
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="room"></param>
		public static void WriteRoomInfo(this ServerBasePacket packet, GameInstance room)
		{
			//TODO: Move this to helper or maybe even room info obj
			packet.WriteString("StringA"); // Room / session code?

			packet.WriteInt(room.Id); // Unknown
			packet.WriteInt(0); // Unknown
			packet.WriteInt((int)room.GameType);


			packet.WriteString(room.Name);
			packet.WriteString("StringB"); // Unknown
			packet.WriteString("StringC"); // Unknown
			packet.WriteString(room.Master != null ? room.Master.CallSign : "UNKNOWN");

			packet.WriteInt(room.Capacity);
			packet.WriteInt(room.Users.Count);

			packet.WriteByte(0); // Unknown
			packet.WriteByte(0); // Unknown

			packet.WriteInt(0); // Status?
			packet.WriteUInt(room.GameTemplate); // Not sure why this is sent twice?
			packet.WriteUInt(room.GameTemplate); // This was supposed to be adhoc id

			packet.WriteByte(0); // Unknown

			packet.WriteInt(0); // Unknown
			packet.WriteInt(0); // Unknown
			packet.WriteInt(0); // Unknown
			packet.WriteInt(0); // Unknown
			packet.WriteInt(0); // Unknown

			packet.WriteByte(0); // Unknown

			packet.WriteInt(0); // Unknown

			packet.WriteByte(0); // Unknown

			packet.WriteInt(50); // Min level
			packet.WriteInt(51); // max Level
			packet.WriteInt(52); // Unknown
			packet.WriteInt(53); // Unknown
			packet.WriteInt(54); // Unknown
		}

		public static void WriteRoomUserInfo(this ServerBasePacket packet, GameInstance room, Player user)
		{
			packet.WriteString("StringA"); // Unknown
			packet.WriteString(user.CallSign);
			packet.WriteString(""); // Clan name

			packet.WriteInt(0); // Unknown
			packet.WriteInt(0); // Unknown - clan id?

			packet.WriteUInt(user.Id);
			packet.WriteUInt(user.Team);

			packet.WriteUInt((uint)user.Rank); // Unknown
			packet.WriteInt(0); // Unknown
			packet.WriteInt(0); // Unknown
			packet.WriteInt(0); // Unknown
			packet.WriteInt(0); // Unknown
			packet.WriteInt(0); // Unknown

			packet.WriteBool(user.IsReady); // Unknown - ready?
			packet.WriteBool(user.Id == room.MasterId);

			for (var i = 0; i < 13; i++)
			{
				packet.WriteInt(0); //Unknown
			}

			for (var i = 0; i < 12; i++)
			{
				packet.WriteInt(0); //Unknown
			}

			for (var i = 0; i < 13; i++)
			{
				packet.WriteInt(0); //Unknown
			}

			for (var i = 0; i < 14; i++)
			{
				packet.WriteInt(0); // Unknown
			}

			for (var i = 0; i < 11; i++)
			{
				packet.WriteInt(0); // Unknown
			}

			for (var i = 0; i < 11; i++)
			{
				packet.WriteInt(0); // Unknown
			}

			// Unknown struct
			packet.WriteInt(0); 
			packet.WriteInt(0); 
			packet.WriteInt(0); 
			packet.WriteInt(0); 
			packet.WriteInt(0); 
			packet.WriteInt(0); 
			packet.WriteInt(0); 
			packet.WriteInt(0); 

			// Another unknown struct. Seems to be an array?
			packet.WriteInt(0); // Size?

			packet.WritePilotInfo(user.PilotInfo);
		}
	}
}