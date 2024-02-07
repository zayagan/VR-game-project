using UnityEngine;

/// <summary>
/// Abstract activatable
/// </summary>
public abstract class Activatable : MonoBehaviour, IActivatable
{
	public abstract void Activate();
}
