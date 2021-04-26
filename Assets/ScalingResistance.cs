using UnityEngine;

public class ScalingResistance : MonoBehaviour
{
    ScaleManager grid;

    private void Start()
    {
        grid = FindObjectOfType<ScaleManager>();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (grid.zooming)
        {
            Debug.Log("ABOOORT!!!");
            grid.Abort();
        }
    }
}
