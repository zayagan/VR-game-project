using UnityEngine;

/// <summary>
/// Plays sound when the object is rotating. Changes volume and pitch based on rotation speed.
/// </summary>
[RequireComponent(typeof(AudioSource), typeof(Rigidbody))]
public class RotationSound : MonoBehaviour
{
	[Tooltip("Speed limit is in deg/s")]
	[Range(0, 1)]
	[SerializeField] private float volume = 1f;
	[SerializeField] private float minSpeed = 10;
	[SerializeField] private float fullSpeed = 45;
	[SerializeField] private float pitchMinSpeed = 10;
	[SerializeField] private float pitchMaxSpeed = 180;
	[SerializeField] private float minPitch = 0.1f;
	[SerializeField] private float maxPitch = 1.5f;

	private Rigidbody body;
	private AudioSource audioSource;

	private AnimationCurve volumeBySpeed;
	private AnimationCurve pitchBySpeed;

	private const float MinVolumeToPlay = 0.01f;

	private void Awake()
	{
		body = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		audioSource.Play();
		audioSource.Pause();

		ComputeCurves();
	}

	private void FixedUpdate()
	{
		var speed = body.angularVelocity.magnitude * Mathf.Rad2Deg;
		var volume = volumeBySpeed.Evaluate(speed);
		var pitch = pitchBySpeed.Evaluate(speed);
		var play = volume >= MinVolumeToPlay;

		audioSource.volume = volume * this.volume;
		audioSource.pitch = pitch;
		if (play && !audioSource.isPlaying)
		{
			audioSource.UnPause();
		}
		else if (!play && audioSource.isPlaying)
		{
			audioSource.Pause();
		}
	}

	/// <summary>
	/// Compute volume and pitch curves based on input values.
	/// </summary>
	public void ComputeCurves()
	{
		volumeBySpeed = AnimationCurve.EaseInOut(minSpeed, 0, fullSpeed, 1);
		pitchBySpeed = AnimationCurve.EaseInOut(pitchMinSpeed, minPitch, pitchMaxSpeed, maxPitch);
	}
}
