using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private float duration = 0;
    [SerializeField] private Transform finalPosition;
    [SerializeField] private float blackDuration = 0;
    [SerializeField] private Image black;

    private Vector3 initialPosition;


    void Start()
    {
        initialPosition = transform.localPosition;
        StartCoroutine(ScrollRoutine());
    }

    IEnumerator ScrollRoutine()
    {
        float time = 0f;
        float percent = 0f;
        float lastTime = Time.realtimeSinceStartup;

        // Black fade ----------------------
       

        do
        {
            time += Time.realtimeSinceStartup - lastTime;
            lastTime = Time.realtimeSinceStartup;
            percent = Mathf.Clamp01(time / blackDuration);

            Color c = black.color;
            c.a = Mathf.Clamp01(1f - percent);
            black.color = c;

            yield return null;
        } while (percent < 1);


        // Credits scroll ----------------------

        time = 0f;
        percent = 0f;
        lastTime = Time.realtimeSinceStartup;

        do
        {
            time += Time.realtimeSinceStartup - lastTime;
            lastTime = Time.realtimeSinceStartup;
            percent = Mathf.Clamp01(time / duration);

            transform.localPosition = initialPosition + new Vector3(0, percent * finalPosition.localPosition.y, 0);

            yield return null;
        } while (percent < 1);

        yield return new WaitForSecondsRealtime(5f);

		// Credits finished
		time = 0f;
		percent = 0f;
		lastTime = Time.realtimeSinceStartup;

		// Black fade ----------------------


		do
		{
			time += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			percent = Mathf.Clamp01(time / blackDuration);

			Color c = black.color;
			c.a = Mathf.Clamp01(percent);
			black.color = c;

			yield return null;
		} while (percent < 1);

		yield return new WaitForSecondsRealtime(0.5f);

		SceneManager.LoadScene("Menu");
		//Debug.Log("Quitting application...");

		yield return null;
    }
}