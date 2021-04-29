using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleProtocol : MonoBehaviour
{
    public int step = 0;
    public float fadeSpeed = 0;
    public Image[] images;

    public bool isWorking = false;
    Animator animator;
    private TextBoxManager textBoxManager;
    private AudioManager audioManager;
    private GameManager gameManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        textBoxManager = FindObjectOfType<TextBoxManager>();
        audioManager = FindObjectOfType<AudioManager>();
        gameManager = FindObjectOfType<GameManager>();

        images[0].color = new Color(
            images[0].color.r,
            images[0].color.g,
            images[0].color.b,
            1f
        );
        images[2].color = new Color(
            images[2].color.r,
            images[2].color.g,
            images[2].color.b,
            0f
        );
        images[3].color = new Color(
            images[3].color.r,
            images[3].color.g,
            images[3].color.b,
            0f
        );
    }

    public IEnumerator StartTitleProtocol()
    {
        if (!isWorking)
        {
            isWorking = true;
            // Fade out title

            yield return _WaitUntilImageHasFadedOut(images[0]);

            yield return _WaitUntilConversationHasEnded("intro1");

            // Fade out fire
            yield return _WaitUntilImageHasFadedOut(images[1]);

            yield return audioManager._WaitUntilThemeFadedOut("ttl", 1f);

            // PlayOneShot
            yield return _PlaySoundAndWaitUntilItWasPlayed("hmhmhm", 1f);

            // Fade in magic man
            yield return _WaitUntilImageHasFadedIn(images[2]);


            audioManager.PlayTheme("mmtl");

            yield return _WaitUntilConversationHasEnded("intro2");

            yield return _WaitUntilImageHasFadedOut(images[2]);

            yield return _WaitUntilImageHasFadedIn(images[3]);
            
            yield return _WaitUntilConversationHasEnded("intro3");

            yield return _WaitUntilImageHasFadedOut(images[3]);


            audioManager.FadeOutMagicManIntro();
            yield return new WaitUntil(() =>
            {
                return !audioManager.isThemePlaying("mmtlo");
            });

            yield return _WaitUntilConversationHasEnded("intro4");

            gameManager.LoadScene("1_intro");
        }
    }

    private object _WaitUntilImageHasFadedIn(Image image)
    {
        float timePassed = 0f;
        Color current = image.color;
        image.color = new Color(
                current.r,
                current.g,
                current.b,
                0f
            );
        return new WaitUntil(() => {
            image.color = new Color(
                current.r,
                current.g,
                current.b,
                timePassed
            );
            timePassed += Time.deltaTime;
            return timePassed >= 1f;
        });
    }

    WaitUntil _WaitUntilImageHasFadedOut(Image image)
    {
        float timePassed = 1f;

        Color current = image.color;
        image.color = new Color(
                current.r,
                current.g,
                current.b,
                1f
            );

        return new WaitUntil(() => {
            image.color = new Color(
                current.r,
                current.g,
                current.b,
                timePassed
            );
            timePassed -= Time.deltaTime;
            return timePassed <= 0f;
        });
    }

    private WaitUntil _WaitUntilConversationHasEnded(string conversationId)
    {
        Conversation c = FindObjectOfType<TextImporter>().textFileToConversation(conversationId);
        textBoxManager.EnableTextBox(c, null);
        return new WaitUntil(() => {
            if (Input.GetKeyDown(KeyCode.Space))
                textBoxManager.UpdateConversation();
                return c.hasEnded();
            }
        );
    }

    private WaitUntil _PlaySoundAndWaitUntilItWasPlayed(string soundName, float volumeBetween0and1)
    {
        audioManager.PlaySound(soundName);
        return new WaitUntil(() => !audioManager.isSoundPlaying(soundName));
    }
}

