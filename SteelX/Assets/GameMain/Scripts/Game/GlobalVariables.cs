using System.Collections.Generic;
using Exteel.Core.Player;
//using UnityEngine;

namespace Exteel
{
	public class GlobalVariables //: MonoBehaviour
	{
		#region Variables
		//public static Dictionary<int, Data> data;
		//public static Data myData;
		//public static int preferredFrameRate = 40;

		//Assigned when login
		public static string version = "0.0";

		public static float cameraRotationSpeed = 5;//mouse sensitivity in game    
		public static float generalVolume = 1f;

		public static Player Player { get; private set; }
		#endregion

		#region Methods
		public void Login(string user, string pass)
		{
			UnityEngine.WWWForm form = new UnityEngine.WWWForm();
			
			form.AddField("username", user);
			form.AddField("password", pass);

			UnityEngine.WWW www = new UnityEngine.WWW(Settings.LoginURL, form);

			//Debug.Log("Authenticating...");

			//print ("PlayerName :" + PhotonNetwork.playerName);

			while (!www.isDone) {}
			foreach (KeyValuePair<string,string> entry in www.responseHeaders) {
				//Debug.Log(entry.Key + ": " + entry.Value);
			}

			if (www.responseHeaders["STATUS"] == "HTTP/1.1 200 OK") {
				string json = www.text;
				//Data test = new Data();
				//print(JsonUtility.ToJson (test));
				Player d = UnityEngine.JsonUtility.FromJson<Player>(json);
				//UserData.myData = d;
				//UserData.myData.Mech0.PopulateParts();
				//PhotonNetwork.playerName = fields [0].text;
				//Application.LoadLevel (1);
			} else {
				//error.SetActive(true);
			}

			// for debug
			//UserData.myData.Mech = new Mech[4];
			//UserData.region = FindRegionCode(region);
			//UserData.version = gameVersion;
			//for (int i = 0; i < 4; i++) UserData.myData.Mech[i].PopulateParts();
			//PhotonNetwork.playerName = (string.IsNullOrEmpty(fields[0].text)) ? "Guest" + Random.Range(0, 9999) : fields[0].text;
			//
			//ConnectToServerSelected();
			//StartCoroutine(LoadLobbyWhenConnected());
		}

		public static void ExitGame()
		{
			UnityEngine.Application.Quit();
		}
		#endregion
	}

	public static class Settings
	{
		public const string LoginURL = "https://afternoon-temple-1885.herokuapp.com/login";

		//public const 
	}
}