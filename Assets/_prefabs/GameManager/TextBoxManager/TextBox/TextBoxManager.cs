using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    public GameObject textBox;
    public GameObject background;
    public GameObject author;
    public GameObject content;
    private AudioSource audioSource;
    public bool active = false;
    public bool paused = false;

    private FollowTarget focus;

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
        focus = FindObjectOfType<FollowTarget>();
    }

    public void UpdateConversation()
    {
        if (conversation != null)
        {
            conversation.currentSnippet++;
            if (conversation.hasEnded())
            {
                DisableTextBox();
                if (focus) focus.alternativeTarget = null;
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

    public void EnableTextBox(Conversation conversation, Transform target)
    {
        Time.timeScale = 0f;
        active = true;
        foreach (Transform go in textBox.GetComponentsInChildren<Transform>(true))
        {
            go.gameObject.SetActive(true);
        }
        this.conversation = conversation;
        DisplayConversation();
        textBox.SetActive(true);
        if (target) focus.alternativeTarget = target;
    }

    private void DisableTextBox()
    {
        Time.timeScale = 1f;
        textBox.GetComponent<Image>().CrossFadeAlpha(0F, 0.1f, false);
        textBox.SetActive(false);
        active = false;
    }

    public bool isPlayingSound()
    {
        return audioSource != null && audioSource.isPlaying;
    }
}
