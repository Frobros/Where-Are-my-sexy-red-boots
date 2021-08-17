using System;
using UnityEngine;

public class Movable : MonoBehaviour
{
    private Rigidbody2D rb;
    public float smoothDamping = 0f;
    public bool startActive = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Activate(startActive);
    }

    void Update()
    {
        float damping = Damp(smoothDamping);
        rb.velocity = rb.velocity * damping;
    }

    internal void Activate(bool active)
    {
        rb.isKinematic = !active;
    }

    private float Damp(float dampingFactor)
    {
        return Mathf.Pow(1f - dampingFactor, Time.deltaTime);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.transform.GetComponent<PlayerController>();
        if (player)
        {
            player.movable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.transform.GetComponent<PlayerController>();
        if (player && !player.isMovingMovable())
        {
            player.movable = null;
        }
    }

    internal void StartMoving(Transform player)
    {
        rb.isKinematic = true;
        IgnoreCollisionsWith(player, true);
    }


    internal void StopMoving(Transform player)
    {
        rb.isKinematic = false;
        IgnoreCollisionsWith(player, false);
    }


    private void IgnoreCollisionsWith(Transform player, bool ignore)
    {
        Collider2D[] colliders = Array.FindAll<Collider2D>(GetComponentsInChildren<Collider2D>(), collider => {
            return collider.enabled;
        });
        Array.ForEach(colliders, collider =>
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), collider, ignore);
        });
    }
}
