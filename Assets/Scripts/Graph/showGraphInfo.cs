using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showGraphInfo : MonoBehaviour {

	GameObject panel;

	graphmanager sc;

	// Use this for initialization
	void Start () {
		sc = GameObject.Find ("Graph Manager").GetComponent<graphmanager> ();
	}

	public void HoverEnter(){
		panel = this.gameObject.transform.GetChild (0).gameObject;
		panel.SetActive (true);
		Transform barpf = panel.transform.parent;
		Transform spawnPoint = panel.transform.parent.parent;
		int index = barpf.GetSiblingIndex ();
		Text hrs = panel.transform.GetChild (0).GetComponent<Text> ();
		hrs.text = sc.hour [index].ToString () + ":00";
		Text val = panel.transform.GetChild (1).GetComponent<Text> ();
		if (spawnPoint.name == "spawnBar Light") {
			val.text = sc.avglight [index].ToString ("F2");
		} else if (spawnPoint.name == "spawnBar Water") {
			val.text = sc.avgwater [index].ToString ("F2");
		} else if (spawnPoint.name == "spawnBar Temp") {
			val.text = sc.avgtemp [index].ToString ("F2");
		}

	}

	public void HoverExit(){
		panel.SetActive (false);
	}
}
