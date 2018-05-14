using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddingPlant : MonoBehaviour {

	public InputField plantnameInput;
	public Dropdown planttypeInput;
	public GameObject addpanel;

	int userID;
	string plantname;
	int planttype;

	public void AddPlantSubmit(){
		userID = PlayerPrefs.GetInt ("userID");
		plantname = plantnameInput.text.ToString ();
		planttype = planttypeInput.value;
		planttype++;
		StartCoroutine ("InsertPlant");
		TogglePanel ();
	}

	IEnumerator InsertPlant () {
		WWWForm form = new WWWForm ();
		form.AddField ("action", "insertPlant");
		form.AddField ("userID", userID);
		form.AddField ("plantname", plantname);
		form.AddField ("planttype", planttype.ToString ());
		WWW insertData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
		yield return insertData;


		SceneManager.LoadScene("Loading");
	}

	public void TogglePanel(){
		if (addpanel.activeInHierarchy) {
			addpanel.SetActive (false);
		} else {
			addpanel.SetActive (true);
		}
	}
}
