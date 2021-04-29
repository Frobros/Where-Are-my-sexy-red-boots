using UnityEngine;

public class ScalingResistance : MonoBehaviour
{
    ScaleManager grid;

    private void Start()
    {
        grid = FindObjectOfType<ScaleManager>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (grid.zooming)
        {
            Debug.Log("ABOOORT!!!");
            grid.Abort();
        }
    }
}
