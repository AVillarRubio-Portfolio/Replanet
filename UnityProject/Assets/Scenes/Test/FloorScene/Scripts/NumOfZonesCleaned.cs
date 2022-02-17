using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumOfZonesCleaned : MonoBehaviour
{
	private int numOfZonesCleaned = 0;
	public int totalNumOfZonesCleaned = 0;

	public void checkIfLevelCompleted()
	{
		if (numOfZonesCleaned + 1 >= totalNumOfZonesCleaned && totalNumOfZonesCleaned > 1)
		{
			ChangeMyZoneScript[] cm = FindObjectsOfType<ChangeMyZoneScript>();

			foreach (ChangeMyZoneScript c in cm)
			{
				c.poblateZone();
			}

			DestroyImmediate(this);
		}
	}

	public void increaseNumberOfZonesCleaned(GameObject obj)
	{
		checkIfLevelCompleted();

		numOfZonesCleaned++;

        if (numOfZonesCleaned < totalNumOfZonesCleaned && obj.GetComponent<ChangeMyZoneScript>())
		{
            obj.GetComponent<ChangeMyZoneScript>().despoblateZone();
		}
	}

	public void decreaseNumberOfZonesCleaned()
	{
		numOfZonesCleaned--;
	}
}
