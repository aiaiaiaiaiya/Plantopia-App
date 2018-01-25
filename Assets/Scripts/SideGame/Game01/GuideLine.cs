using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideLine : MonoBehaviour {

	bool inGuide;

	void Start(){
		inGuide = false;
	}

	void OnTriggerEnter(Collider other){
		inGuide = true;
		print ("IN");
	}

	void OnTriggerExit(Collider other){
		inGuide = false;
		print ("OUT");
	}

}
