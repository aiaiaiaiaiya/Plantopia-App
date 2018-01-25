using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Dialog : MonoBehaviour {

	public Text ditext;
	int status;

	void Awake() {
	}
	// Use this for initialization
	void Start () {
		status = 0;
	}

	// Update is called once per frame
	void Update () {
		if(status==0)
			ditext.text = "จงผสมสารลงในโถด้านล่าง\nให้มีสีเหมือนตัวอย่างด้านซ้ายมือ\nกดเพื่อไปต่อ";
	}
}
