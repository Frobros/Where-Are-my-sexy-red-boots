using UnityEngine;

public class LoadCredits : MonoBehaviour
{
    private string sceneName = "2_credits";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            FindObjectOfType<AudioManager>().FadeOutTheme();
            FindObjectOfType<GameManager>().LoadScene(sceneName);
        }
    }
}
