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
        images[1].color = new Color(
            images[1].color.r,
            images[1].color.g,
            images[1].color.b,
            0f
        );
        images[2].color = new Color(
            images[2].color.r,
            images[2].color.g,
            images[2].color.b,
            0f
        );
    }

    public IEnumerator Next()
    {
        if (!isWorking)
        {
            isWorking = true;
            Conversation c = FindObjectOfType<TextImporter>().textFileToConversation("intro1");
            textBoxManager.EnableTextBox(c, null);
            bool init = false;
            yield return new WaitUntil(() => {
                if (init && Input.GetKeyDown(KeyCode.Space))
                    textBoxManager.UpdateConversation();
                else init = true;
                return c.hasEnded();
            });
            step++;
            float timePassed = 1f;
            Color current = images[0].color;
            images[0].color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    1f
                );
                
            yield return new WaitUntil(() => {
                images[0].color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    timePassed
                );
                audioManager.Fade(timePassed);
                timePassed -= Time.deltaTime;
                return timePassed <= 0f;
            });

            audioManager.StopThemes();
            audioManager.PlaySound("hmhm", 1f);
            yield return new WaitUntil(() => !audioManager.isSoundPlaying("hmhm"));
            timePassed = 0f;
            current = images[1].color;
            images[1].color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    0f
                );

            yield return new WaitUntil(() => {
                images[1].color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    timePassed
                );
                audioManager.Fade(timePassed);
                timePassed += Time.deltaTime;
                return timePassed >= 1f;
            });
            audioManager.PlayTheme("mmtl");
            c = FindObjectOfType<TextImporter>().textFileToConversation("intro2");
            textBoxManager.EnableTextBox(c, null);
            yield return new WaitUntil(() => {
                if (Input.GetKeyDown(KeyCode.Space))
                    textBoxManager.UpdateConversation();
                return c.hasEnded();
            });

            timePassed = 1f;
            current = images[1].color;
            images[1].color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    1f
                );

            yield return new WaitUntil(() => {
                images[1].color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    timePassed
                );
                timePassed -= Time.deltaTime;
                return timePassed <= 0f;
            });

            timePassed = 0f;
            current = images[2].color;
            images[2].color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    0f
                );

            yield return new WaitUntil(() => {
                images[2].color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    timePassed
                );
                timePassed += Time.deltaTime;
                return timePassed >= 1f;
            });

            c = FindObjectOfType<TextImporter>().textFileToConversation("intro3");
            textBoxManager.EnableTextBox(c, null);
            yield return new WaitUntil(() => {
                if (Input.GetKeyDown(KeyCode.Space))
                    textBoxManager.UpdateConversation();
                return c.hasEnded();
            });

            timePassed = 1f;
            current = images[2].color;
            images[2].color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    1f
                );

            yield return new WaitUntil(() => {
                images[2].color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    timePassed
                );
                timePassed -= Time.deltaTime;
                return timePassed <= 0f;
            });
            c = FindObjectOfType<TextImporter>().textFileToConversation("intro4");
            textBoxManager.EnableTextBox(c, null);
            yield return new WaitUntil(() => {
                if (Input.GetKeyDown(KeyCode.Space))
                    textBoxManager.UpdateConversation();
                return c.hasEnded();
            });

            audioManager.FadeOutMagicManIntro();
            yield return new WaitUntil(() =>
            {
                return !audioManager.isThemePlaying("mmtlo");
            });

            gameManager.LoadScene("1_intro");
        }
    }
}

