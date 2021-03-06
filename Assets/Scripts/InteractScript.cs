﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InteractScript : MonoBehaviour
{

	RaycastHit hit;
	Ray ray;
	UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController controller;
	PlayerCoreController player;

	public float rayDistance = 10f;

	GameObject lightObject;
	Light lightSet;

	public bool toOpenDoor;
	public GameObject currentDoor = null;

	public static InteractScript _instance;

	public static InteractScript Instance { get { return _instance; } }

	void Awake ()
	{
		player = GetComponent<PlayerCoreController> ();
		lightObject = GameObject.Find ("Directional Light");
		lightSet = FindObjectOfType<Light> ();

		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		} else {
			_instance = this;
		}

	}

	void Start ()
	{
		
	}

	void Update ()
	{
		CheckInteract ();
	}

	//When left mouse button is pressed, shoot out a ray cast from screen to pointer.//
	//If the player is within the radius of the object, it will go towards it.//

	void CheckInteract ()
	{
		if (Input.GetMouseButtonDown (0) || Input.touchCount > 0) {
			
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, rayDistance)) {
				
				Debug.DrawRay (transform.position, hit.transform.position, Color.red);
				//Debug.Log (hit.transform.name);

				if (hit.transform.tag == "MountainDew") {
					//lightObject.transform.rotation = Quaternion.Euler (20f, -90f, lightObject.transform.rotation.z);
					//lightObject.transform.rotation = Quaternion.Lerp (lightObject.transform.rotation, Quaternion.identity, Time.deltaTime);
					lightSet.color = Random.ColorHSV (0f, 1f, 0f, 1f, 0f, 1f, 0f, 1f);
					hit.transform.Translate (Vector3.up * Time.deltaTime * 3.0f);
					//hit.transform.gameObject.SetActive (false);
				}
				if (hit.transform.tag == "DoorSwitch") {
					
					Transform doorThing = hit.transform.GetChild (0); // Get the child of the switch and deactivates it

					Renderer doorRender = hit.transform.gameObject.GetComponent<Renderer> ();

					if (doorRender.material.color == Color.red) {
						doorRender.material.color = Color.green;
						doorThing.gameObject.SetActive (false);
						//Time.timeScale = 0.5f;
						//player.RotateTowards (hit.transform.position);
					} else {
						doorRender.material.color = Color.red;
						doorThing.gameObject.SetActive (true);
					}
				}
				if (hit.transform.tag == "TrapSwitch") {
					
					Transform trapThing = hit.transform.GetChild (1);

					Renderer trapRender0 = hit.transform.GetChild (0).gameObject.GetComponent<Renderer> ();
					Renderer trapRender1 = hit.transform.GetChild (1).gameObject.GetComponent<Renderer> ();
					Transform trapObject1 = hit.transform.GetChild (1).gameObject.GetComponent<Transform> ();

					if (trapRender0.material.color == Color.red || trapRender1.material.color == Color.red) {
						
						trapRender0.material.color = Color.green;
						trapRender1.material.color = Color.green;
						trapObject1.transform.eulerAngles = new Vector3 (120.0f, trapObject1.transform.rotation.y, trapObject1.transform.rotation.z);
						//trapThing.gameObject.SetActive (false);

					} else {
						
						trapRender0.material.color = Color.red;
						trapRender1.material.color = Color.red;
						trapObject1.transform.eulerAngles = new Vector3 (-140.0f, trapObject1.transform.rotation.y, trapObject1.transform.rotation.z);
						//trapThing.gameObject.SetActive (true);

					}
				}
			}
		}

		if (Input.GetMouseButton (0) || Input.touchCount > 0) {
			
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, rayDistance)) {

				Debug.DrawRay (transform.position, hit.transform.position, Color.red);
				//Debug.Log (hit.transform.name);

				if (hit.transform.tag == "Interactable") {
					
					player.RotateTowards (hit.transform.position);

					currentDoor = hit.transform.gameObject;

					toOpenDoor = true;
				} else {
					toOpenDoor = false;
				}
			}
		}


		if (SwipeScript.Instance.GetSwipe() == SwipeDirection.Right || Input.GetKeyDown(KeyCode.D))
		{
			if (toOpenDoor)
			{
				Debug.Log ("Open Door");
				currentDoor.SetActive (false);
			}
		}
	}
}
