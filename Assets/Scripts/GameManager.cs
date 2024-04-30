using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TilesManager tilesManager;
    public Color currentSelectedColor;
    private StencilScriptableObject currentSelectedStencilScriptableObject;
    public Stencil currentSelectedStencil;
    public GameObject winPanel;
    public ParticleSystem winConfetti;
    public GameObject wrongStencilSpawnPrefab;
    public GameObject PoolTransform;
    public int poolSize = 15;
    private List<GameObject> wrongGameObjectPool;

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
        InstantiateWrongGameObjectPool();
        //Subscribe to Tile Event
        Tile.tileFilled += CheckAndEndGame;
        Tile.wrongStencilClicked += SpawnWrongStencil;
        ColourUI.ColorUIClicked += UpdateCurrentSelectedColour;
        UI_StencilManager.CurrentStencilScriptableObjectUpdated += UpdateCurrentSelectedStencilScriptableObject;
    }

    private void InstantiateWrongGameObjectPool()
    {
        wrongGameObjectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(wrongStencilSpawnPrefab, PoolTransform.transform);
            obj.SetActive(false);
            wrongGameObjectPool.Add(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        // Search for an inactive object in the pool
        foreach (GameObject obj in wrongGameObjectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // If no inactive object is found, create a new one and add it to the pool
        GameObject newObj = Instantiate(wrongStencilSpawnPrefab);
        wrongGameObjectPool.Add(newObj);
        return newObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        // Deactivate the object and return it to the pool
        obj.SetActive(false);
    }

    private void SpawnWrongStencil(Vector2 position)
    {
        GameObject wrongInstantiatedGameObject = GetObjectFromPool();
        wrongInstantiatedGameObject.transform.position = position;
        SpriteRenderer SR = wrongInstantiatedGameObject.GetComponent<SpriteRenderer>();
        SR.sprite = currentSelectedStencilScriptableObject.sprite;
        SR.color = currentSelectedColor;
    }

    private void OnDestroy()
    {
        Tile.tileFilled -= CheckAndEndGame;
        Tile.wrongStencilClicked -= SpawnWrongStencil;
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
            AudioManager.Instance.PlaySound(AudioManager.Instance.winSound);
            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
            if (winConfetti != null)
            {
                winConfetti.Play();
            }
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}