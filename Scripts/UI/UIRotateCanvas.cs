using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Always face the UI towards player i.e: Health bar
/// </summary>
public class UIRotateCanvas : MonoBehaviour {

    GameObject mainCamera;

    Canvas canvas;
    // Use this for initialization
    void Start () {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        canvas = GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        canvas.transform.LookAt(mainCamera.transform);       
	}
}
