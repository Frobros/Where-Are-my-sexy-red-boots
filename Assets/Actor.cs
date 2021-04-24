using UnityEngine;

public abstract class Actor : MonoBehaviour
{
	public abstract void Move(Vector2 vector2);
	public abstract void Zoom(int zoomFactor);
	public abstract void Attack(Vector2 vector2);
}
