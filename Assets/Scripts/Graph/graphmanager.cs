using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class graphmanager : MonoBehaviour {
	
	public Image bar;
	public GameObject spawnBarLight;
	public GameObject spawnBarWater;
	public GameObject spawnBarTemp;
	public int width = 40;
	string[] items;
	public List<int> hour = new List<int>();
	public List<float> avglight = new List<float>();
	public List<float> avgwater = new List<float>();
	public List<float> avgtemp = new List<float>();
//	List<Image> golight = new List<Image>();
//	List<Image> gowater = new List<Image>();
//	List<Image> gotemp = new List<Image>();

	public Text dateTxt;

	// Use this for initialization
	void Start () {
//		string date = System.DateTime.Now.ToString("yyyy-MM-dd"); //.ToString("yyyy-MM-dd HH:mm:ss");
//		print (date);
		string date;
		date = System.DateTime.Now.ToString ("dd/MM/yyyy");
		dateTxt.text = date;
		StartCoroutine (ReadDBDay (date));

	}

	void makeGraphLight(){
		for (int i = 0; i < avglight.Count; i++) {
			Image l = Instantiate (bar,spawnBarLight.transform);
			l.transform.localPosition += new Vector3 (i * width, 0, 0);
			l.rectTransform.sizeDelta = new Vector2 (width, (avglight[i]/400)*260);
//			golight.Add (l);
			Image w = Instantiate (bar,spawnBarWater.transform);
			w.transform.localPosition += new Vector3 (i * width, 0, 0);
			w.rectTransform.sizeDelta = new Vector2 (width, (avgwater[i]/40)*260);
//			gowater.Add (w);
			Image t = Instantiate (bar,spawnBarTemp.transform);
			t.transform.localPosition += new Vector3 (i * width, 0, 0);
			t.rectTransform.sizeDelta = new Vector2 (width, (avgtemp[i]/40)*260);
//			gotemp.Add (t);
		}
	}
		
	IEnumerator ReadDBDay (string date) {
		print ("At "+date);
		WWWForm form = new WWWForm ();
		form.AddField ("action", "readPotInputForGraphDayHourly");
		form.AddField ("date", date);
		WWW itemsData = new WWW ("http://54.169.202.67/plantopia_API.php", form);
		yield return itemsData;
		print (itemsData.text);
		string itemsDataString = itemsData.text;
		items = itemsDataString.Split (';');
		for (int i = 0; i < items.Length - 1; i++) {
			GetEachLine(items[i]);
		}
		makeGraphLight ();
	}

	void GetEachLine(string data){
		print ("DATA:" + data);
		string[] d;
		d = data.Split (',');
		hour.Add (int.Parse (d [0]));
		avglight.Add (float.Parse (d [1]));
		avgwater.Add (float.Parse (d [2]));
		avgtemp.Add (float.Parse (d [3]));
	}

}

