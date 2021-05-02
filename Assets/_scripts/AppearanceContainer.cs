using System;

public class AppearanceContainer
{
    internal Appearance[] list;
    internal int scalingFactor;

    internal Appearance GetCurrentAppearance(int currentLevel)
    {
        return Array.Find(list, a => a.isVisibleInLevel(currentLevel));
    }

    internal void SetScalingFactorAndAppearanceOffset(int scaleDirection)
    {
        scalingFactor += scaleDirection;
        int newScalingFactor = scalingFactor;
        Array.ForEach(list, a => a.SetOffset(newScalingFactor));
    }

    internal void SetIsScaling(bool isScaling)
    {
        Array.ForEach(list, a => a.isScaling = isScaling);
    }

    internal void SetRedishColor(int from, int to)
    {
        Appearance fromAppearance = Array.Find(list, a => a.isVisibleInLevel(from));
        Appearance toAppearance = Array.Find(list, a => a.isVisibleInLevel(to));
        if (fromAppearance)
            fromAppearance.SetReddishColor();
        if (toAppearance)
            toAppearance.SetReddishColor();
    }

    internal void UnsetRedishColor()
    {
        Array.ForEach(list, a => a.UnsetReddishColor());
    }
}
