using UnityEngine;

public class TransitionLevel : MonoBehaviour
{
    public int activeLevel;
    public bool isAborting;
    public Color current;

    private Renderer[] renderers;
    private Collider2D[] collidersInLevel;
    private bool collidersActive;


    private void Start()
    {
        collidersInLevel = GetComponentsInChildren<Collider2D>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void FadeOut(float transitionFor, float inSec)
    {
        if (collidersActive)
            SetCollidersActive(false);

        float toAlpha = Mathf.Clamp(1f - transitionFor / inSec , 0f, 1f);
        for (int i = 0; i < renderers.Length; i++)
        {
            Color current = renderers[i].material.color;
            renderers[i].material.color = new Color(
                current.r,
                current.g,
                current.b,
                toAlpha
            );
        }
    }

    public void FadeIn(float transitionFor, float inSec)
    {
        if (!collidersActive && transitionFor > 0.5f * inSec)
            SetCollidersActive(true);

        float toAlpha = Mathf.Clamp(transitionFor / inSec, 0f, 1f);

        for (int i = 0; i < renderers.Length; i++)
        {
            Color current = renderers[i].material.color;
            renderers[i].material.color = new Color(
                current.r,
                current.g,
                current.b,
                toAlpha
            );
        }
    }

    private void SetCollidersActive(bool active)
    {
        collidersActive = active;
        foreach (Collider2D col in collidersInLevel)
        {
            col.enabled = active;
        }
    }

}
