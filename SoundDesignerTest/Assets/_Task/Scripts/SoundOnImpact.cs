using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Plays sound on impact relative to the velocity.
/// Must be placed on the object with rigidbody.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SoundOnImpact : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;

	[Tooltip("Volume multiplier")]
	[SerializeField] private float volume = 1f;

	[Tooltip("At min speed volume = 0, at full volume = 1 and does not go higher after")] 
	[SerializeField] private float minSpeed = 1;
	[SerializeField] private float fullSpeed = 5;
	[Tooltip("Minimum time before it plays again")] 
	[SerializeField] private float minDelay = 0.02f;

	private double lastPlayed;

	private void Awake()
	{
		Assert.IsNotNull(audioSource, $"{gameObject} audioSource is null");
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!enabled || !gameObject.activeInHierarchy)
		{
			return;
		}

		if (lastPlayed + minDelay > Time.timeAsDouble)
		{
			return;
		}

		var speed = collision.relativeVelocity.magnitude;
		if (speed <= minSpeed)
		{
			return;
		}

		var currVolume = Mathf.InverseLerp(minSpeed, fullSpeed, speed);
		audioSource.volume = currVolume * volume;
		audioSource.Play();

		lastPlayed = Time.timeAsDouble;
	}
}
