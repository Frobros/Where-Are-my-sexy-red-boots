using UnityEngine;

public class Player : MonoBehaviour
{

    public float movementSpeed;
    Rigidbody2D rb;

    // Zoom Levels
    TileGridManager grid;
    
    // Talk
    public Talk talkTo;
    private bool talking;
    
    // Scalables
    public GetScalables scalables;
    public bool scaleChildren;

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<TileGridManager>();
        scalables = GetComponentInChildren<GetScalables>();
    }

    private void Update()
    {
        if (talking && talkTo.ConversationHasEnded()) talking = false;
        
        // SCALABLES
        if (!grid.zooming && scaleChildren)
        {
            scalables.EndZooming();
            scaleChildren = false;
        }
    }

    public void Action(Vector2 vector2)
    {
        if (talkTo && !talking)
        {
            talking = true;
            talkTo.StartConversation();
            Debug.Log("TALK!!!");
        }
        else if (talking) 
        {
            talkTo.UpdateConversation();
        }
        else Debug.Log("ATTACK!!!");
    }

    public void Move(Vector2 direction)
    {
        if (!scaleChildren)
        {
            Vector2 moveDirection = rb.position + (Vector2)Vector3.Normalize(direction) * movementSpeed * transform.localScale.x * Time.fixedDeltaTime;
            rb.MovePosition(moveDirection);
        }
    }

    public void Zoom(int zoomDirection)
    { 
        if (!grid.zooming)
        {
            grid.ActivateLevel(zoomDirection);

            scaleChildren = true;
            scalables.StartZooming(transform, zoomDirection);
        }
    }
}
