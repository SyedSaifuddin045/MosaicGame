using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    [SerializeField]
    private GridInfoScriptableObject gridInfo;
    [SerializeField]
    private GameObject tileParent;
    private List<List<Tile>> gameTiles = new();
#if UNITY_EDITOR
    private bool RegenerationRequired;
    private int currentHeight, currentWidth;
#endif

    private void Start()
    {
        currentHeight = gridInfo.height;
        currentWidth = gridInfo.width;
        GenerateTiles();
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (gridInfo.height != currentHeight || gridInfo.width != currentWidth)
        {
            RegenerationRequired = true;
        }

        if (RegenerationRequired)
        {
            RegenerateTiles();
        }
#endif
    }

    private void RegenerateTiles()
    {
        ClearAllTiles();
        GenerateTiles();
    }

    private void ClearAllTiles()
    {
        for (int i = 0; i < gameTiles.Count; i++)
        {
            for (int j = 0; j < gameTiles[i].Count; j++)
            {
                Destroy(gameTiles[i][j].gameObject);
            }
        }
        gameTiles.Clear();
    }

    private void GenerateTiles()
    {
        if (gridInfo.tilePrefab == null)
            return;

        for (int x = 0; x < gridInfo.width; x++)
        {
            List<Tile> tiles = new List<Tile>();
            for (int y = 0; y < gridInfo.height; y++)
            {
                GameObject instantiatedTileGameObject = Instantiate(gridInfo.tilePrefab, new Vector3(x + (x * gridInfo.spacing_x), y + (y * gridInfo.spacing_y)), Quaternion.identity, tileParent.transform);
                instantiatedTileGameObject.name = $"TILE {x},{y}";
                if (instantiatedTileGameObject.TryGetComponent<Tile>(out var tileClassOnInstantiatedGameObject))
                {
                    GridData gridData = new GridData(x, y);
                    tileClassOnInstantiatedGameObject.Initialize(gridData);
                    if (gridInfo.stencilDataForPositions.ContainsKey(gridData))
                    {
                        tileClassOnInstantiatedGameObject.hasStencil = true;
                        // Debug.Log("Stencil Activated for : " + gridData.pos_x + "," + gridData.pos_y);
                        GridColorWithStencil gridColorWithStencil = gridInfo.stencilDataForPositions[gridData];
                        tileClassOnInstantiatedGameObject.stencil = StencilGenerator.Instance.GenerateStencil(gridColorWithStencil.color, gridColorWithStencil.stencilScriptableObject.type, gridColorWithStencil.stencilScriptableObject.rotation);
                        tileClassOnInstantiatedGameObject.SetStencilGameObjectSprite(gridColorWithStencil.stencilScriptableObject.sprite);
                        tileClassOnInstantiatedGameObject.ActivateStencil(true);
                    }
                    tiles.Add(tileClassOnInstantiatedGameObject);
                }
                else
                {
                    Debug.LogError("Add Tile Script on tile Prefab in order for proper functioning");
                }
            }
            gameTiles.Add(tiles);
        }
        SetVariablesForTileGenerationCompleted();
        PositionCameraCentre(Camera.main);
    }

    private void SetVariablesForTileGenerationCompleted()
    {
        currentHeight = gridInfo.height;
        currentWidth = gridInfo.width;
        RegenerationRequired = false;
    }

    public bool CheckAllTilesFilled()
    {
        // Debug.Log("CHecking for all tiles filled");
        foreach (var tiles in gameTiles)
        {
            foreach (var tile in tiles)
            {
                if (tile.hasStencil && !tile.StencilFilled)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void PositionCameraCentre(Camera camera)
    {
        // Calculate the total width and height of the grid including spacing
        float totalWidth = gridInfo.width + (gridInfo.width - 1) * gridInfo.spacing_x;
        float totalHeight = gridInfo.height + (gridInfo.height - 1) * gridInfo.spacing_y;

        // Calculate the half-width and half-height of the grid
        float halfWidth = totalWidth * 0.5f;
        float halfHeight = totalHeight * 0.5f;

        // Calculate the aspect ratio of the screen
        float aspectRatio = (float)Screen.width / Screen.height;

        // Calculate the distance to place the camera so that all tiles are visible
        float cameraDistanceX = halfWidth / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float cameraDistanceY = halfHeight / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad / aspectRatio);

        // Choose the maximum of the two distances calculated to ensure that all tiles are visible
        float cameraDistance = Mathf.Max(cameraDistanceX, cameraDistanceY);

        int X = gridInfo.width / 2, Y = gridInfo.height / 2;
        float Offset_X = 0, Offset_Y = 0;
        if (gridInfo.width % 2 == 0)
        {
            X -= 1;
            Offset_X = 0.5f + gridInfo.spacing_x / 2;
        }

        if (gridInfo.height % 2 == 0)
        {
            Y -= 1;
            Offset_Y = 0.5f + gridInfo.spacing_y / 2;
        }

        Vector2 closestCentreTilePosition = gameTiles[X][Y].gameObject.transform.position;
        camera.transform.position = new Vector3(closestCentreTilePosition.x + Offset_X, closestCentreTilePosition.y + Offset_Y, -cameraDistance);
    }
}