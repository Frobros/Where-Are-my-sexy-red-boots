using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    ScaleManager scaleManager;
    Vector3 originalPos;
    public float magnitude;

    private void Start()
    {
        scaleManager = FindObjectOfType<ScaleManager>();
        
    }

    private void LateUpdate()
    {

        if (scaleManager.isScaling)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
        } else
        {
            originalPos = transform.localPosition;
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
