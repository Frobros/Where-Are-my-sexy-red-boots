using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleManager : MonoBehaviour
{
    
    private AudioManager audioManager;
    private AudioManager GetAudioManagerFromActiveGameManager() { return FindObjectOfType<AudioManager>(); }
    
    Transform mainCam;
    PlayerController player;

    public int startInLevel;
    public int currentLevel = 0;
    private int scaleDirection = 0;

    internal int getToLevel()
    {
        return currentLevel + scaleDirection;
    }

    public bool isScaling = false;

    public int minimumZoomLevel;
    public int maximumZoomLevel;
    public bool isAborting;

    public float scaleSpeed = 1f;
    private bool isPreventing = false;
    internal List<int> preventLevels = new List<int>();

    // For modifying scale behaviour
    private float zoomFactor = 1.5f;
    private float scaleFactor = 2f;
    
    private float scalingForSeconds;
    private float fCameraStartPositionZ;

    private int minimumScaleLevelMusic = 0;
    private int maximumScaleLevelMusic = 2;

    private Pocket pocket;
    private Theme[] themes;
    private Appearance[] appearances;
    private Appearance[] affectedAppearances;

    private void Awake()
    {
        audioManager = GetAudioManagerFromActiveGameManager();

        mainCam = Camera.main.transform;
        player = FindObjectOfType<PlayerController>();
        pocket = FindObjectOfType<Pocket>();
        themes = audioManager.themes;
        fCameraStartPositionZ = Camera.main.transform.position.z;

        appearances = FindObjectsOfType<Appearance>();
        Array.ForEach(appearances, a => a._Initialize(currentLevel));
    }

    private void Update()
    {
        // if scaling is being aborted
        if (isAborting) scalingForSeconds -= Time.deltaTime;
        // else if zooming
        else if (isScaling) scalingForSeconds += Time.deltaTime;
    }

    internal void AddToPreventList(int preventLevel)
    {
        preventLevels.Add(preventLevel);
    }

    internal void RemoveFromPreventList(int preventLevel)
    {
        preventLevels.Remove(preventLevel);
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

    private void ScalePlayerTo(float scalingProgress, Vector3 from, Vector3 to)
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

    internal void ScaleTo(int _scaleDirection)
    {
        int toLevel = currentLevel + _scaleDirection;

        if (player && minimumZoomLevel <= toLevel && toLevel <= maximumZoomLevel)
        {
            isScaling = true;
            scaleDirection = _scaleDirection;
            StartCoroutine(ScaleToLevel(toLevel));
        }
    }

    private IEnumerator ScaleToLevel(int toLevel)
    {
        int fromLevel = currentLevel;
        float fromDepth = mainCam.position.z;
        float toDepth = fCameraStartPositionZ / Mathf.Pow(2f, zoomFactor * toLevel);
        float scaleDuration = 1f / scaleSpeed;
        float targetScaleFactor = 1 / Mathf.Pow(2f, scaleFactor * toLevel);
        Vector3 fromScale = player.transform.localScale;
        Vector3 toScale = new Vector3(targetScaleFactor, targetScaleFactor, 1f);
        

        isScaling = true;
        isAborting = false;
        isPreventing = false;

        // fetch affected appearances (Textures and Colliders), that are not scaled
        affectedAppearances = Array.FindAll<Appearance>(appearances, a => !a.isScaling && a.isAffected(fromLevel, toLevel));
     
        // incremented in Update() during scaling
        scalingForSeconds = 0f;

        yield return _WaitUntilScaled(scaleDuration, fromLevel, toLevel, fromDepth, toDepth, fromScale, toScale);
        
        if (isAborting)
        {
            yield return _WaitUntilScalingAborted(scaleDuration, fromLevel, toLevel, fromDepth, toDepth, fromScale, toScale);
        }

        if (isInRangeForMusicTransition(fromLevel, toLevel))
            StopMutedTheme(!isAborting ? fromLevel : toLevel);
        
        currentLevel = isAborting ? fromLevel : toLevel;
        
        isAborting = false;
        isPreventing = false;
        isScaling = false;
        scaleDirection = 0;
    }

    private WaitUntil _WaitUntilScaled(float scaleDuration, int fromLevel, int toLevel, float fromDepth, float toDepth, Vector3 fromScale, Vector3 toScale)
    {
        float scalingProgress;
        return new WaitUntil(() =>
        {
            scalingProgress = scalingForSeconds / scaleDuration;


            // if inside a NoScaleArea-Collider
            if (preventLevels.Contains(toLevel) && scalingProgress > 0.15f)
            {
                player.GetScalingResistance().Abort();
            }

            // move camera
            MoveCameraTo(scalingProgress, fromDepth, toDepth);

            // scale player
            ScalePlayerTo(scalingProgress, fromScale, toScale);


            // Interpolate Music
            if (isInRangeForMusicTransition(fromLevel, toLevel))
            {
                InterpolateThemes(scalingProgress, fromLevel, toLevel);
            }

            // Transition texture and colliders
            for (int i = 0; i < affectedAppearances.Length; i++)
            {
                affectedAppearances[i].Fade(scalingProgress, fromLevel, toLevel, false);
            }


            // Debug.LogWarning(scalingProgress + "% done! ");

            return isPreventing || isAborting || scalingForSeconds >= scaleDuration;
        });
    }


    // Instead of reusing the method above, use this to abort successfully!
    private WaitUntil _WaitUntilScalingAborted(float scaleDuration, int fromLevel, int toLevel, float fromDepth, float toDepth, Vector3 fromScale, Vector3 toScale)
    {
        Debug.LogWarning("START ABORTION!");
        float scalingProgress;
        return new WaitUntil(() =>
        {
            scalingProgress = scalingForSeconds / scaleDuration;
            
            // scale player
            ScalePlayerTo(scalingProgress, fromScale, toScale);

            // move camera
            MoveCameraTo(scalingProgress, fromDepth, toDepth);

            // Interpolate Music
            if (isInRangeForMusicTransition(fromLevel, toLevel))
            {
                InterpolateThemes(scalingProgress, fromLevel, toLevel);
            }

            // transition textures and colliders
            for (int i = 0; i < affectedAppearances.Length; i++)
            {
                affectedAppearances[i].Fade(scalingProgress, fromLevel, toLevel, true);
            }

            // Debug.LogWarning(scalingProgress + "% done! ");

            return scalingForSeconds <= 0f;
        });
    }

    internal void Abort()
    {
        if (isScaling)
        {
            isAborting = true;
            pocket.Abort();
        }
    }

    bool isInRangeForMusicTransition(int fromLevel, int toLevel)
    {
        return
            // is in Range for music transition
            fromLevel >= minimumScaleLevelMusic && fromLevel <= maximumScaleLevelMusic
            && toLevel >= minimumScaleLevelMusic && toLevel <= maximumScaleLevelMusic;
    }
}
