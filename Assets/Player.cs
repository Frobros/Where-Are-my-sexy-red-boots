using UnityEngine;

public class Player : Actor
{

    public float movementSpeed;
    
    private float tanga;

    Rigidbody2D rb;

    TileGridManager grid;

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<TileGridManager>();

        // standard scale divided standard camera distance
        tanga = 1f / 10f;
    }
    public override void Attack(Vector2 vector2)
    {
        Debug.Log("ATTACK!!!");
    }

    public override void Move(Vector2 direction)
    {
        
        rb.MovePosition(rb.position + (Vector2) Vector3.Normalize(direction) * movementSpeed * transform.localScale.x * Time.fixedDeltaTime);
    }

    public override void Zoom(int zoomDirection)
    { 
        if (!grid.zooming)
        {
            Debug.Log("ZOOM!!!");
            grid.ActivateLevel(zoomDirection);
        }
    }
}
