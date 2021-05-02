using System.Collections.Generic;
using UnityEngine;

public class PreventScalingToLevel: MonoBehaviour
{
    private ScaleManager scaleManager;
    private Scalable scalable;
    public int preventLevel = 0;
    public bool prevented;
    public List<Movable> movables = new List<Movable>();

    private void Start()
    {
        scaleManager = FindObjectOfType<ScaleManager>();
        scalable = GetComponent<Scalable>();
    }

    private void Update()
    {
        if (scalable && scalable.isScaling)
        {
            foreach (Movable movable in movables)
            {
                movable.Activate(true);
                movables.Remove(movable);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<ScalingResistance>() && collider.transform.parent.GetComponent<PlayerController>())
        {
            scaleManager.AddToPreventList(preventLevel);
        }
        else if (collider.GetComponentInParent<Movable>())
        {
            movables.Add(collider.GetComponentInParent<Movable>());
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<ScalingResistance>() && collider.transform.parent.GetComponent<PlayerController>())
        {
            scaleManager.RemoveFromPreventList(preventLevel);
        }
        else if (collider.GetComponentInParent<Movable>())
        {
            movables.Remove(collider.GetComponentInParent<Movable>());
        }
    }

    public void ActivateMovables(int toLevel)
    {
        foreach (Movable movable in movables)
        {
            movable.Activate(toLevel != preventLevel);
        }
    }
}
