using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float smoothSpeed = 0.125f;
    public Vector3 offset;

    private bool looking = true;
    private bool active = true;

    private MenuPlayerController playerController;

    private void Awake()
    {
        playerController = target.GetComponent<MenuPlayerController>();
    }

    private void FixedUpdate()
    {
        if (active)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;

            if (looking)
                transform.LookAt(target);
        }
    }

    public void setSpeed(float v)
    {
        smoothSpeed = v;
    }

    public float getSpeed()
    {
        return smoothSpeed;
    }

    public void setTarget(Transform t, bool look = true)
    {
        target = t;
        looking = look;
    }

    public void setLooking(bool look = true)
    {
        looking = look;
    }

    public void setActive(bool v)
    {
        active = v;
    }

}
