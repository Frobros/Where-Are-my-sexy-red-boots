using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float movementSpeed;
    public Vector2 moveDirection;
    public Vector3 previousPosition;

    Rigidbody2D rb;

    // Zoom Levels
    ScaleManager grid;

    // Talk
    public Talk talkTo;
    private bool talking;
    
    // Scalables
    public GetScalables scalables;
    public bool scalingChildren;
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
        
        // Reset Parents of Scalables
        if (!grid.zooming && scalingChildren)
        {
            scalables.EndZooming();
            scalingChildren = false;
        }
    }

    public void Action()
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
        if (!scalingChildren)
        {
            moveDirection = (Vector2)Vector3.Normalize(direction) * movementSpeed * transform.localScale.x * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDirection);
        } else
        {
            moveDirection = Vector2.zero;
        }
    }

    public void Zoom(int zoomDirection)
    { 
        if (!grid.zooming)
        {
            grid.ActivateLevel(zoomDirection);

            scalingChildren = ;
            scalables.StartZooming(transform, zoomDirection);
        }
    }
}
