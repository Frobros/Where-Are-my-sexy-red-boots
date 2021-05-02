using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampingObjectSpeed : MonoBehaviour
{
    public float smoothDamping = 0f;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        float damping = Damp(smoothDamping);
        rb.velocity = rb.velocity * damping;
    }


    private float Damp(float dampingFactor)
    {
        return Mathf.Pow(1f - dampingFactor, Time.deltaTime);
    }
}
