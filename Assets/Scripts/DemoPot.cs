using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPot : MonoBehaviour {


	Animator anim;
	string[] items;
	WaitForSeconds repeatInTime = new WaitForSeconds(1f); //Repeat every 30 seconds
	Light pnk;
	Light blu;
	string old_p = "";
	string old_b = "";
	string p = "";
	string b = "";
	int i;

	void Start () {
		i = 0;
		pnk = gameObject.transform.Find ("Pink Light").gameObject.GetComponent <Light>();
		blu = gameObject.transform.Find ("Blue Light").gameObject.GetComponent <Light>();
		anim = gameObject.GetComponent<Animator> ();
		pnk.intensity = 0f;
		blu.intensity = 0f;
		StartCoroutine ("ShowLight");
	}

	IEnumerator ShowLight () {
		while (true) {
			if (p != old_p && p != "" && i > 1) {
				anim.SetTrigger ("pnk");
			} else if (b != old_b && b != "" && i > 1) {
				anim.SetTrigger ("blu");
			} else {
				pnk.intensity = 1f;
				blu.intensity = 0f;
			}

			old_p = p;
			old_b = b;

			StartCoroutine ("ReadDataQuery");

			i++;

			yield return repeatInTime;
		}	
	}

	IEnumerator ReadDataQuery () {
		WWWForm form = new WWWForm ();
		form.AddField ("action", "readControl");
		form.AddField ("plantId", 1);
		WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
		yield return itemsData;
		print (itemsData.text);
		string itemsDataString = itemsData.text;
		items = itemsDataString.Split (',');
		p = items [2].ToString ();
		b = items [3].ToString ();
	}
}
