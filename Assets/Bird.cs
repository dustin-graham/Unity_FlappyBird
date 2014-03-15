using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bird : MonoBehaviour {

	public System.Action OnBirdCollision;
	public System.Action OnBirdScore;

	public float upForce = 100f;

	public LayerMask hazardLayerMask;
	private int pipeGateLayer;

	void OnEnable() {
		GameManager.OnGameStart += OnGameStart;
		GameManager.OnGameStop += OnGameStop;
	}

	void OnDisable() {
		GameManager.OnGameStart -= OnGameStart;
		GameManager.OnGameStop -= OnGameStop;
	}

	private void OnGameStart() {
		rigidbody.isKinematic = false;
	}

	private void OnGameStop() {
		rigidbody.isKinematic = true;
	}

	void Start() {
		pipeGateLayer = LayerMask.NameToLayer("PipeGate");
	}

	void Update() {
		bool touched = Input.GetMouseButtonDown(0) || (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began);
		if (touched) {
			//apply upForce
			rigidbody.AddForce(Vector3.up * upForce);
		}
	}

	void OnCollisionEnter(Collision collision) {
		Collider other = collision.collider;
		if ((1<<other.gameObject.layer & hazardLayerMask.value) == 1<<other.gameObject.layer && OnBirdCollision != null) {
			OnBirdCollision();
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == pipeGateLayer && OnBirdScore != null) {
			OnBirdScore();
		}
	}
}
