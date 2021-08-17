using System;
using System.Collections.Generic;
using UnityEngine;

public class Pocket : MonoBehaviour
{
    [SerializeField]
    private List<Scalable> scalables = new List<Scalable>();
    private ScaleManager grid;
    [SerializeField] private bool _isScaling = false;
    [SerializeField] private bool _isAborting = false;
    private bool isPocketReset = true;
    private int scaleDirection;


    public bool isScalingChildren() { return scalables.Count > 0 && _isScaling; }
    public bool isScaling() { return _isScaling; }
    public bool isAborting() { return _isAborting; }

    public bool atLeastOneChildToScaleExists() { return scalables.Count > 0; }
    private bool isNewScalable(Scalable scalable) { return !scalables.Contains(scalable); }

    internal void Abort()
    {
        foreach(Scalable scalable in scalables)
        {
            scalable.Abort(scaleDirection);
        }
    }

    private void Awake()
    {
        grid = FindObjectOfType<ScaleManager>();
    }

    private void Update() { 
        // Reset Parents of Scalables
        if (!grid.isScaling && _isScaling)
        {
            _isScaling = false;
            EndScalingChildren();
        } else if (!_isScaling)
            isPocketReset = true;
    }

    internal void ScaleTo(int _scaleDirection)
    {
        if (isPocketReset)
        {
            isPocketReset = false;
            if (!grid.isScaling)
            {
                _isScaling = true;
                scaleDirection = _scaleDirection;
                if (atLeastOneChildToScaleExists())
                {
                    foreach (Scalable s in scalables)
                    {
                        s.AttemptChangingParent(transform, _scaleDirection);
                    }
                }
                grid.ScaleTo(_scaleDirection);
            }
        }
    }

    private void EndScalingChildren()
    {
        foreach (Scalable s in scalables)
        {
            s.ResetParent();
        }
        scalables.RemoveAll(s => true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_isScaling)
        {
            Scalable scalable = other.GetComponentInParent<Scalable>();
            if (scalable != null && isNewScalable(scalable)) {
                scalables.Add(scalable);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isScalingChildren())
        {

            Scalable scalable = other.GetComponentInParent<Scalable>();
            if (scalable != null && !isNewScalable(scalable))
            {
                scalables.Remove(scalable);
            }
        }
    }
}
