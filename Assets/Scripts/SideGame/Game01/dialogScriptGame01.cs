using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dialogScriptGame01 : MonoBehaviour {

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
			dialog.text = "เอาล่ะ! ถ้าเธอตั้งใจจะปลูกผักให้งอกงาม ต้องเริ่มจากฐานรองที่ดีเสียก่อน ฐานรองปลูกก็เปรียบเตียงนอนของเจ้าพวกนี้น่ะ ทำมาจากฟองน้ำหนานุ่ม แต่ต้องกรีดให้เป็นร่องเสียก่อนถึงจะใช้งานได้";
		} else if (step == 1) {
			dialog.text = "วิธีการก็คือ เธอต้องนำฟองน้ำรูปลูกบาศก์ขนาด 1 นิ้ว มากรีดให้เป็นรูปกากบาทยาวประมาณ 1 เซนติเมตร อย่าลืมกรีดให้ทะลุไปถึงด้านล่างเลยนะ เมื่อเมล็ดผักน้อย ๆ ของเธอเริ่มมีราก รากของเจ้าพวกนี้จะได้เติบโตลงไปที่สารอาหารได้ง่าย";
		} else if (step == 2) {
			dialog.text = "ไม่ยากใช่ไหมล่ะ ฉันมีแบบทดสอบเล็กๆมาให้เธอลองทำด้วยล่ะ ลองจัดการฟองน้ำพวกนี้ดูนะ ถ้าพร้อมแล้วก็ลุยกันเลย!";
		} 

	}

	public void updateDialog(){
		if (step == 2) {
			s3.Stop ();
			SceneManager.LoadScene ("Game01");
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
