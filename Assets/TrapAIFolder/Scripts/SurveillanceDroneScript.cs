﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceDroneScript : MonoBehaviour {

	public GameObject player; // Public for now

	public float movementSpeed = 4.0f;
	public float turnSpeed = 8.0f;

	public int chaseMaxDistance = 10; // To be adjusted
	public int chaseMinDistance = 2; // To be adjusted

	public float hoverForce = 90.0f; // To be adjusted
	public float hoverHeight = 3.5f; // To be adjusted

	private Rigidbody surveillanceDroneRigidbody;

	void Awake()
	{
		surveillanceDroneRigidbody = GetComponent<Rigidbody>();
	}


	void Start()
	{
		player = GameObject.FindWithTag("Player");

		float randNum = Random.Range(3,6);
		hoverHeight = randNum;
	}


	void Update()
	{
		// Currently chases you without a defined path. Will add following waypoints later when there is a defined path.
		transform.LookAt(player.transform.position);

		if(Vector3.Distance(transform.position, player.transform.position) >= chaseMinDistance)
		{
			transform.position += transform.forward * movementSpeed * Time.deltaTime;

			if(Vector3.Distance(transform.position, player.transform.position) <= chaseMinDistance)
			{
				// Some action here when close to the player
				Debug.Log("Suveillance Drone Used Taser!");
			}
		}
	}


	void FixedUpdate()
	{
		// For Drone Hovering
		Ray hoverRay = new Ray (transform.position, -transform.up);
		RaycastHit hoverHit;

		if(Physics.Raycast(hoverRay, out hoverHit, hoverHeight))
		{
			float propotionalHeight = (hoverHeight - hoverHit.distance) / hoverHeight;
			Vector3 appliedHoverForce = Vector3.up * propotionalHeight * hoverForce;
			surveillanceDroneRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
		}
	}
}
