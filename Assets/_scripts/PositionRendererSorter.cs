using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRendererSorter : MonoBehaviour
{
    private int sortingOrderBase = 5000;
    [SerializeField]
    private float offset = 0;
    [SerializeField]
    private bool runOnlyOnce = false;

    private Renderer myRenderer;

    private void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }


    private void LateUpdate()
    {
        myRenderer.sortingOrder = (int) (sortingOrderBase - (transform.position.y - offset) * 10);
        if (runOnlyOnce)
            Destroy(this);
    }

}
