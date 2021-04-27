using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] themes;
    public Sound[] sounds;
    internal bool startRound;
    internal bool endRound;
    private float fadeOutSpeed = 5f;

    private void Awake()
    {
        foreach (Sound theme in themes)
        {
            theme.source = gameObject.AddComponent<AudioSource>();
            theme.source.clip = theme.clip;
            theme.source.pitch = theme.pitch;
            theme.source.volume = theme.volume;
            theme.source.loop = true;
            theme.isTheme = true;
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.pitch = sound.pitch;
            sound.source.volume = sound.volume;
            sound.source.loop = false;
            sound.isTheme = false;
        }
    }

    internal void FadeOutMagicManIntro()
    {
        Sound currentTheme = Array.Find<Sound>(themes, t => t.source.isPlaying);
        Sound nextTheme = Array.Find<Sound>(themes, t => t.name == "mmtlo");

        int beat = (int) (currentTheme.source.time / 1.87425f);
        float remainder = currentTheme.source.time - beat * 1.87425f;
        currentTheme.source.Stop();
        nextTheme.source.time = 2f * 1.87425f + remainder;
        nextTheme.source.Play();
        nextTheme.source.loop = false;
    }

    internal void OnLevelFinishedLoading(Scene scene)
    {
        ResetThemes();
        if (scene.name == "0_title")
        {
            StartCoroutine(FadeToTheme("ttl"));
        }
        else if (scene.name == "1_intro")
        {
            StartCoroutine(FadeToTheme("wctl1"));
        }
    }

    private void ResetThemes()
    {
        foreach (Sound t in themes)
        {
            t.source.volume = t.volume;
        }
    }

    public IEnumerator FadeToTheme(string name)
    {
        Sound currentTheme = Array.Find<Sound>(themes, t => t.source.isPlaying);
        if (currentTheme == null)
        {
            PlayTheme(name);
        }
        else
        {
            Sound theme1 = Array.Find<Sound>(themes, t => t.name == name);
            yield return new WaitUntil(() =>
            {
                currentTheme.source.volume -= Time.deltaTime * fadeOutSpeed;
                return currentTheme.source.volume < 0.1f;
            });

            currentTheme.source.Stop();
            currentTheme.source.volume = currentTheme.volume;
            theme1.source.Play();
            float x = currentTheme.source.time;
        }

    }

    internal void StopThemes()
    {
        Sound[] currentThemes = Array.FindAll<Sound>(themes, t => t.source.isPlaying);
        foreach (Sound t in currentThemes)
        {
            t.source.Stop();
        }
    }

    internal void Fade(float timePassed)
    {
        Sound currentTheme = Array.Find<Sound>(themes, t => t.source.isPlaying);
        if (currentTheme != null)
            currentTheme.source.volume = timePassed;
    }

    internal void SetThemeLevel(float timePassed, float scaleDuration, int fromLevel, int toLevel)
    {
        string fromName = "wctl" + (fromLevel + 1);
        string toName = "wctl" + (toLevel + 1);
        Sound fromTheme = Array.Find<Sound>(themes, t => t.name == fromName);
        Sound toTheme = Array.Find<Sound>(themes, t => t.name == toName);
        toTheme.source.time = fromTheme.source.time;
        if (!toTheme.source.isPlaying) toTheme.source.Play();

        fromTheme.source.volume = Mathf.Lerp(1f, 0f, timePassed / scaleDuration);
        toTheme.source.volume = Mathf.Lerp(0f, 1f, timePassed / scaleDuration);
    }

    private void StartRound()
    {
        if (startRound)
        {
            Sound currentTheme = Array.Find<Sound>(themes, t => t.source.isPlaying);
            if (currentTheme == null)
            {
                PlayTheme("stage_theme");
                startRound = false;
            }
            else
            {
                Sound theme1 = Array.Find<Sound>(themes, t => t.name == "stage_theme");
                float x = currentTheme.source.time;
                if (x % 6f < 0.05f && x <= 36f
                    || (x + 3f) % 6f < 0.05f && x >= 39f && x <= 81f) 
                {
                    currentTheme.source.Stop();
                    theme1.source.time = 0f;
                    theme1.source.volume = theme1.volume;
                    theme1.source.Play();
                    startRound = false;
                }
            }
        }
    }

    internal void SetTheme(int from)
    {
        string fromName = "wctl" + (from + 1);
        Sound fromTheme = Array.Find<Sound>(themes, t => t.name == fromName);
        fromTheme.source.Stop();
    }

    private void EndRound()
    {
        if (endRound)
        {
            Sound currentTheme = Array.Find<Sound>(themes, t => t.source.isPlaying);
            Sound theme1 = Array.Find<Sound>(themes, t => t.name == "title_theme");
            float x = currentTheme.source.time;
            if (x % 6f < 0.1f)
            {
                currentTheme.source.Stop();
                theme1.source.time = 36f;
                theme1.source.volume = theme1.volume;
                theme1.source.Play();
                endRound = false;
            }
        }
    }

    public void PlaySound(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound: " + name + " was not found");
            return;
        }
        else if (s.source.isPlaying)
        {
            s.source.Stop();
        }
        s.source.pitch = pitch;
        // s.source.volume = s.volume;
        s.source.Play();
    }

    internal bool isSoundPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return false;
        }
        return s.source.isPlaying;
    }
    internal bool isThemePlaying(string name)
    {
        Sound s = Array.Find(themes, theme => theme.name == name);
        if (s == null)
        {
            return false;
        }
        return s.source.isPlaying;
    }

    public void PlayTheme(string name)
    {
        Sound t1 = Array.Find<Sound>(themes, theme => theme.name == name);
        if (t1 == null)
        {
            Debug.Log("Theme: " + name + " was not found");
            return;
        }
        else if (t1.source.isPlaying)
        {
            Debug.Log("Theme: " + name + " is already Playing");
            return;
        }
        t1.source.Play();
    }

    internal void PlaySoundWithRandomPitch(string name)
    {
        float pitch = UnityEngine.Random.Range(.9f, 1.1f);
        PlaySound(name, pitch);
    }

    public void StopSound(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound: " + name + " was not found");
            return;
        }

        sound.source.Stop();
    }

    public void StopAllSounds()
    {
        foreach (Sound sound in sounds)
        {
            if (!sound.isFadingOut)
            {
                StartCoroutine(sound.FadeOut());
            }
        }
    }
}
