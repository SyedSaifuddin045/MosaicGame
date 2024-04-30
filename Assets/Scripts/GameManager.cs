using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TilesManager tilesManager;
    public Color currentSelectedColor;
    private StencilScriptableObject currentSelectedStencilScriptableObject;
    public Stencil currentSelectedStencil;


    public bool GameOver;
    public static GameManager Instance
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

        GameOver = false;
        //Subscribe to Tile Event
        Tile.tileFilled += CheckAndEndGame;
        ColourUI.ColorUIClicked += UpdateCurrentSelectedColour;
        UI_StencilManager.CurrentStencilScriptableObjectUpdated += UpdateCurrentSelectedStencilScriptableObject;
    }

    private void OnDestroy()
    {
        Tile.tileFilled -= CheckAndEndGame;
        ColourUI.ColorUIClicked -= UpdateCurrentSelectedColour;
        UI_StencilManager.CurrentStencilScriptableObjectUpdated -= UpdateCurrentSelectedStencilScriptableObject;
    }

    public void GenerateStencilFromSelection()
    {
        currentSelectedStencil = StencilGenerator.Instance.GenerateStencil(currentSelectedColor, currentSelectedStencilScriptableObject);
    }

    private void UpdateCurrentSelectedColour(Color color)
    {
        currentSelectedColor = color;
        GenerateStencilFromSelection();
    }

    private void UpdateCurrentSelectedStencilScriptableObject(StencilScriptableObject SSobject)
    {
        currentSelectedStencilScriptableObject = SSobject;
        GenerateStencilFromSelection();
    }

    public void CheckAndEndGame(GridData gridData)
    {
        if (tilesManager.CheckAllTilesFilled())
        {
            GameOver = true;
        }

    }
}