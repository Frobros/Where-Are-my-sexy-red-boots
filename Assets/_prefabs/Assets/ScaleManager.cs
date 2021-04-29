using System;
using System.Collections;
using UnityEngine;

public class ScaleManager : MonoBehaviour
{
    public TransitionLevel[] tileLevels;
    private AudioManager audioManager;
    private Theme[] themes;

    Transform mainCam;
    Player player;

    public int currentLevel = 0;

    public bool zooming = false;

    public int minimumZoomLevel;
    public int maximumZoomLevel;
    public bool isAborting;

    public float scaleSpeed = 1f;
    internal int preventLevel = -1;

    // For modifying scale behaviour
    private float zoomFactor = 1.5f;
    private float scaleFactor = 2f;
    private float scalingForSeconds;

    internal void PreventFromScalingTo(int toLevel, bool b)
    {
        if (b) preventLevel = toLevel;
        else preventLevel = -1;
    }

    private void Start()
    {
        mainCam = Camera.main.transform;
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<Player>();
        themes = audioManager.themes;
        
        for (int i = 0; i < tileLevels.Length; i++)
        {
            bool isCurrentLevel = i == currentLevel;
            tileLevels[i].gameObject.SetActive(isCurrentLevel);
        }

    }

    private void Update()
    {
        // if scaling is being aborted
        if (isAborting) scalingForSeconds -= Time.deltaTime;
        // else if zooming
        else if (zooming) scalingForSeconds += Time.deltaTime;
    }

    internal void ActivateLevel(int zoomDirection)
    {
        int toLevel = currentLevel + zoomDirection;

        Debug.Log("Zoom from " + currentLevel + " to " + toLevel);
        if (player && minimumZoomLevel <= toLevel && toLevel <= maximumZoomLevel)
        {
            Debug.Log("Lets goo");
            zooming = true;
            StartCoroutine(ZoomTo(toLevel));
        }
    }

    private IEnumerator ZoomTo(int toLevel)
    {
        isAborting = false;
        int fromLevel = currentLevel;
        float scaleDuration = 1f / scaleSpeed;
        
        float targetScaleFactor = 1 / Mathf.Pow(2f, scaleFactor * toLevel);
        Vector3 fromScale = player.transform.localScale;
        Vector3 toScale = new Vector3(targetScaleFactor, targetScaleFactor, 1f);

        float fromDepth = mainCam.position.z;
        float toDepth = -10f / Mathf.Pow(2f, zoomFactor * toLevel);

        Debug.Log("Zoom from " + fromLevel + " to " + toLevel + " in " + scaleDuration);
        for (int i = 0; i < tileLevels.Length; i++)
        {
            tileLevels[i].gameObject.SetActive(isAffectedLevel(i, fromLevel, toLevel));
        }
        scalingForSeconds = 0f;
        yield return _WaitUntilScaled(scaleDuration, fromLevel, toLevel, fromDepth, toDepth, fromScale, toScale);
        if (isAborting)
        {
            yield return _WaitUntilScalingAborted(scaleDuration, fromLevel, toLevel, fromDepth, toDepth, fromScale, toScale);
        }

        StopMutedTheme(!isAborting ? fromLevel : toLevel);
        
        currentLevel = isAborting ? fromLevel : toLevel;

        for (int i = 0; i < tileLevels.Length; i++)
        {
            tileLevels[i].gameObject.SetActive(i == currentLevel);
        }
        isAborting = false;
        zooming = false;
    }

    private WaitUntil _WaitUntilScaled(float scaleDuration, int fromLevel, int toLevel, float fromDepth, float toDepth, Vector3 fromScale, Vector3 toScale)
    {
        float scalingProgress;
        return new WaitUntil(() =>
        {
            scalingProgress = scalingForSeconds / scaleDuration;
            // transition texture
            InterpolateImages(scalingProgress, fromLevel, toLevel);

            // move camera
            MoveCameraTo(scalingProgress, fromDepth, toDepth);

            // scale player
            ScaleTo(scalingProgress, fromScale, toScale);

            // Interpolate Music
            InterpolateThemes(scalingProgress, fromLevel, toLevel);

            // if inside a NoScaleArea-Collider
            if (preventLevel == toLevel && scalingProgress > 0.3f) 
                Abort();

            return isAborting || scalingForSeconds >= scaleDuration;
        });
    }

    // Instead of reusing the method above, use this. The scaling will then abort successfully
    private WaitUntil _WaitUntilScalingAborted(float scaleDuration, int fromLevel, int toLevel, float fromDepth, float toDepth, Vector3 fromScale, Vector3 toScale)
    {
        float scalingProgress;
        return new WaitUntil(() =>
        {
            scalingProgress = scalingForSeconds / scaleDuration;
            // transition texture
            InterpolateImages(scalingProgress, fromLevel, toLevel);

            // move camera
            MoveCameraTo(scalingProgress, fromDepth, toDepth);

            // scale player
            ScaleTo(scalingProgress, fromScale, toScale);

            InterpolateThemes(scalingProgress, fromLevel, toLevel);

            return scalingForSeconds <= 0f;
        });
    }


    private void InterpolateImages(float scalingProgress, int fromLevel, int toLevel)
    {
        tileLevels[fromLevel].FadeOut(scalingProgress);
        tileLevels[toLevel].FadeIn(scalingProgress);
    }

    private void InterpolateThemes(float scalingProgress, int fromLevel, int toLevel)
    {
        Theme fromTheme = audioManager.GetThemeById("wctl" + (fromLevel + 1));
        Theme toTheme = audioManager.GetThemeById("wctl" + (toLevel + 1));
        if (!toTheme.isPlaying())
        {
            toTheme.Play();
        }
        toTheme.setTime(fromTheme.getTime());
        toTheme.setVolume(scalingProgress);
        fromTheme.setVolume(1f - scalingProgress);
    }

    private void MoveCameraTo(float scalingProgress, float from, float to)
    {
        float newDepth = Mathf.Lerp(from, to, scalingProgress);

        mainCam.position = new Vector3(
            mainCam.position.x,
            mainCam.position.y,
            newDepth
        );
    }

    private void ScaleTo(float scalingProgress, Vector3 from, Vector3 to)
    {
        player.transform.localScale = Vector3.Lerp(
            from,
            to,
            scalingProgress
        );
    }

    internal bool isAffectedLevel(int i, int from, int to)
    {
        return i == from || i == to;
    }

    internal void StopMutedTheme(int from)
    {
        Theme fromTheme = themes[from];
        fromTheme.Stop();
    }
    public void Abort()
    {
        isAborting = zooming;
    }
}
