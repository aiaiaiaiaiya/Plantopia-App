using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public Text userName;
	public Text plantName;

	string[] items;

	public GameObject alertImg;

	int eventNum;

	float light;
	float idealLight;
	float temp;
	float idealtemp;
	float wtemp;
	float idealwtemp;

	public GameObject diBox;
	public Text diText;

	WaitForSeconds repeatInTime = new WaitForSeconds(10f); //Repeat every x seconds

	void Awake () {
		userName.text = PlayerPrefs.GetString ("username");
		plantName.text = PlayerPrefs.GetString ("plantName");
		StartCoroutine ("ReadEvent");
		StartCoroutine ("ReadIdealInfo");
	}

	void Update () {
		
	}

	IEnumerator ReadIdealInfo () {
		while (true) {
			WWWForm form = new WWWForm ();
			form.AddField ("action", "readLevelInfo");
			string ptn = (PlayerPrefs.GetInt ("plantTypeNo")).ToString ();
			form.AddField ("plantTypeNo", ptn);
			string lvl = (PlayerPrefs.GetInt ("level")).ToString ();
			form.AddField ("level", lvl );
			print ("What fielddddddddddd "+ptn+" "+lvl);
			WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
			yield return itemsData;
			print ("Level Info = " + itemsData.text);
			string itemsDataString = itemsData.text;
			items = itemsDataString.Split (',');

			light = PlayerPrefs.GetFloat ("light");
			idealLight = float.Parse (items [0]);
			temp = PlayerPrefs.GetFloat ("temperature");
			idealtemp = float.Parse (items [2]);
			wtemp = PlayerPrefs.GetFloat ("waterTemp");
			idealwtemp = float.Parse (items [1]);
//			PlayerPrefs.GetFloat ("diameter", float.Parse(items [5]));

			print ("INFO: light->" + light + " | ideal " + idealLight);
			print ("INFO: temp->" + temp + " | ideal " + idealtemp);
			print ("INFO: water temp->" + wtemp + " | ideal " + idealwtemp);


			if (light > idealLight) {
				diBox.SetActive (true);
				diText.text = "โห สว่างจังเลย แสบตา~~~ ใบจะไหม้ไหมเนี่ย";
			} else if (light < idealLight) {
				diBox.SetActive (true);
				diText.text = "หวา...มืดจังเลยนะ ใบฉันสังเคราะห์แสงไม่พอแน่ๆ";
			}

			// if (temp > idealtemp) {
			// 	diBox.SetActive (true);
			// 	diText.text = "";
			// } else if (temp < idealtemp) {
			// 	diBox.SetActive (true);
			// 	diText.text = "";
			// } 

			if (wtemp > idealwtemp) {
				diBox.SetActive (true);
				diText.text = "จ๊ากกกก! รากร้อนมากเลย ช่วยเพิ่มความเร็วน้ำให้หน่อย!";
			} else if (wtemp < idealwtemp) {
				diBox.SetActive (true);
				diText.text = "หวือ หนาวววววว หยุดเพิ่มความเร็วน้ำสักพักน้า";
			} 
				

			yield return repeatInTime;
		}
	}

	IEnumerator ReadEvent () {
		while (true) {
			WWWForm form = new WWWForm ();
			form.AddField ("action", "readEvent");
			WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
			yield return itemsData;
			print ("Event polling = " + itemsData.text);
			string itemsDataString = itemsData.text;
			items = itemsDataString.Split (',');

			eventNum = int.Parse (items [2]);

			if (eventNum == 0) {
				alertImg.SetActive (false);
			} else {
				alertImg.SetActive (true);
			}
				
			yield return repeatInTime;
		}
	}

	public void LoadEvent(){
		if (eventNum == 1) {
			SceneManager.LoadScene ("Game01");
		} else if (eventNum == 2) {
			SceneManager.LoadScene ("Game02");
		}
	}
}
