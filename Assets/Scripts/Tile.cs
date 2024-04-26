using UnityEngine;

public class Tile : MonoBehaviour
{
    public GridData gridData;
    public void Initialize(GridData gridData)
    {
        this.gridData = gridData;
    }
}