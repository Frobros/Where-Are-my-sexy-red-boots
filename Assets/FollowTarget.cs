using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target2D;

    void Update()
    {
        transform.position = new Vector3(target2D.position.x, target2D.position.y, transform.position.z);
    }
}
