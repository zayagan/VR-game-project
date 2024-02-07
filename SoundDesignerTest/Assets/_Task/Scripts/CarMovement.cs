using UnityEngine;

/// <summary>
/// Moves from starting point to some distance and then teleports to start again.
/// </summary>
public class CarMovement : MonoBehaviour
{
	[SerializeField] private float travelDistance = 20;
	[SerializeField] private float speed = 5;

	private Vector3 startPoint;

	private void Start()
	{
		startPoint = transform.position;
	}

	private void Update()
	{
		transform.position += transform.forward * (speed * Time.deltaTime);
		if ((transform.position - startPoint).sqrMagnitude >= travelDistance * travelDistance)
		{
			transform.position = startPoint;
		}
	}
}
