using UnityEngine;

public class DeactivateOverlayOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private GameObject overlay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.isTrigger)
        {
            overlay.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.isTrigger)
        {
            overlay.SetActive(true);
        }
    }
}
