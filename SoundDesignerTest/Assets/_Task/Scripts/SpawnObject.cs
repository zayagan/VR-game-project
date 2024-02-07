using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// On activate spawns the prefab at the point. 
/// </summary>
public class SpawnObject : Activatable
{
	[Header("Prefab")] [Tooltip("Prefab to spawn")] [SerializeField]
	private GameObject prefab;

	[Header("Spawn point")] [Tooltip("Spawn point, if null, uses current transform")] [SerializeField]
	private Transform spawnPoint;

	[Tooltip("If not null, set as new parent")] [SerializeField]
	private Transform newParent;

	[Tooltip("If the spawned object has rigidbody, set this velocity")] [SerializeField]
	private Vector3 applyBodyVelocity = Vector3.zero;

	[Tooltip("True - applies velocity in object space\nFalse - applies velocity in world space")] [SerializeField]
	private bool applyVelocityLocal = true;

	[Tooltip("Scale the spawned object")] [SerializeField]
	private ScaleType applySpawnPointScale = ScaleType.None;

	public GameObject Prefab { get => prefab; set => prefab = value; }
	public Transform SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
	public Transform NewParent => newParent;
	public Vector3 ApplyBodyVelocity { get => applyBodyVelocity; set => applyBodyVelocity = value; }
	public bool ApplyVelocityLocal => applyVelocityLocal;
	public ScaleType ApplySpawnPointScale { get => applySpawnPointScale; set => applySpawnPointScale = value; }

	private void Awake()
	{
		Assert.IsNotNull(prefab, $"{gameObject} prefab is null");
	}

	public override void Activate()
	{
		// create object
		GameObject obj;
		var point = GetFinalSpawnPoint();
		obj = Instantiate(prefab, point.position, point.rotation);

		// apply parent
		if (newParent != null)
		{
			obj.transform.SetParent(newParent, true);
		}

		// apply scale
		if (applySpawnPointScale == ScaleType.Local)
		{
			obj.transform.localScale = point.localScale;
		}
		else if (applySpawnPointScale == ScaleType.Hierarchy)
		{
			obj.transform.localScale = point.lossyScale;
		}

		// apply velocity
		if (applyBodyVelocity != Vector3.zero)
		{
			var velocity = applyVelocityLocal ? obj.transform.TransformVector(applyBodyVelocity) : applyBodyVelocity;
			obj.GetComponentInChildren<Rigidbody>().velocity = velocity;
		}
	}

	public Transform GetFinalSpawnPoint()
	{
		if (spawnPoint != null)
		{
			return spawnPoint;
		}
		return transform;
	}

	public enum ScaleType
	{
		None,
		Local,
		Hierarchy,
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		var point = GetFinalSpawnPoint();
		Gizmos.color = Color.green;
		if (applyBodyVelocity != Vector3.zero)
		{
			var velocity = applyVelocityLocal ? transform.TransformVector(applyBodyVelocity) : applyBodyVelocity;
			Gizmos.DrawLine(point.position, velocity);
		}
		else
		{
			Gizmos.DrawWireSphere(point.position, 0.05f);
		}
	}
#endif
}
