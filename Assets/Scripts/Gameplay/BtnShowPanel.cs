using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnShowPanel : MonoBehaviour {

	Button btn;
	public GameObject panel;

	WaitForSeconds repeatInTime = new WaitForSeconds(10f); //Repeat every x seconds
	string[] items;
	public Text lightTxt;
	public Text temperatureTxt;
	public Text waterTempTxt;
	public Text pumpSpeedTxt;

	int plantId;

	void Start () {
		plantId = PlayerPrefs.GetInt ("plantID");

		btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (ToggleShowPanel);
	}
	
	void ToggleShowPanel(){
		if (!panel.activeSelf) {
			panel.SetActive (true);
			StartCoroutine ("ReadDataQuery");
		} else {
			panel.SetActive (false);
			StopCoroutine ("ReadDataQuery");
		}
	}

	IEnumerator ReadDataQuery () {
		while (true) {
			WWWForm form = new WWWForm ();
			form.AddField ("action", "readPotInput");
			form.AddField ("plantId", plantId);
			WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
			yield return itemsData;
			print (itemsData.text);
			string itemsDataString = itemsData.text;
			items = itemsDataString.Split (',');

			lightTxt.text = items [2] + " lux";
			temperatureTxt.text = items [3] + " °C";
			waterTempTxt.text = items [4] + " °C";
			pumpSpeedTxt.text = items [5] + " L/min";

			PlayerPrefs.SetFloat ("light", float.Parse(items [2]));
			PlayerPrefs.SetFloat ("temperature", float.Parse(items [3]));
			PlayerPrefs.SetFloat ("waterTemp", float.Parse(items [4]));
			PlayerPrefs.SetFloat ("pumpSpeed", float.Parse(items [5]));
			PlayerPrefs.Save ();

			yield return repeatInTime;
		}
	}
}
