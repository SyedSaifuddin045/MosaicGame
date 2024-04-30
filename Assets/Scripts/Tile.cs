using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color color { get; set; }
    public GridData gridData;
    public bool hasStencil;
    private Stencil _stencil;

    public Stencil stencil
    {
        get { return _stencil; }
        set
        {
            _stencil = value;
            hasStencil = true;
        }
    }

    public bool StencilFilled { get; private set; }
    public bool StencilVisible { get; private set; }
    private GameObject stencilGameobject;
    private GameObject stencilFillGameObject;
    private GameObject stencilMaskObject;
    public static Action<Vector2> wrongStencilClicked;
    public static Action<GridData> tileFilled;
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
    private void OnMouseDown()
    {
        // Debug.Log("GameObject name : " + gameObject.name + "hasStencil : " + hasStencil);
        if (hasStencil)
        {
            if (!StencilFilled && stencil == GameManager.Instance.currentSelectedStencil)
            {
                // Debug.Log("Stencil not filled and current Selected Stencil == this.Stencil");
                AudioManager.Instance.PlaySound(AudioManager.Instance.correctSound);
                StencilFilled = true;
                FillStencilGameObject();
                tileFilled?.Invoke(gridData);
            }
            else
            {
                wrongStencilClicked?.Invoke(transform.position);
                AudioManager.Instance.PlaySound(AudioManager.Instance.wrongSound);
            }
        }
    }
    private void FillStencilGameObject()
    {
        // Debug.Log("Stencil filled GameObject Activated");
        stencilFillGameObject.SetActive(true);
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
            stencilFillGameObject.GetComponent<SpriteRenderer>().color = stencil.color;
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