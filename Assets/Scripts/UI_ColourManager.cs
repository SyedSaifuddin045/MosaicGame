using UnityEngine;
using UnityEngine.UI;

public class UI_ColourManager : MonoBehaviour
{
    public ConstantsListScriptableObject colorListScriptableObject;
    public GameObject colourUIPrefab;
    public GameObject ColourUIParent;
    public Color currentSelectedColor;
    private int currentSelectedIndex;
    public int CurrentSelectedIndex
    {
        get { return currentSelectedIndex; }
        set
        {
            currentSelectedIndex = value;
            currentSelectedColor = colorListScriptableObject.Colors[currentSelectedIndex];
            DeactivateAllHighlight();
            ColourUIParent.transform.GetChild(value).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    public UI_ColourManager Instance
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

        ColourUI.ColorUIClicked += CurrentSelectedColourChanged;
    }
    private void OnDestroy()
    {
        ColourUI.ColorUIClicked -= CurrentSelectedColourChanged;
    }
    private void DeactivateAllHighlight()
    {
        for (int i = 0; i < ColourUIParent.transform.childCount; i++)
        {
            ColourUIParent.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        PopulateColourUI();
        CurrentSelectedIndex = 0;
        ColourUIParent.transform.GetChild(CurrentSelectedIndex).gameObject.GetComponent<ColourUI>().InvokeColourChangeEvent();
    }

    private void PopulateColourUI()
    {
        foreach (var color in colorListScriptableObject.Colors)
        {
            GameObject ColourUIGameObject = Instantiate(colourUIPrefab, ColourUIParent.transform);

            if (ColourUIGameObject.TryGetComponent<Image>(out var image))
            {
                image.color = color;
            }
        }
    }

    public void CurrentSelectedColourChanged(Color newColor)
    {
        if (colorListScriptableObject.Colors.Contains(newColor))
        {
            CurrentSelectedIndex = colorListScriptableObject.Colors.IndexOf(newColor);
        }
    }
}