using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
	public float maxSize = 1;
	public float increaseSize = 1;
	public float decreaseSize = 0.1f;
	public float overFloorLevel = 0.1f;

	private void Start()
	{
		gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + overFloorLevel, transform.position.z);
	}

	IEnumerator startGrouwingSize()
	{
        while (gameObject.transform.localScale.x < maxSize)
        {
            float newSize = transform.localScale.x + (increaseSize * Time.deltaTime);
            gameObject.transform.localScale = new Vector3(newSize, transform.localScale.y, newSize);
			yield return null;
		}

	    gameObject.transform.localScale = new Vector3(maxSize, transform.localScale.y, maxSize);

        if (GetComponent<lastFlowerBehaviour>())
			GetComponent<lastFlowerBehaviour>().increase();
	}

	IEnumerator startDecreasingSize()
	{
		while (gameObject.transform.localScale.x > 0)
		{
		    float newSize = transform.localScale.x - (decreaseSize * Time.deltaTime);
		    gameObject.transform.localScale = new Vector3(newSize, transform.localScale.y, newSize);
            yield return null;
		}

	    gameObject.transform.localScale = new Vector3(0, transform.localScale.y, 0);

		if (GetComponent<lastFlowerBehaviour>())
			GetComponent<lastFlowerBehaviour>().decrease();
	}

	public void startGrowAnimation()
	{
		StopAllCoroutines();
		StartCoroutine(startGrouwingSize());
	}

	public void startDecreaseAnimation()
	{
		StopAllCoroutines();
		StartCoroutine(startDecreasingSize());
	}
}
