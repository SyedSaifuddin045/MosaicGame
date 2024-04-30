using UnityEngine;

public class StencilGenerator : MonoBehaviour
{
    public Stencil_Type_To_Sprite stencilTypeToSprite;
    public static StencilGenerator Instance { get; private set; }
    public Stencil GenerateStencil(Color color, Stencil_Type type, Stencil_Rotation rotation = Stencil_Rotation.Zero)
    {
        switch (type)
        {
            case Stencil_Type.Square:
                return GenerateSquareStencil(color);
            case Stencil_Type.Circle:
                return GenerateCircleStencil(color);
            case Stencil_Type.Arc:
                return GenerateArcStencil(color, rotation);
            case Stencil_Type.Triangle:
                return GenerateTriangleStencil(color, rotation);

            case Stencil_Type.None:
            default:
                return new Stencil(Color.clear, Stencil_Type.None, null);
        }
    }

    public Stencil GenerateStencil(Color color, StencilScriptableObject stencilScriptableObject)
    {
        return GenerateStencil(color, stencilScriptableObject.type, stencilScriptableObject.rotation);
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
    }
    public Stencil GenerateCircleStencil(Color color)
    {
        return new Stencil(color, Stencil_Type.Circle, stencilTypeToSprite.SpriteArray.GetValue(Stencil_Type.Circle));
    }
    public Stencil GenerateSquareStencil(Color color)
    {
        return new Stencil(color, Stencil_Type.Square, stencilTypeToSprite.SpriteArray.GetValue(Stencil_Type.Square));
    }
    public Stencil GenerateTriangleStencil(Color color, Stencil_Rotation stencil_Rotation)
    {
        return new AsymmetricStencil(color, Stencil_Type.Triangle, stencilTypeToSprite.SpriteArray.GetValue(Stencil_Type.Triangle), stencil_Rotation);
    }
    public Stencil GenerateArcStencil(Color color, Stencil_Rotation stencil_Rotation)
    {
        return new AsymmetricStencil(color, Stencil_Type.Arc, stencilTypeToSprite.SpriteArray.GetValue(Stencil_Type.Arc), stencil_Rotation);
    }
}