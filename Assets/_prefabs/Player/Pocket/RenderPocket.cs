using UnityEngine;

public class RenderPocket : MonoBehaviour
{
    // visual feedback
    private SpriteRenderer spriteRenderer;
    private Pocket pocket;

    public Sprite neutral;
    public Sprite isPocketScalingChildren;
    public Sprite atLeastOneChildToScaleExists;


    // Start is called before the first frame update
    void Start()
    {
        pocket = GetComponent<Pocket>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pocket.isScalingChildren())
        {
            spriteRenderer.sprite = isPocketScalingChildren;
        }
        else if (pocket.atLeastOneChildToScaleExists())
        {
            spriteRenderer.sprite = atLeastOneChildToScaleExists;
        }
        else
        {
            spriteRenderer.sprite = neutral;
        }
    }
}
