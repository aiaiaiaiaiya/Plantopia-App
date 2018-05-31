using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BtnShowPanel1 : MonoBehaviour {

	public GameObject panel;
	RectTransform rect;
	bool show = false;

	WaitForSeconds repeatInTime = new WaitForSeconds(10f); //Repeat every x seconds
	string[] items;
	public Text lightTxt;
	public Text temperatureTxt;
	public Text waterTempTxt;

	void Start () {
		rect = panel.GetComponent<RectTransform> ();
	}
	
	public void ToggleShowPanel(){
		if (!show) {
			rect.anchoredPosition = Vector3.zero;
			EditPanel ();
			show = true;

		} else {
			rect.anchoredPosition = new Vector3(-220,0,0);
			show = false;
		}
	}

	IEnumerator ReadDataQuery () {
		while (true) {
			WWWForm form = new WWWForm ();
			form.AddField ("action", "readPotInput");
			WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
			yield return itemsData;
			print (itemsData.text);
			string itemsDataString = itemsData.text;
			items = itemsDataString.Split (',');

			lightTxt.text = PlayerPrefs.GetFloat ("light").ToString () + " lux";
			temperatureTxt.text = PlayerPrefs.GetFloat ("temperature").ToString ("#.##") + " °C";
			waterTempTxt.text = PlayerPrefs.GetFloat ("waterTemp").ToString () + " °C";


			PlayerPrefs.SetFloat ("light", float.Parse(items [2]));
			PlayerPrefs.SetFloat ("temperature", float.Parse(items [4]));
			PlayerPrefs.SetFloat ("waterTemp", float.Parse(items [3]));
			PlayerPrefs.SetFloat ("diameter", float.Parse(items [5]));
			PlayerPrefs.Save ();

			yield return repeatInTime;
		}
	}

	void EditPanel(){
		lightTxt.text = PlayerPrefs.GetFloat ("light").ToString () + " lux";
		temperatureTxt.text = PlayerPrefs.GetFloat ("temperature").ToString ("#.##") + " °C";
		waterTempTxt.text = PlayerPrefs.GetFloat ("waterTemp").ToString () + " °C";
	}
}
