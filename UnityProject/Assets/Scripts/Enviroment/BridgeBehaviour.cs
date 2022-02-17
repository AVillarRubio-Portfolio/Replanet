using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBehaviour : MonoBehaviour
{
	public GameObject waterTileUnderMe;

	public void youShallFinallyPass()
	{
		waterTileUnderMe.GetComponent<Collider>().enabled = false;
		transform.GetChild(0).gameObject.SetActive(true);
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<FMODUnity.StudioEventEmitter>().Play();
	}
}
