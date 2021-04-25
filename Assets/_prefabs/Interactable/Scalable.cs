using UnityEngine;

public class Scalable : MonoBehaviour
{
    public Transform originalParent;

    private void Awake()
    {
        originalParent = transform.parent;
    }

    internal void ChangeParent(Transform parent)
    {
        transform.SetParent(parent);
        Debug.Log("Setting up parent!");
    }

    internal void ResetParent()
    {
        Debug.Log("Resetting parent!");
        if (originalParent == null)
        {
            transform.parent = null;
        }
        else transform.SetParent(originalParent);
    }
}
