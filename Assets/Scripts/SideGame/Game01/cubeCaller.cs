using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class cubeCaller : MonoBehaviour {

	public GameObject cubePrefab;
	Animator cAnim;
	GameObject c;
	bool hasCube;

	public Text scoreText;
	int score;

	public Text timerTxt;
	public int timer;
	int countdown = 100;

	public GameObject btn;

	// Use this for initialization
	void Start () {
		hasCube = false;
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (countdown <= 0) {
			if (score < 3)
				scoreText.color = Color.red;
			btn.SetActive (true);
		} else {
			countdown = (timer - (int)Time.timeSinceLevelLoad);
			if (score > 3)
				scoreText.color = Color.green;
			timerTxt.text = countdown.ToString ();
		}
		
	}

	public void Coming(){
		if (!hasCube) {
			c = Instantiate (cubePrefab);
			cAnim = c.GetComponent<Animator> ();
			cAnim.Play ("come");
			hasCube = true;
		}
	}

	public void Done(){
		if (hasCube) {
			foreach(GameObject t in GameObject.FindGameObjectsWithTag ("trail")){
				Destroy (t);
			}
			cAnim.SetTrigger ("done");
			hasCube = false;
			score += 1;
			scoreText.text = score.ToString ();
		}
	}
}
