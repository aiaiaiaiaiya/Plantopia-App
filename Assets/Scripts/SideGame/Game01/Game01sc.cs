using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game01sc : MonoBehaviour {

//	// Creates a line renderer that follows a Sin() function
//	// and animates it.
//
//	public Color c1 = Color.yellow;
//	public Color c2 = Color.red;
//	public int lengthOfLineRenderer = 20;
//
//	void Start()
//	{
//		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
//		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
//		lineRenderer.widthMultiplier = 0.2f;
//		lineRenderer.positionCount = lengthOfLineRenderer;
//
//		// A simple 2 color gradient with a fixed alpha of 1.0f.
//		float alpha = 1.0f;
//		Gradient gradient = new Gradient();
//		gradient.SetKeys(
//			new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
//			new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
//		);
//		lineRenderer.colorGradient = gradient;
//	}
//
//	void Update()
//	{
//		LineRenderer lineRenderer = GetComponent<LineRenderer>();
//		var t = Time.time;
//		for (int i = 0; i < lengthOfLineRenderer; i++)
//		{
//			lineRenderer.SetPosition(i,Input.mousePosition);
//		}
//	}
	public GameObject trailPrefab;
	private GameObject thisTrail;
	private Vector3 startPos;
	public Plane objPlane;


	// Use this for initialization
	void Start ()
	{
		objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
	}

	// Update is called once per frame
	void Update ()
	{
		if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
		{
			thisTrail = (GameObject)Instantiate(trailPrefab, this.transform.position, Quaternion.identity);
			Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			float rayDistance;

			if(objPlane.Raycast(mRay, out rayDistance))
			{
				startPos = mRay.GetPoint(rayDistance);
			}
		}
		else if(((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
		{
			Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			float rayDistance;

			if(objPlane.Raycast(mRay, out rayDistance))
			{
				thisTrail.transform.position = mRay.GetPoint(rayDistance);
			}
		}
		else if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0))
		{
			if(Vector3.Distance(thisTrail.transform.position, startPos) < 0.1)
			{
				Destroy(thisTrail);
			}
		}
	}﻿




}