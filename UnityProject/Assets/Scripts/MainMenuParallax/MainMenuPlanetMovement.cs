using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlanetMovement : MonoBehaviour
{
    float velRot;
    [HideInInspector]
    public float velocity;

    private void Start()
    {
        velRot = Random.Range(0f, 10f);
    }
    void Update()
    {
        transform.Translate(Vector3.right * velocity * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.up * -velRot * Time.deltaTime, Space.World);
    }


}
