using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private AudioManager audioManager;
    private KeyboardHandler keyboard;
    bool isLoading = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(instance);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string name)
    {
        if (!isLoading)
        {
            isLoading = true;
            SceneManager.LoadScene(name);
        }
    }


    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        isLoading = false;
        audioManager = GetComponentInChildren<AudioManager>();
        keyboard = GetComponent<KeyboardHandler>();
        audioManager.OnLevelFinishedLoading(scene);
        keyboard.OnLevelFinishedLoading(scene);
    }

    // The following Methods handle the event of a freshly loaded scene
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
}
