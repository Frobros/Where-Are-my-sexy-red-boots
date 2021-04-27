using UnityEngine;

public class TransitionNPC : MonoBehaviour
{
    public SpriteRenderer[] sprites;
    public int minimumLevel;
    public int maximumLevel;
    private ScaleManager scaleManager;

    private void Start()
    {
        scaleManager = FindObjectOfType<ScaleManager>();
    }

    private void Update()
    {
        int currentLevel = FindObjectOfType<ScaleManager>().currentLevel;
        // GetComponent<SpriteRenderer>().sprite = sprites[currentLevel];
    }
}
