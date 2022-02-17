using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum levelState { blocked, available, done }
public enum particles { red, green }

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int levelIndex;

    public Mesh grey;
    public Mesh red;
    public Mesh green;

    private MeshFilter meshFilter;

    private GameObject greenParticles;
    private GameObject redParticles;

    private bool locked = false;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (transform.childCount > 0)
        {
            greenParticles = transform.GetChild(0).gameObject;
            redParticles = transform.GetChild(1).gameObject;
        }
    }

    public string getLevel() {
        if(!locked)
            return "Level"+levelIndex.ToString();
        else
            return null;
    }

    public int getLevelIdx()
    {
        if (!locked)
            return levelIndex;
        else
            return -1;
    }

    public void setState(levelState l)
    {
        switch (l)
        {
            case levelState.available:
                meshFilter.mesh = red;
                locked = false;
                break;
            case levelState.done:
                meshFilter.mesh = green;
                locked = false;
                break;
            case levelState.blocked:
                meshFilter.mesh = grey;
                locked = true;
                break;
        }
    }

    public void activeParticles(particles p)
    {
        switch (p)
        {
            case particles.green:
                greenParticles.SetActive(true);
                break;
            case particles.red:
                redParticles.SetActive(true);
                break;
        }
    }
}
