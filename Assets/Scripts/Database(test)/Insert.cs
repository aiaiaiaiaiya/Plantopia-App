using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insert : MonoBehaviour {

	public string InsertItemUserName;
	public string InsertItemfName;
	public string InsertItemlName;
	public string InsertItemEmail;
	public string InsertItemPassword;

	// Use this for initialization
	void Start () {
		StartCoroutine (InsertDataQuery (InsertItemUserName, InsertItemfName, InsertItemlName,InsertItemEmail,InsertItemPassword));
	}
	IEnumerator InsertDataQuery(string usernameitem,string fnameitem,string lnameitem,string emailitem,string passworditem){
		WWWForm form = new WWWForm ();
		form.AddField ("action", "insert");
		form.AddField ("username", usernameitem);
		form.AddField ("fName", fnameitem);
		form.AddField ("lName", lnameitem);
		form.AddField ("email", emailitem);
		form.AddField ("password", passworditem);
		WWW insertData = new WWW ("http://54.169.202.67/test.php",form);
		yield return insertData;
	}
}