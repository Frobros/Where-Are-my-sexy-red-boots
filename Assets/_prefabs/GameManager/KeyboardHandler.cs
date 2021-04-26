using System.Collections.Generic;
using UnityEngine;

public class KeyboardHandler : MonoBehaviour
{
    Player player;
    private Vector2 direction;
    public float mouseSensitivity;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        HandleInputs();
    }

    void FixedUpdate()
    {
        // MOVEMENT (1)
        if (direction != Vector2.zero)
            player.Move(direction);
    }

    private void HandleInputs()
    {
        // MOVEMENT (0)
        direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            direction += Vector2.up;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            direction += Vector2.down;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            direction += Vector2.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            direction += Vector2.right;
        // Move in Method FixedUpdate()

        // ATTACK
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftControl))
            player.Action(direction);

        // ZOOM
        int zoomDirection = 0;
        
        float mouseDirection = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(KeyCode.PageUp)
            || Input.GetAxis("Mouse ScrollWheel") > 0
            || Input.GetMouseButton(1) && mouseDirection > mouseSensitivity
        )
            zoomDirection += 1;
        if (Input.GetKeyDown(KeyCode.PageDown)
            || Input.GetAxis("Mouse ScrollWheel") < 0f
            || Input.GetMouseButton(1) && mouseDirection < -mouseSensitivity
        )
            zoomDirection -= 1;
        if (zoomDirection != 0)
            player.Zoom(zoomDirection);

        // RELOAD
        if (Input.GetKeyDown(KeyCode.R))
            FindObjectOfType<GameManager>().ReloadScene();
    }
}