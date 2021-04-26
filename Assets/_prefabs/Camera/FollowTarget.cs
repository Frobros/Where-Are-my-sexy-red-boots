using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target2D;
    public Transform alternativeTarget;
    public float smoothDamp;

    void LateUpdate()
    {
        Transform target = alternativeTarget ? alternativeTarget : target2D;
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothDamp);
    }
}
