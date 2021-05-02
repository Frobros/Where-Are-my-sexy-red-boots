using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform alternativeTarget;
    internal void setAlternativeTarget(Transform target) { alternativeTarget = target; }
    
    [SerializeField]
    private float smoothDamp;
    internal void setSmoothDamp(float smoothDamp) { this.smoothDamp = smoothDamp; }

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }
    void LateUpdate()
    {
        Transform target = alternativeTarget ? alternativeTarget : player;
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, (alternativeTarget ? 0.01f : 1f ) * smoothDamp);
    }

}
