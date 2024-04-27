using CustomInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Stencil_Type_To_Sprite", menuName = "Stencil_Type_To_Sprite", order = 0)]
public class Stencil_Type_To_Sprite : ScriptableObject
{
    public SerializableDictionary<Stencil_Type, Sprite> SpriteArray = new SerializableDictionary<Stencil_Type, Sprite>();
}