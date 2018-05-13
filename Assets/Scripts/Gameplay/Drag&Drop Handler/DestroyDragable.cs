using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DestroyDragable : MonoBehaviour {

	public AudioSource actionSound;
	public AudioSource sunSound;
	public AudioSource dropSound;

	Animator anim;

	WaitForSeconds delay = new WaitForSeconds(2f);

	bool addlight = false;
	bool addpumpSpeed = false;

	float lightVal = 255; //255 ---- 15 min -> 0
	float pumpSpeedVal = 80; //80 ---- 5 min -> 50

	/*TEMP CODE*/
//	int userID = 4;
	int userID;

	public GameObject Sunobj;
	public GameObject Dropobj;
	public GameObject Seedobj;
//	public GameObject blockpanel;

	void Start () {
		anim = this.gameObject.GetComponent <Animator> ();
		userID = PlayerPrefs.GetInt ("userID");
	}

	void Update(){
		System.DateTime CurrentDate = new System.DateTime();
		CurrentDate = System.DateTime.Now;
		if (CurrentDate.Hour == 5) {
			StartCoroutine (InsertControlLight (lightVal.ToString ()));
		}
	}

//	void BlockPanel(){
//		
//	}

	void OnTriggerEnter2D(Collider2D other) {
		print ("Crash!-----------------------------");
//		userID = PlayerPrefs.GetInt ("plantID");
//		lightVal = PlayerPrefs.GetFloat ("light");
//		pumpSpeedVal = PlayerPrefs.GetFloat ("pumpSpeed");

		GameObject obj = other.gameObject;


		if (obj.CompareTag ("draggable")) {
//			blockpanel.SetActive (true);
			if (obj.name.Equals ("Sun") && !addlight) {
				anim.SetTrigger ("Sun");
				sunSound.Play ();
				addlight = true;
				InstantiacteObj (obj,Sunobj);
				print ("+255 => light = "+lightVal);
				StartCoroutine (InsertControlLight (lightVal.ToString ()));
//				StartCoroutine (DelayToggle("light"));

			} else if (obj.name.Equals ("Drop") && !addpumpSpeed) {
				anim.SetTrigger ("Water");
				dropSound.Play ();
				addpumpSpeed = true;
				InstantiacteObj (obj,Dropobj);
				print ("+80 => pump = "+pumpSpeedVal);
				StartCoroutine (InsertControlPump (pumpSpeedVal.ToString ()));
				StartCoroutine (DelayToggle("pump"));

			} else if (obj.name.Equals ("Seed")) {
				InstantiacteObj (obj,Seedobj);
			}  

//			else if (obj.name.Equals ("Seed")) {
//				lightVal = PlayerPrefs.GetFloat ("light");
//				lightVal = lightVal + 10;
//				PlayerPrefs.SetFloat ("light", lightVal);
//			}
			PlayerPrefs.Save ();
			Destroy (obj);

		}
	}

	IEnumerator InsertControlLight(string l){
		WWWForm form = new WWWForm ();
		form.AddField ("action", "insertControlLight");
		form.AddField ("userID", userID);
		form.AddField ("light", l);
		WWW insertData = new WWW ("http://54.169.202.67/plantopia_API.php",form);
		yield return insertData;
	}

	IEnumerator InsertControlPump(string ps){
		WWWForm form = new WWWForm ();
		form.AddField ("action", "insertControlPump");
		form.AddField ("userID", userID);
		form.AddField ("pumpSpeed", ps);
		WWW insertData = new WWW ("http://54.169.202.67/plantopia_API.php",form);
		yield return insertData;
	}

	void InstantiacteObj (GameObject obj, GameObject eachObj) {
		Vector3 v = obj.GetComponent<DragHandle> ().pos;
		Transform slot = obj.transform.parent;

		GameObject ins = Instantiate (eachObj, v, slot.rotation, slot);
//		ins.transform.DOShakeScale (1f);
		ins.name = eachObj.name;
		actionSound.Play ();
	}

	IEnumerator DelayToggle(string choice){
		if (choice == "light") {
			yield return new WaitForSeconds (30f);
			print ("Delay light is done RESET value and put it to DB");
			StartCoroutine (InsertControlLight ("0"));
			addlight = false;
		} else if (choice == "pump") {
			yield return new WaitForSeconds (30f);
			print ("Delay pump is done RESET value and put it to DB");
			StartCoroutine (InsertControlPump ("50"));
			addpumpSpeed = false;
		}
	}

}
