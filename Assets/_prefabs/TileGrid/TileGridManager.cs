using System.Collections;
using UnityEngine;

public class TileGridManager : MonoBehaviour
{
    public GameObject[] tileLevels;
    public int currentLevel = 0;

    private bool scaling;
    private bool moving;
    public bool zooming = false;

    public int minimumZoomLevel;
    public int maximumZoomLevel;
    
    public float scaleSpeed;
    public float zoomSpeed;
    Transform mainCam;
    Scalable currentActor;
    public float zoomFactor;

    private void Start()
    {
        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        currentActor = FindObjectOfType<KeyboardHandler>().player;
    }

    internal void ActivateLevel(int zoomDirection)
    {
        int toZoomLevel = currentLevel + zoomDirection;
        Debug.Log("Jump with " + currentActor);
        if (currentActor && minimumZoomLevel <= toZoomLevel && toZoomLevel <= maximumZoomLevel)
        {
            zooming = true;
            StartCoroutine(ZoomTo(zoomDirection > 0, toZoomLevel));
        }
    }

    private IEnumerator ZoomTo(bool isShrinking, int toZoomLevel)
    {
        currentLevel = toZoomLevel;
        for (int i = 0; i < tileLevels.Length; i++)
        {
            if (i != currentLevel)
                tileLevels[i].SetActive(false);
            else
                tileLevels[i].SetActive(true);
        }
        scaling = true;
        moving = true;
        StartCoroutine(ScaleTo(currentLevel, isShrinking));
        StartCoroutine(MoveCameraTo(currentLevel, isShrinking));
        yield return new WaitUntil(() => !moving && !scaling);

        zooming = false;
    }

    private IEnumerator ScaleTo(float toZoomLevel, bool isShrinking)
    {
        float targetScaleFactor = 1 / Mathf.Pow(2f, zoomFactor * toZoomLevel);
        float scalingFor = 0f;
        
        Vector3 fromScale = currentActor.transform.localScale;
        Vector3 targetScale = new Vector3(targetScaleFactor, targetScaleFactor, 1f);

        while (scaling)
        {
            currentActor.transform.localScale = Vector3.Lerp(
                fromScale,
                targetScale,
                scalingFor * scaleSpeed
            );
            scaling = isShrinking && currentActor.transform.localScale.x > targetScale.x
                || !isShrinking && currentActor.transform.localScale.x < targetScale.x;
            scalingFor += Time.deltaTime;
            yield return null;
        }

    }

    private IEnumerator MoveCameraTo(float toZoomLevel, bool isShrinking)
    {
        float movingFor = 0f;

        float fromDepth = mainCam.position.z;
        float targetDepth = -10f / Mathf.Pow(2f, zoomFactor * toZoomLevel);

        while (moving)
        {

            Debug.LogWarning("Try to zoom to " + toZoomLevel
                + " by jumping to " + targetDepth
                + " by " + (isShrinking ? "shrinking!" : "enhancing!"));

            float newDepth = Mathf.Lerp(fromDepth, targetDepth, movingFor * zoomSpeed);
            mainCam.position = new Vector3(
                mainCam.position.x,
                mainCam.position.y,
                newDepth
            );
            moving = isShrinking && mainCam.position.z < targetDepth
                || !isShrinking && mainCam.position.z > targetDepth;
            movingFor += Time.deltaTime;
            yield return null;
        }
    }
}
