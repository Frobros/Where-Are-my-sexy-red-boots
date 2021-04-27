using System;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private bool facingRight = true;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("velocity", Mathf.Abs(player.moveDirection.magnitude));

        Flip();
    }

    private void Flip()
    {
        if (facingRight && player.moveDirection.x < 0)
        {
            facingRight = false;
            
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else if (!facingRight && player.moveDirection.x > 0)
        {
            facingRight = true;
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }
}
