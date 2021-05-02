using UnityEngine;

public class ScalingResistance : MonoBehaviour
{
    private Pocket pocket;
    private ScaleManager scaleManager;
    public SpriteRenderer spriteRenderer;
    
    public bool hasAborted = false;
    private Color startColor;

    private void Start()
    {
        pocket = FindObjectOfType<Pocket>();
        scaleManager = FindObjectOfType<ScaleManager>();
        startColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (hasAborted)
        {
            if (pocket.isPocketScaling())
            {
                spriteRenderer.color = Color.red;
            }
            else
            {
                spriteRenderer.color = startColor;
                hasAborted = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.isTrigger && pocket.isPocketScaling() && !pocket.isAborting && collider.tag != "Player" && collider.tag != "PreventArea")
        {
            Debug.LogWarning(transform.parent.gameObject.name + ": Sorry! Not in my house, " + collider.name);
            Abort();
        }
    }

    internal void Abort()
    {
        hasAborted = true;
        scaleManager.Abort();
    }
}