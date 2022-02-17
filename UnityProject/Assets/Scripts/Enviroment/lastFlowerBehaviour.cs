using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lastFlowerBehaviour : MonoBehaviour
{
	public GameObject myFather;

	public void increase()
	{
		if (FindObjectOfType<NumOfZonesCleaned>())
			FindObjectOfType<NumOfZonesCleaned>().increaseNumberOfZonesCleaned(myFather);

	}

	public void decrease()
	{
		if (FindObjectOfType<NumOfZonesCleaned>())
			FindObjectOfType<NumOfZonesCleaned>().decreaseNumberOfZonesCleaned();
	}
}
