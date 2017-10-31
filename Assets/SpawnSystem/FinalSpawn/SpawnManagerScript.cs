﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManagerScript : MonoBehaviour {

	private static SpawnManagerScript mInstance;
	public static SpawnManagerScript Instance
	{
		get { return mInstance; }
	}

	public SpawnData spawn_Data;


	public Transform player;
	public int reputation;
	public float countDownTimer;
	public int sdCount;
	public int hdCount;

	//calculation
	public bool isHorizontal;
	public Transform target; //change to waypoints
	public float prevDistance;
	public float distance;
	public Vector3 spawnPoint;
	public float offsetY = 1.0f;
	public float offset = 1.0f;
	public int currentSpawnIndex;

	void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);
	}

	void Start()
	{
		
	}

	// Update is called once per frame
	void Update () {
		if(WaypointManagerScript.Instance.playerDirection == Direction.North || WaypointManagerScript.Instance.playerDirection == Direction.South)
		{
			isHorizontal = false;
		}
		else
		{
			isHorizontal = true;
		}
		//currentSpawnIndex 
		reputation = ReputationManagerScript.Instance.lastRep;

		if(countDownTimer >= spawn_Data.spawnTime)
		{
			countDownTimer = 0;
			//CalculateSpawnPoint();
			if(reputation == 1)
			{
				sdCount+=spawn_Data.spawnSDCount[0];
				hdCount+=spawn_Data.spawnHDCount[0];
				Spawn("Hunting_Droid");
				Spawn("Surveillance_Drone");
				//PoolManagerScript.Instance.Spawn("Surveillance_Drone",spawnPoint,Quaternion.identity);
				//PoolManagerScript.Instance.Spawn("Hunting_Droid",spawnPoint,Quaternion.identity);
				TimelineScript.Instance.CreateEnemyIcon("Surveillance_Drone", spawn_Data.spawnSDCount[0]);
				TimelineScript.Instance.CreateEnemyIcon("Hunting_Droid", spawn_Data.spawnHDCount[0]);
			}
			else if(reputation == 2)
			{
				hdCount+=spawn_Data.spawnHDCount[1];
				SpawnMultiple("Hunting_Droid",spawn_Data.spawnHDCount[1]);
				//PoolManagerScript.Instance.SpawnMuliple("Hunting_Droid",spawnPoint,Quaternion.identity,2,offsetY,offset,isHorizontal);
				TimelineScript.Instance.CreateEnemyIcon("Hunting_Droid", spawn_Data.spawnHDCount[1]);
			}
			else if(reputation == 3)
			{
				sdCount+=spawn_Data.spawnSDCount[2];
				hdCount+=spawn_Data.spawnHDCount[2];
				Spawn("Surveillance_Drone");
				//PoolManagerScript.Instance.Spawn("Surveillance_Drone",spawnPoint,Quaternion.identity);
				ApplyOffsetVertically();
				SpawnMultiple("Hunting_Droid",spawn_Data.spawnHDCount[2]);
				//PoolManagerScript.Instance.SpawnMuliple("Hunting_Droid",spawnPoint,Quaternion.identity,2,offsetY,offset,isHorizontal);
				TimelineScript.Instance.CreateEnemyIcon("Surveillance_Drone", spawn_Data.spawnSDCount[2]);
				TimelineScript.Instance.CreateEnemyIcon("Hunting_Droid", spawn_Data.spawnHDCount[2]);
			}
			else if(reputation == 4)
			{
				sdCount+=spawn_Data.spawnSDCount[3];
				hdCount+=spawn_Data.spawnHDCount[3];
				Spawn("Surveillance_Drone");
				//PoolManagerScript.Instance.Spawn("Surveillance_Drone",spawnPoint,Quaternion.identity);
				ApplyOffsetVertically();
				SpawnMultiple("Hunting_Droid",spawn_Data.spawnHDCount[3]);
				/*for(int i=0; i<3; i++)
				{
					PoolManagerScript.Instance.Spawn("Hunting_Droid",spawnPoint,Quaternion.identity);
				}*/
				TimelineScript.Instance.CreateEnemyIcon("Surveillance_Drone", spawn_Data.spawnSDCount[3]);
				TimelineScript.Instance.CreateEnemyIcon("Hunting_Droid", spawn_Data.spawnHDCount[3]);
			}
			else if(reputation == 5)
			{
				sdCount+=spawn_Data.spawnSDCount[4];
				hdCount+=spawn_Data.spawnHDCount[4];
				SpawnMultiple("Surveillance_Drone",spawn_Data.spawnSDCount[4]);
//				for(int i=0; i<3; i++)
//				{
//					PoolManagerScript.Instance.Spawn("Surveillance_Drone",spawnPoint,Quaternion.identity);
//				}
				ApplyOffsetVertically();
				SpawnMultiple("Hunting_Droid",spawn_Data.spawnHDCount[4]);
//				for(int i=0; i<3; i++)
//				{
//					PoolManagerScript.Instance.Spawn("Hunting_Droid",spawnPoint,Quaternion.identity);
//				}
				TimelineScript.Instance.CreateEnemyIcon("Surveillance_Drone", spawn_Data.spawnSDCount[4]);
				TimelineScript.Instance.CreateEnemyIcon("Hunting_Droid", spawn_Data.spawnHDCount[4]);
			}
		}
	}

	void LateUpdate()
	{
		if(reputation >= 1 && reputation == ReputationManagerScript.Instance.currentRep)
		{
			countDownTimer += Time.deltaTime;
		}
		else
		{
			countDownTimer = 0;
		}
	}

	void ApplyOffsetVertically()
	{
		spawnPoint.y += offsetY;
	}

	public float CalculateRange(Vector3 a, Vector3 b, float distance)
	{
		float range = distance / (a-b).magnitude;
		return range;
	}

	public void CalculateSpawnPoint()
	{
		spawnPoint = Vector3.zero;
		target = WaypointManagerScript.Instance.tracePlayerNodes[WaypointManagerScript.Instance.tracePlayerNodes.Count-1].transform;
		distance = Vector3.Distance(player.position,target.transform.position);
		if(distance == spawn_Data.spawnDistance)
		{
			spawnPoint = target.transform.position;
			currentSpawnIndex = WaypointManagerScript.Instance.tracePlayerNodes.Count-1;
		}
		else if (distance > spawn_Data.spawnDistance)
		{
			spawnPoint = Vector3.Lerp(player.position,target.transform.position,CalculateRange(player.position,target.transform.position,spawn_Data.spawnDistance));
			currentSpawnIndex = WaypointManagerScript.Instance.tracePlayerNodes.Count-1;
		}
		else if(distance < spawn_Data.spawnDistance)
		{
			for(int i = WaypointManagerScript.Instance.tracePlayerNodes.Count-2; i >= 0; i--)
			{
				prevDistance = distance;
				distance += Vector3.Distance(target.transform.position,WaypointManagerScript.Instance.tracePlayerNodes[i].transform.position);
				if(distance >= spawn_Data.spawnDistance)
				{
					if(distance == spawn_Data.spawnDistance)
					{
						spawnPoint = WaypointManagerScript.Instance.tracePlayerNodes[i].transform.position;
						currentSpawnIndex = i;
						break;
					}
					else if(distance > spawn_Data.spawnDistance)
					{
						distance = spawn_Data.spawnDistance - prevDistance;
						spawnPoint = Vector3.Lerp(target.transform.position,WaypointManagerScript.Instance.tracePlayerNodes[i].transform.position,CalculateRange(target.transform.position,WaypointManagerScript.Instance.tracePlayerNodes[i].transform.position,distance));
						currentSpawnIndex = i;
						break;
					}
				}
				else
				{
					target = WaypointManagerScript.Instance.tracePlayerNodes[i].transform;
				}
			}

		}
	}

	public void Spawn(string name)
	{
		CalculateSpawnPoint();
		GameObject obj = PoolManagerScript.Instance.GetObject(name);
		if(obj != null)
		{
			obj.transform.position = spawnPoint;
			obj.SetActive(true);
		}
	}


	public void SpawnMultiple(string name, int amount)
	{
		CalculateSpawnPoint();
		if(!isHorizontal)
		{
			spawnPoint.x -= offset;
		}
		else
		{
			spawnPoint.z -= offset;
		}
		for(int i=0; i<amount ; i++)
		{
			GameObject obj = PoolManagerScript.Instance.GetObject(name);
			if(obj != null)
			{
				obj.transform.position = spawnPoint;
				obj.SetActive(true);
			}
			if(!isHorizontal)
			{
				spawnPoint.x += offset;
			}
			else
			{
				spawnPoint.z += offset;
			}
		}
	}
//	public GameObject Spawn(string objectName,Vector3 newPosition, Quaternion newRotation){
//		if(pool[objectName].Count > 0)
//		{
//			GameObject go = pool[objectName].Pop();
//			go.transform.position = newPosition;
//			go.transform.rotation = newRotation;
//			go.SetActive(true);
//			return go;
//		}
//		return null;
//	}
}
