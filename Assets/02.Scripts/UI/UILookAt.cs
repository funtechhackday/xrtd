using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAt : MonoBehaviour {

    public Camera ARCamera;

	// Use this for initialization
	void Start () {
        if (ARCamera == null)
            ARCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.LookAt(ARCamera.transform);
	}
}
