using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class DragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	[HideInInspector]
	public Vector3 pos;

	private Vector3 screenPoint;


	void Start () {
		pos = this.transform.position;
	}
		
	public void OnBeginDrag (PointerEventData eventData)
	{
		
	}

	public void OnDrag (PointerEventData eventData)
	{
		screenPoint = Input.mousePosition;
		screenPoint.z = 1f; //distance of the plane from the camera
		transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		if (this.gameObject.transform.position.y > 0) {
			this.gameObject.transform.position = pos;
		} else { 
			this.gameObject.transform.DOMove (pos,1f);
		}
	}





}
