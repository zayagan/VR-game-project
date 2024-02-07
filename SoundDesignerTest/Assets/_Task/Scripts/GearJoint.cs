using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Joins two objects with as gears. Both gears must have a hinge joint.
/// </summary>
public class GearJoint : MonoBehaviour
{
	[SerializeField] private Rigidbody gear1;

	[SerializeField] private Rigidbody gear2;

	// 2 means gear2 is turning 2x faster than gear 1, negative reverses direction
	[SerializeField] private float ratio = 1f;
	[SerializeField] private bool enableCollision;

	private HingeJoint hingeJoint1;
	private HingeJoint hingeJoint2;
	private Rigidbody rigidbody1;
	private Rigidbody rigidbody2;
	private bool hasRigidbodies;

	private void Awake()
	{
		hingeJoint1 = gear1.GetComponent<HingeJoint>();
		hingeJoint2 = gear2.GetComponent<HingeJoint>();
		Assert.IsNotNull(hingeJoint1, $"{gameObject} - {gear1} has no hinge joint");
		Assert.IsNotNull(hingeJoint2, $"{gameObject} - {gear2} has no hinge joint");
		rigidbody1 = gear1.GetComponent<Rigidbody>();
		rigidbody2 = gear2.GetComponent<Rigidbody>();
		hasRigidbodies = rigidbody1 != null && rigidbody2 != null;

		if (!enableCollision)
		{
			Physics.IgnoreCollision(gear1.GetComponentInChildren<Collider>(), gear2.GetComponentInChildren<Collider>());
		}
	}

	private void FixedUpdate()
	{
		var t = GetRatio();

		// compute velocity
		var velocity1 = hingeJoint1.velocity;
		var velocity2 = hingeJoint2.velocity;
		var velocity = Mathf.Lerp(velocity1 * ratio, velocity2, t);
		velocity1 = velocity / ratio;
		velocity2 = velocity;
		// TODO this might not work properly if the axis is not in the center
		var axis1 = GetWorldAxis(gear1.transform, hingeJoint1.axis);
		var axis2 = GetWorldAxis(gear2.transform, hingeJoint2.axis);
		gear1.angularVelocity = axis1 * (velocity1 * Mathf.Deg2Rad);
		gear2.angularVelocity = axis2 * (velocity2 * Mathf.Deg2Rad);
	}

	private float GetRatio()
	{
		if (hasRigidbodies)
		{
			if (rigidbody1.isKinematic && !rigidbody2.isKinematic)
			{
				return 0;
			}
			if (!rigidbody1.isKinematic && rigidbody2.isKinematic)
			{
				return 1;
			}
		}
		return gear2.mass / (gear1.mass + gear2.mass);
	}

	private Vector3 GetWorldAxis(Transform parent, Vector3 axis)
	{
		return parent.rotation * axis;
	}
}
