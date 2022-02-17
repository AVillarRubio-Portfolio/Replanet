using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLetters : MonoBehaviour
{
    public GameObject[] _re;
    public GameObject[] _pair;
    public ParticleSystem[] _particleSystems;
    private int _pairIndex;
    public GameObject[] _planet;
    private int _planetIndex;

    public float _timeBtwLetterDestroy;
    public float _timeBtwLetterCreate;


    // Start is called before the first frame update
    void Start()
    {
        _pairIndex = _pair.Length;
        _planetIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartAnimation(true);
        }
    }

    private void DestroyLetter()
    {
        _particleSystems[_pairIndex - 1].Play();
        Destroy(_pair[_pairIndex - 1].gameObject);
        _pairIndex--;

        if (_pairIndex == 0)
        {
            Invoke("CreateLetter", _timeBtwLetterCreate);
        }
        else
        {
            Invoke("DestroyLetter", _timeBtwLetterDestroy);
        }
    }

    private void CreateLetter()
    {
        _planet[_planetIndex].SetActive(true);
        _planetIndex++;

        if (_planetIndex != _planet.Length)
        {
            Invoke("CreateLetter", _timeBtwLetterCreate);
        }
    }

    public void StartAnimation(bool b)
    {
        Invoke("DestroyLetter", _timeBtwLetterDestroy);
    }
}
