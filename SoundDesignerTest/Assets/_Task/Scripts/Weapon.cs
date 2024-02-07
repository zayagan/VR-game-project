using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] private SpawnObject primarySpawner;
	[SerializeField] private SpawnObject primaryEffect;
	[SerializeField] private SpawnObject secondarySpawner;
	[SerializeField] private SpawnObject secondaryEffect;
	[SerializeField] private SpawnObject secondaryCharge;
	[SerializeField] private float secondaryDelay = 3f;

	private bool secondaryCharging;
	private float secondaryChargeLeft;

	private void Update()
	{
		if (secondaryCharging)
		{
			secondaryChargeLeft -= Time.deltaTime;
			if (secondaryChargeLeft <= 0)
			{
				secondaryCharging = false;
				secondaryEffect.Activate();
				secondarySpawner.Activate();
			}
			return;
		}

		if (Input.GetButtonDown("Fire1"))
		{
			primaryEffect.Activate();
			primarySpawner.Activate();
		}

		if (Input.GetButtonDown("Fire2"))
		{
			secondaryCharging = true;
			secondaryChargeLeft = secondaryDelay;
			secondaryCharge.Activate();
		}
	}
}
