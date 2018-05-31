using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSel : MonoBehaviour {

	string[] items;
	public Text name1;

	// Use this for initialization
	void Start () {
		StartCoroutine ("LoadPlant");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator LoadPlant () {
		int userID = PlayerPrefs.GetInt ("userID");
		print (userID);
		WWWForm form = new WWWForm ();
		form.AddField ("action", "readPlantbyUserID");
		form.AddField ("userId", userID); 
		WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
		yield return itemsData;
		print ("LoadPlant"+itemsData.text);
		string itemsDataString = itemsData.text;
		items = itemsDataString.Split (',');

		name1.text = items [3];
	}
}
