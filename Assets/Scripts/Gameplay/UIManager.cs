﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public Text userName;
	public GameObject charactersPanel;
	public GameObject addBtn;

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

	List<int> dialogList;

	WaitForSeconds repeatInTime = new WaitForSeconds(30f); //Repeat every x seconds

	void Awake () {
		userName.text = PlayerPrefs.GetString ("username");

		StartCoroutine ("ReadEvent");

		StartCoroutine ("ReadIdealInfo");
	}

	void Start(){
		dialogList = new List<int> ();
	}

	void Update () {
		
	}

	public void ChangePlant(int chanum){
		bool showname = false;
		string label = "plantID_" + chanum.ToString ();
		print ("THIS IS "+PlayerPrefs.GetInt (label));
		for (int i = 0; i < 4; i++) {
			if (i == chanum - 1 && PlayerPrefs.GetInt (label) != 0) { //Show this slot
				charactersPanel.transform.GetChild (i).gameObject.SetActive (true);

				//Show character by level
				string level = "plantLevel_" + chanum.ToString ();
				for (int j = 0; j < 2; j++) {
					charactersPanel.transform.GetChild (i).GetChild (0).gameObject.SetActive (false);
					charactersPanel.transform.GetChild (i).GetChild (1).gameObject.SetActive (false);
					if(PlayerPrefs.GetInt (level) == 1){
						print (charactersPanel.transform.GetChild (i).GetChild (0).name);
						print (charactersPanel.transform.GetChild (i).GetChild (1).name);
						charactersPanel.transform.GetChild (i).GetChild (0).gameObject.SetActive (true);
						charactersPanel.transform.GetChild (i).GetChild (1).gameObject.SetActive (false);
					} else if(PlayerPrefs.GetInt (level) == 2){
						charactersPanel.transform.GetChild (i).GetChild (0).gameObject.SetActive (false);
						charactersPanel.transform.GetChild (i).GetChild (1).gameObject.SetActive (true);
					}
				}

				addBtn.SetActive (false);
				label = "plantName_" + chanum.ToString ();
				charactersPanel.transform.GetChild (4).GetComponent<Text> ().text = PlayerPrefs.GetString (label);
				showname = true;
			} else if (i != chanum - 1) { //Hide this slot
				charactersPanel.transform.GetChild (i).gameObject.SetActive (false);
				addBtn.SetActive (false);
			} else { //Empty slot
				addBtn.SetActive (true);
			}

			if (showname) {
				charactersPanel.transform.GetChild (4).gameObject.SetActive (true);
			} else {
				charactersPanel.transform.GetChild (4).gameObject.SetActive (false);
			}

		}

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

			print ("INFO: light->" + light + " | ideal " + idealLight);
			print ("INFO: temp->" + temp + " | ideal " + idealtemp);
			print ("INFO: water temp->" + wtemp + " | ideal " + idealwtemp);


			if (light > idealLight) {
				dialogList.Add (1);
				print ("Add 1");
			} else if (light < idealLight) {
				dialogList.Add (2);
				print ("Add 2");
			}

			if (wtemp > idealwtemp) {
				dialogList.Add (3);
				print ("Add 3");
			} else if (wtemp < idealwtemp) {
				dialogList.Add (4);
				print ("Add 4");
			} 

			yield return repeatInTime;

			RandomDialogList ();

		}
	}

	void RandomDialogList(){
		int c = dialogList.Count;
		int r = Random.Range (0, c);
		if (dialogList [r] == 1) {
			diBox.SetActive (true);
			diText.text = "โห สว่างจังเลย แสบตา~~~ ใบจะไหม้ไหมเนี่ย";
		} else if (dialogList [r] == 2) {
			diBox.SetActive (true);
			diText.text = "หวา...มืดจังเลยนะ ใบฉันสังเคราะห์แสงไม่พอแน่ๆ";
		} else if (dialogList [r] == 3) {
			diBox.SetActive (true);
			diText.text = "จ๊ากกกก! รากร้อนมากเลย ช่วยเพิ่มความเร็วน้ำให้หน่อย!";
		} else if (dialogList [r] == 4) {
			diBox.SetActive (true);
			diText.text = "หวือ หนาวววววว หยุดเพิ่มความเร็วน้ำสักพักน้า";
		} else {
			diBox.SetActive (false);
		}

		dialogList.Clear ();

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
			SceneManager.LoadScene ("Game01-p");
		} else if (eventNum == 2) {
			SceneManager.LoadScene ("Game02-p");
		}
	}
}
