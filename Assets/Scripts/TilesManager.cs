using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject tileParent;
    [SerializeField]
    private int height, width;
    [SerializeField]
    private float spacing_x, spacing_y = 0.25f;
    private List<List<Tile>> gameTiles = new();
    private float cameraDistance = 10.0f;
#if UNITY_EDITOR
    private bool RegenerationRequired;
    private int currentHeight, currentWidth;
#endif

    private void Start()
    {
        currentHeight = height;
        currentWidth = width;
        GenerateTiles();
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (height != currentHeight || width != currentWidth)
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
        if (tilePrefab == null)
            return;

        for (int x = 0; x < width; x++)
        {
            List<Tile> tiles = new List<Tile>();
            for (int y = 0; y < height; y++)
            {
                GameObject instantiatedTileGameObject = Instantiate(tilePrefab, new Vector3(x + (x * spacing_x), y + (y * spacing_y)), Quaternion.identity, tileParent.transform);
                instantiatedTileGameObject.name = $"TILE {x},{y}";
                if (instantiatedTileGameObject.TryGetComponent<Tile>(out var tileClassOnInstantiatedGameObject))
                {
                    GridData gridData = new GridData(x, y);
                    tileClassOnInstantiatedGameObject.Initialize(gridData);
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
        currentHeight = height;
        currentWidth = width;
        RegenerationRequired = false;
    }

    private void PositionCameraCentre(Camera camera)
    {
        // Calculate the total width and height of the grid including spacing
        float totalWidth = width + (width - 1) * spacing_x;
        float totalHeight = height + (height - 1) * spacing_y;

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

        int X = width / 2, Y = height / 2;
        float Offset_X = 0, Offset_Y = 0;
        if (width % 2 == 0)
        {
            X -= 1;
            Offset_X = 0.5f + spacing_x / 2;
        }

        if (height % 2 == 0)
        {
            Y -= 1;
            Offset_Y = 0.5f + spacing_y / 2;
        }

        Vector2 closestCentreTilePosition = gameTiles[X][Y].gameObject.transform.position;
        camera.transform.position = new Vector3(closestCentreTilePosition.x + Offset_X, closestCentreTilePosition.y + Offset_Y, -cameraDistance);
    }


}