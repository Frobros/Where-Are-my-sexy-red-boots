using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public Vector2 moveDirection;
    public Vector3 previousPosition;

    Rigidbody2D rb;

    // Talk
    public Talk talkTo;
    private bool talking;

    public Movable movable;
    private bool movingMovable;
    
    // Scalables
    private Pocket pocket;

    public int zoomDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pocket = GetComponentInChildren<Pocket>();
    }

    private void Update()
    {
        if (talking && talkTo.ConversationHasEnded()) talking = false;
        
    }

    public void Talk()
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

    public void Move(Vector2 direction)
    {
        if (!pocket.isPocketScalingChildren())
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
            pocket.ScaleTo(scaleDirection);
    }

    internal ScalingResistance GetScalingResistance()
    {
        return GetComponentInChildren<ScalingResistance>();
    }

    internal void MoveMovable()
    {
        if (!talkTo && movable)
        {
            if (!movingMovable)
            {
                movingMovable = true;
                movable.StartMoving(transform);
            }
        }
    }

    internal void LeaveMovable()
    {
        if (movingMovable)
        {
            movingMovable = false;
            movable.StopMoving(transform);
            movable = null;
        }
    }
}
