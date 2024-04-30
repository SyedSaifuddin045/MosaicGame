using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_StencilManager : MonoBehaviour
{
    public ConstantsListScriptableObject constantListScriptableObject;
    public GameObject stencilUIPrefab;
    public GameObject StencilUIParent;
    public StencilScriptableObject currentSelectedStencil;
    private int currentSelectedIndex;

    public static Action<StencilScriptableObject> CurrentStencilScriptableObjectUpdated;
    public int CurrentSelectedIndex
    {
        get { return currentSelectedIndex; }
        set
        {
            currentSelectedIndex = value;
            currentSelectedStencil = constantListScriptableObject.stencilScriptableObjects[currentSelectedIndex];
            DeactivateAllHighlight();
            StencilUIParent.transform.GetChild(value).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    public UI_StencilManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        StencilUI.StencilUIClicked += StencilSelected;
        ColourUI.ColorUIClicked += UpdateAllStencilUIColor;
    }
    private void OnDestroy()
    {
        StencilUI.StencilUIClicked -= StencilSelected;
        ColourUI.ColorUIClicked -= UpdateAllStencilUIColor;
    }
    private void DeactivateAllHighlight()
    {
        for (int i = 0; i < StencilUIParent.transform.childCount; i++)
        {
            StencilUIParent.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        PopulateStencilUI();
        CurrentSelectedIndex = 0;
        StencilSelected(currentSelectedIndex);
    }

    private void PopulateStencilUI()
    {
        foreach (var stencilScriptableObject in constantListScriptableObject.stencilScriptableObjects)
        {
            GameObject StencilUIGameObject = Instantiate(stencilUIPrefab, StencilUIParent.transform);

            if (StencilUIGameObject.TryGetComponent<Image>(out var image))
            {
                image.sprite = stencilScriptableObject.sprite;
                if (!stencilScriptableObject.symmetrical)
                {
                    StencilUIGameObject.transform.rotation = Quaternion.Euler(0, 0, -(int)stencilScriptableObject.rotation);
                }
            }
        }
    }

    private void UpdateAllStencilUIColor(Color newColor)
    {
        for (int i = 0; i < StencilUIParent.transform.childCount; i++)
        {
            StencilUIParent.transform.GetChild(i).gameObject.GetComponent<Image>().color = newColor;
        }
    }

    public void StencilSelected(int newIndex)
    {
        CurrentSelectedIndex = newIndex;
        CurrentStencilScriptableObjectUpdated?.Invoke(constantListScriptableObject.stencilScriptableObjects[currentSelectedIndex]);
    }
}