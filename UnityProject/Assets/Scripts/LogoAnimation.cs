using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoAnimation : MonoBehaviour
{
    [SerializeField] private bool animateOnStart = false;
    [SerializeField] private GameObject UfoHint;
    [SerializeField] private MainMenuUfo mainMenuUfo;
    [SerializeField] private float delay = 2f;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float timeBetween = 0.4f;

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        if (animateOnStart)
            StartCoroutine(AnimationRoutine());
    }

    IEnumerator AnimationRoutine()
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < sprites.Length-1; i++)
        {
            image.sprite = sprites[i];
            yield return new WaitForSeconds(timeBetween);
        }

        yield return new WaitForSeconds(timeBetween);

        GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.03f);
        image.sprite = sprites[sprites.Length-1];

        yield return new WaitForSeconds(0.7f);
        GetComponent<Animator>().SetTrigger("Idle");
        UfoHint.SetActive(true);
        mainMenuUfo.enabled = true;

        yield return null;
    }
}
