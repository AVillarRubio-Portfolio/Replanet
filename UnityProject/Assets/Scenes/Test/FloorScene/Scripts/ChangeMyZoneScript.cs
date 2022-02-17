using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeMyZoneScript : MonoBehaviour
{
	public GameObject _myZone;
	public float _timeBetweenPoblateAnimation = 1f, _timeBetweenDespoblateAnimation = 100f;
	private List<GameObject> _tiles, _auxTiles;
	bool[] plantAlreadyCreated;
	int[] selectedFlowerInTile;
	private bool expandingZone = false;
	public GameObject _brokenTree, _treePlaceholder;
	public bool _tutorialLevel;
	public GameObject [] _flowers;

	public void Awake()
	{
		_tiles = new List<GameObject>();
		_auxTiles = new List<GameObject>();
		FillTiles();
	}

	private void Start()
	{
		if (_tutorialLevel)
		{
			poblateZone();
		}
	}

	public void poblateZone()
	{

		if (!expandingZone)
		{
			StopAllCoroutines();
			expandingZone = true;

			_tiles.Clear();

			foreach (GameObject g in _auxTiles)
				_tiles.Add(g);
			StartCoroutine(PoblateZone());
		}
	}

	public void despoblateZone()
	{
		if (!expandingZone)
		{
			StopAllCoroutines();

			_tiles.Clear();

			foreach (GameObject g in _auxTiles)
				_tiles.Add(g);

			StartCoroutine(quitZone());
		}
	}

	private void FillTiles()
	{
		foreach (Transform t in _myZone.transform.GetComponentsInChildren<Transform>())
		{
			if (t != _myZone.transform && t.GetComponent<TileBehaviour>())
			{
				_auxTiles.Add(t.gameObject);
			}
		}

		selectedFlowerInTile = new int[_auxTiles.Count];
		plantAlreadyCreated = new bool[_auxTiles.Count];

		int random = _flowers.Length + 20;

		for(int i = 0; i < selectedFlowerInTile.Length; i++)
		{
			int rnd = Random.Range(0, random);

			selectedFlowerInTile[i] = rnd;
			plantAlreadyCreated[i] = false;
		}
	}

	IEnumerator PoblateZone()
	{
		GameObject last = null;

		while(_tiles.Count > 0)
		{
			int rnd = Random.Range(0, _tiles.Count);

			GameObject g = _tiles[rnd].gameObject;

			TileBehaviour tb = g.GetComponent<TileBehaviour>();

			int index = _auxTiles.FindIndex(x => x == g);

			if (selectedFlowerInTile[index] < _flowers.Length && !plantAlreadyCreated[index])
			{
				plantAlreadyCreated[index] = true;
				GameObject f = Instantiate(_flowers[selectedFlowerInTile[index]], transform.position, Quaternion.identity);

				f.transform.parent = g.transform;
				f.transform.localPosition = new Vector3(0, 0.5f, 0);
				f.transform.localScale = new Vector3(1, 25, 1);
			}

			tb.startGrowAnimation();

			_tiles.RemoveAt(rnd);

			last = g;

			yield return new WaitForSeconds(_timeBetweenPoblateAnimation * Time.deltaTime);	
		}

		expandingZone = false;

		if (last.GetComponent<lastFlowerBehaviour>())
			Destroy(last.GetComponent<lastFlowerBehaviour>());

		last.AddComponent<lastFlowerBehaviour>();
		last.GetComponent<lastFlowerBehaviour>().myFather = gameObject;
	}

	IEnumerator quitZone()
	{
		expandingZone = false;

		GameObject last = null;

		while (_tiles.Count > 0)
		{
			int rnd = Random.Range(0, _tiles.Count);

			GameObject g = _tiles[rnd].gameObject;

			TileBehaviour tb = g.GetComponent<TileBehaviour>();

			tb.startDecreaseAnimation();

			_tiles.RemoveAt(rnd);

			yield return new WaitForSeconds(_timeBetweenDespoblateAnimation * Time.deltaTime);

			last = g;
		}

		if (last.GetComponent<lastFlowerBehaviour>())
			Destroy(last.GetComponent<lastFlowerBehaviour>());

		last.AddComponent<lastFlowerBehaviour>();
		last.GetComponent<lastFlowerBehaviour>().myFather = gameObject;

		Instantiate(_brokenTree, transform.position, Quaternion.identity);
		GameObject ga = Instantiate(_treePlaceholder, transform.position, Quaternion.identity);
		ga.SetActive(false);

		ga.transform.GetChild(1).GetComponent<ChangeMyZoneScript>()._myZone = gameObject.GetComponent<ChangeMyZoneScript>()._myZone;
		ga.transform.GetChild(1).GetComponent<ChangeMyZoneScript>()._brokenTree = gameObject.GetComponent<ChangeMyZoneScript>()._brokenTree;
		ga.transform.GetChild(1).GetComponent<ChangeMyZoneScript>()._treePlaceholder = gameObject.GetComponent<ChangeMyZoneScript>()._treePlaceholder;
		ga.transform.GetChild(1).GetComponent<ChangeMyZoneScript>()._timeBetweenDespoblateAnimation = gameObject.GetComponent<ChangeMyZoneScript>()._timeBetweenDespoblateAnimation;
		ga.transform.GetChild(1).GetComponent<ChangeMyZoneScript>()._timeBetweenPoblateAnimation = gameObject.GetComponent<ChangeMyZoneScript>()._timeBetweenPoblateAnimation;

		LevelManager.Instance.addToActionList(new LevelAction(ActionType.TreeDied));
		LevelManager.Instance.PerformAction(ActionType.TreeDied);

		LevelManager.Instance.addToActionList(new LevelAction(ActionType.PlantTree));

		UnityEvent ev = new UnityEvent();

		ev.AddListener(ga.GetComponent<Constructable>().EnableObject);

		LevelManager.Instance.addToActionList(new LevelAction(ActionType.CraftTree, ev));

		DestroyImmediate(transform.parent.gameObject);
	}
}
