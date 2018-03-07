using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dialogScriptGame02 : MonoBehaviour {

	public Text dialog;
	public GameObject click;
	int step; 

	WaitForSeconds w = new WaitForSeconds(3);

	public AudioSource s1;
	public AudioSource s2;
	public AudioSource s3;

	// Use this for initialization
	void Start () {
		step = 0;
		click.SetActive (false);
		StartCoroutine ("delayHideClick");
		s1.Play ();
	}

	// Update is called once per frame
	void Update () {
		if (step == 0) {
			dialog.text = "มาถึงขั้นตอนต่อไปแล้วล่ะ ตอนนี้เธอต้องผสมสารอาหารให้เด็กๆพวกนั้น ซึ่งวิธีผสมก็มีสูตรที่จำได้ไม่ยากเลย";
		} else if (step == 1) {
			dialog.text = "น้ำ 1 ลิตร ต่อ ปุ๋ย A (สารสีแดง) 5 ซีซี และ ปุ๋ย B (สารสีน้ำเงิน) 5 ซีซี ไม่ยากใช่ไหมล่ะ!";
		} else if (step == 2) {
			dialog.text = "คราวนี้เธอลองผสมสารสีต่างๆให้ได้ตามในตัวอย่างนะ ถ้าพร้อมแล้วก็ลุยกันเลย!";
		} 

	}

	public void updateDialog(){
		if (step == 2) {
			s3.Stop ();
			SceneManager.LoadScene ("Game02");
		} else if (step == 0) {
			s1.Stop ();
			s2.Play ();
		} else if (step == 1) {
			s2.Stop ();
			s3.Play ();
		}
		click.SetActive (false);
		step += 1;
		StartCoroutine ("delayHideClick");
	}

	IEnumerator delayHideClick(){
		yield return w;
		click.SetActive (true);
	}
}
