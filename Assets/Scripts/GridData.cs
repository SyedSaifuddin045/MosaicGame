using System;
using UnityEngine;

[Serializable]
public class GridData
{
    public int pos_x;
    public int pos_y;
    public GridData(int x, int y)
    {
        this.pos_x = x;
        this.pos_y = y;
    }
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        GridData other = (GridData)obj;
        return pos_x == other.pos_x && pos_y == other.pos_y;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + pos_x.GetHashCode();
            hash = hash * 23 + pos_y.GetHashCode();
            return hash;
        }
    }
}