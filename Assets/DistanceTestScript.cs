﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTestScript : MonoBehaviour {
	public Transform hi;
	float distance;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		distance = Vector3.Distance(transform.position, hi.position);
		Debug.Log(distance);
	}
}
