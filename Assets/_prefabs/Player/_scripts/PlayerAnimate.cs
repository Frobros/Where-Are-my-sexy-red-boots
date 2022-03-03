using System;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour
{
    private PlayerController player;
    private Animator animator;
    public bool isFacingRight = true;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("velocity", Mathf.Abs(player.moveDirection.magnitude));

        Flip();
    }

    private void Flip()
    {
        if (isFacingRight && player.moveDirection.x < 0)
        {
            isFacingRight = false;
            
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else if (!isFacingRight && player.moveDirection.x > 0)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }
}
