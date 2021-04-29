using UnityEngine;

public class NotScaleArea : MonoBehaviour
{
    int preventLevel = 0;
    public bool prevented;
    ScaleManager scaleManager;

    private void Start()
    {
        scaleManager = FindObjectOfType<ScaleManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            scaleManager.PreventFromScalingTo(preventLevel, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            scaleManager.PreventFromScalingTo(preventLevel, false);
        }
    }
}
