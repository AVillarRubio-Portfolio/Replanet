using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float launchSpeed;
    [SerializeField] private float landSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float finalRotSpeed;
    [SerializeField] private float accFactor;
    [SerializeField] private float flightSpeed;
    [SerializeField] private float redFactor;
    [Range(0f, 1f)] [SerializeField] private float buildUpSpeed;

    private float minInput;
    private float buildUpRot;

    private Vector2 movementInput;
    private Vector2 newMovementInput;
    private Vector3 forward;
    private Vector3 right;

    private Rigidbody rb;

    private MenuPlayerController playerController;
    private MenuInputHandler playerInput;

    [SerializeField] private bool moveIsometric;
    
    private float originalY;
    private float offsetY = 0;
    private float timeAccumulated = 0;

    private bool active = false;
    private bool launching = false;
    private bool landing = false;    

    public GameObject mesh;

    void Awake()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<MenuInputHandler>();
        playerController = GetComponent<MenuPlayerController>();
        minInput = playerController.GetMinInput();
        buildUpRot = playerController.GetBuildUpRot();

        originalY = transform.position.y;        
    }

    private void Start()
    {
        move();
    }

    void FixedUpdate()
    {
        if (active)
        {
            move();
        }
    }

    void move()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        newMovementInput = playerInput.movementInput;
        if (newMovementInput.magnitude < minInput)
            newMovementInput = Vector2.zero;

        float realBuildUpSpeed = 1f - Mathf.Pow(1f - buildUpSpeed, Time.deltaTime * 60);
        movementInput = Vector2.Lerp(movementInput, newMovementInput, realBuildUpSpeed);

        // Movement ---------------------------------------------------
        Vector3 hMovement;
        Vector3 vMovement;

        if (moveIsometric)
        {
            hMovement = Vector3.right * movementInput.x;
            vMovement = Vector3.forward * movementInput.y;
        }
        else
        {
            hMovement = Quaternion.Euler(new Vector3(0, -15, 0)) * right * movementInput.x;
            vMovement = Quaternion.Euler(new Vector3(0, -15, 0)) * forward * movementInput.y;
        }

        Vector3 finalMovement = Vector3.ClampMagnitude((hMovement + vMovement), 1.0f) * speed * Time.deltaTime;

        transform.position += finalMovement;

        timeAccumulated += Time.deltaTime * flightSpeed;
        //transform.position = new Vector3(transform.position.x, originalY + Mathf.Cos(timeAccumulated) / redFactor, transform.position.z);
        mesh.transform.position = new Vector3(transform.position.x, originalY + Mathf.Cos(timeAccumulated) / redFactor, transform.position.z);

        // Rotation ---------------------------------------------------
        if (newMovementInput.magnitude > minInput)
        {
            Vector3 heading = Vector3.Normalize(hMovement + vMovement);
            Vector3 finalHeading = Vector3.Lerp(transform.forward, heading, buildUpRot);
            float angleDiff = Vector3.Angle(heading, transform.forward);

            transform.forward = (angleDiff < 160) ? finalHeading : heading;
        }
    }

    public void setActive(bool v)
    {
        active = v;
    }

    public void launch()
    {
        if (!launching)
        {
            launching = true;
            StartCoroutine("launchRoutine");
        }
    }

    public void land()
    {
        if (!landing)
        {
            landing = true;
            Debug.Log("Land");
            StartCoroutine("landRoutine");
        }
    }

    IEnumerator launchRoutine()
    {
        float dist = 0;

        while (dist < 7.5f)
        {
            dist += Time.deltaTime * launchSpeed;

            transform.position -= new Vector3(0, Time.deltaTime * launchSpeed);
            transform.Rotate(new Vector3(0, 1, 0), rotSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();

        }

        yield return new WaitForSeconds(0.1f);
        dist = 0;

        while (dist < 200)
        {
            dist += Time.deltaTime * launchSpeed;

            transform.position += new Vector3(0, Time.deltaTime * launchSpeed * 5);
            launchSpeed += Time.deltaTime * accFactor;

            yield return new WaitForEndOfFrame();

        }
    }

    public void LastLaunch()
    {
        StartCoroutine("launchRoutine");        
    }

    IEnumerator landRoutine()
    {
        float dist = 0;

        while (dist < 10)
        {
            dist += Time.deltaTime * landSpeed;

            transform.position -= new Vector3(Time.deltaTime * landSpeed, 0, Time.deltaTime * landSpeed);

            yield return new WaitForEndOfFrame();

        }

        yield return new WaitForSeconds(0.1f);
        dist = 0;

        while (dist < 10)
        {
            dist += Time.deltaTime * launchSpeed;

            transform.position -= new Vector3(0, Time.deltaTime * landSpeed);
            launchSpeed -= Time.deltaTime * accFactor * 2;

            yield return new WaitForEndOfFrame();
        }

        LevelSelectorManager.instance.sceneOut();
    }

}
