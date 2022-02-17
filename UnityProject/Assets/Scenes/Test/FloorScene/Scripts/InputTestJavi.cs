using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTestJavi : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.W))
		{
			FindObjectOfType<ChangeMyZoneScript>().poblateZone();
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			FindObjectOfType<ChangeMyZoneScript>().despoblateZone();
		}
	}
}
