using System;
//using GameServer.ClientPackets;
//using GameServer.ClientPackets.Bridge;
//using GameServer.ClientPackets.Game;
//using GameServer.ClientPackets.Inventory;
//using GameServer.ClientPackets.Lobby;
//using GameServer.ClientPackets.Room;
//using GameServer.ServerPackets.Bridge;

namespace SteelX.Shared.Packets
{
	/// <summary>
	/// This class handles all packets as they come in
	/// </summary>
	public abstract class PacketHandler
	{
		/// <summary>
		/// The size of the header in bytes
		/// </summary>
		private const int HeaderSize = 3;
		
		/*public static ClientBasePacket HandlePacket(byte[] data, int offset, GameSession client)
		{
			// Calculate message size
			var size = (short)((data[offset + 1] << 8) + data[offset]);
			
			// Copy the packet to a new byte array
			// Skipping the header
			var packet = new byte[size];
			Array.Copy(data, offset, packet, 0, size);

			ClientBasePacket msg;
			
			// Get the id
			var id = packet[2];
			
			// Handle the packet
			//TODO: Can we group these into login / game / etc?
			switch (id)
			{
				case 0x00:
					msg = new ProtocolVersion(packet, client);
					break;
				
				case 0x02:
					msg = new ConnectClient(packet, client);
					break;
				
				case 0x03:
					msg = new ConnectSwitch(packet, client);
					break;
				
				case 0x04:
					msg = new SwitchServer(packet, client);
					break;
				
				case 0x05:
					msg = new ServerTime(packet, client);
					break;
				
				case 0x0e:
					msg = new SyncMoney(packet, client);
					break;
				
				case 0x0f:
					msg = new Log(packet, client);
					break;
				
				case 0x1b:
					msg = new RequestInventory(packet, client);
					break;
				
				case 0x1c:
					msg = new RequestSearchGame(packet, client);
					break;
				
				case 0x20:
					msg = new EnterGame(packet, client);
					break;
				
				case 0x25:
					msg = new ListUser(packet, client);
					break;
				
				case 0x26:
					msg = new Ready(packet, client);
					break;
				
				case 0x28:
					msg = new StartGame(packet, client);
					break;
				
				case 0x31:
					msg = new SelectBase(packet, client);
					break;
				
				case 0x32:
					msg = new ReadyGame(packet, client);
					break;
				
				case 0x35:
					msg = new RequestPalette(packet, client);
					break;
				
				case 0x36:
					msg = new MoveUnit(packet, client);
					break;
				
				case 0x37:
					msg = new AimUnit(packet, client);
					break;
				
				case 0x38:
					msg = new StartAttack(packet, client);
					break;
				
				case 0x39:
					msg = new StopAttack(packet, client);
					break;
				
				case 0x3d:
					msg = new RequestRegain(packet, client);
					break;
				
				case 0x65:
					msg = new UnAimUnit(packet, client);
					break;
				
				case 0x48:
					msg = new RequestGoodsData(packet, client);
					break;
				
				case 0x4c:
					msg = new RequestAvatarInfo(packet, client);
					break;
				
				case 0x4d:
				case 0x4e:
				case 0x4f:
				case 0x50:
				case 0x51:
				case 0x52:
					msg = new RequestStatsInfo(packet, client);
					break;
				
				case 0x53:
					msg = new RequestBestInfo(packet, client);
					break;
				
				default:
					msg = new UnknownPacket(packet, client);
					//Console.WriteLine("Unknown packet id [{0}] from user {1}", id, client.GetUserName());
					break;
			}

			return msg;
		}*/
	}
}