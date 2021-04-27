using UnityEngine;

public class LoadCredits : MonoBehaviour
{
    public string name;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            FindObjectOfType<GameManager>().LoadScene("0_title");
        }
    }
}
