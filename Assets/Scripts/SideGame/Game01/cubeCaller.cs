using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeCaller : MonoBehaviour {

	public GameObject cubePrefab;
	Animator cAnim;
	GameObject c;
	bool hasCube;

	// Use this for initialization
	void Start () {
		hasCube = false;
	}
	
	// Update is called once per frame
	void Update () {
		
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
		}
	}
		
}
