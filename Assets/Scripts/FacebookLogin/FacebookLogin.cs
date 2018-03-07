using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FacebookLogin : MonoBehaviour {

	string FBuserid;
	string username;
	public GameObject addUsernameModule;
	public InputField UsernameInput;

//	public Text testText;

	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		addUsernameModule.SetActive (false);
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}
		


	private void AuthCallback (ILoginResult result) {
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log (aToken.UserId);
			FBuserid = aToken.UserId;
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log (perm);
			}
			StartCoroutine ("QueryFBuserID");
		} else {
			Debug.Log ("User cancelled login");
		}
	}

	public void LoginWithFB(){
		FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" },AuthCallback);
	}

	IEnumerator QueryFBuserID () {
		WWWForm form = new WWWForm ();
		form.AddField ("action", "readFBuserID");
		form.AddField ("FBid", FBuserid);
		WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
		yield return itemsData;
		if (itemsData.text == "0") {
			addUsernameModule.SetActive (true);
		} else {
			string[] items;
			string itemsDataString = itemsData.text;
			items = itemsDataString.Split (',');
			PlayerPrefs.SetInt("userID", int.Parse(items [0]));
			PlayerPrefs.SetString("username", items [1]);
			PlayerPrefs.Save ();

			SceneManager.LoadScene ("CharacterSelection");
//			testText.text = (PlayerPrefs.GetInt("userID") + PlayerPrefs.GetString("username"));
		}
	}

	//No account yet
	public void SubmitUsername(){
		username = UsernameInput.text.ToString ();
		StartCoroutine ("InsertFBuserID");
	}

	IEnumerator InsertFBuserID () {
		WWWForm form = new WWWForm ();
		form.AddField ("action", "insertFBuserID");
		form.AddField ("FBid", FBuserid);
		form.AddField ("username", username);
		WWW insertData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
		yield return insertData;
		StartCoroutine ("QueryFBuserID");
	}
}
