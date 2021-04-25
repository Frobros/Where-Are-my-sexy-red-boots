﻿using UnityEngine;

public class Player : Scalable
{

    public float movementSpeed;
    Rigidbody2D rb;

    TileGridManager grid;
    
    // Talk
    public Talk talkTo;
    private bool talking;

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<TileGridManager>();
    }

    private void Update()
    {
        if (talking && talkTo.ConversationHasEnded()) talking = false;
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
        Vector2 moveDirection = rb.position + (Vector2)Vector3.Normalize(direction) * movementSpeed * transform.localScale.x * Time.fixedDeltaTime;
        rb.MovePosition(moveDirection);
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