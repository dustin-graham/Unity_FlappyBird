using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bird : MonoBehaviour {

	public System.Action OnBirdCollision;
	public System.Action OnBirdScore;

	public float upForce = 100f;

	public LayerMask hazardLayerMask;
	private int pipeGateLayer;

	private bool touched = false;

	private Animator _animator;

	void OnEnable() {
		GameManager.OnGameStart += OnGameStart;
		GameManager.OnGameStop += OnGameStop;
	}

	void OnDisable() {
		GameManager.OnGameStart -= OnGameStart;
		GameManager.OnGameStop -= OnGameStop;
	}

	private void OnGameStart() {
		GetComponent<Rigidbody>().isKinematic = false;
	}

	private void OnGameStop() {
		GetComponent<Rigidbody>().isKinematic = true;
	}
	void Start() {
		pipeGateLayer = LayerMask.NameToLayer("PipeGate");
		_animator = GetComponent<Animator> ();
	}

	void Update() {
		touched = Input.anyKeyDown;
	}

	void FixedUpdate() {
		if (touched) {
			//apply upForce
			//...zero out the birds current y velocity before...
			GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			GetComponent<Rigidbody>().AddForce(Vector3.up * upForce);
		}
		_animator.SetFloat("verticalVelocity",GetComponent<Rigidbody>().velocity.y);
	}

	void OnCollisionEnter(Collision collision) {
		Collider other = collision.collider;
		if ((1<<other.gameObject.layer & hazardLayerMask.value) == 1<<other.gameObject.layer && OnBirdCollision != null) {
			Debug.Log("on collision!");
			OnBirdCollision();
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == pipeGateLayer && OnBirdScore != null) {
			OnBirdScore();
		}
	}
}
