using UnityEngine;

public class KeyboardHandler : MonoBehaviour
{
    public Actor currentActor;
    private Vector2 direction;

    private void Start()
    {
        currentActor = FindObjectOfType<Player>();
    }

    void Update()
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

        // ATTACK
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftControl))
            currentActor.Attack(direction);

        // ZOOM
        int zoomDirection = 0;
        if (Input.GetKeyDown(KeyCode.PageUp))
            zoomDirection += 1;
        if (Input.GetKeyDown(KeyCode.PageDown))
            zoomDirection -= 1;
        if (zoomDirection != 0)
            currentActor.Zoom(zoomDirection);

    }

    void FixedUpdate()
    {
        // MOVEMENT (1)
        if (direction != Vector2.zero)
            currentActor.Move(direction);
    }
}