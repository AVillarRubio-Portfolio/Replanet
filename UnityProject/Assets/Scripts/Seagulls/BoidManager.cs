//
// File: BoidManager.cs
// Project: HalloUEM2019
// Description : GameJam Project
// Author: Original File from Sebastian Lague at https://github.com/SebLague/Boids
// Modificated by Jesús Fermín Villar Ramírez (pokoidev)


// MIT License
// © Copyright (C) 2019  pokoidev

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{

    const int threadGroupSize = 1024;

    public BoidSettings settings;
    // public ComputeShader compute;
    public Transform target1;
    public Transform target2;
    private Transform currTarg;
    public bool target1Active = true;
    List<Boid> boids = new List<Boid>();
    public GameObject boidObject;
    public int boidsNumber;
    public int spawnRadius;
    public Transform seagullsGroup;

    private Collider boxColl;

    public static BoidManager Instance { get { return instance; } }
    static BoidManager instance;


 
    public GameObject SpawnInit()
    {
       
        return Spawn(new Vector3
            (Random.Range(boxColl.bounds.min.x, boxColl.bounds.max.x), Random.Range(boxColl.bounds.min.y, boxColl.bounds.max.y),
            Random.Range(boxColl.bounds.min.z, boxColl.bounds.max.z)));

        //return Spawn(transform.position + Random.insideUnitSphere * spawnRadius);
    }
    
    public GameObject Spawn(Vector3 position)
    {
        //var rotation = Quaternion.Slerp(transform.rotation, Random.rotation, 0.3f);
        var boid = Instantiate(boidObject, new Vector3(position.x, transform.position.y, position.z), Quaternion.identity) as GameObject;
        boid.transform.parent = seagullsGroup;

        return boid;
    }
    private void Start()
    {
        boxColl = GetComponent<BoxCollider>();

        Debug.Log("Global lvl: " + Globals.maxLevel);
        if (Globals.maxLevel > 3)
            boidsNumber = 200;
        else if (Globals.maxLevel > 1)
            boidsNumber = 120;
        else boidsNumber = 0;

        if (!(boidsNumber > 0))
            return;

        for (int i = 0; i < boidsNumber; i++)
        {
            int r = Random.Range(0, 2);
            if (r == 0)
                currTarg = target1;
            else currTarg = target2;

            GameObject go = SpawnInit();
            Boid b = go.GetComponent<Boid>();

            b.Initialize(settings, currTarg);
            AddBoidToArray(b);
        }


        changeTargetPoint();
    }
    public void RemoveBoid(Boid b)
    {
        // b.velocityModifier = 0;
    }
    public void InitializeBoid(Boid b)
    {
        //b.Initialize(settings, target);
    }

    public void AddBoidToArray(Boid b)
    {
        boids.Add(b);
    }

    void Update()
    {
        if (boids != null)
        {

            int numBoids = boids.Count;
            var boidData = new BoidData[numBoids];

            for (int i = 0; i < boids.Count; i++)
            {
                boidData[i].position = boids[i].position;
                boidData[i].direction = boids[i].forward;
            }

            //var boidBuffer = new ComputeBuffer(numBoids, BoidData.Size);
            //boidBuffer.SetData(boidData);

            //compute.SetBuffer (0, "boids", boidBuffer);
            //compute.SetInt    ("numBoids", boids.Count);
            //compute.SetFloat  ("viewRadius", settings.perceptionRadius);
            //compute.SetFloat  ("avoidRadius", settings.avoidanceRadius);

           // int threadGroups = Mathf.CeilToInt(numBoids / (float)threadGroupSize);
            //compute.Dispatch (0, threadGroups, 1, 1);

           // boidBuffer.GetData(boidData);

            for (int i = 0; i < boids.Count; i++)
            {
                boids[i].avgFlockHeading = boidData[i].flockHeading;
                boids[i].centreOfFlockmates = boidData[i].flockCentre;
                boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
                boids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

                boids[i].UpdateBoid();
            }

            //boidBuffer.Release();
        }
    }

    public struct BoidData
    {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public static int Size
        {
            get
            {
                return sizeof(float) * 3 * 5 + sizeof(int);
            }
        }
    }

    public bool ContainsBoid(ref Boid b)
    {
        return boids.Contains(b);
    }


    private void changeTargetPoint()
    {
        StartCoroutine(updateTarget());
    }

    IEnumerator updateTarget()
    {
        yield return new WaitForSeconds(12);

        Debug.Log("CAMBIO");

        

        for (int i = 0; i < boidsNumber; i++)
        {

            int r = Random.Range(0, 2);
            if (r == 0)
                currTarg = target1;
            else currTarg = target2;
            boids[i].Initialize(settings, currTarg);
        }

       

        changeTargetPoint();

    }
}