using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleFadeOut : MonoBehaviour
{
    private RectTransform rectTrans;
    public Sprite fullBlackIMG;
    public Sprite hugeIMG;
    public float speedOut;

    private Image img;
    public int scaleLimit;

    public float speedIn;
    // Start is called before the first frame update
    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        img.sprite = fullBlackIMG;

        enter();

    }


    internal void exit()
    {
        StartCoroutine(ExitAnim());
    }
    IEnumerator ExitAnim()
    {
        LevelSelectorManager.instance.activePlayer(false);

        yield return new WaitForSeconds(0.25f);

        img.sprite = hugeIMG;

        while (rectTrans.localScale.x > 1.5f)
        {
            rectTrans.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * speedIn;
            if(rectTrans.localScale.x < 1)
                rectTrans.localScale = new Vector3(1, 1, 1);

            yield return new WaitForEndOfFrame();
        }

       img.sprite = fullBlackIMG;
        rectTrans.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(1f);

        LevelSelectorManager.instance.changeScene();
    }


    internal void enter()
    {
        StartCoroutine("EnterAnim");
    }
    IEnumerator EnterAnim()
    {
        //  LevelSelectorManager.instance.activePlayer(false);

        
        yield return new WaitForSeconds(1f);
        img.sprite = hugeIMG;

        while (transform.localScale.x < scaleLimit)
        {
            transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * speedOut;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }
}
