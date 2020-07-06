using System;
using System.Net.Sockets;
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
		public const int HeaderSize = 3;

		public static TcpClient TcpConn	{ get; private set; }
		public static string TcpServer	{ get; private set; }
		public static Int32 TcpPort		{ get; private set; }

		public static PacketTypes ReceivePacket(byte[] data, out byte[] packet)//, int offset
		{
			//The number of bytes the packet Id takes up
			int offset = 0;

			// Calculate message size
			short size = (short)((data[offset + 1] << 8) + data[offset]);

			// Copy the packet to a new byte array
			//byte[] packet = new byte[size];
			packet = new byte[size];
			// Skipping the header
			Array.Copy(data, offset, packet, 0, size);

			// Get the id
			byte id = packet[2];

			return (PacketTypes)id;
		}

		/// <summary>
		/// Used to send packets via TCP connection
		/// </summary>
		/// Not used, sample code for concept dev
		/// <param name="data"></param>
		public static void SendTcpPacket(byte[] data)
		{
			try
			{
				// Create a TcpClient.
				// Note, for this client to work you need to have a TcpServer
				// connected to the same address as specified by the server, port
				// combination.
				TcpClient client = new TcpClient(TcpServer, TcpPort);

				// Translate the passed message into ASCII and store it as a Byte array.
				//Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

				// Get a client stream for reading and writing.
				//  Stream stream = client.GetStream();

				NetworkStream stream = client.GetStream();

				// Send the message to the connected TcpServer.
				stream.Write(data, 0, data.Length);

				// Receive the TcpServer.response.

				// Buffer to store the response bytes.
				data = new Byte[256];

				// String to store the response ASCII representation.
				String responseData = String.Empty;

				// Read the first batch of the TcpServer response bytes.
				Int32 bytes = stream.Read(data, 0, data.Length);
				responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
				//Console.WriteLine("Received: {0}", responseData);

				// Close everything.
				stream.Close();
				client.Close();
			}
			catch (ArgumentNullException e)
			{
				//Console.WriteLine("ArgumentNullException: {0}", e);
			}
			catch (SocketException e)
			{
				//Console.WriteLine("SocketException: {0}", e);
			}

			//Console.WriteLine("\n Press Enter to continue...");
			//Console.Read();
		}

		/// <summary>
		/// Used to send packets via UDP connection
		/// </summary>
		/// Not used, sample code for concept dev
		/// <param name="data"></param>
		public static void SenUdppPacket(byte[] data)
		{
			// This constructor arbitrarily assigns the local port number.
			UdpClient udpClient = new UdpClient(TcpPort);
			try
			{
				udpClient.Connect(TcpServer, TcpPort);

				// Sends a message to the host to which you have connected.
				//Byte[] data = Encoding.ASCII.GetBytes("Is anybody there?");

				udpClient.Send(data, data.Length);

				// Sends a message to a different host using optional hostname and port parameters.
				//UdpClient udpClientB = new UdpClient();
				//udpClientB.Send(data, data.Length, "AlternateHostMachineName", 11000);

				//IPEndPoint object will allow us to read datagrams sent from any source.
				System.Net.IPEndPoint RemoteIpEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);

				// Blocks until a message returns on this socket from a remote host.
				Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
				//string returnData = Encoding.ASCII.GetString(receiveBytes);

				// Uses the IPEndPoint object to determine which of these two hosts responded.
				//Console.WriteLine("This is the message you received " +
				//							 returnData.ToString());
				//Console.WriteLine("This message was sent from " +
				//							RemoteIpEndPoint.Address.ToString() +
				//							" on their port number " +
				//							RemoteIpEndPoint.Port.ToString());

				udpClient.Close();
				//udpClientB.Close();
			}
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
			}
		}
	}
}