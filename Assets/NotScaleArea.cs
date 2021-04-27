using UnityEngine;

public class NotScaleArea : MonoBehaviour
{
    public bool prevented;
    ScaleManager scaleManager;

    private void Start()
    {
        scaleManager = FindObjectOfType<ScaleManager>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            scaleManager.PreventFromScalingTo(0, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            scaleManager.PreventFromScalingTo(0, false);
        }
    }
}
