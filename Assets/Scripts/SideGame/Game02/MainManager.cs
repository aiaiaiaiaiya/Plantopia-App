using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainManager : MonoBehaviour {
	public GameObject mainSolution;
	Color colorSolution;
	Color newColorSolution;
	Image mainImg;
	float t;

	public GameObject colorShowobj;
	Image colorShowImg;
	public Color[] colorList;
	Color colorShow;

	public Text scoreText;

	int score;

	public AudioSource correct;
	public AudioSource wrong;

	public GameObject btn;

	int i;


	// Use this for initialization
	void Start () {
		mainImg = mainSolution.GetComponent<Image> ();
		colorShowImg = colorShowobj.GetComponent<Image> ();
		i = 0;
		RandomColor ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MixButton(string color){
		colorSolution = mainImg.color;
		if (colorSolution.a < 1)
			colorSolution = new Color(1,1,1,1);

		if (color == "Blue")
			newColorSolution = CombineColors(colorSolution, Color.blue);
		else if (color == "Red")
			newColorSolution = CombineColors(colorSolution, Color.red);
		else if (color == "Green")
			newColorSolution = CombineColors(colorSolution, Color.green);
		else if (color == "Yellow")
			newColorSolution = CombineColors(colorSolution, Color.yellow);

		mainImg.color = newColorSolution;
			
	}

	public static Color CombineColors(params Color[] aColors)
	{
		Color result = new Color(0,0,0,1);
		foreach(Color c in aColors)
		{
			result += c;
		}
		result /= aColors.Length;
		return result;
	}

	public void Discard(){
		mainImg.color = new Color (0, 0, 0, 0);
	}

	void RandomColor(){
//		colorShow = colorList [Random.Range (0, 3)];


		colorShow = colorList [i];
		colorShowImg.color = colorShow;
		i++;

	}

	public void Submit(){
		if (mainImg.color == colorShow && score < 3) {
			score += 1;
			scoreText.text = score.ToString ();
			correct.Play ();
			RandomColor ();
			if (score == 3) {
				btn.SetActive (true);
			}
		} else {
			wrong.Play ();
		}
		Discard ();

	}
}
