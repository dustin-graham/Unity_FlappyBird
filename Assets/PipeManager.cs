using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeManager : MonoBehaviour {

	public float speed = 1f;
	public float maxHeight = 2f;
	public float minHeight = -2f;
	public float pipeDistance = 2f;
	public float initialDelaySpace = 3f;
	public float offscreenBuffer = 1f;
	public Transform pipeCarousel;
	public Transform mountainRoot;
	public GameObject pipeSetPrefab;
	public GameObject mountainPrefab;
	public float mountainDistance = 50f;
	public float mountainSpacing = 51.2f;

	private Queue<GameObject> pipes = new Queue<GameObject>();
	private Queue<GameObject> mountains = new Queue<GameObject>();

	private float _offscreenLeft;
	private Vector3 _mountainStart;
	private int _initialPipeCount = 4;
	private float pipeSpawnX;
	private float nextMountainSwapPointX = 0;
	private float nextMountainSpawnPointX = 0;
	private int mountainRotations = 1;

	private bool _gameRunning;

	void OnEnable() {
		GameManager.OnGameStart += OnGameStart;
		GameManager.OnGameStop += OnGameStop;
	}

	void OnDisable() {
		GameManager.OnGameStart -= OnGameStart;
		GameManager.OnGameStop -= OnGameStop;
	}

	void OnGameStart() {
		_gameRunning = true;
	}

	void OnGameStop() {
		_gameRunning = false;
	}

	void Start() {
		float cameraDistance = (transform.position - Camera.main.transform.position).z;
		_offscreenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0,0,cameraDistance)).x - offscreenBuffer;
		SetMountains();

		pipeSpawnX = pipeDistance * initialDelaySpace;
		for (int i = 0; i < _initialPipeCount; i++) {
			var pipeSet = Instantiate(pipeSetPrefab) as GameObject;
			pipeSet.transform.parent = pipeCarousel;
			SetNextPipeSet(pipeSet.transform,pipeSpawnX,minHeight,maxHeight,0);
			pipes.Enqueue(pipeSet);
			pipeSpawnX += pipeDistance;
		}

		for (int i = 0; i < 3; i++) {
			var mountain = Instantiate(mountainPrefab) as GameObject;
			mountain.transform.parent = mountainRoot;
			mountain.transform.localPosition = new Vector3(nextMountainSpawnPointX,0,0);
			nextMountainSpawnPointX += mountainSpacing;
			mountains.Enqueue(mountain);
		}
	}

	// Update is called once per frame
	void Update () {
		if (!_gameRunning) return;
		pipeCarousel.Translate(Vector3.left*Time.deltaTime*speed);

		if (pipes.Peek().transform.position.x < _offscreenLeft) {
			//move the pipe back
			GameObject pipeSet = pipes.Dequeue();
			pipes.Enqueue(pipeSet);
			SetNextPipeSet(pipeSet.transform,pipeSpawnX,minHeight,maxHeight,0);
			pipeSpawnX += pipeDistance;
		}

		if (mountainRoot.position.x < nextMountainSwapPointX) {
			var mountain = mountains.Dequeue();
			nextMountainSwapPointX -= mountainSpacing;
			mountain.transform.localPosition = new Vector3(nextMountainSpawnPointX,0,0);
			nextMountainSpawnPointX += mountainSpacing;
		}
	}

	private void SetNextPipeSet(Transform pipeTrans, float x, float maxY, float minY, float z) {
		float y = Random.Range(minY,maxY);
		pipeTrans.localPosition = new Vector3(x,y,z);
	}

	//responsible for placing the mountains at just the right position so they rest on the horizon
	public void SetMountains() {
		float mountainDistanceFromCamera = mountainDistance - Camera.main.transform.position.z;
		_mountainStart = Camera.main.ScreenToWorldPoint(new Vector3(0,0,mountainDistanceFromCamera));
		nextMountainSwapPointX = _mountainStart.x - mountainSpacing;
		mountainRoot.position = _mountainStart;
	}
}
