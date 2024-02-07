using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BodyGravity : MonoBehaviour
{
	[SerializeField] private float gravityMultiplier = 1f;

	private Rigidbody thisBody;

	private void Awake()
	{
		thisBody = GetComponent<Rigidbody>();
		thisBody.useGravity = false;
	}

	private void Update()
	{
		thisBody.velocity += (Time.deltaTime * gravityMultiplier) * Physics.gravity;
	}
}
