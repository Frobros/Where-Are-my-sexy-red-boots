using System;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    public GameObject textBox;
    public GameObject background;
    public GameObject author;
    public GameObject content;
    public bool active = false;
    public bool paused = false;

    private CameraMovement cameraMovement;

    public Conversation conversation;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (textBox == null)
        {
            Debug.LogWarning("Configure a Text Box!");
        } else
        {
            textBox.SetActive(false);
        }
    }

    public void UpdateConversation()
    {
        if (conversation != null)
        {
            conversation.currentSnippet++;
            if (conversation.hasEnded())
            {
                DisableTextBox();
                if (cameraMovement) cameraMovement.setAlternativeTarget(null);
            }
            else
            {
                DisplayConversation();
            }
        }
    }

    private void DisplayConversation()
    {
        if (audioSource) audioSource.Stop();

        content.GetComponent<Text>().text = conversation.getCurrentSnippet().text;
        Debug.Log(content.GetComponent<Text>().text);

        if (conversation.getAuthor().Length == 0)
        {
            author.gameObject.SetActive(false);
        }
        else
        {
            author.gameObject.SetActive(true);
            author.GetComponent<Text>().text = conversation.getAuthor();
        }

        if (conversation.getCurrentSnippet() != null)
        {
            AudioClip vocals = conversation.getCurrentSnippet().vocals;
            if (audioSource && vocals != null)
            {
                audioSource.PlayOneShot(vocals);
            }
        }

    }

    internal void OnLevelFinishedLoading()
    {
        cameraMovement = FindObjectOfType<CameraMovement>();
    }

    public void EnableTextBox(Conversation conversation, Transform target)
    {
        Time.timeScale = 0f;
        active = true;
        textBox.SetActive(true);

        this.conversation = conversation;
        DisplayConversation();
        if (target) cameraMovement.setAlternativeTarget(target);
    }

    private void DisableTextBox()
    {
        Time.timeScale = 1f;
        textBox.GetComponent<Image>().CrossFadeAlpha(0F, 0.1f, false);
        textBox.SetActive(false);
        active = false;
    }

    private AudioSource audioSource;
    public bool isPlayingSound() { return audioSource != null && audioSource.isPlaying; }
}
