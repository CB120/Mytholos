using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAbility : Ability
{
	[Tooltip("Position we want to hit")]
	public Vector3 targetPos;

	[Tooltip("Horizontal speed, in units/sec")]
	public float speed = 10;

	[Tooltip("How high the arc should be, in units")]
	public float arcHeight = 1;

	Vector3 startPos;

	void Start()
	{
		startPos = transform.position;
	}

	void Update()
	{
		// Compute the next position, with arc added in
		float x0 = startPos.x;
		float x1 = targetPos.x;
		float dist = x1 - x0;
		float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
		float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
		float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
		Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

		// Rotate to face the next position, and then move there
		transform.rotation = LookAt2D(nextPos - transform.position);
		transform.position = nextPos;

		// Do something when we reach the target
		if (nextPos == targetPos) Arrived();
	}

	void Arrived()
	{
		Destroy(gameObject);
	}

	static Quaternion LookAt2D(Vector2 forward)
	{
		return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
	}
}
