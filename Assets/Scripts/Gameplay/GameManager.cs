using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	//these variables will send the values in.
	public static int plantIDset = 11;

	//these vaibles will collect the saved values
	//	int plantID;

	string[] items;



	//	void CollectSavedValues () {
	//		plantID = PlayerPrefs.GetInt ("plantID");
	//	}

	void Awake () {
		print ("Game Awake!");
		SetPlantID (); //temporary function bc no have LOGIN feature
		StartCoroutine ("LoadPotInput");
	}

	public static void SetPlantID () {
		PlayerPrefs.SetInt ("plantID", plantIDset);
		PlayerPrefs.Save ();
		print ("Already set Plant ID for tesing");
	}

	IEnumerator LoadPotInput () {
		WWWForm form = new WWWForm ();
		form.AddField ("action", "readPotInput");
		form.AddField ("plantId", plantIDset);
		WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
		yield return itemsData;
		print (itemsData.text);
		string itemsDataString = itemsData.text;
		items = itemsDataString.Split (',');

		PlayerPrefs.SetFloat ("light", float.Parse(items [2]));
		PlayerPrefs.SetFloat ("temperature", float.Parse(items [3]));
		PlayerPrefs.SetFloat ("waterTemp", float.Parse(items [4]));
		PlayerPrefs.SetFloat ("pumpSpeed", float.Parse(items [5]));
		PlayerPrefs.Save ();

		print ("Initial value Completed!");
	}
		
}
