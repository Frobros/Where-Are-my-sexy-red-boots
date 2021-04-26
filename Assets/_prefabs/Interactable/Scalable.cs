using UnityEngine;

public class Scalable : MonoBehaviour
{
    private Transform originalParent;
    public bool zooming = false;
    public int currentLevel;
    public int minimumLevel;
    public int maximumLevel;

    private SpriteRenderer spriteRenderer;
    private Color start;

    private void Awake()
    {
        originalParent = transform.parent;
        spriteRenderer = GetComponent<SpriteRenderer>();
        start = spriteRenderer.color;
    }

    internal void ChangeParent(Transform parent, int zoomDirection)
    {
        int newLevel = currentLevel + zoomDirection;
        if (minimumLevel <= newLevel && newLevel <= maximumLevel)
        {
            currentLevel = newLevel;
            transform.SetParent(parent);
            Debug.Log("Setting up parent!");
        } else
        {
            spriteRenderer.color = Color.red;
        }
    }

    internal void ResetParent()
    {
        Debug.Log("Resetting parent!");
        transform.SetParent(originalParent);
        spriteRenderer.color = start;
    }
}
