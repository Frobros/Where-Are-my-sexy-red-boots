﻿using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAnimate animate;
    // Scalables
    private Pocket pocket;
    public Vector3 previousPosition;
    public Vector2 moveDirection;

    public float movementSpeed;
    bool isScaling;
    // public int zoomDir;

    // Talk
    public Talk talkTo;
    private bool talking;
    public bool IsTalking { get { return talking; } }

    // Move Object
    public Movable movable;
    private bool movingMovable;
    


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pocket = GetComponentInChildren<Pocket>();
        animate = GetComponentInChildren<PlayerAnimate>();
    }

    private void Update()
    {
        if (talking && talkTo.ConversationHasEnded()) talking = false;
    }

    public void Talk()
    {
        if (!pocket.isScaling())
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
        }
    }

    public void Move(Vector2 direction)
    {
        if (!pocket.isScalingChildren())
        {
            moveDirection = (Vector2)Vector3.Normalize(direction) * movementSpeed * transform.localScale.x * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDirection);
        } else
        {
            moveDirection = Vector2.zero;
        }
    }

    internal bool isMovingMovable()
    {
        return movingMovable;
    }

    public void ScaleTo(int scaleDirection)
    {
        if (!movingMovable && !talking)
        {
            pocket.ScaleTo(scaleDirection);
        }
    }

    internal ScalingResistance GetScalingResistance()
    {
        return GetComponentInChildren<ScalingResistance>();
    }

    internal void HandleMovable()
    {
        if (!talkTo && movable)
        {
            if (!movingMovable)
            {
                movingMovable = true;
                movable.StartMoving(transform);

                movable.transform.parent = transform;
                movable.transform.localPosition = Vector2.up;
            }
            else
            {
                movingMovable = false;
                movable.StopMoving(transform);

                movable.transform.localPosition = animate.isFacingRight ? Vector2.right : Vector2.left;
                movable.transform.parent = null;
            }
        }
    }
}
