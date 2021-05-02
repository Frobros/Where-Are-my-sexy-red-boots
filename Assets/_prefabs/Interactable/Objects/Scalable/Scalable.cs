using System;
using UnityEngine;

public class Scalable : MonoBehaviour
{
    public Transform originalParent;
    public ScaleManager scaleManager;

    public bool isScaling;

    public int minimumScaleFactor;
    public int maximumScaleFactor;
    public AppearanceContainer appearances;


    private void Start()
    {
        scaleManager = FindObjectOfType<ScaleManager>();
        originalParent = transform.parent;
        appearances = new AppearanceContainer();
        appearances.list = GetComponentsInChildren<Appearance>();
    }

    internal void Abort(int scaleDirection)
    {
        if (isScaling) appearances.SetScalingFactorAndAppearanceOffset(-scaleDirection);
    }

    internal void AttemptChangingParent(Transform parent, int scaleDirection)
    {
        int newScaleFactor = appearances.scalingFactor + scaleDirection;
        if (minimumScaleFactor <= newScaleFactor && newScaleFactor <= maximumScaleFactor)
        {
            isScaling = true;
            appearances.SetScalingFactorAndAppearanceOffset(scaleDirection);
            transform.SetParent(parent);
        }
        else
        {
            isScaling = false;
            int from = scaleManager.currentLevel;
            int to = scaleManager.currentLevel + scaleDirection;
            appearances.SetRedishColor(from, to);
        }
        appearances.SetIsScaling(isScaling);
    }

    internal void ResetParent()
    {
        isScaling = false;
        transform.SetParent(originalParent);
        appearances.SetIsScaling(false);
        appearances.UnsetRedishColor();
    }

    internal bool _isScaling()
    {
        return isScaling;
    }

    internal int GetScalingFactor()
    {
        return appearances.scalingFactor;
    }
}
