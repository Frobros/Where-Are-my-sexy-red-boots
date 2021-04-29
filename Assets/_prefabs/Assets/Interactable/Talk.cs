using System;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public string conversationId;
    TextImporter conversationImporter;
    TextBoxManager textBoxManager;

    // for visual feedback
    private SpriteRenderer spriteRenderer;
    private Color start;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        conversationImporter = FindObjectOfType<TextImporter>();
        textBoxManager = FindObjectOfType<TextBoxManager>();
        start = spriteRenderer.color;
    }

    internal void StartConversation()
    {
        Conversation conversation = conversationImporter.textFileToConversation(conversationId);
        textBoxManager.EnableTextBox(conversation, transform);

    }

    internal void UpdateConversation()
    {
        textBoxManager.UpdateConversation();
    }

    internal bool ConversationHasEnded()
    {
        return textBoxManager.conversation.hasEnded();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (player)
        {
            player.talkTo = null;
            spriteRenderer.color = start;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (player)
        {
            player.talkTo = this;
            spriteRenderer.color = Color.red;
        }

    }

}
