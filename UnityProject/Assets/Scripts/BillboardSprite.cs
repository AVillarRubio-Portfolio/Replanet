using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    [SerializeField] private float distance = 0.5f;
    [SerializeField] private float speed = 2f;
    private Vector3 startPos;

    void Start()
    {
        transform.rotation = Camera.main.transform.rotation;
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time * speed) * distance, 0);
    }
}
