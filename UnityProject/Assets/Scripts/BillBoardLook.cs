using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardLook : MonoBehaviour
{
    Vector3 forward;

    private void Awake()
    {
        forward = transform.forward;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}
