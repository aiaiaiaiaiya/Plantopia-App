using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BtnShowPanel2 : MonoBehaviour {

	public GameObject panel;
	RectTransform rect;
	bool show = false;


	void Start () {
		rect = panel.GetComponent<RectTransform> ();
	}
	
	public void ToggleShowPanel(){
		if (!show) {
			rect.anchoredPosition = Vector3.zero;
			show = true;
		} else {
			rect.anchoredPosition = new Vector3(220,0,0);
			show = false;
			Transform sun =  transform.Find ("Sun Slot/Sun");
			Transform drop =  transform.Find ("Drop Slot/Drop");
			sun.GetComponent<RectTransform> ().anchoredPosition = Vector3.zero;
			drop.GetComponent<RectTransform> ().anchoredPosition = Vector3.zero;
		}
	}


}
