using UnityEngine;

public class MouseProtocol : MonoBehaviour
{
    private Talk[] talks;
    private int protocol = 0;

    void Start()
    {
        talks = GetComponentsInChildren<Talk>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Cheese")
        {
            Scalable scalable = collision.GetComponentInParent<Scalable>();
            if (scalable.GetScalingFactor() >= 0) protocol = 1;
            else protocol = 2;
            
            foreach (Talk talk in talks)
            {
                talk.conversationId = talk.conversationId.Remove(talk.conversationId.Length - 1, 1) + protocol;
            }
        }
    }
}
