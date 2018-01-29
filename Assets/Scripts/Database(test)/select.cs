using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class select : MonoBehaviour {

	public int WhereID;
	public string[] items;

	void Start(){
		StartCoroutine (ReadDataQuery (WhereID));
	}

	IEnumerator ReadDataQuery (int id) {
		WWWForm form = new WWWForm ();
		form.AddField ("action", "read");
		form.AddField ("id", id);
		WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php",form);
		yield return itemsData;
		print (itemsData.text);
		string itemsDataString = itemsData.text;
		items = itemsDataString.Split (';');
		//print (items);
	}

}