using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Destroys when particles or sound completed. 
/// </summary>
public class DestroyParticleSoundOnComplete : MonoBehaviour
{
	[Tooltip("If > 0, will override duration")]
	[SerializeField] private float overrideDuration = -1;
	
	private ParticleSystem[] particles;
	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		particles = GetComponentsInChildren<ParticleSystem>();
		Assert.IsTrue(particles.Length > 0, $"{gameObject} no particle system in children");
		Assert.IsTrue(audioSource, $"{gameObject} audioSource is null");
		audioSource.loop = false;
	}

	protected void Start()
	{
		var duration = GetMaxDuration();
		Destroy(gameObject, duration);
	}

	protected float GetMaxDuration()
	{
		if (overrideDuration > 0)
		{
			return overrideDuration;
		}
		return Mathf.Max(particles.Select(particle => particle.main.duration).Prepend(0).Max(),
			audioSource.clip.length);
	}
}
