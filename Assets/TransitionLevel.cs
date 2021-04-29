using UnityEngine;

public class TransitionLevel : MonoBehaviour
{
    public bool collidersActive;
    public int activeLevel;
    public bool isAborting;
    public Color current;

    private Renderer[] renderers;
    private Collider2D[] collidersInLevel;


    private void Start()
    {
        collidersInLevel = GetComponentsInChildren<Collider2D>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void FadeOut(float scalingProgress)
    {
        FadeFromAlphaToAlpha(0f, 1f, scalingProgress);
        SetCollidersActive(scalingProgress < 0.2f);
    }

    public void FadeIn(float scalingProgress)
    {
        FadeFromAlphaToAlpha(0f, 1f, scalingProgress);
        SetCollidersActive(scalingProgress >= 0.2f);
    }

    private void FadeFromAlphaToAlpha(float gradient, float fromAlpha, float toAlpha)
    {
        if (renderers != null)
        {
            float newAlpha = Mathf.Clamp(fromAlpha + gradient * (toAlpha - fromAlpha), fromAlpha, toAlpha);
            for (int i = 0; i < renderers.Length; i++)
            {
                Color current = renderers[i].material.color;
                renderers[i].material.color = new Color(
                    current.r,
                    current.g,
                    current.b,
                    newAlpha
                );
            }
        }
    }

    public void SetCollidersActive(bool active)
    {
        collidersActive = active;
        if (collidersInLevel != null)
        {
            foreach (Collider2D col in collidersInLevel)
            {
                col.enabled = active;
            }
        }
    }
}
