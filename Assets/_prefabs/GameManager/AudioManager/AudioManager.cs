using System;
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Theme[] themes;
    public Sound[] sounds;

    internal void FadeOutTheme()
    {
        foreach (Theme theme in themes)
        {
            if (theme.isPlaying()) FadeOutTheme(theme.id);
        }
    }

    private void Awake()
    {
        foreach (Theme theme in themes)
        {
            theme._InitializeAudioSource(gameObject.AddComponent<AudioSource>());
        }

        foreach (Sound sound in sounds)
        {
            sound._InitializeAudioSource(gameObject.AddComponent<AudioSource>());
        }
    }

    internal void OnLevelFinishedLoading(Scene scene)
    {
        if (scene.name == "0_title")
        {
            PlayTheme("ttl");
        }
        else if (scene.name == "1_intro")
        {
            PlayTheme("wctl1");
        }
    }

    internal void FadeOutMagicManIntro()
    {
        Theme currentTheme = GetCurrentlyPlayedThemes()[0];
        Theme nextTheme = GetThemeById("mmtlo");

        int beat = (int) (currentTheme.getTime() / 1.87425f);
        float remainder = currentTheme.getTime() - beat * 1.87425f;
        currentTheme.Stop();
        nextTheme.setTime(2f * 1.87425f + remainder);
        nextTheme.setLoop(false);
        nextTheme.Play();
    }

    internal Theme[] GetAllThemes()
    {
        return themes;
    }

    internal void StopThemes()
    {
        Theme[] currentThemes = GetCurrentlyPlayedThemes();
        foreach (Theme t in currentThemes)
        {
            t.Stop();
        }
    }

    internal bool isSoundPlaying(string id)
    {
        Sound s = GetSoundById(id);
        if (s == null)
        {
            return false;
        }
        return s.isPlaying();
    }

    internal bool isThemePlaying(string id)
    {
        Theme s = GetThemeById(id);
        if (s == null)
        {
            return false;
        }
        return s.isPlaying();
    }

    public void StopSound(string id)
    {
        Sound sound = GetSoundById(id);
        if (sound == null)
        {
            Debug.LogError("Sound: " + id + " was not found");
            return;
        }
        sound.Stop();
    }

    public void FadeOutTheme(string id)
    {
        StartCoroutine(FadeOutThemeInCoroutine(id));
    }


    public void PlaySound(string id)
    {
        Sound s = GetSoundById(id);
        if (s != null)
        {
            s.Play();
        }
    }

    public void PlayTheme(string id)
    {
        Theme t1 = GetThemeById(id);
        if (t1.isPlaying())
        {
            Debug.Log("Theme: " + id + " is already Playing");
            return;
        }
        t1.Play();
    }

    public Sound GetSoundById(string id)
    {
        Sound sound = Array.Find(sounds, s => s.id == id);
        if (sound == null)
        {
            Debug.LogError("Theme: " + id + " was not found");
            return null;
        }
        else return sound;
    }

    public Theme GetThemeById(string id)
    {
        Theme theme = Array.Find(themes, t => t.id == id);
        if (theme == null)
        {
            Debug.LogError("Theme: " + id + " was not found");
            return null;
        }
        else return theme;
    }

    private Theme[] GetCurrentlyPlayedThemes()
    {
        return Array.FindAll<Theme>(themes, t => t.isPlaying());
    }

    public WaitUntil _WaitUntilThemeFadedOut(string themeId, float inSeconds)
    {
        Theme theme = GetThemeById(themeId);
        return _WaitUntilThemeFadedToVolume(theme, inSeconds, 0f);
    }

    private WaitUntil _WaitUntilThemeFadedToVolume(Theme theme, float inSeconds, float newVolume)
    {
        float fromVolume = theme.getVolume();
        return _WaitUntilThemeFadedFromVolumeToVolume(theme, inSeconds, fromVolume, newVolume);
    }

    private WaitUntil _WaitUntilThemeFadedFromVolumeToVolume(Theme theme, float inSeconds, float startVolume, float newVolume)
    {
        float fromVolume = startVolume;
        float volumeDragPerSecond = (newVolume - fromVolume) / inSeconds;
        float timePassed = 0f;
        return new WaitUntil(() =>
        {
            timePassed += Time.deltaTime;
            theme.setVolume(theme.getVolume() + volumeDragPerSecond * Time.deltaTime);
            return timePassed >= inSeconds;
        });
    }

    public WaitUntil _WaitUntilThemeFadedIn(Theme theme, float inSeconds, float newVolume)
    {
        return _WaitUntilThemeFadedToVolume(theme, inSeconds, newVolume);
    }

    private IEnumerator FadeOutThemeInCoroutine(string id)
    {
        yield return _WaitUntilThemeFadedOut(id, 1f);
    }

    private WaitUntil _WaitUntilCrossFadedFromThemeToTheme(string fromId, string toId, float inSeconds)
    {
        Theme fromTheme = GetThemeById(fromId);
        Theme toTheme = GetThemeById(toId);
        toTheme.setTime(fromTheme.getTime());
        toTheme.setVolume(0f);
        toTheme.Play();

        float timePassed = 0f;
        float fromVolume = fromTheme.getVolume();
        float volumeGradient = fromVolume / inSeconds;


        return new WaitUntil(() =>
        {
            timePassed += Time.deltaTime;
            fromTheme.setVolume(fromVolume - volumeGradient * timePassed);
            fromTheme.setVolume(volumeGradient * timePassed);
            return timePassed >= inSeconds;
        });
    }
}