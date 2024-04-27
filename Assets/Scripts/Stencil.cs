using UnityEngine;

public enum Stencil_Type
{
    None = 0,
    Square,
    Circle,
    Arc,
    Triangle
}

public enum Stencil_Rotation
{
    Zero = 0,
    Ninety = 90,
    OneEighty = 180,
    TwoSeventy = 270,
}
public class Stencil
{
    public Color color;
    public Stencil_Type type;
    public bool symmetrical;
    public Sprite sprite;
    public Stencil(Color color, Stencil_Type type, Sprite sprite)
    {
        this.color = color;
        this.type = type;
        if (type == Stencil_Type.None)
            this.sprite = null;
        else
            this.sprite = sprite;
        if (type == Stencil_Type.Square || type == Stencil_Type.Circle)
        {
            symmetrical = true;
        }
        else
        {
            symmetrical = false;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        Stencil other = (Stencil)obj;
        return color.Equals(other.color) && type == other.type && symmetrical == other.symmetrical;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator ==(Stencil s1, Stencil s2)
    {
        if (ReferenceEquals(s1, s2))
        {
            return true;
        }
        if ((object)s1 == null || (object)s2 == null)
        {
            return false;
        }
        return s1.Equals(s2);
    }

    public static bool operator !=(Stencil s1, Stencil s2)
    {
        return !(s1 == s2);
    }

}
public class AsymmetricStencil : Stencil
{
    public Stencil_Rotation rotation;
    public bool InverseZRotation { get; private set; } = false;

    public AsymmetricStencil(Color color, Stencil_Type type, Sprite sprite, Stencil_Rotation stencil_Rotation) : base(color, type, sprite)
    {
        this.rotation = stencil_Rotation;
    }
    public override bool Equals(object obj)
    {
        if (!base.Equals(obj))
        {
            return false;
        }
        Stencil stencil = (Stencil)obj;
        AsymmetricStencil other = (AsymmetricStencil)obj;
        return stencil == this && rotation == other.rotation && InverseZRotation == other.InverseZRotation;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public static bool operator !=(AsymmetricStencil s1, AsymmetricStencil s2)
    {
        return !(s1 == s2);
    }
    public static bool operator ==(AsymmetricStencil s1, AsymmetricStencil s2)
    {
        if (ReferenceEquals(s1, s2))
        {
            return true;
        }
        if ((object)s1 == null || (object)s2 == null)
        {
            return false;
        }
        return s1.Equals(s2);
    }
}