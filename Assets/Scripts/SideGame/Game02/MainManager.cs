using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour {
	public GameObject mainSolution;
	Color colorSolution;
	Color newColorSolution;
	Image mainImg;
	float t;


	// Use this for initialization
	void Start () {
		mainImg = mainSolution.GetComponent<Image> ();
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
}
