using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text userName;
	public Text plantName;

	void Awake () {
		userName.text = PlayerPrefs.GetString ("username");
		plantName.text = PlayerPrefs.GetString ("plantName");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
