using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSpawner : MonoBehaviour
{
    public GameObject[] planetsToSpawn;
    public float spawnerTimeMin;
    public float spawnerTimeMax;
    public float objVelocity;

    public Transform _leftTop;
    public Transform _leftBottom;

    public Transform _rightTop;
    public Transform _rightBottom;

    public int _initialNumOfPlanets;


    private void Start()
    {
        for (int i = 0; i < _initialNumOfPlanets; i++)
        {
            int index = Random.Range(0, planetsToSpawn.Length);
            float randRotation = Random.Range(0.0f, 200.0f);

            float x = Random.Range(_leftBottom.position.x, _rightBottom.position.x);
            float y = Random.Range(_leftBottom.position.y, _leftTop.position.y);

            Vector3 pos = new Vector3(x, y, transform.position.z);
            GameObject go = Instantiate(planetsToSpawn[index], pos, Quaternion.Euler(new Vector3(randRotation, randRotation, randRotation)));
            go.transform.localScale = transform.localScale;
            go.GetComponent<MainMenuPlanetMovement>().velocity = objVelocity;
            go.transform.parent = transform;
        }

        float spawnT = Random.Range(spawnerTimeMin, spawnerTimeMax);
        Invoke("Spawn", spawnT);
    }

    private void Spawn()
    {
        int index = Random.Range(0, planetsToSpawn.Length);
        float randRotation = Random.Range(0.0f, 200.0f);

        float x = _leftBottom.position.x;
        float y = Random.Range(_leftBottom.position.y, _leftTop.position.y);

        Vector3 pos = new Vector3(x, y, transform.position.z);
        GameObject go = Instantiate(planetsToSpawn[index], pos, Quaternion.Euler(new Vector3(randRotation, randRotation, randRotation)));
        go.transform.localScale = transform.localScale;
        go.GetComponent<MainMenuPlanetMovement>().velocity = objVelocity;
        go.transform.parent = transform;

        float spawnT = Random.Range(spawnerTimeMin, spawnerTimeMax);
        Invoke("Spawn", spawnT);
    }
}
