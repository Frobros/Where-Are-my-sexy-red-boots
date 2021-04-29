using UnityEngine;

public class LoadCredits : MonoBehaviour
{
    private string sceneName = "2_credits";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            FindObjectOfType<AudioManager>().FadeOutTheme();
            FindObjectOfType<GameManager>().LoadScene(sceneName);
        }
    }
}
