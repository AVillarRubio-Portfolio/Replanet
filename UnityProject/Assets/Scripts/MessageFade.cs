using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageFade : MonoBehaviour
{
    public float speed = 5f;
    public SpriteRenderer [] spriteRenderer;
    

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine("fadeIn");
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine("fadeOut");
    }

    IEnumerator fadeIn()
    {
        Color c;

        while (spriteRenderer[0].color.a < 1)
        {
            for (int i = 0; i<spriteRenderer.Length; i++) {
                c = spriteRenderer[i].color;
                c.a += Time.deltaTime * speed;
                spriteRenderer[i].color = c;
            }
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            c = spriteRenderer[i].color;
            c.a = 1;
            spriteRenderer[i].color = c;
        }
    }

    IEnumerator fadeOut()
    {
        Color c;

        while (spriteRenderer[0].color.a > 0)
        {
            for (int i = 0; i<spriteRenderer.Length; i++)
            {
                c = spriteRenderer[i].color;
                c.a -= Time.deltaTime * speed;
                spriteRenderer[i].color = c;
            }
            
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            c = spriteRenderer[i].color;
            c.a -= Time.deltaTime * speed;
            spriteRenderer[i].color = c;
        }
    }
}
