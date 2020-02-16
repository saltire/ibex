using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour {
	public Transform[] frontFeet;
	public Transform[] backFeet;
	public Transform frontFeetMark;
	public Transform backFeetMark;
	public Collider2D ground;

	public float speed = 0.05f;
	public float feetDistance = 1f;
	public float stepLength = 0.25f;

	Vector2 inputVelocity;

	public void OnMove(InputValue value) {
		inputVelocity = value.Get<Vector2>();
	}

	void Update() {
		RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x, 100, 0), Vector2.down);
		transform.position = hit.point;

		Vector3 angledRight = -Vector2.Perpendicular(hit.normal);
		transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, angledRight));

		frontFeetMark.position = ground.ClosestPoint(
			transform.position + angledRight * feetDistance / 2);
		backFeetMark.position = ground.ClosestPoint(
			transform.position - angledRight * feetDistance / 2);

		transform.position += angledRight * inputVelocity.x * speed;

		PlaceFoot(frontFeet[0], frontFeetMark, false);
		PlaceFoot(frontFeet[1], frontFeetMark, true);
		PlaceFoot(backFeet[0], backFeetMark, false);
		PlaceFoot(backFeet[1], backFeetMark, true);
	}

	void PlaceFoot(Transform foot, Transform midpoint, bool altFoot) {
		float x = midpoint.position.x +
			(Mathf.PingPong(transform.position.x, stepLength) - stepLength / 2) * (altFoot ? -1 : 1);

		RaycastHit2D hit = Physics2D.Raycast(new Vector3(x, 100, 0), Vector2.down);
		float y = hit.point.y;

		foot.position = new Vector3(x, y, 0);
	}
}
