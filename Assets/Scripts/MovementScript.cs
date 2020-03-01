using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour {
	public Transform frontLeftFoot;
	public Transform frontRightFoot;
	public Transform backLeftFoot;
	public Transform backRightFoot;
	public Collider2D ground;

	public float speed = 0.05f;
	public float feetDistance = 1f;
	public float stepLength = 0.25f;

	Vector3 slope = Vector3.right;
	Vector2 inputVelocity;
	float angleVelocity;

	public float angleTime = 0.1f;
	public float angleAdjust = 0.9f;

	public void OnMove(InputValue value) {
		inputVelocity = value.Get<Vector2>();
	}

	void Update() {
		// Move horizontally.
		transform.position += slope * inputVelocity.x * speed;

		// Get ground positions directly below player, and roughly below front and back feet.
		RaycastHit2D hitMid = Physics2D.Raycast(new Vector3(transform.position.x, 100, 0), Vector2.down);
		RaycastHit2D hitFwd = Physics2D.Raycast(
			new Vector3(transform.position.x + feetDistance / 2, 100, 0), Vector2.down);
		RaycastHit2D hitBack = Physics2D.Raycast(
			new Vector3(transform.position.x - feetDistance / 2, 100, 0), Vector2.down);

		// Get a weighted average of the slope of the ground at each of the above points.
		Vector3 slopeMid = -Vector2.Perpendicular(hitMid.normal);
		Vector3 slopeFwd = -Vector2.Perpendicular(hitFwd.normal);
		Vector3 slopeBack = -Vector2.Perpendicular(hitBack.normal);
		slope = (slopeMid + slopeMid + slopeFwd + slopeBack) / 4;

		// Move to the correct height and place the front and back feet marks using the slope.
		transform.position = hitMid.point;
		Vector3 frontFeetMark = ground.ClosestPoint(transform.position + slope * feetDistance / 2);
		Vector3 backFeetMark = ground.ClosestPoint(transform.position - slope * feetDistance / 2);

		// Smoothly rotate toward the slope angle.
		float currentAngle = transform.rotation.eulerAngles.z;
		float targetAngle = Vector2.SignedAngle(Vector2.right, slope) * angleAdjust;
		transform.rotation = Quaternion.Euler(0, 0,
			Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, angleTime));

		// Place the IK targets for each individual foot.
		frontLeftFoot.position = GetFootPosition(frontFeetMark, false);
		frontRightFoot.position = GetFootPosition(frontFeetMark, true);
		backLeftFoot.position = GetFootPosition(backFeetMark, false);
		backRightFoot.position = GetFootPosition(backFeetMark, true);
	}

	Vector3 GetFootPosition(Vector3 midpoint, bool altFoot) {
		// Ping pong the foot's x position based on the player's overall x position.
		float x = midpoint.x +
			(Mathf.PingPong(transform.position.x, stepLength) - stepLength / 2) * (altFoot ? -1 : 1);

		// Set the foot's height at ground level.
		RaycastHit2D hit = Physics2D.Raycast(new Vector3(x, 100, 0), Vector2.down);
		float y = hit.point.y;

		return new Vector3(x, y, 0);
	}
}
