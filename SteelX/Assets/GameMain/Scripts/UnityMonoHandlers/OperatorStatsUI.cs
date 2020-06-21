using UnityEngine;
using UnityEngine.UI;

namespace SteelX.Unity
{
	public class OperatorStatsUI : MonoBehaviour
	{
		/*#region Variables
		#region New UI Fix
		//[SerializeField] private Text UI_HP;
		//[SerializeField] private Text UI_EN;
		//[SerializeField] private Text UI_SP;
		//[SerializeField] private Text UI_MPU;
		//[SerializeField] private Text UI_Size;
		//[SerializeField] private Text UI_Weight;
		//[SerializeField] private Text UI_MoveSpeed;
		//[SerializeField] private Text UI_DashSpeed;
		//[SerializeField] private Text UI_ENRecovery;
		//[SerializeField] private Text UI_MinENRequired;
		//[SerializeField] private Text UI_DashENDrain;
		//[SerializeField] private Text UI_JumpENDrain;
		//[SerializeField] private Text UI_DashAccel;
		//[SerializeField] private Text UI_DashDecel;
		//[SerializeField] private Text UI_MaxHeat;
		//[SerializeField] private Text UI_CooldownRate;
		//[SerializeField] private Text UI_ScanRange;
		//[SerializeField] private Text UI_Marksmanship;

		private Color32 BLUE = new Color32(39, 67, 253, 255), RED = new Color32(248, 84, 84, 255);
		private const int STAT_LABELS = 18; 
		#endregion
		[SerializeField] private Transform MechInfoStats;

		private Part[] MechParts = new Part[5];
		private WeaponData[] MechWeapons = new WeaponData[4];
		private Text[] stat_texts = null, stat_labels = null, stat_differences = null;
		//private string[] STAT_LABELS = new string[18]; 
		//{
		//   "HP","EN","SP","MPU","Size","Weight","Move Speed","Dash Speed","EN Recovery","Min. EN Required",
		//	"Dash EN Drain","Jump EN Drain","Dash Accel","Dash Decel","Max Heat","Cooldown Rate","Scan Range","Marksmanship"
		//};
		#endregion

		#region Unity
		private void Awake() {
			stat_texts = new Text[MechInfoStats.childCount];
			stat_labels = new Text[MechInfoStats.childCount];
			stat_differences = new Text[MechInfoStats.childCount];

			for (int i = 0; i < MechInfoStats.childCount; i++) {
				stat_texts[i] = MechInfoStats.GetChild(i).Find("Stats").GetComponent<Text>();
				stat_labels[i] = MechInfoStats.GetChild(i).Find("Label").GetComponent<Text>();

				//Init labels
				//stat_labels[i].text = STAT_LABELS[i]; //you can name them from here, but they're already set from Unity...
				stat_differences[i] = MechInfoStats.GetChild(i).Find("Change/Difference").GetComponent<Text>();
				stat_differences[i].enabled = false;
			}
		}
		#endregion

		#region Methods
		public void CompareDisplay()
		{
			//Whatever player is viewing on screen, create it, and calculate data
			Mechanaught shopMech = new Mechanaught(new Exteel.Mech());
			Mechanaught active = new Player().ActiveMech;

			//int[] displayMech = TransformMechPropertiesToArray();

			//foreach (var item in TransformMechPropertiesToArray())
			for (int i = 0; i < shopMech.OperatorStats.Length; i++)
			{
				//Return the data for the model player is viewing
				//stat_differences.text = currentModel;

				//if there is a difference in stats from model player is viewing, and the one player has equiped
				int diff = shopMech.OperatorStats[i] - active.OperatorStats[i];
				if (System.Math.Abs(diff) > 0) //If difference is positive or negative
				{
					//stat_differences[j].text = (newMechPropertiesArray[j] - curMechPropertiesArray[j] > 0 ? "▲" : "▼") + (Mathf.Abs(newMechPropertiesArray[j] - curMechPropertiesArray[j])).ToString();
					//stat_differences[j].color = newMechPropertiesArray[j] - curMechPropertiesArray[j] > 0 ? RED : BLUE;
					//stat_differences.text = diff;
					//stat_differences.color = diff > 0 ? RED : BLUE;
					stat_differences[i].text = string.Format("{0} {1}", diff > 0 ? "▲" : "▼", System.Math.Abs(diff).ToString());
					stat_differences[i].color = diff > 0 ? RED : BLUE;
				}
			}
		}
		#endregion*/
	}
}