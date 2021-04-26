using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float movementSpeed;
    Rigidbody2D rb;

    // Zoom Levels
    ScaleManager grid;

    // Talk
    public Talk talkTo;
    private bool talking;
    
    // Scalables
    public GetScalables scalables;
    public bool scaling;
    public int zoomDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<ScaleManager>();
        scalables = GetComponentInChildren<GetScalables>();
    }

    private void Update()
    {
        if (talking && talkTo.ConversationHasEnded()) talking = false;
        
        // SCALABLES
        if (!grid.zooming && scaling)
        {
            scalables.EndZooming();
            scaling = false;
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
        if (!scaling)
        {
            Vector2 moveDirection = rb.position + (Vector2)Vector3.Normalize(direction) * movementSpeed * transform.localScale.x * Time.fixedDeltaTime;
            rb.MovePosition(moveDirection);
        }
    }

    public void Zoom(int zoomDirection)
    { 
        if (!grid.zooming)
        {
            scaling = true;
            grid.ActivateLevel(zoomDirection);
            scalables.StartZooming(transform, zoomDirection);
        }
    }

}
