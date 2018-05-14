using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LoadScenewithDelay ("GameplayScene-RE", 3);
	}


	void LoadScenewithDelay(string sceneName,int sec){
		StartCoroutine (LoadSceneEnum (sceneName,sec));
	}

	IEnumerator LoadSceneEnum (string sceneName,int sec) {
		yield return new WaitForSeconds (sec);
		SceneManager.LoadScene(sceneName);
	}
}
