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

    private Appearance appearance;
    private ScaleManager scaleManager;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        appearance = GetComponentInParent<Appearance>();
        conversationImporter = FindObjectOfType<TextImporter>();
        textBoxManager = FindObjectOfType<TextBoxManager>();
        scaleManager = FindObjectOfType<ScaleManager>();
        start = spriteRenderer.color;
    }

    internal void StartConversation()
    {
        if (!appearance || (appearance.isVisibleInLevel(scaleManager.currentLevel)))
        {
            Conversation conversation = conversationImporter.textFileToConversation(conversationId);
            textBoxManager.EnableTextBox(conversation, transform);
        }
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
        PlayerController player = collision.transform.GetComponent<PlayerController>();
        if (player)
        {
            player.talkTo = null;
            spriteRenderer.color = start;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.transform.GetComponent<PlayerController>();
        if (player)
        {
            player.talkTo = this;
            spriteRenderer.color = Color.red;
        }

    }

}
