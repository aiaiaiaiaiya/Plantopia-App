using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager1 : MonoBehaviour {

	public void LoadScene(string sceneName){
		SceneManager.LoadScene (sceneName);
	}

	public void OffNoti(){
		StartCoroutine ("ClearEvent");
	}

	IEnumerator ClearEvent(){
		WWWForm form = new WWWForm ();
		form.AddField ("action", "resetEvent");
		WWW insertData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
		yield return insertData;
	}
}
