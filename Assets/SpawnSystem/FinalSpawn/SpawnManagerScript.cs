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

	public Transform player;
	public int reputation;
	public float countDownTimer;
	public float spawnTime;
	public int sdCount;
	public int hdCount;

	//calculation
	public bool isHorizontal;
	public float spawnDistance = 4.0f;
	public List<Transform> pointList;
	public Transform target; //change to waypoints
	public float prevDistance;
	public float distance;
	public Vector3 spawnPoint;
	public float offsetY = 1.0f;
	public float offset = 1.0f;

	void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		reputation = ReputationManagerScript.Instance.lastRep;
		if(reputation >= 1 && reputation == ReputationManagerScript.Instance.currentRep)
		{
			countDownTimer += Time.deltaTime;
		}
		else
		{
			countDownTimer = 0;
		}

		if(countDownTimer >= spawnTime)
		{
			countDownTimer = 0;
			CalculateSpawnPoint();
			if(reputation == 1)
			{
				sdCount+=1;
				hdCount+=1;
				PoolManagerScript.Instance.Spawn("Surveillance_Drone",spawnPoint,Quaternion.identity);
				ApplyOffsetVertically();
				PoolManagerScript.Instance.Spawn("Hunting drone",spawnPoint,Quaternion.identity);
			}
			else if(reputation == 2)
			{
				hdCount+=2;
				for(int i=0; i<2; i++)
				{
					PoolManagerScript.Instance.Spawn("Hunting drone",spawnPoint,Quaternion.identity);
				}
			}
			else if(reputation == 3)
			{
				sdCount+=1;
				hdCount+=2;
				PoolManagerScript.Instance.Spawn("Surveillance_Drone",spawnPoint,Quaternion.identity);
				ApplyOffsetVertically();
				for(int i=0; i<2; i++)
				{
					PoolManagerScript.Instance.Spawn("Hunting drone",spawnPoint,Quaternion.identity);
				}
			}
			else if(reputation == 4)
			{
				sdCount+=1;
				hdCount+=3;
				PoolManagerScript.Instance.Spawn("Surveillance_Drone",spawnPoint,Quaternion.identity);
				ApplyOffsetVertically();
				for(int i=0; i<3; i++)
				{
					PoolManagerScript.Instance.Spawn("Hunting drone",spawnPoint,Quaternion.identity);
				}
			}
			else if(reputation == 5)
			{
				sdCount+=3;
				hdCount+=3;
				for(int i=0; i<3; i++)
				{
					PoolManagerScript.Instance.Spawn("Surveillance_Drone",spawnPoint,Quaternion.identity);
				}
				ApplyOffsetVertically();
				for(int i=0; i<3; i++)
				{
					PoolManagerScript.Instance.Spawn("Hunting drone",spawnPoint,Quaternion.identity);
				}
			}
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
		distance = Vector3.Distance(player.position,target.transform.position);
		if(distance == spawnDistance)
		{
			spawnPoint = target.transform.position;
		}
		else if (distance > spawnDistance)
		{
			spawnPoint = Vector3.Lerp(player.position,target.transform.position,CalculateRange(player.position,target.transform.position,spawnDistance));
		}
		else if(distance < spawnDistance)
		{
			for(int i = pointList.Count-2; i >= 0; i--)
			{
				prevDistance = distance;
				distance += Vector3.Distance(target.transform.position,pointList[i].transform.position);
				if(distance >= spawnDistance)
				{
					if(distance == spawnDistance)
					{
						spawnPoint = pointList[i].transform.position;
						break;
					}
					else if(distance > spawnDistance)
					{
						distance = spawnDistance - prevDistance;
						spawnPoint = Vector3.Lerp(target.transform.position,pointList[i].transform.position,CalculateRange(target.transform.position,pointList[i].transform.position,distance));
						break;
					}
				}
				else
				{
					target = pointList[i];
				}
			}

		}
	}
}
