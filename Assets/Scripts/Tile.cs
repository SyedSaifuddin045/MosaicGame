using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color color { get; set; }
    public GridData gridData;
    public bool hasStencil;
    public Stencil stencil;
    public bool StencilFilled { get; private set; }
    public bool StencilVisible { get; private set; }
    private GameObject stencilGameobject;
    private GameObject stencilFillGameObject;
    private GameObject stencilMaskObject;
    public void Initialize(GridData gridData)
    {
        this.gridData = gridData;
        stencilGameobject = this.gameObject.transform.GetChild(0).gameObject;
        if (stencilGameobject != null)
        {
            stencilFillGameObject = stencilGameobject.transform.GetChild(0).gameObject;
            stencilMaskObject = stencilGameobject.transform.GetChild(1).gameObject;
        }
    }
    public void ActivateStencil(bool boolean)
    {
        StencilVisible = boolean;
        if (stencilGameobject != null)
            stencilGameobject.SetActive(boolean);
        else
            Debug.LogError("No Stencil Found on tile GameObject");
    }
    public void SetStencilGameObjectSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            SpriteRenderer stencilGameobjectSpriteRenderer = stencilGameobject.GetComponent<SpriteRenderer>();
            stencilGameobjectSpriteRenderer.sprite = sprite;
            stencilGameobjectSpriteRenderer.color = stencil.color;
            stencilFillGameObject.GetComponent<SpriteRenderer>().sprite = sprite;
            stencilMaskObject.GetComponent<SpriteMask>().sprite = sprite;

            if (!stencil.symmetrical)
            {
                if (stencil is AsymmetricStencil asymmetricStencil)
                {
                    stencilGameobject.transform.rotation = Quaternion.Euler(0, 0, -(int)asymmetricStencil.rotation);
                }
            }
        }
    }
}