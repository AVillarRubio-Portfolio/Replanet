using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxStarSpawner : MonoBehaviour
{

    public Transform _leftTop;
    public Transform _leftBottom;

    public Transform _rightTop;
    public Transform _rightBottom;

    public int _initialNumOfStars;

    public float _spawnerTimeMin;
    public float _spawnerTimeMax;

    public float _objVelocity;

    public GameObject _starGameObject;

    public float _scale;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", 0f);

        for(int i = 0; i < _initialNumOfStars; i++)
        {
            float x = Random.Range(_leftBottom.position.x, _rightBottom.position.x);
            float y = Random.Range(_leftBottom.position.y, _leftTop.position.y);

            Vector3 pos = new Vector3(x, y, 40);
            GameObject go = Instantiate(_starGameObject);
            go.transform.position = pos;
            go.transform.localScale *= _scale;
            go.GetComponent<MainMenuPlanetMovement>().velocity = _objVelocity;
            go.transform.parent = transform;
        }
    }

    private void Spawn()
    {
        float randomY = Random.Range(_leftBottom.position.y, _leftTop.position.y);

        Vector3 pos = new Vector3(_leftBottom.position.x, randomY, 40);
        GameObject go = Instantiate(_starGameObject);
        go.transform.position = pos;
        go.transform.localScale *= _scale;
        go.GetComponent<MainMenuPlanetMovement>().velocity = _objVelocity;
        go.transform.parent = transform;

        float spawnT = Random.Range(_spawnerTimeMin, _spawnerTimeMax);
        Invoke("Spawn", spawnT);
    }
}
