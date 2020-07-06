using System;
using SteelX.Shared;
//using GameServer.ServerPackets;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Sent when the client first connects to do an encryption handshake?
	/// </summary>
	/// Validation is usually a step that occurs when an error is encountered
	/// probably this is for log in conflicts or multiple sign-in attempts
	public class ValidateClient : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.VALIDATE_CLIENT;
			}
		}

		public ValidateClient(byte[] data, GameSession client) : base(data, client)
		{
			//Console.WriteLine("Validation: {0}", GetString());
		}

		/*public override string GetType()
		{
			return "VALIDATE_CLIENT";
		}*/

		protected override void RunImpl()
		{
			//GetClient().SendPacket(new ClientValidated());
		}
	}
}