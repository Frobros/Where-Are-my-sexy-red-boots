using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [HideInInspector]
    public AudioSource source;
    public AudioClip clip;

    public string name;

    [HideInInspector]
    public bool loop;
    [HideInInspector]
    public bool isTheme;

    public float pitch = 1F;
    public float volume = 1f;

    [HideInInspector]
    public bool isFadingOut = false;
    private float fadeOutTargetVolume = .2f;
    private float fadeOutSpeed = 4f;
    private float fadeInSpeed = 4f;
    private bool isFadingIn;

    public IEnumerator FadeOut()
    {
        isFadingOut = true;
        source.volume = volume;
        while (source.volume > fadeOutTargetVolume)
        {
            source.volume -= fadeOutSpeed * Time.deltaTime;
            yield return null;
        }
        source.Stop();
        isFadingOut = false;
        yield return null;
    }

    internal IEnumerator FadeIn()
    {
        isFadingIn = true;
        source.volume = 0f;
        source.Play();
        while (source.volume < volume)
        {
            source.volume += fadeInSpeed * Time.deltaTime;
            yield return null;
        }
        isFadingIn = false;
        yield return null;
    }
}