using UnityEngine;

public class DestroyOnInpact : MonoBehaviour
{
	[SerializeField] private float lifetime = 10f;
	[SerializeField] private GameObject spawnPrefab;

	private void Start()
	{
		Destroy(gameObject, lifetime);
	}

	private void OnCollisionEnter(Collision other)
	{
		var contact = other.contacts[0];
		var pos = contact.point;
		var rotation = Quaternion.LookRotation(contact.normal);
		Instantiate(spawnPrefab, pos, rotation);
		
		Destroy(gameObject);
	}
}
