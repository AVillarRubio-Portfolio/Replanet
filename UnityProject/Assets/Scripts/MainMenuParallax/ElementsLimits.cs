using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsLimits : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MainMenuPlanetMovement>() != null)
        {
            Destroy(other.gameObject);
        }
    }
}
