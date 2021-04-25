using System;
using System.Collections.Generic;
using UnityEngine;

public class GetScalables : MonoBehaviour
{
    public List<Scalable> Get() { return scalables; }
    [SerializeField]
    private List<Scalable> scalables = new List<Scalable>();
    private Scalable parentScalable;
    bool isScaling = false;

    // visual feedback
    private SpriteRenderer spriteRenderer;
    private Color start;

    private void Awake()
    {
        parentScalable = transform.parent.GetComponent<Scalable>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        start = spriteRenderer.color;
    }

    private void Update()
    {
        if (scalables.Count > 0)
        {
            spriteRenderer.color = new Color(
                start.r,
                1f,
                start.b,
                start.a
            );
        }
        else
        {
            spriteRenderer.color = start;
        }
    }

    internal void StartZooming(Transform parent)
    {
        isScaling = true;
        foreach (Scalable s in Get())
        {
            s.ChangeParent(parent);
        }
    }

    internal void EndZooming()
    {
        isScaling = false;
        foreach (Scalable s in Get())
        {
            s.ResetParent();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isScaling)
        {
            Scalable scalable = other.GetComponent<Scalable>();
            if (scalable != null && !scalable.Equals(parentScalable) && !scalables.Contains(scalable))
            {
                scalables.Add(scalable);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isScaling)
        {
            Scalable scalable = other.GetComponent<Scalable>();
            if (scalable != null && !scalable.Equals(parentScalable) && scalables.Contains(scalable))
            {
                scalables.Remove(scalable);
            }
        }
    }

}
