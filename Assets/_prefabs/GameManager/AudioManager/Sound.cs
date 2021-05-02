using UnityEngine;

[System.Serializable]
public abstract class Audio
{
    protected AudioSource source;
    
    [SerializeField]
    protected AudioClip clip;

    [SerializeField]
    public string id;

    public virtual void _InitializeAudioSource(AudioSource source) {
        Debug.LogWarning("No Implementation provided");
    }

    internal float getVolume() { return source.volume; }
    internal AudioSource getAudioSource() { return source; }

    internal void setVolume(float volume) { source.volume = volume; }

    internal float getTime() { return source.time; }

    internal void setTime(float time) { source.time = time; }

    internal void setLoop(bool loop) { source.loop = loop; }

    internal bool isPlaying() { return source.isPlaying; }

    internal void Play() { if (!isPlaying()) source.Play(); }

    internal void Stop() { source.Stop(); }
}

[System.Serializable]
public class Sound : Audio
{
    public override void _InitializeAudioSource(AudioSource source)
    {
        this.source = source;
        this.source.clip = this.clip;
        this.source.pitch = 1f;
        this.source.volume = 1f;
        this.source.loop = false;
    } 

}

[System.Serializable]
public class Theme : Audio
{
    public float initialVolume;
    public override void _InitializeAudioSource(AudioSource source)
    {
        this.source = source;
        this.source.clip = this.clip;
        this.source.pitch = 1f;
        this.source.volume = 1f;
        this.source.loop = true;
    }
}