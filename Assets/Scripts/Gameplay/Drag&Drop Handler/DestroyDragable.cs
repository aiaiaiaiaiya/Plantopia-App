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

	[HideInInspector]
	public bool addlight;
	[HideInInspector]
	public bool addpumpSpeed;

	float lightVal;
	float pumpSpeedVal;

	int plantId;

	public GameObject Sunobj;
	public GameObject Dropobj;
	public GameObject Seedobj;

	void Start () {
		anim = this.gameObject.GetComponent <Animator> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		plantId = PlayerPrefs.GetInt ("plantID");
		lightVal = PlayerPrefs.GetFloat ("light");
		pumpSpeedVal = PlayerPrefs.GetFloat ("pumpSpeed");

		print ("TRIGGER: light = "+lightVal);
		print ("TRIGGER: pump = "+pumpSpeedVal);

		GameObject obj = other.gameObject;
		Destroy (obj);

		if (obj.CompareTag ("draggable")) {
			
			if (obj.name.Equals ("Sun")) {
				
				StartCoroutine (InstantiacteObj (obj,other,Sunobj));
				lightVal += 200;
				print ("+200 => light = "+lightVal);
				StartCoroutine (InsertControlLight (lightVal.ToString ()));
				PlayerPrefs.SetFloat ("light", lightVal);
				anim.SetTrigger ("Sun");
				sunSound.Play ();
			} else if (obj.name.Equals ("Drop")) {
				
				StartCoroutine (InstantiacteObj (obj,other,Dropobj));
				pumpSpeedVal += 0.2f;
				print ("+0.2 => pump = "+pumpSpeedVal);
				StartCoroutine (InsertControlPump (pumpSpeedVal.ToString ()));
				PlayerPrefs.SetFloat ("pumpSpeed", pumpSpeedVal);
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


}
