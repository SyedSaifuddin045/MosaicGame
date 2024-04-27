using System;
using CustomInspector;
using UnityEngine;

[Serializable]
public class GridColorWithStencil
{
    public Color color;
    public StencilScriptableObject stencilScriptableObject;
}
[CreateAssetMenu(fileName = "GridInfoScriptableObject", menuName = "GridInfoScriptableObject", order = 0)]
public class GridInfoScriptableObject : ScriptableObject
{
    public GameObject tilePrefab;
    public float spacing_x = 0.25f, spacing_y = 0.25f;
    public int height, width;
    public SerializableDictionary<GridData, GridColorWithStencil> stencilDataForPositions;
}