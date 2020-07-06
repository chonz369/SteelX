using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Dynamic;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SteelX.Shared.Packets
{
	public static class Packet
	{
		/// <summary>
		/// Serialize/Encode
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static byte[] ToByteArray(this object obj)
		{
			if (obj == null)
				return null;
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
			//Serializing a Json Obj is faster and less memory
			//return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
		}

		/// <summary>
		/// Serialize/Encode
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static byte[] ToByteArray<T>(this T obj)
		{
			if (obj == null)
				return null;
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}

		/// <summary>
		/// Deserialize/Decode
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <returns></returns>
		//public static T PacketToObject<T>(this byte[] data)
		public static T FromByteArray<T>(this byte[] data)
		{
			if (data == null)
				return default(T);
			BinaryFormatter bf = new BinaryFormatter();
			//using (MemoryStream ms = new MemoryStream())
			using (MemoryStream ms = new MemoryStream(data))
			{
				//ms.Write(blob, 0, blob.Length);
				//ms.Seek(0, SeekOrigin.Begin);
				return (T)bf.Deserialize(ms);
			}
		}
	}
}