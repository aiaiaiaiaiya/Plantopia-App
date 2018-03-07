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

	int plantId;

	public GameObject Sunobj;
	public GameObject Dropobj;
	public GameObject Seedobj;

	void Start () {
		anim = this.gameObject.GetComponent <Animator> ();
	}

	void Update(){
		System.DateTime CurrentDate = new System.DateTime();
		CurrentDate = System.DateTime.Now;
		if (CurrentDate.Hour == 5) {
			StartCoroutine (InsertControlLight (lightVal.ToString ()));
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		plantId = PlayerPrefs.GetInt ("plantID");
//		lightVal = PlayerPrefs.GetFloat ("light");
//		pumpSpeedVal = PlayerPrefs.GetFloat ("pumpSpeed");

		GameObject obj = other.gameObject;
		Destroy (obj);

		if (obj.CompareTag ("draggable")) {
			
			if (obj.name.Equals ("Sun") && !addlight) {
				addlight = true;
				StartCoroutine (InstantiacteObj (obj,other,Sunobj));
				print ("+255 => light = "+lightVal);
				StartCoroutine (InsertControlLight (lightVal.ToString ()));
//				StartCoroutine (DelayToggle("light"));
				anim.SetTrigger ("Sun");
				sunSound.Play ();
			} else if (obj.name.Equals ("Drop") && !addpumpSpeed) {
				addpumpSpeed = true;
				StartCoroutine (InstantiacteObj (obj,other,Dropobj));
				print ("+80 => pump = "+pumpSpeedVal);
				StartCoroutine (InsertControlPump (pumpSpeedVal.ToString ()));
				StartCoroutine (DelayToggle("pump"));
				anim.SetTrigger ("Water");
				dropSound.Play ();
			} else if (obj.name.Equals ("Seed")) {
				StartCoroutine (InstantiacteObj (obj,other,Seedobj));
			}  

//			else if (obj.name.Equals ("Seed")) {
//				lightVal = PlayerPrefs.GetFloat ("light");
//				lightVal = lightVal + 10;
//				PlayerPrefs.SetFloat ("light", lightVal);
//			}
			PlayerPrefs.Save ();


		}
	}

	IEnumerator InsertControlLight(string l){
		WWWForm form = new WWWForm ();
		form.AddField ("action", "insertControlLight");
		form.AddField ("plantId", plantId);
		form.AddField ("light", l);
		WWW insertData = new WWW ("http://54.169.202.67/plantopia_API.php",form);
		yield return insertData;
	}

	IEnumerator InsertControlPump(string ps){
		WWWForm form = new WWWForm ();
		form.AddField ("action", "insertControlPump");
		form.AddField ("plantId", plantId);
		form.AddField ("pumpSpeed", ps);
		WWW insertData = new WWW ("http://54.169.202.67/plantopia_API.php",form);
		yield return insertData;
	}

	IEnumerator InstantiacteObj (GameObject obj, Collider2D other,GameObject eachObj) {
		Vector3 v = obj.GetComponent<DragHandle> ().pos;
		Transform slot = other.gameObject.transform.parent;

		yield return new WaitForSeconds(2f);

		GameObject ins = Instantiate (eachObj, v, slot.rotation, slot);
		ins.transform.DOShakeScale (1f);
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
			addlight = false;
		}
	}

}
