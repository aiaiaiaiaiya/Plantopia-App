using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSel : MonoBehaviour {

	string[] items;
	public Text name1;
//	public GameObject img1;

	// Use this for initialization
	void Start () {
//		PlayerPrefs.SetInt ("userID",10); //get this back after test Login with Mon FB or it is success!
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
//		img1.SetActive (true);
	}
}
