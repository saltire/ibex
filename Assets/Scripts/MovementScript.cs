using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour {
	public Transform frontFeetMark;
	public Transform backFeetMark;

	public Transform[] frontFeet;
	public Transform[] backFeet;

	public float speed = 0.05f;
	public float stepLength = 0.25f;

	Vector2 inputVelocity;

	public void OnMove(InputValue value) {
		inputVelocity = value.Get<Vector2>();
	}

	void Update() {
		transform.position += Vector3.right * inputVelocity.x * speed;

		frontFeet[0].position = frontFeetMark.position + Vector3.right *
			(-stepLength / 2 + Mathf.PingPong(transform.position.x, stepLength));
		frontFeet[1].position = frontFeetMark.position + Vector3.right *
			(stepLength / 2 - Mathf.PingPong(transform.position.x, stepLength));
		backFeet[0].position = backFeetMark.position + Vector3.right *
			(stepLength / 2 - Mathf.PingPong(transform.position.x, stepLength));
		backFeet[1].position = backFeetMark.position + Vector3.right *
			(-stepLength / 2 + Mathf.PingPong(transform.position.x, stepLength));
	}
}
