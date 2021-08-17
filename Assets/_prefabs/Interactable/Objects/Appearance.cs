using UnityEngine;

public enum CanSeeFrom
{
    ABOVE,
    BELOW,
    BOTH_DIRECTIONS,
    ONLY_IN_RANGE
}

[System.Serializable]
public class Appearance : MonoBehaviour
{
    private PreventScalingToLevel preventLevel;
    public Collider2D[] colliders;
    public Color startColor;
    public CanSeeFrom canSeeFrom = CanSeeFrom.ONLY_IN_RANGE;
    public new Renderer renderer;

    public bool isScaling;
    public int activeFrom, activeTo;    
    public int offset;

    internal int getActiveFrom() { return activeFrom + offset; }
    internal int getActiveTo() { return activeTo + offset; }
    internal void SetOffset(int scalingFactor) { offset = scalingFactor; }

    public void _Initialize(int currentLevel)
    {
        renderer = GetComponent<Renderer>();
        colliders = GetComponentsInChildren<Collider2D>();
        preventLevel = GetComponentInChildren<PreventScalingToLevel>();

        if (activeFrom > activeTo)
            Debug.LogError("activeFrom Field has to be less or equal than activeTo!!!");

        if (renderer != null)
        {
            startColor = renderer.material.color;
        }

        if (isVisibleInLevel(currentLevel))
        {
            FadeIn(1f);
            SetCollidersActive(true);
        }
        else
        {
            FadeOut(1f);
            SetCollidersActive(false);
        }
    }

    public void Fade(float scalingProgress, int fromLevel, int toLevel, bool isAborting)
    {
        if (isEnteringAppearanceRange(fromLevel, toLevel))
        {
            FadeIn(scalingProgress);
            SetCollidersActive(scalingProgress >= 0.2f);
            if (preventLevel)
            {
                if (isAborting) preventLevel.ActivateMovables(fromLevel);
                else preventLevel.ActivateMovables(toLevel);
            }
        }
        else if (isExitingAppearanceRange(fromLevel, toLevel))
        {
            FadeOut(scalingProgress);
            SetCollidersActive(scalingProgress < 0.2f);
            if (preventLevel)
            {
                if (isAborting) preventLevel.ActivateMovables(fromLevel);
                else preventLevel.ActivateMovables(toLevel);
            }
        }
    }

    internal void SetReddishColor()
    {
        if (renderer != null)
        {
            Color newColor = GetRendererColor();
            newColor.r = 1f;
            newColor.g *= 0.25f;
            newColor.b *= 0.25f;
            SetRendererColor(newColor);
        }
    }

    internal void UnsetReddishColor()
    {
        if (renderer != null)
        {
            Color newColor = startColor;
            newColor.a = GetRendererColor().a;
            SetRendererColor(newColor);
        }
    }

    internal bool isVisibleInLevel(int currentLevel)
    {
        switch (canSeeFrom)
        {
            case CanSeeFrom.ONLY_IN_RANGE:
                return isInVisibilityRange(currentLevel);
            case CanSeeFrom.BOTH_DIRECTIONS:
                return true;
            case CanSeeFrom.ABOVE:
                return isAboveRange(currentLevel);
            case CanSeeFrom.BELOW:
                return isBelowRange(currentLevel);
            default:
                return false;
        }
    }

    private bool isBelowRange(int currentLevel)
    {
        return currentLevel >= getActiveFrom();
    }

    private bool isAboveRange(int currentLevel)
    {
        return currentLevel <= getActiveTo();
    }

    private void FadeOut(float scalingProgress)
    {
        FadeFromAlphaToAlpha(1f, 0f, scalingProgress);
    }

    private void FadeIn(float scalingProgress)
    {
        FadeFromAlphaToAlpha(0f, 1f, scalingProgress);
    }

    private bool isInVisibilityRange(int level)
    {
        return level >= getActiveFrom()
            && level <= getActiveTo();
    }

    private void SetCollidersActive(bool active)
    {
        if (colliders != null)
        {
            foreach (Collider2D col in colliders)
            {
                col.enabled = active;
            }
        }
    }

    private void FadeFromAlphaToAlpha(float fromAlpha, float toAlpha, float scalingProgress)
    {
        if (renderer != null)
        {
            float newAlpha = Mathf.Lerp(fromAlpha, toAlpha, scalingProgress);
            Color current = GetRendererColor();
            current.a = newAlpha;
            SetRendererColor(current);
        }
    }

    public bool isAffected(int fromLevel, int toLevel)
    {
        return isEnteringAppearanceRange(fromLevel, toLevel)
            || isExitingAppearanceRange(fromLevel, toLevel);
    }

    public bool isEnteringAppearanceRange(int fromLevel, int toLevel)
    {
        // entering the scaleLevel
        return
        !isVisibleInLevel(fromLevel)
        && isVisibleInLevel(toLevel);
    }

    private bool isExitingAppearanceRange(int fromLevel, int toLevel)
    {
        // exiting the scaleLevel
        return
        isVisibleInLevel(fromLevel)
        && !isVisibleInLevel(toLevel);
    }

    internal Renderer GetRenderer()
    {
        Debug.Log("Get Renderer from appearance " + gameObject.name);
        return renderer;
    }

    private void SetRendererColor(Color color)
    {
        if (renderer is SpriteRenderer) ((SpriteRenderer)renderer).color = color;
        else renderer.material.color = color;
    }

    private Color GetRendererColor()
    {
        if (renderer is SpriteRenderer)
        {
            return ((SpriteRenderer)renderer).color;
        }
        else
        {
            return renderer.material.color;
        }
    }
}
