using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardHandler : MonoBehaviour
{
    PlayerController player;
    private Vector2 direction;
    public float mouseSensitivity;
    private bool onTitle;
    private bool onStage;
    private bool onCredits;
    private bool initialized;

    void Update()
    {
        if (onStage)
            HandleStageInputs();
        else if (onCredits)
            HandleCreditInput();
        else
            HandleTitleInputs();
    }

    private void HandleCreditInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            FindObjectOfType<GameManager>().LoadScene("0_title");
        }
    }

    private void HandleTitleInputs()
    {
        if (!initialized && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftControl)))
        {
            StartCoroutine(FindObjectOfType<TitleProtocol>().StartTitleProtocol());
            initialized = true;
        }
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
        // Actual Moving in FixedUpdate

        // Talk
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftControl))
            player.Talk();

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl))
            player.MoveMovable();

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.LeftControl))
            player.LeaveMovable();

        // ZOOM
        int zoomDirection = 0;
        
        float mouseDirection = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(KeyCode.PageUp)
            || Input.GetAxis("Mouse ScrollWheel") > 0
            || Input.GetMouseButton(1) && mouseDirection > mouseSensitivity
        )
            zoomDirection = 1;
        if (Input.GetKeyDown(KeyCode.PageDown)
            || Input.GetAxis("Mouse ScrollWheel") < 0f
            || Input.GetMouseButton(1) && mouseDirection < -mouseSensitivity
        )
            zoomDirection = -1;
        if (zoomDirection != 0)
            player.ScaleTo(zoomDirection);

        // RELOAD
        if (Input.GetKeyDown(KeyCode.R))
            FindObjectOfType<GameManager>().ReloadScene();


        if (Input.GetKeyDown(KeyCode.Escape))
            FindObjectOfType<GameManager>().LoadScene("0_title");
    }

    internal void OnLevelFinishedLoading(Scene scene)
    {
        initialized = false;
        onTitle = scene.name == "0_title";
        onStage = scene.name == "1_intro";
        onCredits = scene.name == "2_credits";
        if (onStage)
        {
            player = FindObjectOfType<PlayerController>();
        }
    }
}