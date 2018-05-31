using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	public static int setUserID = 11;
	string username = "Tonkla";

	string[] items;

	int userID;

	void Awake () {
		print ("Game Awake!");
		SetUserID (); //temporary function bc no have LOGIN feature
		StartCoroutine ("LoadPlants");
	}

	void SetUserID () { //INCASE of on PC
		PlayerPrefs.SetInt ("userID", setUserID);
		PlayerPrefs.SetString ("username", username);
		PlayerPrefs.Save ();
		print ("Already set user ID for testing");
		userID = PlayerPrefs.GetInt ("userID");
	}

	IEnumerator LoadPlants () {
		//clear player pref
		for (int i = 0; i < 4; i++) {
			string label = "plantID_" + (i + 1).ToString ();
			PlayerPrefs.SetInt (label, 0);
			print ("SET: " + label + " = " + PlayerPrefs.GetInt (label));

			label = "plantName_" + (i + 1).ToString ();
			PlayerPrefs.SetString (label, "");
			print ("SET: " + label + " = " + PlayerPrefs.GetString (label));

			label = "plantTypeNo_" + (i + 1).ToString ();
			PlayerPrefs.SetInt (label, 0);
			print ("SET: " + label + " = " + PlayerPrefs.GetInt (label));

			label = "plantLevel_" + (i + 1).ToString ();
			PlayerPrefs.SetInt (label, 0);
			print ("SET: " + label + " = " + PlayerPrefs.GetInt (label));
			PlayerPrefs.Save ();
		}


		int userID = PlayerPrefs.GetInt ("userID");
		print (userID);
		WWWForm form = new WWWForm ();
		form.AddField ("action", "readPlantbyUserID");
		form.AddField ("userId", userID); 
		WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
		yield return itemsData;
		print ("--------------LoadPlant"+itemsData.text);
		if (itemsData.text != "0") {
			string itemsDataString = itemsData.text;
			string[] rows = itemsDataString.Split (';');
			print ("ROWS LENGHT : " + rows.Length);
			for (int i = 0; i < rows.Length - 1; i++) { //-1 for split for over row
				print ("LOOP LOADPLANTS: " + rows [i]);
				items = rows [i].Split (',');
				string label = "plantID_" + (i + 1).ToString ();
				PlayerPrefs.SetInt (label, int.Parse (items [1]));
				print ("SET: " + label + " = " + PlayerPrefs.GetInt (label));

				label = "plantName_" + (i + 1).ToString ();
				PlayerPrefs.SetString (label, items [2]);
				print ("SET: " + label + " = " + PlayerPrefs.GetString (label));

				label = "plantTypeNo_" + (i + 1).ToString ();
				PlayerPrefs.SetInt (label, int.Parse (items [3]));
				print ("SET: " + label + " = " + PlayerPrefs.GetInt (label));

				label = "plantLevel_" + (i + 1).ToString ();
				PlayerPrefs.SetInt (label, int.Parse (items [5]));
				print ("SET: " + label + " = " + PlayerPrefs.GetInt (label));

				PlayerPrefs.Save ();
			}
		}

		UIManager sc = gameObject.GetComponent<UIManager> ();
		sc.ChangePlant(1);

	 


	}

	public void CallLoadPot(int chanum){
		StartCoroutine (LoadPotInput(chanum));
	}

	IEnumerator LoadPotInput (int chanum) {
		WWWForm form = new WWWForm ();
		form.AddField ("action", "readPotInput");
		form.AddField ("userID", userID);
		WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
		yield return itemsData;
		if (itemsData.text!=null) {
			string itemsDataString = itemsData.text;
			items = itemsDataString.Split (',');

			PlayerPrefs.SetFloat ("light", float.Parse (items [2]));
			PlayerPrefs.SetFloat ("temperature", float.Parse (items [4]));
			PlayerPrefs.SetFloat ("waterTemp", float.Parse (items [3]));
			PlayerPrefs.Save ();

			print ("Initial value Completed!");
		}
	}


		
}
