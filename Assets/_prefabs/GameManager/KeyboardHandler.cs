using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardHandler : MonoBehaviour
{
    Player player;
    private Vector2 direction;
    public float mouseSensitivity;
    private bool onStage;

    void Update()
    {
        if (onStage)
            HandleStageInputs();
        else
            HandleTitleInputs();
    }

    private void HandleTitleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftControl))
            StartCoroutine(FindObjectOfType<TitleProtocol>().Next());
        if (Input.GetKeyDown(KeyCode.Escape))
            FindObjectOfType<GameManager>().ReloadScene();

    }

    void FixedUpdate()
    {
        // MOVEMENT (1)
        if (player)
            player.Move(direction);
    }

    private void HandleStageInputs()
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


        if (Input.GetKeyDown(KeyCode.Escape))
            FindObjectOfType<GameManager>().LoadScene("0_title");
    }

    internal void OnLevelFinishedLoading(Scene scene)
    {
        onStage = scene.name != "0_title";
        if (onStage)
        {
            player = FindObjectOfType<Player>();
        }
    }
}