using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IncreaseVelocity: MonoBehaviour
{
	[SerializeField] private AnimationCurve velocity = AnimationCurve.EaseInOut(0, 0,  1, 10);

	private Rigidbody thisBody;

	private float time;
	
	private void Awake()
	{
		thisBody = GetComponent<Rigidbody>();
		time = 0;
	}

	private void FixedUpdate()
	{
		time += Time.fixedDeltaTime;
		var speed = velocity.Evaluate(time);
		thisBody.velocity += transform.forward * speed;
	}
}
