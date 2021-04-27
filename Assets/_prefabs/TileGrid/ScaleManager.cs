using System;
using System.Collections;
using UnityEngine;

public class ScaleManager : MonoBehaviour
{
    public TransitionLevel[] tileLevels;
    Transform mainCam;
    Player player;
    
    public int currentLevel = 0;

    public bool zooming = false;

    public int minimumZoomLevel;
    public int maximumZoomLevel;
    public bool isAborting;

    internal int fromLevel;
    internal int toLevel;

    public float scaleSpeed = 1f;
    internal int preventLevel = -1;

    // For modifying scale behaviour
    private float zoomFactor = 1.5f;
    private float scaleFactor = 2f;

    internal void PreventFromScalingTo(int v, bool b)
    {
        if (b)
            preventLevel = v;
        else preventLevel = -999;
    }

    private void Start()
    {
        mainCam = Camera.main.transform;
        for (int i = 0; i < tileLevels.Length; i++)
        {
            bool isCurrentLevel = i == currentLevel;
            tileLevels[i].gameObject.SetActive(isCurrentLevel);
        }
    }

    private void Update()
    {
        player = FindObjectOfType<Player>();
    }

    internal void ActivateLevel(int zoomDirection)
    {
        fromLevel = currentLevel;
        toLevel = currentLevel + zoomDirection;
        if (toLevel != preventLevel && player && minimumZoomLevel <= toLevel && toLevel <= maximumZoomLevel)
        {
            zooming = true;
            StartCoroutine(ZoomTo());
        }
    }

    private IEnumerator ZoomTo()
    {
        isAborting = false;
        
        float timePassed = 0f;
        float scaleDuration = 1f / scaleSpeed;
        
        float targetScaleFactor = 1 / Mathf.Pow(2f, scaleFactor * toLevel);
        Vector3 fromScale = player.transform.localScale;
        Vector3 toScale = new Vector3(targetScaleFactor, targetScaleFactor, 1f);

        float fromDepth = mainCam.position.z;
        float toDepth = -10f / Mathf.Pow(2f, zoomFactor * toLevel);
        
        for (int i = 0; i < tileLevels.Length; i++)
        {
            tileLevels[i].gameObject.SetActive(isAffectedLevel(i, fromLevel, toLevel));
        }

        yield return new WaitUntil(() =>
        {
            // transition texture
            tileLevels[fromLevel].FadeOut(timePassed, scaleDuration);
            tileLevels[toLevel].FadeIn(timePassed, scaleDuration);

            tileLevels[fromLevel].SetCollidersActive(timePassed/scaleDuration < 0.2f);
            tileLevels[toLevel].SetCollidersActive(timePassed / scaleDuration >= 0.2f);

            // move camera
            MoveCameraTo(timePassed, scaleDuration, fromDepth, toDepth);

            // scale player
            ScaleTo(timePassed, scaleDuration, fromScale, toScale);

            FindObjectOfType<AudioManager>().SetThemeLevel(timePassed, scaleDuration, fromLevel, toLevel);

            timePassed += Time.deltaTime;

            return isAborting || timePassed >= scaleDuration;
        });
        if (isAborting)
        {
            yield return new WaitUntil(() =>
            {
                // transition texture
                tileLevels[fromLevel].FadeOut(timePassed, scaleDuration);
                tileLevels[toLevel].FadeIn(timePassed, scaleDuration);

                tileLevels[fromLevel].SetCollidersActive(timePassed / scaleDuration < 0.2f);
                tileLevels[toLevel].SetCollidersActive(timePassed / scaleDuration >= 0.2f);

                // move camera
                MoveCameraTo(timePassed, scaleDuration, fromDepth, toDepth);

                // scale player
                ScaleTo(timePassed, scaleDuration, fromScale, toScale);

                FindObjectOfType<AudioManager>().SetThemeLevel(timePassed, scaleDuration, fromLevel, toLevel);

                timePassed -= Time.deltaTime;
                return timePassed <= 0f;
            });
        }

        currentLevel = isAborting ? fromLevel : toLevel;
        
        FindObjectOfType<AudioManager>().SetTheme(!isAborting ? fromLevel : toLevel);

        for (int i = 0; i < tileLevels.Length; i++)
        {
            tileLevels[i].gameObject.SetActive(i == currentLevel);
        }
        isAborting = false;
        zooming = false;
    }

    private void MoveCameraTo(float timePassed, float scaleDuration, float from, float to)
    {
        float newDepth = Mathf.Lerp(from, to, timePassed * scaleSpeed);

        mainCam.position = new Vector3(
            mainCam.position.x,
            mainCam.position.y,
            newDepth
        );
    }

    private void ScaleTo(float timePassed, float scaleDuration, Vector3 from, Vector3 to)
    {
        player.transform.localScale = Vector3.Lerp(
            from,
            to,
            timePassed * scaleSpeed
        );
    }

    internal void Abort()
    {
        isAborting = zooming;
    }

    internal bool isAffectedLevel(int i, int from, int to)
    {
        return i == from || i == to;
    }
}
